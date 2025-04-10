using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public UnityEvent<Inventory> ArtifactsUpdated;
        public UnityEvent<int> MoneyUpdated;
        public UnityEvent OnInventoryChanged;
        
        [FormerlySerializedAs("artifactsCount")] 
        [SerializeField] 
        private int _artifactsCount;

        [FormerlySerializedAs("money")] 
        [SerializeField] 
        private int _money;
        
        [field: SerializeField]
        public ArtifactSpawner ArtifactSpawner { get; set; }
        
        [field: SerializeField]
        public List<ArtifactData> CollectedArtifacts { get; private set; } = new();

        public ObservableCollection<ArtifactData> Arts { get; private set; } = new();
        
        [field: SerializeField]
        public int MaxSize { get; private set; }

        public bool IsEnoughSpace => CollectedArtifacts.Count < MaxSize;
        
    
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
            Money = 0;
        }

        public void OnArtifactCollected(ArtifactData artifact)
        {
            if (CollectedArtifacts.Count < MaxSize)
            {
                CollectedArtifacts.Add(artifact);
                ArtifactsUpdated?.Invoke(this);
                OnInventoryChanged?.Invoke();
            }
        }

        public void OnArtifactSpawned(Artifact artifact)
        {
            artifact.Collected.AddListener(OnArtifactCollected);
        }

        public void DropArtifacts()
        {
            CollectedArtifacts.Clear();
            OnInventoryChanged?.Invoke();
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

        public void DropArtifact(ArtifactData artifact)
        {
            ArtifactSpawner.DropArtifact(artifact);
            CollectedArtifacts.Remove(artifact);
            OnInventoryChanged?.Invoke();
        }
    }
}
