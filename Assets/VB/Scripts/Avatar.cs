
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using UnityEngine;
namespace VB
{
  public class Avatar : MonoBehaviour
  {
    public GameObject mute;
    public Player mPlayer;
    PhotonView view;
    public PhotonVoiceView voice;

    private void Start()
    {
      view = PhotonView.Get(this);
    }
    public void OnSelect()
    {
      bool _mute = mute.activeInHierarchy;
      mute.SetActive(!_mute);
      view.RPC("MuteMe", mPlayer,_mute);
    }
    [PunRPC]
    public void MuteMe(bool mute)
    {
      voice.enabled = mute;
    }
  }
}
