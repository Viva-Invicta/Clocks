using System;

namespace Clocks
{
    public class TimeHandler
    {
        private readonly TimeRequestService _timeRequestService;
        private readonly TimeTickService _timeTickService;
        private readonly ClockView[] _clockViews;

        private DateTime _currentTime = DateTime.MinValue;
        public TimeHandler(TimeRequestService timeRequestService, TimeTickService timeTickService, ClockView[] clockViews)
        {
            _timeRequestService = timeRequestService;
            _timeTickService = timeTickService;
            _clockViews = clockViews;
        }

        public void Initialize()
        {
            _timeRequestService.TimeUpdated += HandleTimeUpdated;
            _timeTickService.Tick += HandleTick;
            _timeRequestService.UpdateTime();
            _timeTickService.StartTick();
        }

        private void HandleTick(float deltaTime)
        {
            _currentTime += TimeSpan.FromSeconds(deltaTime);

            foreach (var clockView in _clockViews)
            {
                clockView.SetTime(_currentTime);
            }
        }

        private void HandleTimeUpdated()
        {
            _currentTime = _timeRequestService.LastRecievedTime;

            foreach (var clockView in _clockViews)
            {
                clockView.SetTime(_currentTime);
            }
        }
    }
}