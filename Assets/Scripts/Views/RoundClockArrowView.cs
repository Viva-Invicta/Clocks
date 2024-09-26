using System;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Clock
{
    public class RoundClockArrowView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnEdit;
        public event Action TurnedClockwise;
        public event Action TurnedCounterclockwise;

        [SerializeField] private float _transitionDuration;
        [SerializeField] private bool _inEditMode;    

        private bool _isGrabbed;
        private float _progress;
        private Tweener _rotationTween;

        public float Progress => _progress;

        public void StartEditing()
        {
            _inEditMode = true;
        }

        public void StopEditing()
        {
            _inEditMode = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_inEditMode)
            {
                _isGrabbed = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isGrabbed = false;
        }
        

        public void Update()
        {
            if (_isGrabbed)
            {
                HandleGrabbed();
            }
        }

        private void HandleGrabbed()
        {
            var mouseWorldPosition = Input.mousePosition;
            var direction = mouseWorldPosition - transform.position;

            //angle between x axis and vector from mouse to arrow
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //we need this magic number to make arrow point to the direction of the mouse
            //(or just use sprites that's oriented left-to-right, but that's too easy)
            angle -= 90;

            if (angle < 0)
            {
                angle = 360 + angle;
            }

            //vector3.forward is z axis (in this case)
            //so we create a quaternion rotation by angle around this axis
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var newProgress = 1 - (angle / 360);

            if (_progress != newProgress)
            {
                OnEdit?.Invoke();

                if (_progress - newProgress > 0.6f)
                {
                    TurnedClockwise?.Invoke();
                }
                else if (_progress - newProgress < -0.6f)
                {
                    TurnedCounterclockwise?.Invoke();
                }

                _progress = newProgress;
            }
        }

        //progress is [0, 1]
        public void SetProgress(float progress)
        {
            if (_isGrabbed)
            {
                return;
            }

            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }

            var angle = 360 - (360 * progress);
            var newRotation = new Vector3(transform.rotation.x, transform.rotation.y, angle);

            _rotationTween = transform.DORotate(newRotation, _transitionDuration);

            _progress = progress;
        }
    }
}