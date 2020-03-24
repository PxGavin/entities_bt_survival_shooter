using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class PlayerShootingSystem : JobComponentSystem
{
    private struct PlayerShootingJob : IJobParallelFor
    {
        [ReadOnly] public Entity entity;
        public EntityCommandBuffer.Concurrent entityCommandBuffer;
        public bool isFiring;

        public void Execute(int index)
        {
            if (!isFiring) return;
            entityCommandBuffer.AddComponent(index, entity, new Firing());
        }
    }

    private struct Data
    {
        public readonly int Length;
        public Entity[] Entities;
        public ComponentDataFromEntity<Weapon>[] Weapons;
        //public SubtractiveComponent<Firing> Firings;
    }

    //[Inject] private Data data;
    //[Inject] private PlayerShootingBarrier barrier;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //return new PlayerShootingJob
        //{
        //    entity = data.Entities,
        //    entityCommandBuffer = barrier.CreateCommandBuffer(),
        //    isFiring = Input.GetButton("Fire1"),
        //}.Schedule(data.Length, 64, inputDeps);

        return new PlayerShootingJob { }.Schedule(0, 64, inputDeps);
    }
}

//public class PlayerShootingBarrier: BarrierSystem
//{

//}
