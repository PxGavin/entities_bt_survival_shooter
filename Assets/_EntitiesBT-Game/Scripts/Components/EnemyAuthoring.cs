using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public struct Enemy : IComponentData
{

}

public struct Target : IComponentData
{
    public Entity entity;
    public float3 position;
}

//public struct EnemyMovement : IComponentData
//{
//    public NavMeshAgent nav;
//}

public class EnemyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Enemy());
        dstManager.AddComponentData(entity, new Target());

        //EnemyMovement enemyMovement = new EnemyMovement
        //{
        //    nav = GetComponent<NavMeshAgent>(),
        //};
        //dstManager.AddComponentData(entity, enemyMovement);
    }
}