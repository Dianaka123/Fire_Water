using Assets.Scripts.Configs;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Data;
using Assets.Scripts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BlocksManger
    {
        private readonly GameResources _gameResources;
        private readonly ICanvasManger _canvasManger;

        public Block[,] Blocks;

        public BlocksManger(GameResources gameResources, ICanvasManger canvasManger)
        {
            _gameResources = gameResources;
            _canvasManger = canvasManger;
        }

        public void CreateBlocks(LevelDesc level, GridData grid)
        {
            var row = grid.Indexes.GetLength(0);
            var column = grid.Indexes.GetLength(1);
            Blocks = new Block[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    var blockIndex = level.LevelBlocksSequence[i * column + j];
                    if (blockIndex >= 0)
                    {
                        var position = grid.Indexes[i, j];
                        var block = GameObject.Instantiate(_gameResources.Bloks[blockIndex], _canvasManger.DynamicCanvasTransform);
                        block.transform.localPosition = position;
                        block.SetSize(grid.CellSize);

                        Blocks[i, j] = block;
                    }
                }
            }
        }

    }
}
