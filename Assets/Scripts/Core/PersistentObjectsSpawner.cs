using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectsPrefab;

        static bool hasSpawned = false;

        private void Awake() 
        { 
            if(hasSpawned)
            {
                return;
            }
            SpawnPersistenObjects();
            hasSpawned = true;
        }

        private void SpawnPersistenObjects()
        {
                GameObject persistentObject = Instantiate(persistentObjectsPrefab);

                DontDestroyOnLoad(persistentObject);
        }


    }
}