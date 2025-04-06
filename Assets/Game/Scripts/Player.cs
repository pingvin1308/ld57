using System;
using Game.Scripts.Levels;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Game.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private float _oxygenTimer = 0f;
        private Rigidbody2D _rigidbody;
        private Vector2 _movement;
        private LevelBase _currentLevel;

        [field: SerializeField]
        public int Acceleration { get; private set; }

        [field: SerializeField]
        public int Friction { get; private set; }

        [field: SerializeField]
        public float Speed { get; private set; }

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
            _currentLevel = level;
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
            var lerpWeight = Time.deltaTime * (inputDirection != Vector2.zero ? Acceleration : Friction);
            _movement = Vector2.Lerp(_movement, inputDirection * Speed, lerpWeight);

            _oxygenTimer += Time.deltaTime;
            if (_oxygenTimer >= 1f)
            {
                Oxygen.Use(OxygenConsumptionRate);
                _oxygenTimer = 0f;
            }
            
            Radar?.Scan(_currentLevel?.Artifacts ?? ArraySegment<Artifact>.Empty);
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _movement * Speed;
        }
    }
}