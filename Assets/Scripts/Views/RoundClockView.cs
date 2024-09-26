using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Clock;
using TMPro;
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
            _secondsArrow.ProgressUpdated += HandleSecondsArrowProgressChange;
            _minutesArrow.ProgressUpdated += HandleMinutesArrowProgressChange;
            _hoursArrow.ProgressUpdated += HandleHoursArrowProgressChange;

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

        private void HandleSecondsArrowClockwise()
        {
            var newTime = _lastTime + TimeSpan.FromMinutes(1);
            SetLastTime(true, newTime);
        }   

        private void HandleSecondsArrowCounterwise()
        {
            var newTime = _lastTime - TimeSpan.FromMinutes(1);
            SetLastTime(true, newTime);
        }   

        private void HandleMinutesArrowClockwise()
        {   
            var newTime = _lastTime + TimeSpan.FromHours(1);
            SetLastTime(true, newTime);
        }

        private void HandleMinutesArrowCounterwise()
        {
            var newTime = _lastTime - TimeSpan.FromHours(1);
            SetLastTime(true, newTime);
        }

        private void HandleHoursArrowClockwise()
        {
            var newTime = _lastTime + TimeSpan.FromHours(12);
            SetLastTime(true, newTime);
        }

        private void HandleHoursArrowCounterwise()
        {
            var newTime = _lastTime - TimeSpan.FromHours(12);
            SetLastTime(true, newTime);
        }

        private void HandleSecondsArrowProgressChange()
        {
            var newTime = _lastTime - TimeSpan.FromSeconds(_lastTime.Second);
            newTime += TimeSpan.FromSeconds(_secondsArrow.Progress * 60);
            if (newTime.Minute != _lastTime.Minute)
            {
                newTime -= TimeSpan.FromMinutes(1);
            }

            UnityEngine.Debug.Log(newTime + "**");
            SetLastTime(true, newTime);
        }

        private void HandleMinutesArrowProgressChange()
        {
            var newTime = _lastTime - TimeSpan.FromMinutes(_lastTime.Minute);
            newTime += TimeSpan.FromMinutes(_minutesArrow.Progress * 60);

            SetLastTime(true, newTime);
        }

        private void HandleHoursArrowProgressChange()
        {
            var newTime = _lastTime - TimeSpan.FromHours(_lastTime.Hour);
            if (_lastTime.Hour > 12)
            {
                newTime += TimeSpan.FromHours(12);
            }
            newTime += TimeSpan.FromHours(_hoursArrow.Progress * 12);

            SetLastTime(true, newTime);
        }
    }
}
