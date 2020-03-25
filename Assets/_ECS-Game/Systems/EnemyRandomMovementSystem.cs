using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class EnemyRandomMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        float3 targetDir = float3.zero;
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.
            WithNone<HasTarget>().
            ForEach((Entity entity, ref Translation translation, ref SpeedData speedData,ref RandomPosData randomPosData, ref EnemyTag enemyTag) =>
        {
            if (randomPosData.targetPos.Equals(float3.zero))
            {
                randomPosData.targetPos = new float3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            }

            randomPosData.time += deltaTime;
            if(randomPosData.time > 1)
            {
                randomPosData.time = 0;
                randomPosData.targetPos = new float3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)) * 40;
            }

            targetDir = math.normalize(randomPosData.targetPos - translation.Value);
            translation.Value += targetDir * speedData.speed * 5 * deltaTime;            
        });
    }
}