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
        Entities.ForEach((Entity entity, ref Translation translation, ref SpeedData speedData, ref PlayerTag playerTag, ref HasTarget hasTarget) =>
        {
            if (entityManager.Exists(hasTarget.targetEntity))
            {
                Translation targetTranslation = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(hasTarget.targetEntity);
                float3 targetDir = math.normalize(targetTranslation.Value - translation.Value);
                translation.Value += targetDir * speedData.speed * deltaTime;

                if (math.distance(translation.Value, targetTranslation.Value) < .2f)
                {
                    // Close to target, destroy it
                    PostUpdateCommands.DestroyEntity(hasTarget.targetEntity);
                    PostUpdateCommands.RemoveComponent(entity, typeof(HasTarget));
                }
            }
            else
            {
                // Target Entity already destroyed
                PostUpdateCommands.RemoveComponent(entity, typeof(HasTarget));
            }
        });
        //float3 playerInput = new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Entities.ForEach((Entity entity, ref Translation translation, ref SpeedData speedData, ref PlayerTag playerTag) =>
        //{
        //    float3 pos = translation.Value;
        //    pos += playerInput * speedData.speed * deltaTime;

        //    translation.Value = pos;
        //});
    }
}