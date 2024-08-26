using Assets.Scripts.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.States
{
    public class SMClient : ITickable
    {
        private IEnumerable<ISMContext> _stateMachines;

        public SMClient(IEnumerable<ISMContext> stateMachines)
        {
            _stateMachines = stateMachines;
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
