using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Data;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Views;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BalloonManager : IBallonManager
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

        private readonly IUIManger _uiManager;

        private BalloonConfig _balloonConfig;
        private BalloonParams[] _balloonParams;

        public BalloonManager(GameResources gameResources, IUIManger uiManger)
        {
            _uiManager = uiManger;

            _balloonConfig = gameResources.Balloons;
            _balloonParams = new BalloonParams[_balloonConfig.TotalCount];
        }

        public void SpawnBallons()
        {
            float bottomOffset = _uiManager.Size.y * _balloonConfig.RelativeBottomOffset;
            float stepY = (_uiManager.Size.y - bottomOffset) / (_balloonConfig.TotalCount + 1);

            for (int i = 0; i < _balloonConfig.TotalCount; i++)
            {
                var ballonView = GameObject.Instantiate(_balloonConfig.Prefab, _uiManager.BallonRoot);
                SetSprite(ballonView, i);

                float speed = Random.Range(_balloonConfig.MinSpeed, _balloonConfig.MaxSpeed);
                float currentOffsetY = bottomOffset + (i + 1) * stepY;

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

        public void MoveBalloonsBySin()
        {
            float time = Time.time;

            foreach (var ballon in _balloonParams)
            {
                float phase = (time / ballon.Duration + ballon.StartPhase) * 2 * Mathf.PI;
                float x = ballon.AmpX * Mathf.Sin(phase);
                float y = ballon.StartY + ballon.AmpY * Mathf.Sin(phase * 4);
                ballon.Balloon.SetPosition(new Vector2(x, y));
            }
        }

        private void SetSprite(BalloonView ballonPrefab, int index)
        {
            int spriteIndex = index % _balloonConfig.Sprites.Count;
            var sprite = _balloonConfig.Sprites[spriteIndex];
            ballonPrefab.SetImage(sprite);
            ballonPrefab.ResizeImage(GetSize());
        }

        private float GetSize()
        {
            float minSide = _uiManager.Size.x;
            return minSide * _balloonConfig.RelativeSize;
        }
    }
}
