using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Extensions
{
    public static class Extensions
    {
        public static void SetSafeActive(this GameObject gameObject, bool active)
        {
            if (gameObject is null)
            {
                return;
            }

            if (gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
        
        public static GameObject AddNestedObjectFromResources(this Transform transform, string resourcesPath)
        {
            GameObject prefab = Resources.Load(resourcesPath) as GameObject;

            if (prefab == null)
            {
                Debug.LogError("Can't find prefab in Resources " + resourcesPath);
                throw new NullReferenceException("Can't find prefab");
            }

            GameObject obj = Object.Instantiate(prefab, transform, false);
            obj.transform.localPosition = Vector3.zero;

            return obj;
        }
    }
}
