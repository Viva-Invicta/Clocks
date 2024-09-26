using System;
using UnityEngine;

namespace Clocks
{
    public abstract class ClockView : MonoBehaviour
    {
        public abstract event Action<DateTime> OnEdit;
        public abstract void StartEditing();
        public abstract void StopEditing();
        public abstract void SetTime(DateTime time);
    }
}