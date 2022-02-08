using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private Image progressBar;

        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }
        
        public void SetLoadingProgress(float value)
        {
            progressBar.fillAmount = value;
        }
    }
}
