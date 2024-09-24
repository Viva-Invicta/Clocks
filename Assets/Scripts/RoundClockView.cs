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
            _secondsArrow.SetDistance(time.Second / 60f);
            _minutesArrow.SetDistance(time.Minute / 60f);
            _hoursArrow.SetDistance(time.Hour / 12f);
        }
    }
}