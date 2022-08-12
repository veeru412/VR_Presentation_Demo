
using UnityEngine;
using UnityEngine.EventSystems;
namespace VB {
  public class BtnSfxHandler : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
  {

    public void OnPointerDown(PointerEventData eventData)
    {
      SfxManager.instance.PlayBtnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      SfxManager.instance.PlayBtnHover();
    }
  }
}
