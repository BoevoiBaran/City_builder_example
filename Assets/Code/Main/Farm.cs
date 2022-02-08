using Code.Main.Data;
using Settings;
using UnityEngine;

namespace Code.Main
{
    [RequireComponent(typeof(Renderer))]
    public class Farm : MonoBehaviour
    {
        public string InstanceId { get; private set; }
        
        private Renderer _nodeRenderer;

        private MaterialPropertyBlock _collectProperty;
        private MaterialPropertyBlock _miningProperty;
        
        public void SetData(string id, Color miningColor, Color collectColor)
        {
            InstanceId = id;
            
            _collectProperty = new MaterialPropertyBlock();
            _collectProperty.SetColor(Constants.ColorProperty, collectColor);

            _miningProperty = new MaterialPropertyBlock();
            _miningProperty.SetColor(Constants.ColorProperty, miningColor);
            
            _nodeRenderer = GetComponent<Renderer>();
        }
        
        public void SetCanCollectState()
        {
            _nodeRenderer.SetPropertyBlock(_collectProperty);   
        }

        public void SetMiningState()
        {
            _nodeRenderer.SetPropertyBlock(_miningProperty);   
        }
    }
}

