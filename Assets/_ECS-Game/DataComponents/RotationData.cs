using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct RotationData : IComponentData
{
    public float rotateSpeed;
}
