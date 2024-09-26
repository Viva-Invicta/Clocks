using System;
using System.Linq;
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
            
            _inputField.onValueChanged.AddListener(OnTimeInputChanged);
            _inputField.interactable = false;
        }

        public override void SetTime(DateTime time)
        {
            _inputField.SetTextWithoutNotify(time.ToString("HH:mm:ss"));
        }

        public override void StartEditing()
        {
            _inputField.interactable = true;
        }

        public override void StopEditing()
        {
            _inputField.interactable = false;
            _inputField.DeactivateInputField();
        }

        private void OnTimeInputChanged(string input)
        {
            var digits = "";
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    digits += c;
                }
            }

            if (digits.Length > 6)
            {
                digits = digits.Substring(0, 6);
            }

            var formattedInput = "";
            for (int i = 0; i < digits.Length; i++)
            {
                if (i == 2 || i == 4) 
                {
                    formattedInput += ":";
                }

                formattedInput += digits[i];
            }

            // Заполняем недостающие символы нулями
            while (formattedInput.Length < 8)
            {
                if (formattedInput.Length == 2 || formattedInput.Length == 5)
                {
                    formattedInput += ":";
                }
                else
                {
                    formattedInput += "0";
                }
            }

            var timeParts = formattedInput.Split(':');

            var hours = int.Parse(timeParts[0]);
            var minutes = int.Parse(timeParts[1]);
            var seconds = int.Parse(timeParts[2]);

            hours = Mathf.Clamp(hours, 0, 23);
            minutes = Mathf.Clamp(minutes, 0, 59);
            seconds = Mathf.Clamp(seconds, 0, 59);

            formattedInput = $"{hours:00}:{minutes:00}:{seconds:00}";
            _inputField.text = formattedInput;

            SetCursorPosition();

            OnEdit?.Invoke(DateTime.Parse(formattedInput));
        }

        private void SetCursorPosition()
        {
            int cursorPosition = _inputField.caretPosition;

            if (cursorPosition == 2 || cursorPosition == 5)
            {
                cursorPosition++;
            }

            _inputField.caretPosition = Mathf.Clamp(cursorPosition, 0, 8);
        }
    }
}