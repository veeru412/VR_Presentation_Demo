using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T:Component
{
  public static T instance;
  public virtual void Awake()
  {
    if(instance != null)
    {
      Debug.LogWarning($"More that one instance of {typeof(T).Name} found.");
      DestroyImmediate(this.gameObject);
    }
    instance = this as T;
    DontDestroyOnLoad(this.gameObject);
  }
}
