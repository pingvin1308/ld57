using System;
using System.Linq;
using Game.Scripts.Artifacts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class CheatsUI : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_Dropdown Artifacts { get; private set; }

        [field: SerializeField]
        public Button Spawn { get; private set; }

        [field: SerializeField]
        public ArtifactSpawner ArtifactSpawner { get; private set; }

        [field: SerializeField]
        public ArtifactsDatabase ArtifactsDatabase { get; private set; }

        [field: SerializeField]
        public Player Player { get; private set; }


        private int _selectedOption;

        private ArtifactDataSO[] _artifacts;

        private void Awake()
        {
            _artifacts = ArtifactsDatabase.AllArtifacts.ToArray();
            var artifacts = _artifacts
                .Select(x => new TMP_Dropdown.OptionData(x.name));
            Artifacts.options.AddRange(artifacts);
        }

        private void OnEnable()
        {
            Artifacts.onValueChanged.AddListener(OnValueChanged);
            Spawn.onClick.AddListener(OnSpawnPressed);
        }

        private void OnDisable()
        {
            Artifacts.onValueChanged.RemoveListener(OnValueChanged);
            Spawn.onClick.RemoveListener(OnSpawnPressed);
        }

        private void OnValueChanged(int value)
        {
            _selectedOption = value;
        }

        private void OnSpawnPressed()
        {
            var artifact = _artifacts[_selectedOption];
            Debug.Log($"OnSpawnPressed: {artifact.name}");

            var spawnedArtifact = ArtifactSpawner.DropArtifact(
                Player.transform.position,
                new ArtifactData(artifact));
            spawnedArtifact.Reveal();
        }
    }
}