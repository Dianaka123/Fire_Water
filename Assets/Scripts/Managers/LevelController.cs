using Assets.Scripts.Configs;
using Assets.Scripts.Services.Interfaces;

namespace Assets.Scripts.Managers
{
    public class LevelController
    {
        private readonly LevelsConfiguration _configuration;
        private readonly IGridBuilder _boardService;

        public LevelController(LevelsConfiguration levelsConfiguration, IGridBuilder boardService)
        {
            _configuration = levelsConfiguration;
            _boardService = boardService;
        }

        public void CreateLevel(int id)
        {

        }

    }
}
