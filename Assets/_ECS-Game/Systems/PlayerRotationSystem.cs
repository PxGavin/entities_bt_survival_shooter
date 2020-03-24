using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerRotationSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        //var mousePosition = Input.mousePosition;
        //var cameraRay = Camera.main.ScreenPointToRay(mousePosition);
        //var layerMask = LayerMask.GetMask("Floor");

        //if(Physics.Raycast(cameraRay, out RaycastHit hit, 100, layerMask))
        //{
        //    Entities.ForEach((Entity entity, ref Rotation rotation,
        //        ref Translation translation,ref RotationData rotationComponent) =>
        //    {
        //        var forward = hit.point - (Vector3)translation.Value;
        //        var rot = Quaternion.LookRotation(forward);

        //        rotation.Value = new Quaternion(0, rot.y, 0, rot.w).normalized;
        //    });
        //}
    }
}
