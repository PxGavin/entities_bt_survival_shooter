using EntitiesBT.Components;
using EntitiesBT.Core;
using EntitiesBT.DebugView;
using EntitiesBT.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace EntitiesBT.Game
{
    public struct EntityWithPosition
    {
        public Entity entity;
        public float3 position;
    }

    public class KeepAwayFromEnemy : BTNode<KeepAwayFromEnemyNode>
    {
        public float speed;
        public float distance;

        protected override void Build(ref KeepAwayFromEnemyNode data, BlobBuilder _, ITreeNode<INodeDataBuilder>[] __)
        {
            data.speed = speed;
            data.distance = distance;
        }
    }

    [Serializable]
    [BehaviorNode("F5C2EE7E-690A-4B5C-9489-FB362C949193")]
    public struct KeepAwayFromEnemyNode : INodeData
    {
        public float speed;
        public float distance;

        [ReadOnly(typeof(BehaviorTreeTickDeltaTime))]
        [ReadWrite(typeof(Translation))]
        [ReadOnly(typeof (ClosestEnemy))]
        public NodeState Tick(int index, INodeBlob blob, IBlackboard bb)
        {
            ref var translation = ref bb.GetDataRef<Translation>();
            var closestEnemy = bb.GetData<ClosestEnemy>();
            var deltaTime = bb.GetData<BehaviorTreeTickDeltaTime>();

            if (math.distance(translation.Value, closestEnemy.position) < distance)
            {
                float3 direction = math.normalize(translation.Value - closestEnemy.position);
                translation.Value += direction * speed * deltaTime.Value;
            }
            //translation.Value = math.lerp(translation.Value, closestEnemy.position, deltaTime.Value);
            return NodeState.Running;
        }

        public void Reset(int index, INodeBlob blob, IBlackboard blackboard)
        {
        }
    }

    public class KeepAwayFromEnemyDebugView : BTDebugView<KeepAwayFromEnemyNode> { }
}