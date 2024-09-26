using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clocks
{
    public class InputFieldClockView : ClockView
    {
        [SerializeField]
        private InputField _inputField;

        public override event Action<DateTime> OnEdit;

        private void OnEnable()
        {
            _inputField.DeactivateInputField();
            _inputField.interactable = false;
        }

        public override void SetTime(DateTime time)
        {
            _inputField.SetTextWithoutNotify(time.ToString("HH:mm:ss"));
        }

        public override void StartEditing()
        {
        }

        public override void StopEditing()
        {
        }
    }
}