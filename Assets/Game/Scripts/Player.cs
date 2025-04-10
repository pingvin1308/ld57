using System;
using System.Linq;
using Game.Scripts.Artifacts;
using Game.Scripts.Levels;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;

namespace Game.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private float _oxygenTimer;
        private Rigidbody2D _rigidbody;
        private Vector2 _movement;

        public UnityEvent PlayerDied;

        [field: SerializeField]
        public FloatAttribute Speed { get; private set; }

        [field: SerializeField]
        public FloatAttribute Acceleration { get; private set; }

        [field: SerializeField]
        public FloatAttribute Friction { get; private set; }

        [field: SerializeField]
        public LevelBase CurrentLevel { get; private set; }

        [field: SerializeField]
        public Inventory Inventory { get; private set; }

        [field: SerializeField]
        public Oxygen Oxygen { get; private set; }

        [field: SerializeField]
        public Detector Detector { get; private set; }

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

        private void OnDestroy()
        {
            PlayerDied?.RemoveAllListeners();
        }

        private void Update()
        {
            if (Oxygen.Volume == 0)
            {
                Oxygen.Restore();
                Inventory.DropArtifacts();
                Inventory.SpendMoney(300);
                PlayerDied?.Invoke();
                return;
            }
            
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");
            var inputDirection = new Vector2(moveX, moveY).normalized;
            Oxygen.Use(OxygenConsumptionRate * Time.deltaTime);

            var speedModifier = 0;
            var sneakers = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.Sneakers);
            speedModifier += sneakers * 2;

            var lightWeight = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.LightWeight);
            speedModifier += -lightWeight * 2;
            Speed.Apply(speedModifier);
            
            foreach (var mirror in Inventory.CollectedArtifacts.Where(x => x.ArtifactId == ArtifactId.MovementMirror))
            {
                inputDirection = -inputDirection;
            }
            
            _movement = GetMovement(inputDirection);
   

            Detector?.Scan(CurrentLevel?.Artifacts ?? ArraySegment<Artifact>.Empty);
        }

        private Vector2 GetMovement(Vector2 inputDirection)
        {
            var acceleration = Acceleration.Current;
            var speed = Speed.Current;
            var friction = Friction.Current;

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
    public class FloatAttribute : Attribute<float>
    {
        public float Current => BaseValue + Modifier;
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