using EntitiesBT.Entities;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(VirtualMachineSystem))]
public class FindPlayerSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    EntityQuery m_PlayerQuery;

    struct PlayerInfo
    {
        public Entity entity;
        public float3 position;
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        m_PlayerQuery = GetEntityQuery(
            ComponentType.ReadOnly<Player>(),
            ComponentType.ReadOnly<Translation>()
            );
    }

    protected override void OnUpdate()
    {
        int playerCount = m_PlayerQuery.CalculateEntityCount();
        if (playerCount == 0)
        {
            return;
        }

        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

        var players = new NativeArray<PlayerInfo>(playerCount, Allocator.TempJob);

        // Get all Players, even only one.
        Entities
            .WithStoreEntityQueryInField(ref m_PlayerQuery)
            .ForEach((Entity entity,
                int entityInQueryIndex,
                ref Player enm,
                ref Translation trans) =>
            {
                players[entityInQueryIndex] = new PlayerInfo { 
                    entity = entity,
                    position = trans.Value
                };
            })
            .Schedule();

        // Set player infor to enemy
        Entities
            .ForEach((Entity entity,
                      int entityInQueryIndex,
                      ref Target target,
                      in Translation trans,
                      in Enemy enemy) =>
            {
                foreach (var player in players)
                {
                    target.entity = player.entity;
                    target.position = player.position;
                }
            })
            .WithDeallocateOnJobCompletion(players)
            .ScheduleParallel();
    }
}
