using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Settings;
using UnityEngine;

namespace Code.Main
{
    public class Builder : MonoBehaviour
    {
        private readonly Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

        public void Build(string buildingId, Transform root, Vector3 position, Action<GameObject> cb)
        {
            BuildAsync(buildingId, root, position, cb).Forget();
        }

        private async UniTaskVoid BuildAsync(string buildingId, Transform root, Vector3 position, Action<GameObject> cb)
        {
            await BuildInternal(buildingId, root, position, cb);
        }

        private async UniTask BuildInternal(string buildingId, Transform root, Vector3 position, Action<GameObject> cb)
        {
            if (_cache.ContainsKey(buildingId))
            {
                cb?.Invoke(InstantiateBuilding(_cache[buildingId], root, position));
                return;
            }
            
            var assetRequest = await Resources.LoadAsync($"{Constants.GameAssetPath}{buildingId}") as GameObject;

            if (assetRequest is null)
            {
                Debug.LogError($"[Builder] Failed to load {buildingId}");
                
                return;
            }
            
            var prefab = assetRequest.gameObject;
            AddToCache(buildingId, prefab);
            cb?.Invoke(InstantiateBuilding(prefab, root, position));
        }

        private GameObject InstantiateBuilding(GameObject prefab, Transform root, Vector3 position)
        {
            var buildingObject = Instantiate(prefab, root);
            buildingObject.transform.position = position;
            
            return buildingObject;
        }
        
        private void AddToCache(string key, GameObject prefab)
        {
            _cache[key] = prefab;
        }
    }
}
