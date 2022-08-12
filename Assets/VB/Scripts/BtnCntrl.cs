using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnCntrl : MonoBehaviour
{
  [SerializeField]
  private Transform anim;
  [SerializeField] Renderer mesh;
  Coroutine old;

  private void Start()
  {
    mesh.material.color = Color.green;
  }
  private void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Player")
    {
      if (old != null)
        StopCoroutine(old);
      old = StartCoroutine(Animate(true));
      mesh.material.color = Color.cyan;
    }
  }
  private void OnTriggerExit(Collider other)
  {
    if(other.tag == "Player")
    {
      if (old != null)
        StopCoroutine(old);
      old = StartCoroutine(Animate(false));
      mesh.material.color = Color.green;
    }
  }

  IEnumerator Animate(bool _in)
  {
    Vector3 scale = anim.localScale;
    scale.y = _in ? 1.0f : 0.5f;
    if (_in)
    {
      while(scale.y > 0.5f)
      {
        scale.y -= 0.1f;
        anim.localScale = scale;
        yield return null;
      }
    }
    else
    {
      while (scale.y < 1)
      {
        scale.y += 0.1f;
        anim.localScale = scale;
        yield return null;
      }
    }
  }
}
