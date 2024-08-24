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
            
            Container.BindInterfacesAndSelfTo<SaveLevelService>().AsSingle();
            
            InstallStates();
        }

        private void InstallStates()
        {
            Container.BindInterfacesAndSelfTo<GameSM>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSMClient>().AsSingle();

            Container.Bind<InitState>().AsSingle();
            Container.Bind<EmptyState>().AsSingle();
        }
    }
}
