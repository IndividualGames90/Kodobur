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
        [SerializeField] private int poolCapacity = 20;

        private Queue<GameObject> pool = new();

        public GameObject Retrieve()
        {
            if (pool.Count == 0)
            {
                return Instantiate(prefab);
            }

            var go = pool.Dequeue();
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