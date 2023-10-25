using System.Collections.Generic;
using UnityEngine;

namespace IndividualGames.Pool
{
    /// <summary>
    /// Generic pooling of gameobjects.
    /// </summary>
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int poolCapacity = 30;

        private Queue<GameObject> pool = new();

        public GameObject Retrieve()
        {
            if (pool.Count == 0 || pool.Count < 10)
            {
                return Instantiate(prefab);
            }

            var go = pool.Dequeue();
            if (go == null)
            {
                go = Instantiate(prefab);
            }
            go.SetActive(true);
            return go;
        }

        public void ReturnToPool(GameObject go)
        {
            if (pool.Count >= poolCapacity)
            {
                Destroy(go);
            }

            go.SetActive(false);
            pool.Enqueue(go);
        }
    }
}