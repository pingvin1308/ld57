using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Levels;
using UnityEngine;

namespace Game.Scripts
{
    public class Radar : MonoBehaviour
    {
        [SerializeField] private List<Artifact> _artifacts = new();

        [field: SerializeField]
        public Artifact NearestArtifact { get; private set; }
        
        public void OnLevelChanged(Level1 level)
        {
            _artifacts = level.Artifacts.ToList();
        }

        private void Update()
        {
            NearestArtifact = _artifacts
                .OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault();
        }
    }
}