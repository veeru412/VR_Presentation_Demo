using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VB.Util
{
  public class FollowTargetLook : MonoBehaviour
  {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform follower;
    [SerializeField]
    private Vector3 distanceOffset;
    [SerializeField]
    private float smoothTime;

    Vector3 vel;
    Vector3 targetPos;
    Vector3 camLastPos;
    void LateUpdate()
    {
      
      targetPos = target.TransformPoint(distanceOffset);

      targetPos = Vector3.Distance(targetPos, camLastPos) < 5.0f ? camLastPos : targetPos;
   
      targetPos.y = follower.position.y;
      follower.position = Vector3.SmoothDamp(follower.position, targetPos, ref vel, smoothTime);
      //lookPos = new Vector3(target.position.x, follower.position.y, target.position.z);
      //follower.LookAt(lookPos);
      follower.rotation = Quaternion.LookRotation(follower.position - target.position);

      camLastPos = targetPos;
    }
  }
}
