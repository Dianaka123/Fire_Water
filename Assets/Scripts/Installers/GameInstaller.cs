using Assets.Scripts.Services;
using Assets.Scripts.StateMachine;
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
            InstallStates();
        }

        private void InstallStates()
        {
            Container.BindInterfacesAndSelfTo<GameSM>().AsSingle();
            Container.Bind<InitState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSMClient>().AsSingle();
        }
    }
}
