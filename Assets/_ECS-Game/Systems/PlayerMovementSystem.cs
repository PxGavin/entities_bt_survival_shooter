using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class PlayerMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        float deltaTime = Time.DeltaTime;
        Entities.ForEach((Entity playerEntity, ref Translation playerTranslation, ref SpeedData speedData, ref PlayerTag playerTag,ref HasTarget hasTarget) =>
        {
            if(CameraFollow.instance.playerEntity == Entity.Null)
                CameraFollow.instance.playerEntity = playerEntity;

            if (entityManager.Exists(hasTarget.targetEntity))
            {
                Translation targetTranslation = entityManager.GetComponentData<Translation>(hasTarget.targetEntity);
                float3 targetDir = math.normalize(targetTranslation.Value - playerTranslation.Value);
                playerTranslation.Value += targetDir * speedData.speed * deltaTime;

                if (math.distance(playerTranslation.Value, targetTranslation.Value) < .2f)
                {
                    // Close to target, destroy it
                    PostUpdateCommands.DestroyEntity(hasTarget.targetEntity);
                    PostUpdateCommands.RemoveComponent(playerEntity, typeof(HasTarget));
                }
            }
            else
            {
                // Target Entity already destroyed
                PostUpdateCommands.RemoveComponent(playerEntity, typeof(HasTarget));
            }
        });
    }
}