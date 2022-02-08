using Settings;
using UnityEngine;

namespace Code.Main
{
    [RequireComponent(typeof(Renderer))]
    public class Storage : MonoBehaviour
    {
        public string InstanceId { get; private set; }
        
        private Renderer _nodeRenderer;
        private Transform _selfTransform;
        
        private MaterialPropertyBlock _regularColorProperty;

        public void SetData(string instanceId, Color regularColor)
        {
            InstanceId = instanceId;
            
            _regularColorProperty = new MaterialPropertyBlock();
            _regularColorProperty.SetColor(Constants.ColorProperty, regularColor);
            
            _nodeRenderer = GetComponent<Renderer>();
        }
    
        public void SetColor()
        {
            _nodeRenderer.SetPropertyBlock(_regularColorProperty);   
        }
    }
}
