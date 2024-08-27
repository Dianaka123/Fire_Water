using Assets.Scripts.Configs;
using Assets.Scripts.Views;
using ModestTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Services
{
    public class BlocksPool : IInitializable
    {
        private struct UniqBlockData
        {
            public int Id;
            public List<Block> AvailableBlocks;
            public List<Block> UsedBlocks;
        }

        private const int MinBlocksInstances = 5;
        private const string PoolPrefabName = "BlocksPool";

        private readonly GameResources _gameResources;

        private List<UniqBlockData> _blocks = new List<UniqBlockData>();
        private Transform _root;

        public BlocksPool(GameResources gameResources)
        {
            _gameResources = gameResources;
        }

        public void Initialize()
        {
            var poolRoot = new GameObject(PoolPrefabName);
            _root = poolRoot.transform;
        }

        public Block GetBlockByID(int id)
        {
            var blocksData = _blocks.FirstOrDefault(b => b.Id == id);

            if (blocksData.AvailableBlocks == null)
            {
                blocksData = InitUniqBlockData(id, blocksData);
                _blocks.Add(blocksData);
            }

            CheckAvailableBlocks(id, blocksData);
            Block block = UseBlock(blocksData);

            return block;
        }

        public void DestroyBlock(Block block)
        {
            var blockData = _blocks.FirstOrDefault(b => b.UsedBlocks.Contains(block));
            ReturnBlock(blockData, block);
        }

        private UniqBlockData InitUniqBlockData(int id, UniqBlockData blocksData)
        {
            blocksData = new UniqBlockData()
            {
                Id = id,
                AvailableBlocks = new List<Block>(MinBlocksInstances),
                UsedBlocks = new List<Block>(MinBlocksInstances),
            };

            return blocksData;
        }

        private void CheckAvailableBlocks(int id, UniqBlockData blocksData)
        {
            if (blocksData.AvailableBlocks.IsEmpty())
            {
                for (int i = 0; i < MinBlocksInstances; i++)
                {
                    blocksData.AvailableBlocks.Add(InstantiateBlock(id));
                }
            }
        }

        private Block InstantiateBlock(int id)
        {
            var block = GameObject.Instantiate(_gameResources.Bloks[id], _root);
            block.gameObject.SetActive(false);

            return block;
        }

        private Block UseBlock(UniqBlockData blocksData)
        {
            var block = blocksData.AvailableBlocks[0];
            blocksData.AvailableBlocks.Remove(block);
            blocksData.UsedBlocks.Add(block);
            block.gameObject.SetActive(true);

            return block;
        }

        private void ReturnBlock(UniqBlockData blocksData, Block block)
        {
            block.gameObject.SetActive(false);
            block.transform.SetParent(_root);

            blocksData.AvailableBlocks.Add(block);
            blocksData.UsedBlocks.Remove(block);
        }
    }
}
