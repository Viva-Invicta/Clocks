using System;
using UnityEngine;
using UnityEngine.UI;

namespace Clocks
{
    public class EditButtonView : MonoBehaviour
    {
        public event Action Clicked;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject _editingState;

        [SerializeField]
        private GameObject _normalState;

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetState(bool isEditing)
        {
            _editingState.SetActive(isEditing);
            _normalState.SetActive(!isEditing);
        }

        private void HandleClick()
        {
            Clicked?.Invoke();
        }
    }
}