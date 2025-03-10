using UnityEngine;

namespace Clocks
{
    public class EntryPoint : MonoBehaviour
    {
        [Header("Views")]
        [SerializeField]
        private ClockView[] _clockViews;

        [SerializeField]
        private EditButtonView _editButtonView;

        [Space(30)][Header("Services")]
        [SerializeField]
        private TimeRequestService _timeRequestService;

        [SerializeField]
        private TimeTickService _timeTickService;

        private void Awake()
        {   
            var clocksHandler = new ClocksHandler(_timeRequestService, _timeTickService, _clockViews, _editButtonView);

            clocksHandler.Initialize();
        }

    }
}