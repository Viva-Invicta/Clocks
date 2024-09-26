using System;

namespace Clocks
{
    public class ClocksHandler
    {
        private readonly TimeRequestService _timeRequestService;
        private readonly TimeTickService _timeTickService;
        private readonly ClockView[] _clockViews;
        private readonly EditButtonView _editButtonView;

        private bool _isEditing;
        private bool _isEdited;

        private DateTime _currentTime = DateTime.MinValue;

        public ClocksHandler(TimeRequestService timeRequestService, TimeTickService timeTickService, ClockView[] clocks, EditButtonView editButton)
        {
            _timeRequestService = timeRequestService;
            _timeTickService = timeTickService;
            _clockViews = clocks;
            _editButtonView = editButton;
        }

        private DateTime CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;

                foreach (var clockView in _clockViews)
                {
                    clockView.SetTime(_currentTime);
                }
            }
        }

        public void Initialize()
        {
            _editButtonView.Clicked += HandleEditButtonClick;
            _timeRequestService.TimeUpdated += HandleTimeUpdated;
            _timeTickService.Tick += HandleTick;

            foreach (var clockView in _clockViews)
            {
                clockView.OnEdit += HandleClockViewEdit;
            }

            _timeRequestService.UpdateTime();
            _timeTickService.StartTick();
        }

        private void HandleClockViewEdit(DateTime newTime)
        {
            _isEdited = true;
            CurrentTime = newTime;
        }

        private void HandleEditButtonClick()
        {
            _isEditing = !_isEditing;
            _editButtonView.SetState(_isEditing);

            if (_isEditing)
            {
                foreach (var clockView in _clockViews)
                {
                    clockView.StartEditing();
                }
            }
            else
            {
                foreach (var clockView in _clockViews)
                {
                    clockView.StopEditing();
                }
            }
        }

        private void HandleTick(float deltaTime)
        {
            if (!_isEdited && CurrentTime - _timeRequestService.LastRecievedTime > TimeSpan.FromHours(1))
            {
                _timeRequestService.UpdateTime();
            }

            if (_isEditing)
            {
                return;    
            }

            CurrentTime += TimeSpan.FromSeconds(deltaTime);
        }

        private void HandleTimeUpdated()
        {
            if (_isEditing)
            {
                return;    
            }

            CurrentTime = _timeRequestService.LastRecievedTime;
        }
    }
}