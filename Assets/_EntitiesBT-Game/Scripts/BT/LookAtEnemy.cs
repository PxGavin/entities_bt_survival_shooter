using EntitiesBT.Components;
using EntitiesBT.Core;
using EntitiesBT.DebugView;
using EntitiesBT.Entities;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace EntitiesBT.Game
{
    public class LookAtEnemy : BTNode<LookAtEnemyNode>
    {
        public float speed;

        protected override void Build(ref LookAtEnemyNode data, BlobBuilder _, ITreeNode<INodeDataBuilder>[] __)
        {
            data.speed = speed;
        }
    }

    [Serializable]
    [BehaviorNode("F5C2EE7E-690A-4B5C-9489-FB362C949194")]
    public struct LookAtEnemyNode : INodeData
    {
        public float speed;

        [ReadWrite(typeof(Rotation))]
        [ReadOnly(typeof(BehaviorTreeTickDeltaTime))]
        [ReadOnly(typeof(Translation))]
        [ReadOnly(typeof (ClosestEnemy))]
        public NodeState Tick(int index, INodeBlob blob, IBlackboard bb)
        {
            ref var rotation = ref bb.GetDataRef<Rotation>();
            var translation = bb.GetDataRef<Translation>();
            var closestEnemy = bb.GetData<ClosestEnemy>();
            var deltaTime = bb.GetData<BehaviorTreeTickDeltaTime>();

            float3 direction = math.normalize(closestEnemy.position - translation.Value);
            Quaternion targetRot = Quaternion.LookRotation(direction);
            rotation.Value = Quaternion.RotateTowards(rotation.Value, targetRot, speed * deltaTime.Value);

            return NodeState.Running;
        }

        public void Reset(int index, INodeBlob blob, IBlackboard blackboard)
        {
        }
    }

    public class LookAtEnemyDebugView : BTDebugView<LookAtEnemyNode> { }
}