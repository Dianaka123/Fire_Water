using Assets.Scripts.StateMachine;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.States
{
    public class SMClient : ITickable, IInitializable
    {
        private IEnumerable<ISMContext> _stateMachines;

        public SMClient(IEnumerable<ISMContext> stateMachines)
        {
            _stateMachines = stateMachines;
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;
        }

        public void Tick()
        {
            foreach (var stateMachine in _stateMachines)
            {
                stateMachine.Run();
            }
        }
    }
}
