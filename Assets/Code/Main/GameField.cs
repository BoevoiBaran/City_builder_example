using Settings;
using UnityEngine;

namespace Code.Main
{
    public class GameField : MonoBehaviour
    {
        private const float Offset = 0.5f;

        [SerializeField] private Transform gameFieldRoot;

        public void Initialize(int sizeX, int sizeY)
        {
            CreateGameField(sizeX, sizeY);
        }

        private void CreateGameField(int sizeX, int sizeY)
        {
            var nodePrefab = Resources.Load<Node>($"{Constants.GameAssetPath}node");

            for (var indexX = 0; indexX < sizeX; indexX++)
            {
                for (var indexY = 0; indexY < sizeY; indexY++)
                {
                    var node = Instantiate(nodePrefab, gameFieldRoot);
                    var nodePosition = new Vector3(indexX + Offset, 0.0f, indexY + Offset);
                    node.SetupNode(nodePosition);
                }
            }
        }
    }
}
