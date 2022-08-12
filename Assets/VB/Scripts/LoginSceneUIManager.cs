using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;
using ExitGames.Client.Photon;

namespace VB.UI
{
  public class LoginSceneUIManager : MonoBehaviourPunCallbacks
  {
    public int minPlayersToStart = 2;
    [SerializeField]
    private Button loginBtn;
    [SerializeField]
    private Toggle proffesorSwitch;
    [SerializeField]
    private Button createRoomBtn;
    [SerializeField]
    private Button startPresentationBtn;

    [Space(20)]
    [SerializeField]
    private InputField PlayerNameInput;
    [SerializeField]
    private GameObject RoomListEntryPrefab;
    [SerializeField]
    private GameObject RoomListContent;
    [SerializeField]
    private GameObject playerListContent;
    [SerializeField]
    private InputField RoomNameInputField;
    [SerializeField]
    private InputField MaxPlayersInputField;
    [SerializeField]
    private GameObject PlayerListEntryPrefab;

    [Header("Menu Objects")]
    [SerializeField]
    private GameObject loginMenu;
    [SerializeField]
    private GameObject createRoomMenu;
    [SerializeField]
    private GameObject roomListMenu;
    [SerializeField]
    private GameObject insideRoomMenu;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    public delegate void PresetationStart();
    public static event PresetationStart onPresentationStart;
   

    private void Awake()
    {
      cachedRoomList = new Dictionary<string, RoomInfo>();
      roomListEntries = new Dictionary<string, GameObject>();

      PhotonNetwork.AutomaticallySyncScene = true;

      PlayerNameInput.text = "Player " + Random.Range(1000, 10000);
      loginBtn.onClick.AddListener(OnLoginBtnClick);
      proffesorSwitch.onValueChanged.AddListener(SetProfessorMode);
      createRoomBtn.onClick.AddListener(OnCreateRoomButtonClicked);
      startPresentationBtn.onClick.AddListener(LoadPresentationScene);
    }
    private void Start()
    {
      SetActivePanel(loginMenu.name);
    }
    private void OnLoginBtnClick()
    {
      string playerName = PlayerNameInput.text;

      if (!playerName.Equals(""))
      {
        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
        loginBtn.interactable = false;
      }
      else
      {
        Debug.LogError("Player Name is invalid.");
      }
    }
    public void OnCreateRoomButtonClicked()
    {
      string roomName = RoomNameInputField.text;
      roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

      byte maxPlayers;
      byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
      maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

      RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

      PhotonNetwork.CreateRoom(roomName, options, null);
      createRoomBtn.interactable = false;
    }
    public void SetProfessorMode(bool isProffesor)
    {
      Constants.isProffesor = isProffesor;
    }
    private void SetActivePanel(string activePanel)
    {
      loginMenu.SetActive(activePanel.Equals(loginMenu.name));
      createRoomMenu.SetActive(activePanel.Equals(createRoomMenu.name));
      //JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
      roomListMenu.SetActive(activePanel.Equals(roomListMenu.name));  
      insideRoomMenu.SetActive(activePanel.Equals(insideRoomMenu.name));
    }
    public void ExitFromPhoton()
    {
      PhotonNetwork.Disconnect();
      loginBtn.interactable = true;
      createRoomBtn.interactable = true;
      startPresentationBtn.interactable = true;
      SetActivePanel(loginMenu.name);
    }
    private bool CheckPlayersReady()
    {
      if (!PhotonNetwork.IsMasterClient)
      {
        return false;
      }
      return PhotonNetwork.PlayerList.Length >= minPlayersToStart;
    }
    private void ClearRoomListView()
    {
      foreach (GameObject entry in roomListEntries.Values)
      {
        Destroy(entry.gameObject);
      }

      roomListEntries.Clear();
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
      foreach (RoomInfo info in roomList)
      {
        // Remove room from cached room list if it got closed, became invisible or was marked as removed
        if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
        {
          if (cachedRoomList.ContainsKey(info.Name))
          {
            cachedRoomList.Remove(info.Name);
          }

          continue;
        }

        // Update cached room info
        if (cachedRoomList.ContainsKey(info.Name))
        {
          cachedRoomList[info.Name] = info;
        }
        // Add new room info to cache
        else
        {
          cachedRoomList.Add(info.Name, info);
        }
      }
    }
    private void UpdateRoomListView()
    {
      foreach (RoomInfo info in cachedRoomList.Values)
      {
        GameObject entry = Instantiate(RoomListEntryPrefab);
        entry.transform.SetParent(RoomListContent.transform);
        entry.transform.localScale = Vector3.one;
        entry.transform.localPosition = Vector3.zero;
        entry.transform.localRotation = Quaternion.identity;

        entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

        roomListEntries.Add(info.Name, entry);
      }
    }
    private void LoadPresentationScene()
    {
      Logger.Debug("Start Presentation Button Clicked..");
      //if (PhotonNetwork.IsMasterClient)   
      //  PhotonNetwork.LoadLevel(Constants.presentationScene);
      SetActivePanel("hema");
      onPresentationStart?.Invoke();
      startPresentationBtn.interactable = false;
    }
    #region PUN CALLBACKS
    public override void OnConnected()
    {
      base.OnConnected();
      Logger.Debug("Connected to photon network....");

      if (Constants.isProffesor)
        SetActivePanel(createRoomMenu.name);
      else
        SetActivePanel(roomListMenu.name); 
    }
    public override void OnConnectedToMaster()
    {
      if (!PhotonNetwork.InLobby)
        PhotonNetwork.JoinLobby();
      Logger.Debug("Connected to master server ...");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
      //SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
      //SetActivePanel(SelectionPanel.name);
    }
   
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
      ClearRoomListView();

      UpdateCachedRoomList(roomList);
      UpdateRoomListView();
    }
    public override void OnJoinedLobby()
    {
      // whenever this joins a new lobby, clear any previous room lists
      cachedRoomList.Clear();
      ClearRoomListView();
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
      cachedRoomList.Clear();
      //ClearRoomListView();
    }
    public override void OnJoinedRoom()
    {
      // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
      cachedRoomList.Clear();

      SetActivePanel(insideRoomMenu.name);

      if (playerListEntries == null)
      {
        playerListEntries = new Dictionary<int, GameObject>();
      }

      foreach (Player p in PhotonNetwork.PlayerList)
      {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(playerListContent.transform);
        entry.transform.localScale = Vector3.one;
        entry.transform.localPosition = Vector3.zero;
        entry.transform.localRotation = Quaternion.identity;
        entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);
        entry.GetComponent<PlayerListEntry>().SetPlayerReady(true);       

        playerListEntries.Add(p.ActorNumber, entry);
      }
      if(!PhotonNetwork.IsMasterClient && Constants.isProffesor)
      {
        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
      }
      startPresentationBtn.gameObject.SetActive(CheckPlayersReady());

      Hashtable props = new Hashtable
            {
                {AsteroidsGame.PLAYER_LOADED_LEVEL, false}
            };
      PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnLeftRoom()
    {
      //SetActivePanel(SelectionPanel.name);

      //foreach (GameObject entry in playerListEntries.Values)
      //{
      //  Destroy(entry.gameObject);
      //}

      //playerListEntries.Clear();
      //playerListEntries = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
      GameObject entry = Instantiate(PlayerListEntryPrefab);
      entry.transform.SetParent(playerListContent.transform);
      entry.transform.localScale = Vector3.one;
      entry.transform.localPosition = Vector3.zero;
      entry.transform.localRotation = Quaternion.identity;
      entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

      playerListEntries.Add(newPlayer.ActorNumber, entry);

      startPresentationBtn.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      //Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
      //playerListEntries.Remove(otherPlayer.ActorNumber);

      //StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
      //if (playerListEntries == null)
      //{
      //  playerListEntries = new Dictionary<int, GameObject>();
      //}

      //GameObject entry;
      //if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
      //{
      //  object isPlayerReady;
      //  if (changedProps.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
      //  {
      //    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
      //  }
      //}

      //StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
   
    #endregion
  }
}
