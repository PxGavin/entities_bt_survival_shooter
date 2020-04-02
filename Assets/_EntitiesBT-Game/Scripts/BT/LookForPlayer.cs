using EntitiesBT.Components;
using EntitiesBT.Core;
using EntitiesBT.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.AI;

namespace EntitiesBT.Game
{
    public class LookForPlayer : BTNode<LookForPlayerNode>
    {
        public float speed;

        protected override void Build(ref LookForPlayerNode data, BlobBuilder _, ITreeNode<INodeDataBuilder>[] __)
        {
            data.speed = speed;
        }
    }

    [Serializable]
    [BehaviorNode("F5C2EE7E-690A-4B5C-9489-FB362C949196")]
    public struct LookForPlayerNode : INodeData
    {
        public float speed;

        //[ReadWrite(typeof(EnemyMovement))]
        [ReadWrite(typeof(Translation))]
        [ReadOnly(typeof(Target))]
        [ReadOnly(typeof(BehaviorTreeTickDeltaTime))]
        public NodeState Tick(int index, INodeBlob blob, IBlackboard bb)
        {
            //ref var enemyMovement = ref bb.GetDataRef<EnemyMovement>();
            ref var translation = ref bb.GetDataRef<Translation>();
            var target = bb.GetData<Target>();
            var deltaTime = bb.GetData<BehaviorTreeTickDeltaTime>();
            //enemyMovement.nav.destination = target.position;
            //translation.Value = enemyMovement.nav.nextPosition;

            if(math.distance(target.position,translation.Value) > 2)
            {
                float3 direction = math.normalize(target.position - translation.Value);
                translation.Value += direction * speed * deltaTime.Value;
            }

            return NodeState.Running;
        }

        public void Reset(int index, INodeBlob blob, IBlackboard blackboard)
        {
        }
    }
}