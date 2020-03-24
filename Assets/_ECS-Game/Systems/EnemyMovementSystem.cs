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

        Entities.ForEach((ref Translation translation, ref Rotation rot, ref SpeedData speedData, ref EnemyTag enemyTag) =>
        {
            translation.Value += math.forward(rot.Value) * speedData.speed * deltaTime;
        });

        //EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //Entities.ForEach((Entity entity, ref Translation translation, ref SpeedData speedData, ref EnemyTag enemyTag, ref HasTarget hasTarget) =>
        //{
        //    if (entityManager.Exists(hasTarget.targetEntity))
        //    {
        //        Translation targetTranslation = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(hasTarget.targetEntity);
        //        float3 targetDir = math.normalize(targetTranslation.Value - translation.Value);
        //        translation.Value += targetDir * speedData.speed * deltaTime;

        //        if (math.distance(translation.Value, targetTranslation.Value) < .2f)
        //        {
        //            // Close to target, destroy it
        //            //PostUpdateCommands.DestroyEntity(hasTarget.targetEntity);
        //            //PostUpdateCommands.RemoveComponent(entity, typeof(HasTarget));
        //        }
        //    }
        //    else
        //    {
        //        // Target Entity already destroyed
        //        //PostUpdateCommands.RemoveComponent(entity, typeof(HasTarget));
        //    }
        //});
    }
}