using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public UnityEvent<int> ArtifactsUpdated;
        public UnityEvent<int> MoneyUpdated;

        [FormerlySerializedAs("artifactsCount")] 
        [SerializeField] 
        private int _artifactsCount;

        [FormerlySerializedAs("money")] 
        [SerializeField] 
        private int _money;

        public int ArtifactsCount
        {
            get => _artifactsCount;
            private set
            {
                _artifactsCount = value;
                ArtifactsUpdated?.Invoke(_artifactsCount);
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

        public void OnArtifactChanged(int amount)
        {
            ArtifactsCount += amount;
        }

        public void OnArtifactSpawned(Artifact artifact)
        {
            artifact.Collected.AddListener(OnArtifactChanged);
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
