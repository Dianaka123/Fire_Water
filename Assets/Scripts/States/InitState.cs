using Assets.Scripts.Configs;
using Assets.Scripts.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.States
{
    public class InitState : State
    {
        private readonly LevelsConfiguration _levelsConfiguration;

        public InitState(ISMContext context, LevelsConfiguration levelsConfiguration) : base(context)
        {
            _levelsConfiguration = levelsConfiguration;
        }

        public override UniTask Run(CancellationToken token)
        {
            var levelInfo = JsonConvert.DeserializeObject<Level>(_levelsConfiguration.LevelsJSON.text);
            Debug.Log(levelInfo);

            return UniTask.CompletedTask;
        }
    }
}
