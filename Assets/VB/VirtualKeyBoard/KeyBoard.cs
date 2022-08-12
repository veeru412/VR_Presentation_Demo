
using UnityEngine;
using UnityEngine.UI;
namespace VB.UI
{
  public class KeyBoard : MonoBehaviour
  {
    char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    [SerializeField]
    private Key _key;
    [SerializeField]
    private Key spaceKey;
    [SerializeField]
    private Key backSpace;
    [SerializeField]
    RectTransform lettersParent;
    [SerializeField]
    RectTransform numbersParent;
    [SerializeField]
    GameObject virtualKeyBoard;

    private InputField activeField;

    private void Awake()
    {
      virtualKeyBoard.SetActive(false);
#if UNITY_EDITOR
      gameObject.SetActive(false);
#else
Initialize();
#endif
    }
    private void Initialize()
    {
      for (int i = 0; i < letters.Length; i++)
      {
        Instantiate(_key,lettersParent).Initialize(letters[i],this);
      }
      for (int i = 0; i < numbers.Length; i++)
      {
        Instantiate(_key, numbersParent).Initialize(numbers[i],this);
      }
      Instantiate(spaceKey, lettersParent).Initialize(' ',this);
      Instantiate(backSpace, numbersParent).Initialize('<', this);
    }
    public void UpdateString(char ch)
    {
      if(ch == '<')
      {
        if(_value.Length > 0)   
          _value.Remove(_value.Length - 1);
        
      }else
        _value += ch;
      activeField.text = _value;
    }
    private void EnableKeyBoard(InputField field,bool isSelected)
    {
      _value = string.Empty;
      virtualKeyBoard.SetActive(isSelected);
      activeField = isSelected ? field : null;
    }
    string _value;
    private void OnEnable() => InputFieldFocusor.onInputFieldActive += EnableKeyBoard;
    
    private void OnDisable() => InputFieldFocusor.onInputFieldActive -= EnableKeyBoard;
   

  }
}
