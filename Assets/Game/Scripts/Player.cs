using System;
using System.Linq;
using Game.Scripts.Artifacts;
using Game.Scripts.Levels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using Vector2 = UnityEngine.Vector2;

namespace Game.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private bool _inputEnabled = true;
        private float _oxygenTimer;
        private Vector2 _movement;
        private Rigidbody2D _rigidbody;
        private ShadowCaster2D _shadowCaster2d;
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private float bobAmplitude = 0.1f;
        [SerializeField] private float bobFrequency = 4f;
        private Vector2 _baseLocalPos;

        [SerializeField] private PlayerInteractor _playerInteractor;

        public UnityEvent LevelEntered;
        public UnityEvent PlayerDied;

        [field: SerializeField] public int CurrentLevel { get; private set; }

        [field: SerializeField] public LightEmitter LightEmitter { get; private set; }

        [field: SerializeField] public FloatAttribute Speed { get; private set; }

        [field: SerializeField] public FloatAttribute Acceleration { get; private set; }

        [field: SerializeField] public FloatAttribute Friction { get; private set; }

        [field: SerializeField] public Inventory Inventory { get; private set; }

        [field: SerializeField] public Oxygen Oxygen { get; private set; }

        [field: SerializeField] public Detector Detector { get; private set; }

        [field: SerializeField] public float OxygenConsumptionRate { get; private set; }

        public void OnLevelEnabled(LevelBase level)
        {
            CurrentLevel = level.LevelNumber;
            OxygenConsumptionRate = level.OxygenConsumptionRate;
            LevelEntered?.Invoke();
        }

        public void EnableInput()
        {
            _inputEnabled = true;
        }

        public void DisableInput()
        {
            _inputEnabled = false;
        }

        private void Awake()
        {
            _spriteRenderer = _playerInteractor.GetComponent<SpriteRenderer>();
            _shadowCaster2d = _playerInteractor.GetComponent<ShadowCaster2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _shadowCaster2d.enabled = true;
        }

        private void OnDestroy()
        {
            PlayerDied?.RemoveAllListeners();
            LevelEntered?.RemoveAllListeners();
        }

        private void Update()
        {
            if (Oxygen.Volume == 0)
            {
                Oxygen.Restore();
                Inventory.DropArtifacts();
                Inventory.SpendMoney(Math.Min(300, Inventory.Money));
                PlayerDied?.Invoke();
                return;
            }

            var inputDirection = Vector3.zero;
            if (_inputEnabled)
            {
                inputDirection = GetInputDirection();
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Detector.ToggleRevealingMode();
                }
            }

            Oxygen.Use(OxygenConsumptionRate * Time.deltaTime);

            //todo: Вынести расчет эффектов в обработчик эвента обновления инвентаря.
            //Таким образом не будем рассчитывать эффекты в Update
            var speedModifier = 0;
            var frictionModifier = 0;
            var accelerationModifier = 0;
            var sneakers = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.Sneakers);
            speedModifier += sneakers * 2;
            frictionModifier += -sneakers * 2;
            accelerationModifier += -sneakers * 2;

            var lightWeight = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.LightWeight);
            speedModifier += -lightWeight * 2;
            frictionModifier += lightWeight * 2;
            accelerationModifier += lightWeight * 2;

            Speed.Apply(speedModifier);
            Friction.Apply(frictionModifier);
            Acceleration.Apply(accelerationModifier);

            var mirrorCount = Inventory.CollectedArtifacts.Count(x => x.ArtifactId == ArtifactId.MovementMirror);
            var isMirrored = mirrorCount > 0 && mirrorCount % 2 != 0;
            if (isMirrored)
            {
                inputDirection = -inputDirection;
            }

            _movement = GetMovement(inputDirection);
            
            var yOffset = Mathf.Abs(Mathf.Sin(Time.time * bobFrequency)) * bobAmplitude;
            _spriteRenderer.transform.localPosition = _baseLocalPos + Vector2.up * (yOffset * inputDirection.magnitude);
            _spriteRenderer.flipX = (_movement.x < 0) ^ isMirrored;

            if (Inventory.CollectedArtifacts.Any(x => x.ArtifactId == ArtifactId.Firefly))
            {
                LightEmitter.gameObject.SetActive(true);
                _shadowCaster2d.enabled = false;
            }
            else
            {
                LightEmitter.gameObject.SetActive(false);
                _shadowCaster2d.enabled = true;
            }
        }

        private Vector2 GetInputDirection()
        {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");
            return new Vector2(moveX, moveY).normalized;
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
}