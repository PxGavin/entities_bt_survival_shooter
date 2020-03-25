using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct RandomPosData : IComponentData
{
    public float time;
    public float3 targetPos;
}
