using UnityEngine;
using WhaleAppTapGame.Logic;
using WhaleAppTapGame.Logic.Effects;
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
        public CameraShakeEffect CameraShakeEffect;

        [Header("Settings")]
        public Settings.UnitEntitySettings PlayerEntity_Settings;
        public Settings.SpawnSettings UnitSpawnSettings;
        public Settings.ScoreSettings ScoreSettings;
        [Header(" -> Unit Factories")]
        public Settings.EnemyUnitEntitySettings SimpleEnemy_UnitEntity_Settings;
        public Settings.DiagonalEnemyUnitEntitySettings DiagonalEnemy_UnitEntity_Settings;
        public Settings.DiagonalEnemyUnitEntitySettings Friendly_UnitEntity_Settings;


        public override void InstallBindings()
        {
            //Bind instances
            Container.BindInstance(PrefabsLibrary).AsSingle();
            Container.BindInstance(UIManager).AsSingle();
            Container.BindInstance(CameraShakeEffect).AsSingle();

            //Bind classes
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.Bind<ScoreController>().AsSingle().WithArguments(ScoreSettings.ScorePerEnemy);
            Container.Bind<UnitSpawnTimer>().AsSingle().WithArguments(UnitSpawnSettings.FirstSpawnDelay, UnitSpawnSettings.SpawnPeriod);
            Container.Bind<iInputManager>().To<TapInputManager>().AsSingle();
            Container.Bind<iUnitEntity>().WithId(InjectIDs.PLAYER_ENTITY).
                To<UnitEntity>().AsTransient().WithArguments(PlayerEntity_Settings.HP_Damage.x, PlayerEntity_Settings.HP_Damage.y, PlayerEntity_Settings.HightlightColor);

            //Bind factories
            Container.Bind<UnitFactory>().WithId(InjectIDs.SIMPLE_UNIT_FACTORY).To<SimpleEnemyUnitFactory>().AsSingle().
                WithArguments(PrefabsLibrary.UnitViewPrefab, SimpleEnemy_UnitEntity_Settings.SpeedRange,
                                                             SimpleEnemy_UnitEntity_Settings.HightlightColor,
                                                             SimpleEnemy_UnitEntity_Settings.HP_Damage);

            Container.Bind<UnitFactory>().WithId(InjectIDs.DIAGONAL_UNIT_FACTORY).To<DiagonalEnemyUnitFactory>().AsSingle().
                WithArguments(PrefabsLibrary.UnitViewPrefab, DiagonalEnemy_UnitEntity_Settings.SpeedRange,
                                                             DiagonalEnemy_UnitEntity_Settings.HightlightColor,
                                                             DiagonalEnemy_UnitEntity_Settings.HP_Damage, 
                                                             DiagonalEnemy_UnitEntity_Settings.ChanceToMoveDiagonal);

            Container.Bind<UnitFactory>().WithId(InjectIDs.FRIENDLY_UNITY_FACTORY).To<FriendlyUnitFactory>().AsSingle().
                WithArguments(PrefabsLibrary.UnitViewPrefab, Friendly_UnitEntity_Settings.SpeedRange,
                                                             Friendly_UnitEntity_Settings.HightlightColor,
                                                             Friendly_UnitEntity_Settings.HP_Damage, 
                                                             Friendly_UnitEntity_Settings.ChanceToMoveDiagonal);
        }
    }
}