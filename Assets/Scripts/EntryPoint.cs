using UnityEngine;

namespace Clocks
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private ClockView[] _clockViews;

        [SerializeField]
        private TimeRequestService _timeRequestService;

        [SerializeField]
        private TimeTickService _timeTickService;

        private void Awake()
        {   
            var timeHandler = new TimeHandler(_timeRequestService, _timeTickService, _clockViews);

            timeHandler.Initialize();
        }

    }
}