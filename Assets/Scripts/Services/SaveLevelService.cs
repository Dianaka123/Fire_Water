using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Services.Interfaces;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Services
{
    public class SaveLevelService : ISaveLevelService
    {
        private readonly string _savingPath = Path.Combine(Application.persistentDataPath, "gameInfo.dat");

        private readonly ILevelManager _levelManeger;

        public SaveLevelService(ILevelManager levelManager)
        {
            _levelManeger = levelManager;
        }

        public async UniTask<LevelSavingData> GetSavedDataAsync(CancellationToken cancellationToken)
        {
            LevelSavingData data = null;
            if(File.Exists(_savingPath))
            {
                var savedData = await File.ReadAllTextAsync(_savingPath);
                data = JsonConvert.DeserializeObject<LevelSavingData>(savedData);
            }

            return data;
        }

        public async UniTask SaveLevelStateAsync(CancellationToken cancellationToken)
        {
            var savingData = new LevelSavingData() { LevelDesc = _levelManeger.CurrentLevel};
            var json = JsonConvert.SerializeObject(savingData);
            await File.WriteAllTextAsync(_savingPath, json);
        }
    }
}
