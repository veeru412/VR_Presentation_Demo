
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
namespace VB.UI
{
  public class Key : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler
  {
    [SerializeField]
    TextMeshProUGUI txt;

    private KeyBoard board;
    private char _char;
    public void Initialize(char ch,KeyBoard _board)
    {
      txt.text = ch == ' '?"SPACE": ch.ToString();
      _char = ch;
      board = _board;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      board.UpdateString(_char);
      SfxManager.instance.PlayBtnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      SfxManager.instance.PlayBtnHover();
    }
  }
}
