using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Clocks
{
    public class TimeTickService : MonoBehaviour
    {
        public event Action<float> Tick;

        private bool _doTick;

        public void StartTick()
        {
            _doTick = true;
        }

        private void Update()
        {
            if (_doTick)
            {
                Tick?.Invoke(Time.deltaTime);
            }
        }
    }
}