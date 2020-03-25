using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public Entity playerEntity = Entity.Null;
    public float3 offset;

    private EntityManager manager;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if (playerEntity == null) { return; }

        Translation trans = manager.GetComponentData<Translation>(playerEntity);
        transform.position = trans.Value + offset;
    }
}
