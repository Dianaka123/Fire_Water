using UnityEngine;

namespace Assets.Scripts.Views
{
    public class Board : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _canvasRect;

        [SerializeField]
        private int _rowCount = 5;
        [SerializeField]
        private int _columnCount = 5;

        [SerializeField]
        private float _sideOffset;
        [SerializeField] 
        private float _bottomOffset;

        //TODO: Need to clean code. Move all calculation in a separate system. Add indexes for coordinates.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var halfWidth = _canvasRect.rect.width / 2 ;
            var halfHeight = _canvasRect.rect.height / 2;

            var coordinate = new Vector3((halfWidth - _sideOffset) * _canvasRect.localScale.x, (halfHeight - _bottomOffset) * _canvasRect.localScale.y);

            var lengthX = (halfWidth - _sideOffset) * 2 *_canvasRect.localScale.x;
            var lengthY = (halfHeight - _bottomOffset) * 2 * _canvasRect.localScale.y;

            var cellSizeX = lengthX / _columnCount;
            var cellSizeY = lengthY / _rowCount;

            var diffSize = cellSizeX - cellSizeY;

            var emptySpace = diffSize > 0 ? diffSize * _columnCount : 0 ;

            var cellSize = Mathf.Min(cellSizeX, cellSizeY);

            Gizmos.DrawLine(coordinate, new Vector3(coordinate.x, -coordinate.y));
            Gizmos.DrawLine(-coordinate, new Vector3(coordinate.x, -coordinate.y));
            Gizmos.DrawLine(-coordinate, new Vector3(-coordinate.x, coordinate.y));
            Gizmos.DrawLine(new Vector3(-coordinate.x, coordinate.y), coordinate);

            var bottomCoordinate = new Vector3(-coordinate.x + (emptySpace / 2), -coordinate.y);

            var x = bottomCoordinate.x;
            var y = bottomCoordinate.y;
            for (int j = 0; j < _rowCount; j++)
            {
                for (int i = 0; i < _columnCount; i++)
                {
                    Gizmos.DrawWireCube(new Vector3(x + cellSize / 2, y + cellSize / 2), new Vector3(cellSize, cellSize));
                    x += cellSize;
                }

                x = bottomCoordinate.x;
                y += cellSize;
            }

        }

    }
}
