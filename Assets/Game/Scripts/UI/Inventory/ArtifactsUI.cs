using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.UI.Inventory
{
    [RequireComponent(typeof(RectTransform))]
    public class ArtifactsUI : MonoBehaviour
    {
        private Vector3 _startAnchoredPosition;
        private RectTransform _rectTransform;

        [field: SerializeField]
        public ArtifactItemUI ArtifactItemUIPrefab { get; private set; }

        [field: SerializeField]
        public List<ArtifactItemUI> Artifacts { get; private set; }

        [field: SerializeField]
        public Scripts.Inventory Inventory { get; private set; }

        [field: SerializeField]
        public ArtifactSlotUI[] Slots { get; private set; } = Array.Empty<ArtifactSlotUI>();

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startAnchoredPosition = _rectTransform.anchoredPosition3D;
        }

        public void ResetPosition()
        {
            _rectTransform.anchoredPosition = _startAnchoredPosition;
        }

        private void OnEnable()
        {
            Inventory.OnInventoryChanged.AddListener(UpdateUI);
            UpdateUI();
        }

        private void OnDisable()
        {
            Inventory.OnInventoryChanged.RemoveAllListeners();
        }

        public void UpdateUI()
        {
            for (var index = 0; index < Inventory.MaxSize; index++)
            {
                var artifact = Inventory.Artifacts[index];
                if (artifact == null)
                {
                    Slots[index].Unset();
                    continue;
                }

                if (Slots[index].Item != null)
                {
                    continue;
                }

                var artifactGameObject = Instantiate(ArtifactItemUIPrefab, Slots[index].transform);
                Slots[index].Set(artifactGameObject);
                artifactGameObject.Init(artifact, index);
                artifactGameObject.ArtifactDroped.AddListener(Inventory.DropArtifact);
            }
        }

        public void SetUpgradeLevel(int upgradeLevel)
        {
            // throw new NotImplementedException();
        }
    }
}