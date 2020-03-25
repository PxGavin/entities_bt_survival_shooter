using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class EnemyMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float3 targetDir = float3.zero;
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.ForEach((Entity entity, ref Translation translation, ref SpeedData speedData, ref EnemyTag enemyTag, ref HasTarget hasTarget) =>
        {
            if (entityManager.Exists(hasTarget.targetEntity))
            {
                Translation targetTranslation = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(hasTarget.targetEntity);
                targetDir = math.normalize(targetTranslation.Value - translation.Value);
                translation.Value -= targetDir * speedData.speed * deltaTime;
            }
            else
            {
                if (targetDir.Equals(float3.zero))
                {
                    float3 targetPos = new float3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
                    targetDir = math.normalize(targetPos - translation.Value);
                }

                translation.Value += targetDir * speedData.speed * deltaTime;
            }
        });
    }
}