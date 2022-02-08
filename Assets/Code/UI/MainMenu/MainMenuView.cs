using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class MainMenuView : MonoBehaviour
    {
        public event Action OnNewGameStarted;

        [SerializeField] private Button startNewGameBtn;

        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }
        
        private void OnEnable()
        {
            startNewGameBtn.onClick.AddListener(StartGameBtnOnClickHandler);
        }

        private void OnDisable()
        {
            startNewGameBtn.onClick.RemoveAllListeners();
        }

        private void StartGameBtnOnClickHandler()
        {
            OnNewGameStarted?.Invoke();
        }
    }
}
