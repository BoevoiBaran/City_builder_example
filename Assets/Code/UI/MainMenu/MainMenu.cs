using Code.Main;
using UnityEngine;

namespace Code.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuView view;

        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }

        private void OnEnable()
        {
            view.OnNewGameStarted += StartSession;
        }

        private void OnDisable()
        {
            view.OnNewGameStarted -= StartSession;
        }

        private void StartSession()
        {
            Core.Instance.StartSession();
        }
    }
}
