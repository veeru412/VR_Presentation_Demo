using UnityEngine;
using VB.UI;
namespace VB
{
  public class HandMovement : MonoBehaviour
  {
    public bool isLeftHand;
    private Transform _ref;
    private Transform mTransform;
    void Start()
    {
      VB.UI.Spawner sp = FindObjectOfType<VB.UI.Spawner>();
      if (sp == null)
        DestroyImmediate(gameObject);
      _ref = isLeftHand ? sp.lefHandTans : sp.rightHandTrans;
      mTransform = transform;
    }

    void Update()
    {
      if (Spawner.isSessionStarted)
      {
        mTransform.position = _ref.position;
        mTransform.rotation = _ref.rotation;
      }
      else
      {
        mTransform.position = Vector3.back * 100;
      }
    }
  }
}
