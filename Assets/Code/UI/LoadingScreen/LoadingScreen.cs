using UnityEngine;

namespace Code.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private LoadingScreenView view;

        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }
        
        public void SetLoadingProgress(float value)
        {
            view.SetLoadingProgress(value);
        }
    }
}
