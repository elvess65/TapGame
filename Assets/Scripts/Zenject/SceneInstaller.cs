using UnityEngine;
using WhaleAppTapGame.Logic;
using WhaleAppTapGame.Logic.Entities;
using WhaleAppTapGame.Logic.Input;
using WhaleAppTapGame.UI;
using Zenject;

namespace WhaleAppTapGame.DI
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("MonoObjects")]
        public PrefabsLibrary PrefabsLibrary;
        public UIManager UIManager;

        [Header("Settings")]
        public Settings.UnitEntitySettings PlayerEntitySettings;
        public Settings.SpawnSettings UnitSpawnSettings;
        public Settings.ScoreSettings ScoreSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(PrefabsLibrary).AsSingle();
            Container.BindInstance(UIManager).AsSingle();

            Container.Bind<ScoreController>().AsSingle().WithArguments(ScoreSettings.ScorePerEnemy);
            Container.Bind<UnitSpawnTimer>().AsSingle().WithArguments(UnitSpawnSettings.FirstSpawnDelay, UnitSpawnSettings.SpawnPeriod);
            Container.Bind<iInputManager>().To<TapInputManager>().AsSingle();
            Container.Bind<iUnitEntity>().WithId(BindIDs.INJECT_ID_PlayerEntity).
                To<UnitEntity>().AsTransient().WithArguments(PlayerEntitySettings.HP, PlayerEntitySettings.Damage, PlayerEntitySettings.HightlightColor);

            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}