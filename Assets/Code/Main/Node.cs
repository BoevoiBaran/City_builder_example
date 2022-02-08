using Settings;
using UnityEngine;

namespace Code.Main
{
    public class Node : MonoBehaviour
    {
        private Transform _selfTransform;

        private void Awake()
        {
            _selfTransform = GetComponent<Transform>();
        }

        public void SetupNode(Vector3 nodePosition)
        {
            _selfTransform.position = nodePosition;
        }
    }
}
