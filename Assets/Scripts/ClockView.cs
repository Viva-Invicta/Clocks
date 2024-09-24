using System;
using UnityEngine;

namespace Clocks
{
    public abstract class ClockView : MonoBehaviour
    {
        public abstract void SetTime(DateTime time);
    }
}