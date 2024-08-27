using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Assets.Scripts.States;
using Assets.Scripts.States.Contexts;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GridBuilder>().AsSingle();
            Container.Bind<LevelBuilder>().AsSingle();

            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BlocksManger>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveBlocksManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BalloonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelJsonConverter>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoardNormalizer>().AsSingle();
            Container.BindInterfacesAndSelfTo<BlocksPool>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SaveLevelService>().AsSingle();
            
            InstallStates();
        }

        private void InstallStates()
        {
            Container.BindInterfacesAndSelfTo<GameSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallonSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<SMClient>().AsSingle();

            Container.Bind<InitState>().AsSingle();
            Container.Bind<PlayState>().AsSingle();
            Container.Bind<NextLevelState>().AsSingle();
            Container.Bind<RestartLevelState>().AsSingle();
            Container.Bind<BalloonsState>().AsSingle();
        }
    }
}
