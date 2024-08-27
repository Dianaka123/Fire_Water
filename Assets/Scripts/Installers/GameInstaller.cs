using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.States;
using Assets.Scripts.States.Contexts;
using Assets.Scripts.Views;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public UIManager UIManger;

        public override void InstallBindings()
        {
            InstallBuilders();
            InstallManagers();

            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoardNormalizer>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelJsonConverter>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLevelService>().AsSingle();
            Container.Bind<IUIManger>().FromInstance(UIManger);
            
            InstallStatemachines();
            InstallStates();
        }

        private void InstallBuilders()
        {
            Container.BindInterfacesAndSelfTo<GridBuilder>().AsSingle();
            Container.Bind<LevelBuilder>().AsSingle();

            Container.BindInterfacesAndSelfTo<BlocksPool>().AsSingle();
        }

        private void InstallManagers()
        {
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BlocksManger>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridManipulatorFacade>().AsSingle();
            Container.BindInterfacesAndSelfTo<BalloonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BackgroundManager>().AsSingle();
        }

        private void InstallStatemachines()
        {
            Container.BindInterfacesAndSelfTo<GameSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallonSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<SMClient>().AsSingle();
        }

        private void InstallStates()
        {
            Container.Bind<InitState>().AsSingle();
            Container.Bind<PlayState>().AsSingle();
            Container.Bind<NextLevelState>().AsSingle();
            Container.Bind<RestartLevelState>().AsSingle();
            Container.Bind<BalloonsState>().AsSingle();
        }
    }
}
