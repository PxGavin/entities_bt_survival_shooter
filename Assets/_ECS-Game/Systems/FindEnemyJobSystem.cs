using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FindEnemyJobSystem : JobComponentSystem
{
    private struct EntityWithPosition
    {
        public Entity entity;
        public float3 position;
    }

    [RequireComponentTag(typeof(PlayerTag))]
    [ExcludeComponent(typeof(HasTarget))]
    private struct FindTargetJob : IJobForEachWithEntity<Translation>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<EntityWithPosition> targetArray;
        public EntityCommandBuffer.Concurrent entityCommandBuffer;

        public void Execute(Entity entity, int index, ref Translation translation)
        {
            float3 unitPos = translation.Value;
            Entity closestTargetEntity = Entity.Null;
            float3 closestTargetPos = float3.zero;

            for(int i = 0; i< targetArray.Length; i++)
            {
                EntityWithPosition targetEntityWithPosition = targetArray[i];
                if(closestTargetEntity == Entity.Null)
                {
                    closestTargetEntity = targetEntityWithPosition.entity;
                    closestTargetPos = targetEntityWithPosition.position;
                }
                else
                {
                    if (math.distance(unitPos, targetEntityWithPosition.position) <
                        math.distance(unitPos, closestTargetPos))
                    {

                        closestTargetEntity = targetEntityWithPosition.entity;
                        closestTargetPos = targetEntityWithPosition.position;
                    }
                }
            }

            if(closestTargetEntity != Entity.Null)
            {
                entityCommandBuffer.AddComponent(index, entity, new HasTarget { targetEntity = closestTargetEntity });
            }
        }
    }

    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        base.OnCreate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityQuery targetQuery = GetEntityQuery(typeof(EnemyTag), ComponentType.ReadOnly<Translation>());
        NativeArray<Entity> targetEntityArray = targetQuery.ToEntityArray(Allocator.TempJob);
        NativeArray<Translation> targetTranslationArray = targetQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        NativeArray<EntityWithPosition> targetArray = new NativeArray<EntityWithPosition>(targetEntityArray.Length, Allocator.TempJob);

        for(int i = 0; i< targetEntityArray.Length; i++)
        {
            targetArray[i] = new EntityWithPosition
            {
                entity = targetEntityArray[i],
                position = targetTranslationArray[i].Value,
            };
        }

        targetEntityArray.Dispose();
        targetTranslationArray.Dispose();

        FindTargetJob findTargetJob = new FindTargetJob
        {
            targetArray = targetArray,
            entityCommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
        };

        JobHandle jobHandle = findTargetJob.Schedule(this, inputDeps);
        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
