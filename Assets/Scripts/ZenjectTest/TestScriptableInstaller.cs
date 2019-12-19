using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "TestScriptableInstaller", menuName = "Installers/TestScriptableInstaller")]
public class TestScriptableInstaller : ScriptableObjectInstaller<TestScriptableInstaller>
{
    public TestObject TObject;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TestObjectUser>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TestObject>().FromInstance(TObject).AsSingle();
    }
}

[System.Serializable]
public class TestObject
{
    public int A;
    public int B;
}

public class TestObjectUser : IInitializable
{
    [Inject]
    TestObject ob;

    public void Initialize()
    {
        Debug.Log(ob.A + " " + ob.B);
    }
}