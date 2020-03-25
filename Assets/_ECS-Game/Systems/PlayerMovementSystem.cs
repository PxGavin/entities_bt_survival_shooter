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

        EntityQuery playerQuery = GetEntityQuery(typeof(Entity), typeof(Translation), typeof(SpeedData), typeof(PlayerTag), typeof(HasTarget));

        if (playerQuery.CalculateEntityCount() == 0) return;

        Entity playerEntity = playerQuery.ToEntityArray(Allocator.TempJob)[0];
        Translation playerTranslation = playerQuery.ToComponentDataArray<Translation>(Allocator.TempJob)[0];
        HasTarget hasTarget = playerQuery.ToComponentDataArray<HasTarget>(Allocator.TempJob)[0];
        SpeedData speedData = playerQuery.ToComponentDataArray<SpeedData>(Allocator.TempJob)[0];

        float deltaTime = Time.DeltaTime;
        if (entityManager.Exists(hasTarget.targetEntity))
        {
            Translation targetTranslation = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(hasTarget.targetEntity);
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
        //float3 playerInput = new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Entities.ForEach((Entity entity, ref Translation translation, ref SpeedData speedData, ref PlayerTag playerTag) =>
        //{
        //    float3 pos = translation.Value;
        //    pos += playerInput * speedData.speed * deltaTime;

        //    translation.Value = pos;
        //});
    }
}