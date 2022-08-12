using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VB
{
  public class ScaleObject : MonoBehaviour
  {
    private void OnTriggerStay(Collider other)
    {
      if(other.tag == "Player")
      {

      }
    }
  }
}
