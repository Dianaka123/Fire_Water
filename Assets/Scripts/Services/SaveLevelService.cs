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
        private readonly LevelJsonConverter _converter;

        public SaveLevelService(ILevelManager levelManager, LevelJsonConverter converter)
        {
            _levelManeger = levelManager;
            _converter = converter;

        }

        public async UniTask<Level> GetSavedDataAsync(CancellationToken cancellationToken)
        {
            Level data = null;
            if(File.Exists(_savingPath))
            {
                var savedData = await File.ReadAllTextAsync(_savingPath);
                data = _converter.DeserializeLevel(savedData);
            }

            return data;
        }

        public async UniTask SaveLevelStateAsync(CancellationToken cancellationToken)
        {
            var json = _converter.SerializeLevel(_levelManeger.CurrentLevel);
            await File.WriteAllTextAsync(_savingPath, json);
        }
    }
}
