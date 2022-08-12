using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;

namespace VB.UI
{
  public class Spawner : MonoBehaviour
  {
    [SerializeField]
    private Transform xrOrigin;
    [SerializeField]
    private Transform speakerPos;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject classRoom;
    [SerializeField]
    private LoginSceneUIManager canvasLobby;
    [SerializeField]
    private GameObject musicPlayer;
    [SerializeField]
    private InputManager inputManager;
    public Transform lefHandTans, rightHandTrans;

    private Dictionary<int,GameObject> allAvatars = new Dictionary<int, GameObject>();
    private GameObject avatarPrefab;
    public static bool isSessionStarted = false;

    PhotonView view;

    private void Start()
    {
      avatarPrefab = Resources.Load(Constants.avatarName) as GameObject;
      if (avatarPrefab == null)
        Debug.LogError("No avatar prefab in resourse folder.");
      isSessionStarted = false;
      classRoom.SetActive(false);

      view = PhotonView.Get(this);
    }
    private void OnPlayerIDSet()
    {
      view.RPC("RefreshPlayerList", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);
      Logger.Debug("Refreshing Player list");
    }
    [PunRPC]
    public void RefreshPlayerList(Player player)
    {
      int pCount = PhotonNetwork.PlayerList.Length;
      for (int i = 0; i < pCount; i++)
      {
        Player p = PhotonNetwork.PlayerList[i];
        int playerID = p.GetPlayerNumber();
        if (!allAvatars.ContainsKey(playerID) && playerID >= 0)
        {
          Vector3 spawnPos = p.IsMasterClient ? speakerPos.position : spawnPoints[playerID].position;
          Quaternion spawnRot = p.IsMasterClient ? speakerPos.rotation : spawnPoints[playerID].rotation;
          GameObject tempAvatar = PhotonNetwork.Instantiate(Constants.avatarName,spawnPos,spawnRot);
          tempAvatar.GetComponent<Avatar>().mPlayer = p;
          tempAvatar.transform.SetParent(classRoom.transform);
          allAvatars.Add(playerID, tempAvatar);
       
          PhotonNetwork.Instantiate(Constants.leftHand,Vector3.zero,Quaternion.identity);
          PhotonNetwork.Instantiate(Constants.rightHand,Vector3.zero,Quaternion.identity);    
        }
      }

      if (isSessionStarted)
      {
        view.RPC("StartSession", player);
      }
    }
 
   private void OnPresentationBtnClick()
    {
      if (!isSessionStarted)
      {
        view.RPC("StartSession", RpcTarget.All);

        isSessionStarted = true;
      }
    }
    [PunRPC]
    public void StartSession()
    {
      if (!classRoom.activeInHierarchy)
      {
        int myID = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        if(myID >= 0)
        {
          xrOrigin.position = Constants.isProffesor ? speakerPos.position+speakerPos.forward*0.5f : spawnPoints[myID].position+spawnPoints[myID].forward*0.5f;
          xrOrigin.rotation = Constants.isProffesor ? speakerPos.rotation : spawnPoints[myID].rotation;
          classRoom.SetActive(true);
          canvasLobby.gameObject.SetActive(false);
          musicPlayer.SetActive(false);
          inputManager.DisableRayInteraction();
        }
        isSessionStarted = true;
      }
    }
    [PunRPC]
    public void EndSession()
    {
      canvasLobby.ExitFromPhoton();
    }
    private void OnApplicationQuit()
    {
      if (PhotonNetwork.IsMasterClient)
      {
        view.RPC("EndSession", RpcTarget.All);
      }
    }
    private void OnEnable()
    {
      PlayerNumbering.OnPlayerNumberingChanged += OnPlayerIDSet;
      LoginSceneUIManager.onPresentationStart += OnPresentationBtnClick;
    }
    private void OnDisable()
    {
      PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerIDSet;
      LoginSceneUIManager.onPresentationStart -= OnPresentationBtnClick;
    }

  }
}
