using System;
using System.Linq;
using Game.Scripts.Levels;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Game.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private float _oxygenTimer;
        private Rigidbody2D _rigidbody;
        private Vector2 _movement;

        [field: SerializeField]
        public Attribute<float> Speed { get; private set; }

        [field: SerializeField]
        public Attribute<float> Acceleration { get; private set; }

        [field: SerializeField]
        public Attribute<float> Friction { get; private set; }

        [field: SerializeField]
        public LevelBase CurrentLevel { get; private set; }

        [field: SerializeField]
        public Inventory Inventory { get; private set; }

        [field: SerializeField]
        public Oxygen Oxygen { get; private set; }

        [field: SerializeField]
        public Radar Radar { get; private set; }

        [field: SerializeField]
        public float OxygenConsumptionRate { get; private set; }

        public void OnLevelEnabled(LevelBase level)
        {
            CurrentLevel = level;
            OxygenConsumptionRate = level.OxygenConsumptionRate;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");
            var inputDirection = new Vector2(moveX, moveY).normalized;
            _oxygenTimer += Time.deltaTime;
            if (_oxygenTimer >= 1f)
            {
                Oxygen.Use(OxygenConsumptionRate);
                _oxygenTimer = 0f;
            }

            var sneakers = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.Sneakers);
            Speed.Apply(sneakers);

            var lightWeight = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.LightWeight);
            Speed.Apply(-lightWeight);

            foreach (var mirror in Inventory.CollectedArtifacts.Where(x => x.ArtifactId == ArtifactId.MovementMirror))
            {
                inputDirection = -inputDirection;
            }
            
            _movement = GetMovement(inputDirection);
   

            Radar?.Scan(CurrentLevel?.Artifacts ?? ArraySegment<Artifact>.Empty);
        }

        private Vector2 GetMovement(Vector2 inputDirection)
        {
            var acceleration = Acceleration.BaseValue + Acceleration.Modifier;
            var speed = Speed.BaseValue + Speed.Modifier;
            var friction = Friction.BaseValue + Friction.Modifier;

            var lerpWeight = Time.deltaTime * (inputDirection != Vector2.zero ? acceleration : friction);
            return Vector2.Lerp(_movement, inputDirection * speed, lerpWeight);
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _movement;
        }
    }

    [Serializable]
    [InlineProperty]
    public class Attribute<TValue>
    {
        [field: SerializeField]
        public TValue BaseValue { get; private set; }

        [field: SerializeField]
        public TValue Modifier { get; private set; }

        public void Reset()
        {
            Modifier = default;
        }

        public void Apply(TValue modified)
        {
            Modifier = modified;
        }
    }
}