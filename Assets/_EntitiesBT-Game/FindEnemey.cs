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
    public struct EntityWithPosition
    {
        public Entity entity;
        public float3 position;
    }

    public class FindEnemey : BTNode<FindEnemeyNode>
    {
        public Vector3 Velocity;//赋值 从外面给值

        protected override void Build(ref FindEnemeyNode data, BlobBuilder _, ITreeNode<INodeDataBuilder>[] __)
        {
            data.Velocity = Velocity;
        }
    }

    [Serializable]
    [BehaviorNode("F5C2EE7E-690A-4B5C-9489-FB362C949193")]
    public struct FindEnemeyNode : INodeData
    {
        public float3 Velocity;

        [ReadOnly(typeof(BehaviorTreeTickDeltaTime))]
        [ReadWrite(typeof(Translation))]
        [ReadOnly(typeof(PlayerTag))]
        public NodeState Tick(int index, INodeBlob blob, IBlackboard bb)
        {
            ref var translation = ref bb.GetDataRef<Translation>();
            var deltaTime = bb.GetData<BehaviorTreeTickDeltaTime>();
            translation.Value += Velocity * deltaTime.Value;
            return NodeState.Running;
        }

        public void Reset(int index, INodeBlob blob, IBlackboard blackboard)
        {
        }
    }

    public class EntityMoveDebugView : BTDebugView<FindEnemeyNode> { }
}