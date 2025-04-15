using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Artifacts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public UnityEvent<int> MoneyUpdated;
        public UnityEvent OnInventoryChanged;
        
        [FormerlySerializedAs("money")] 
        [SerializeField] 
        private int _money;
        
        [field: SerializeField]
        public ArtifactSpawner ArtifactSpawner { get; set; }

        public IReadOnlyCollection<ArtifactData> CollectedArtifacts => Artifacts
            .Where(x => x != null)
            .ToArray();

        [field: SerializeField]
        public ArtifactData[] Artifacts { get; private set; } = new ArtifactData[7];

        [field: SerializeField]
        public IntAttribute MaxSize { get; private set; }

        [field: SerializeField]
        public int UpgradeLevel { get; private set; }

        public bool IsEnoughSpace => Artifacts.Count(x => x != null) < MaxSize.Current;
        
    
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
            OnInventoryChanged?.Invoke();
        }

        public void OnArtifactCollected(ArtifactData artifact)
        {
            if (artifact.Type == ArtifactType.Currency)
            {
                AddMoney(artifact.BasePrice);
                return;
            }
            
            for (int i = 0; i < MaxSize.Current; i++)
            {
                if (Artifacts[i] == null)
                {
                    Artifacts[i] = artifact;
                        
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        public void OnArtifactSpawned(Artifact artifact)
        {
            artifact.Collected.AddListener(OnArtifactCollected);
        }

        public void DropArtifacts()
        {
            for (var i = 0; i < Artifacts.Length; i++)
            {
                Artifacts[i] = null;
            }

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

        public void DropArtifact(ArtifactData artifact, int index)
        {
            ArtifactSpawner.DropArtifact(artifact);
            Artifacts[index] = null;
            OnInventoryChanged?.Invoke();
        }

        public void ApplyUpgrade(int upgrade, int upgradeLevel)
        {
            UpgradeLevel = upgradeLevel;
            MaxSize.Apply(upgrade);
        }
    }
}
