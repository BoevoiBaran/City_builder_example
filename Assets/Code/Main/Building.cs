using Code.Main.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Main
{
    public class Building : MonoBehaviour
    {
        [FormerlySerializedAs("recoverableResource")] [SerializeField] private ResourceType recoverableResource;
        [SerializeField] private int  recoverableResourceCount;
        [SerializeField] private int  recoverableResourceAdditionalLimit;
        
        
        private Transform _selfTransform;
        
        private void Awake()
        {
            _selfTransform = GetComponent<Transform>();
        }

        public void SetBuildingPosition(Vector3 nodePosition)
        {
            _selfTransform.position = nodePosition;
        }
    }
}
