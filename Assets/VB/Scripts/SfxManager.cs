using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VB
{
  [RequireComponent(typeof(AudioSource))]
  public class SfxManager : SingletonBehavior<SfxManager>
  {
    [SerializeField]
    AudioClip hover;
    [SerializeField]
    AudioClip click;

    AudioSource mSource;

    public override void Awake()
    {
      base.Awake();
      mSource = GetComponent<AudioSource>();
    }
    public void PlayBtnHover()
    {
      mSource.clip = hover;
      mSource.Play();
    }
    public void PlayBtnClick()
    {
      mSource.clip = click;
      mSource.Play();
    }
  }
}
