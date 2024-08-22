using Assets.Scripts.Configs;
using Assets.Scripts.Services.Interfaces;
using System;
namespace Assets.Scripts.Managers
{
    public class LevelManager
    {
        private readonly LevelsConfiguration _configuration;
        private readonly IBoardService _boardService;

        public LevelManager(LevelsConfiguration levelsConfiguration, IBoardService boardService)
        {
            _configuration = levelsConfiguration;
            _boardService = boardService;
        }


    }
}
