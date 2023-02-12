using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    #region Singleton
    public static LobbyController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Lobby hostLobby;

    private float HeartbeatTimer = 0;
    private float UpdatePollTimer = 0;

    private bool LoadedScene = false;

    
    private async void Start()
    {

        MainMenuUIController.instance.LoadingPannelOn();
       
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Player Signedin with id: " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            MainMenuUIController.instance.MenuButtonsPannelOn();
        }

        else
        {
            MainMenuUIController.instance.SetPlayerNamePannelOn();
        }
    }
    
    private void Update()
    {
        HandleLobbyHeartBeat();
        UpdateLobbyPoll();
    }

    public async void CreateLobby()
    {
        try
        {
            if (MainMenuUIController.instance.GetLobbyNameWhileCreating() != null)
            {
                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = GetPlayer(),
                    Data = new Dictionary<string, DataObject>
                    {
                        {"RelayCode", new DataObject(DataObject.VisibilityOptions.Member, "0") }
                    }
                };

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(MainMenuUIController.instance.GetLobbyNameWhileCreating(),
                    MainMenuUIController.instance.GetMaxPlayers(), createLobbyOptions);

                hostLobby = lobby;

                MainMenuUIController.instance.InLobbyPannelOn();
                MainMenuUIController.instance.RefreshLobbyData(isLobbyHost());

                Debug.Log("Lobby Created");
                Debug.Log("Name: " + lobby.Name + "  Max Players: " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);

            MainMenuUIController.instance.UpdateLoadingText(e.Reason.ToString());
            MainMenuUIController.instance.BackButtonOn();
        }
    }

    public async void LeaveLobby()
    {
        if (hostLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(hostLobby.Id, AuthenticationService.Instance.PlayerId);
                Debug.Log("Left Lobby: " + hostLobby.Name);

                hostLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);

                MainMenuUIController.instance.UpdateLoadingText(e.Reason.ToString());
                MainMenuUIController.instance.BackButtonOn();
            }
        }
    }

    private async void HandleLobbyHeartBeat()
    {
        if(hostLobby != null && hostLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            HeartbeatTimer -= Time.deltaTime;

            if(HeartbeatTimer <= 0)
            {
                float ResetTimer = 15f;
                HeartbeatTimer = ResetTimer;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void UpdateLobbyPoll()
    {
        if (hostLobby != null)
        {
            UpdatePollTimer -= Time.deltaTime;

            if (UpdatePollTimer <= 0)
            {
                float ResetTimer = 1f;
                UpdatePollTimer = ResetTimer;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(hostLobby.Id);
                hostLobby = lobby;
                MainMenuUIController.instance.RefreshLobbyData(isLobbyHost());

                CheckStart();
            }
        }
    }

    private void CheckStart()
    {
        if (!LoadedScene && hostLobby.Data["RelayCode"].Value != "0")
        {
            SceneManage.instance.LoadGameScene();
            LoadedScene = true;
        }
    }
    
    public string GetRelayCode()
    {
        Debug.Log("RelayCodeInDic: " + hostLobby.Data["RelayCode"].Value);
        return hostLobby.Data["RelayCode"].Value;
    }

    public List<Player> GetLobbyPlayers()
    {
        return hostLobby.Players;
    }

    public string GetLobbyName()
    {
        return hostLobby.Name;
    }
    

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "1", QueryFilter.OpOptions.GE)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Total Found Lobbies: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                MainMenuUIController.instance.AddLobbyInList(lobby.Name, lobby.Players.Count, lobby.MaxPlayers, lobby.Id);
                Debug.Log("Name: " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id);
            }
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);

            MainMenuUIController.instance.UpdateLoadingText(e.Reason.ToString());
            MainMenuUIController.instance.BackButtonOn();
        }
    }
    
    public async void JoinLobbyById(string LobbyId)
    {
        try
        {
            MainMenuUIController.instance.LoadingPannelOn();

            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(LobbyId, joinLobbyByIdOptions);

            hostLobby = lobby;

            MainMenuUIController.instance.InLobbyPannelOn();
        }

        catch (LobbyServiceException e)
        {
            Debug.Log(e);

            MainMenuUIController.instance.UpdateLoadingText(e.Reason.ToString());
            MainMenuUIController.instance.BackButtonOn();
        }
    }


    public async void QuickJoin()
    {
        try
        {
            MainMenuUIController.instance.LoadingPannelOn();

            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
            hostLobby = lobby;

            MainMenuUIController.instance.InLobbyPannelOn();
            MainMenuUIController.instance.RefreshLobbyData(isLobbyHost());
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);

            MainMenuUIController.instance.UpdateLoadingText(e.Reason.ToString());
            MainMenuUIController.instance.BackButtonOn();
        }
    }

    private Player GetPlayer()
    {
        Player player = new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, PlayerPrefs.GetString("PlayerName"))}
            }
        };

        return player;
    }

    public void StartGame()
    {
        if(isLobbyHost())
        {
            try
            {
                Debug.Log("Start-Game");

                SceneManage.instance.LoadGameScene();
                LoadedScene = true;
                
            }
            catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
    
    public async void UpdateRelayCode(string code)
    {
        try
        {
           
            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"RelayCode", new DataObject(DataObject.VisibilityOptions.Member, code) }
                }
            });

            hostLobby = lobby;
            
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }


    public bool isLobbyHost()
    {
        return hostLobby.HostId == AuthenticationService.Instance.PlayerId;
    }






    [Command]
    private void GetPlayerLobbyCount()
    {
        Debug.Log(GetLobbyPlayers().Count);
    }
}
