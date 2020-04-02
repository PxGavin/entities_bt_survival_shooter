using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CameraFollowPlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity playerEntity, ref Player player) =>
        {
            if (CameraFollow.instance.playerEntity == Entity.Null)
                CameraFollow.instance.playerEntity = playerEntity;
        });
        
    } 
}
