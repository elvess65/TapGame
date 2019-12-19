using System;
using UnityEngine;
using Zenject;

public class TestMonoInstaller : MonoInstaller
{
    public GameManager.PlayerSettings PlayerSettings;
    public GameManager.SpawnSettings SpawnSettings;
    public int Delay;
    public TestParamsLibrary ParamsLibrary;

    public override void InstallBindings()
    {
        Container.BindInstance(ParamsLibrary).AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().WithArguments(PlayerSettings, SpawnSettings, Delay);
        Container.Bind<MessageFactory>().WithId("Simple").To<SimpleMessageFactory>().AsTransient().WithArguments(0);
        Container.Bind<MessageFactory>().WithId("Complex").To<ComplexMessageFacotry>().AsTransient().WithArguments(1, 0.5f);
    }
}


public class GameManager : ITickable, IInitializable
{
    readonly PlayerSettings _playerSettings;
    readonly SpawnSettings _spawnSettings;
    [Inject]
    public TestParamsLibrary _paramsLibrary;

    private bool m_IsExecuted = false;
    private float m_LeftTime;

    [Inject(Id = "Simple")]
    private MessageFactory _messageFactorySimple;
    [Inject(Id = "Complex")]
    private MessageFactory _messageFactoryComplex;

    private MessageFactory[] _factoryList;

    public GameManager(PlayerSettings playerSettings, SpawnSettings spawnSettings, int delay)
    {
        Debug.Log("Create game manager");

        _playerSettings = playerSettings;
        _spawnSettings = spawnSettings;

        m_LeftTime = delay;
    }

    public void Initialize()
    {
        Debug.Log("Initialize");
;
        _factoryList = new MessageFactory[]
        {
             _messageFactorySimple,
             _messageFactoryComplex,
        };
    }

    public float GetPlayerSpeed => _playerSettings.Speed;

    public void Tick()
    {
         if (!m_IsExecuted)
        {
            m_LeftTime -= Time.deltaTime;

            if (m_LeftTime <= 0)
            {
                m_IsExecuted = true;
                Execute();
            }
        }
    }


    void Execute()
    {
        for (int i = 0; i < _factoryList.Length; i++)
            Debug.Log(_factoryList[i].CreateMessage().ShowMessage());
    }


    [Serializable]
    public class PlayerSettings
    {
        public int Damage;
        public float Speed;
    }

    [Serializable]
    public class SpawnSettings
    {
        public float SpawnPeriod;
    }
}


public abstract class MessageFactory
{
    protected int _baseParam;

     public MessageFactory(int baseParam)
    {
        _baseParam = baseParam;
    }

    public abstract iMessageText CreateMessage();
}

public class SimpleMessageFactory : MessageFactory
{
    public SimpleMessageFactory(int baseParam) : base(baseParam)
    {
    }

    public override iMessageText CreateMessage() => new SimpleMessage(_baseParam);
}

public class ComplexMessageFacotry : MessageFactory
{
    float _complexParam;

    public ComplexMessageFacotry(int baseParam, float complexParam) : base(baseParam)
    {
        _complexParam = complexParam;
    }

    public override iMessageText CreateMessage() => new ComplexMessage(_baseParam, _complexParam);
}




public interface iMessageText
{
    string ShowMessage();
}

public class SimpleMessage : iMessageText
{
    int _baseParam;

    public SimpleMessage(int baseParam)
    {
        _baseParam = baseParam;
    }

    public string ShowMessage() => $"Simple. Base param {_baseParam}";
} 

public class ComplexMessage : iMessageText
{
    int _baseParam;
    float _complexParam;

    public ComplexMessage(int baseParam, float complexParam)
    {
        _baseParam = baseParam;
        _complexParam = complexParam;
    }

    public string ShowMessage() => $"Complex. Base param {_baseParam}. Comples param: {_complexParam}";
}
