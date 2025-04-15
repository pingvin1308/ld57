using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UIWorldFollower : MonoBehaviour
    {
        [Header("Canvas Settings")] 
        public Canvas UICanvas;
        public RectTransform UIElement;

        [FormerlySerializedAs("PlaneDistance")]
        [Header("World Space Settings")] 
        [SerializeField] private float _planeDistance = 5f;
        [SerializeField] private float _worldScale = 0.01f;
        [SerializeField] private int _sortingOrder = 10;
        [SerializeField] private Vector3 _worldOffset;

        private Coroutine _moveCoroutine;
        private Tween _moveTween;
        private Vector3 _originalPosition;
        private bool _isFollowing;
        private bool _inWorldSpace;

        private void Awake()
        {
            UICanvas = GetComponent<Canvas>();
            _originalPosition = UIElement.anchoredPosition;
        }

        public void StartFollowing(Vector3 worldTargetPosition)
        {
            if (_isFollowing)
                return;

            _isFollowing = true;

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveToWorldTarget(worldTargetPosition));
        }

        public void StopFollowing()
        {
            _isFollowing = false;

            _moveTween?.Kill();
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            ReturnToScreenSpace();
        }

        private IEnumerator MoveToWorldTarget(Vector3 worldTargetPosition)
        {
            var screenPos = UICanvas.transform.position;
            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _planeDistance));
            UICanvas.transform.position = worldPos;
            UICanvas.renderMode = RenderMode.WorldSpace;
            UICanvas.worldCamera = Camera.main;
            UICanvas.sortingOrder = _sortingOrder;
            UICanvas.GetComponent<RectTransform>().localScale = Vector3.one * _worldScale;
            _inWorldSpace = true;

            var targetWorldPos = worldTargetPosition + _worldOffset;

            _moveTween = UIElement.DOMove(targetWorldPos, 0.5f).SetEase(Ease.OutCubic);
            yield return _moveTween.WaitForCompletion();

            if (!_isFollowing)
            {
                ReturnToScreenSpace();
            }
        }

        private void ReturnToScreenSpace()
        {
            if (!_inWorldSpace) return;

            UICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            UIElement.anchoredPosition = _originalPosition;
            _inWorldSpace = false;
        }
    }
}