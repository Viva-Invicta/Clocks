using System;
using Clock;
using UnityEngine;

namespace Clocks
{
    public class RoundClockView : ClockView
    {
        [SerializeField] private RoundClockArrowView _secondsArrow;
        [SerializeField] private RoundClockArrowView _minutesArrow;
        [SerializeField] private RoundClockArrowView _hoursArrow;

        public override void SetTime(DateTime time)
        {
            var secondsArrowPosition = time.Second / 60f;
            var minutesArrowPosition = time.Minute / 60f + secondsArrowPosition / 60f;

            var hour = time.Hour;
            if (hour > 12)
            {
                hour -= 12;
            }

            var hourArrowPosition = time.Hour / 12f + minutesArrowPosition / 12f;
            
            _secondsArrow.SetProgress(secondsArrowPosition);
            _minutesArrow.SetProgress(minutesArrowPosition);
            _hoursArrow.SetProgress(hourArrowPosition);
        }

    }
}