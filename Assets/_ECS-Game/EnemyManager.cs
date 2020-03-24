using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy; 
    public float spawnTime = 3f;
    public Transform spawnPoint;

    private Entity entityFromPrefab;
    private EntityManager entityManager;

    private void Start()
    {
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        entityFromPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemy, settings);

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        InvokeRepeating("Spawn", 0, spawnTime);
    }

    void Spawn()
    {
        // If the player has no health left...
        //if (playerHealth.currentHealth <= 0f)
        //{
        //    return;
        //}                

        var instance = entityManager.Instantiate(entityFromPrefab);
        
        entityManager.SetComponentData(instance, new Translation { Value = spawnPoint.position });
        entityManager.SetComponentData(instance, new Rotation { Value = spawnPoint.rotation });
        entityManager.SetName(instance, "ZomBunny");

    }
}
