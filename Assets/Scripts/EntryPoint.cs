using UnityEngine;

namespace Clocks
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private ClockView[] _clockViews;

        [SerializeField]
        private TimeRequestService _timeRequestService;

        private void Awake()
        {   
            _timeRequestService.TimeUpdated += HandleTimeUpdated;

            _timeRequestService.UpdateTime();
        }

        private void HandleTimeUpdated()
        {
            foreach (var clockView in _clockViews)
            {
                clockView.SetTime(_timeRequestService.LastRecievedTime);
            }
        }
    }
}