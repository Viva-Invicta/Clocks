using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Clock
{
    public class RoundClockArrowView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _transitionDuration;

        [SerializeField] private bool _inEditMode;
        [SerializeField] private bool _isGrabbed;

        private Tweener _rotationTween;

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
                var mouseWorldPosition =  Input.mousePosition;
                var direction = mouseWorldPosition - transform.position;

                //angle between x axis and direction vector
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //we need this magic number to make arrow point to the direction of the mouse
                //(or just use sprites that's oriented left-to-right, but that's too easy)
                angle -= 90;

                //vector3.forward is z axis (in this case)
                //so we create a quaternion rotation by angle around this axis
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        //progress is [0, 1]
        public void SetProgress(float distance)
        {
            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }

            var angle = 360 - (360 * distance);
            var newRotation = new Vector3(transform.rotation.x, transform.rotation.y, angle);

            _rotationTween = transform.DORotate(newRotation, _transitionDuration);
        }
    }
}