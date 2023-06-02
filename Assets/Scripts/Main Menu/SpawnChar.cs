using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpawnChar : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn; // Assign your GameObjects in the Inspector
    private string[] types = {"Nothing", "rock", "paper", "scissors", "flag", "decoy"};

    void Start()
    {
        SpawnRandom();
    }

    private void SpawnRandom()
    {
        // Select a random GameObject from the array
        GameObject selectedObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

        // Instantiate it at the position of this script's GameObject
        GameObject spawnedObject = Instantiate(selectedObject, transform.position, Quaternion.identity, transform);

        // Get the UnitFX component and call ChangeType with a random type
        UnitFX unit = spawnedObject.GetComponent<UnitFX>();

        // Get the SkeletonAnimation component and set the sorting layer and order
        SkeletonAnimation skeletonAnimation = spawnedObject.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null)
        {
            skeletonAnimation.GetComponent<MeshRenderer>().sortingLayerName = "UI";
            skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = 6;
        }



        if (unit != null)
        {
            spawnedObject.transform.localScale = new Vector3(200f,200f,200f);
            string randomType = types[Random.Range(0, types.Length)];
            unit.ChangeType(randomType, true);
        }
    }
}

