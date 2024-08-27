using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Views;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class BalloonManager : IInitializable
    {
        private struct BalloonParams
        {
            public BalloonView Balloon;
            public float StartY;
            public float StartPhase;
            public float Duration;
            public float AmpY;
            public float AmpX;
        }

        private readonly GameResources _gameResources;
        private readonly IUIManger _uiManager;

        private BalloonConfig _balloonConfig;
        private BalloonParams[] _balloonParams;

        public BalloonManager(GameResources gameResources, IUIManger uiManger)
        {
            _gameResources = gameResources;
            _uiManager = uiManger;
        }

        public void Initialize()
        {
            _balloonConfig = _gameResources.Balloon;
            _balloonParams = new BalloonParams[_balloonConfig.TotalCount];
        }

        public void SpawnBallons()
        {
            var bottomOffset = _uiManager.Size.y * _gameResources.Backgrounds[0].BoardConfig.RelativeBottomOffset;
            var stepY = (_uiManager.Size.y - bottomOffset)  / (_balloonConfig.TotalCount + 1);

            for (int i = 0; i < _balloonConfig.TotalCount; i++)
            {
                var ballonView = GameObject.Instantiate(_balloonConfig.Prefab, _uiManager.BallonRoot);
                SetRandomSprite(ballonView);
                
                var speed = Random.Range(_balloonConfig.MinSpeed, _balloonConfig.MaxSpeed);

                var currentOffsetY = bottomOffset + (i + 1) * stepY;

                _balloonParams[i] = new BalloonParams
                {
                    Balloon = ballonView,
                    StartY = currentOffsetY - _uiManager.Size.y / 2,
                    Duration = _balloonConfig.Duration / speed,
                    StartPhase = Random.Range(0, 1),
                    AmpY = stepY / 2,
                    AmpX = _uiManager.Size.x / 2 + ballonView.Size.x,
                };
            }
        }

        public void MoveBalloons()
        {
            var time = Time.time;

            foreach(var ballon in _balloonParams)
            {
                var phase = (time / ballon.Duration + ballon.StartPhase) * 2 * Mathf.PI;
                var x = ballon.AmpX * Mathf.Sin(phase);
                var y = ballon.StartY + ballon.AmpY * Mathf.Sin(phase * 4);
                ballon.Balloon.SetPosition(new Vector2 (x, y));
            }
        }

        private void SetRandomSprite(BalloonView ballonPrefab)
        {
            int randomSpriteIndex = Random.Range(0, _balloonConfig.Sprites.Count);
            var sprite = _balloonConfig.Sprites[randomSpriteIndex];
            ballonPrefab.SetImage(sprite);
            ballonPrefab.ResizeImage(GetSize());
        }

        private float GetSize()
        {
            var minSide = _uiManager.Size.x;
            return minSide * _balloonConfig.RelativeSize;
        }
    }
}
