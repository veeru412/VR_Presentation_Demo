using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace VB.UI
{
  [RequireComponent(typeof(InputField))]
  public class InputFieldFocusor : MonoBehaviour,ISelectHandler
  {
    InputField field;
    public delegate void OnInpuFieldActive(InputField field,bool isSelect);
    public static event OnInpuFieldActive onInputFieldActive;
    private void Start()
    {
      field = GetComponent<InputField>();

      field.onEndEdit.AddListener(delegate { IsSelected(false); });
    }
    TouchScreenKeyboard keyBoard;
    private void IsSelected(bool isSelected)
    {
      Logger.Debug("Input field gots selected " + isSelected);
      //if (keyBoard != null)
      //  keyBoard.active = false;
      //onInputFieldActive?.Invoke(field, false);
    }

    public void OnSelect(BaseEventData eventData)
    {
      Logger.Debug("Input field gots selected true");
      // keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
#if !UNITY_EDITOR
      onInputFieldActive?.Invoke(field,true);
#endif
    }
  }
}
