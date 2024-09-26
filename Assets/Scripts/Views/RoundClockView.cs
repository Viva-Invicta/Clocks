using System;
using Clock;
using UnityEngine;

namespace Clocks
{
    public class RoundClockView : ClockView
    {
        public override event Action<DateTime> OnEdit;

        [SerializeField] private RoundClockArrowView _secondsArrow;
        [SerializeField] private RoundClockArrowView _minutesArrow;
        [SerializeField] private RoundClockArrowView _hoursArrow;

        private DateTime _lastTime;

        private void OnEnable()
        {
            _secondsArrow.OnEdit += HandleSecondsArrowEdit;
            _minutesArrow.OnEdit += HandleMinutesArrowEdit;
            _hoursArrow.OnEdit += HandleHoursArrowEdit;

            _secondsArrow.TurnedClockwise += HandleSecondsArrowClockwise;
            _minutesArrow.TurnedClockwise += HandleMinutesArrowClockwise;
            _hoursArrow.TurnedClockwise += HandleHoursArrowClockwise;

            _secondsArrow.TurnedCounterclockwise += HandleSecondsArrowCounterwise;
            _minutesArrow.TurnedCounterclockwise += HandleMinutesArrowCounterwise;
            _hoursArrow.TurnedCounterclockwise += HandleHoursArrowCounterwise;
        }

        public override void SetTime(DateTime time)
        {
            var secondsArrowPosition = time.Second / 60f;
            var minutesArrowPosition = time.Minute / 60f + secondsArrowPosition / 60f;

            var hour = time.Hour;
            if (hour > 12)
            {
                hour -= 12;
            }

            var hourArrowPosition = hour / 12f + minutesArrowPosition / 12f;
            
            _secondsArrow.SetProgress(secondsArrowPosition);
            _minutesArrow.SetProgress(minutesArrowPosition);
            _hoursArrow.SetProgress(hourArrowPosition);

            SetLastTime(false, time);
        }

        public override void StartEditing()
        {
            _secondsArrow.StartEditing();
            _minutesArrow.StartEditing();
            _hoursArrow.StartEditing();
        }

        public override void StopEditing()
        {
            _secondsArrow.StopEditing();
            _minutesArrow.StopEditing();
            _hoursArrow.StopEditing();
        }

        private void SetLastTime(bool isEdit, DateTime time)
        {
            _lastTime = time;
            if (isEdit)
            {
                OnEdit?.Invoke(_lastTime);
            }
        }

        private void HandleSecondsArrowClockwise() => SetLastTime(true, _lastTime + TimeSpan.FromMinutes(1));
        private void HandleSecondsArrowCounterwise() => SetLastTime(true, _lastTime - TimeSpan.FromMinutes(1));

        private void HandleMinutesArrowClockwise() => SetLastTime(true, _lastTime + TimeSpan.FromHours(1));
        private void HandleMinutesArrowCounterwise() => SetLastTime(true, _lastTime - TimeSpan.FromHours(1));

        private void HandleHoursArrowClockwise() => SetLastTime(true, _lastTime + TimeSpan.FromHours(12));
        private void HandleHoursArrowCounterwise() => SetLastTime(true, _lastTime - TimeSpan.FromHours(12));

        private void HandleSecondsArrowEdit()
        {
            var newTime = _lastTime - TimeSpan.FromSeconds(_lastTime.Second);
            newTime += TimeSpan.FromSeconds(_secondsArrow.Progress * 60);

            SetLastTime(true, newTime);
        }

        private void HandleMinutesArrowEdit()
        {
            var newTime = _lastTime - TimeSpan.FromMinutes(_lastTime.Minute) - TimeSpan.FromSeconds(_lastTime.Second);
            newTime += TimeSpan.FromMinutes(_minutesArrow.Progress * 60);

            SetLastTime(true, newTime);
        }

        private void HandleHoursArrowEdit()
        {
            var newTime = _lastTime.Date;
            newTime += TimeSpan.FromHours(Mathf.Clamp(_hoursArrow.Progress * 12, 0f, 12f));
            
            if (_lastTime.Hour >= 12)
            {
                newTime += TimeSpan.FromHours(12);
            }

            SetLastTime(true, newTime);
        }
    }
}
