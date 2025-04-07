using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public UnityEvent<Inventory> ArtifactsUpdated;
        public UnityEvent<int> MoneyUpdated;

        [FormerlySerializedAs("artifactsCount")] 
        [SerializeField] 
        private int _artifactsCount;

        [FormerlySerializedAs("money")] 
        [SerializeField] 
        private int _money;
        
        
        [field: SerializeField]
        public List<ArtifactData> CollectedArtifacts { get; private set; } = new();
        
        [field: SerializeField]
        public int MaxSize { get; private set; }

        public bool IsEnoughSpace => CollectedArtifacts.Count < MaxSize;
        
        public int ArtifactsCount
        {
            get => _artifactsCount;
            private set
            {
                _artifactsCount = value;
            } 
        }
    
        public int Money
        {
            get => _money;
            private set
            {
                _money = Math.Max(value, 0);
                MoneyUpdated?.Invoke(_money);
            }
        }

        private void Awake()
        {
            ArtifactsCount = 0;
            Money = 0;
        }

        public void OnArtifactCollected(ArtifactData artifact)
        {
            ArtifactsCount += 1;

            if (CollectedArtifacts.Count < MaxSize)
            {
                CollectedArtifacts.Add(artifact);
                ArtifactsUpdated?.Invoke(this);
            }
        }

        public void OnArtifactSpawned(Artifact artifact)
        {
            artifact.Collected.AddListener(OnArtifactCollected);
        }

        public int DropArtifacts()
        {
            var artifactsCount = ArtifactsCount;
            ArtifactsCount = 0;
            return artifactsCount;
        }

        public void AddMoney(int money)
        {
            Money += money;
        }

        public bool SpendMoney(int price)
        {
            if (Money < price)
            {
                return false;
            }
            
            Money -= price;
            return true;
        }
    }
}
