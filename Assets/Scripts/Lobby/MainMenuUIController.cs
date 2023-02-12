using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUIController : MonoBehaviour
{

    #region Singleton
    public static MainMenuUIController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] LobbyController lobbyController;

    [Header("Pannels")]
    [SerializeField] GameObject MenuButtonsPannel;
    [SerializeField] GameObject CreateLobbyPannel;
    [SerializeField] GameObject InLobbyPannel;
    [SerializeField] GameObject SetPlayerNamePannel;
    [SerializeField] GameObject LoadingPannel;
    [SerializeField] GameObject FindLobbyPannel;

    [Header("Create Lobby Pannel")]
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text maxPlayersText;
    private int maxPlayers;

    [Header("InLobby Pannel")]
    [SerializeField] TMP_Text LobbyName;
    [SerializeField] GameObject PlayerInLobbyPrefab;
    [SerializeField] GameObject PlayerShowArea;
    [SerializeField] Button StartButton;
    List<GameObject> playersInLobby = new List<GameObject>();

    [Header("Set-Player-Name-Pannel")]
    [SerializeField] TMP_InputField PlayerNameField;

    [Header("Loading-Pannel")]
    [SerializeField] TMP_Text LoadingText;
    [SerializeField] Button BackButton;

    [Header("Find-Lobby-Pannel")]
    [SerializeField] GameObject LobbiesShowArea;
    [SerializeField] GameObject LobbyInListPrefab;
    

    private void OffAllPannels()
    {
        MenuButtonsPannel.SetActive(false);
        CreateLobbyPannel.SetActive(false);
        InLobbyPannel.SetActive(false);
        LoadingPannel.SetActive(false);
        SetPlayerNamePannel.SetActive(false);
        FindLobbyPannel.SetActive(false);
    }

    #region OnPannels

    public void InLobbyPannelOn()
    {
        OffAllPannels();
        InLobbyPannel.SetActive(true);
    }
    public void FindLobbyPannelOn()
    {
        OffAllPannels();
        FindLobbyPannel.SetActive(true);
    }
    public void LoadingPannelOn()
    {
        OffAllPannels();
        LoadingPannel.SetActive(true);
    }

    public void MenuButtonsPannelOn()
    {
        OffAllPannels();
        MenuButtonsPannel.SetActive(true);
    }

    public void SetPlayerNamePannelOn()
    {
        OffAllPannels();
        SetPlayerNamePannel.SetActive(true);
    }

    #endregion

    #region InputPlayerName

    public void SetPlayerName()
    {
        if (PlayerNameField.text != null)
        {
            PlayerPrefs.SetString("PlayerName", PlayerNameField.text);
            MenuButtonsPannelOn();
        }
    }

    #endregion

    #region CreateLobbyPannel

    public string GetLobbyNameWhileCreating()
    {
        string name = null;

        if(nameInputField.text != null)
        {
            name = nameInputField.text;
        }

        return name;
    }

    public int GetMaxPlayers()
    {
        return (int) slider.value;
    }

    public void UpdateMaxPlayersText()
    {
        maxPlayersText.text = slider.value.ToString();
    }

    public void CreateLobyyPannelOn()
    {
        OffAllPannels();
        CreateLobbyPannel.SetActive(true);
    }

    #endregion

    #region InLobbyPannel

    private void AddPlayerInLobby()
    {
        GameObject i = Instantiate(PlayerInLobbyPrefab, PlayerShowArea.transform);
        playersInLobby.Add(i);
    }

    public void RefreshLobbyData(bool isHost)
    {
        
        LobbyName.text = "Lobby Name: " + lobbyController.GetLobbyName();
        
        foreach(GameObject i in playersInLobby)
        {
            Destroy(i);
        }

        playersInLobby.Clear();

        for (int i= 1; i <= lobbyController.GetLobbyPlayers().Count; i++)
        {
            AddPlayerInLobby();
        }
        
        for(int i = 0; i < playersInLobby.Count; i++)
        {
            string Name = lobbyController.GetLobbyPlayers()[i].Data["PlayerName"].Value;
            playersInLobby[i].GetComponent<InLobbyPlayer>().Name.text = Name;
            
        }

        if(isHost)
        {
            StartButton.gameObject.SetActive(true);
        }
        else
        {
            StartButton.gameObject.SetActive(false);
        }
    }

    public void LeaveLobby()
    {
        lobbyController.LeaveLobby();
        OffAllPannels();
        MenuButtonsPannel.SetActive(true);
    }
    
    #endregion

    #region LoadingPannel

    public void UpdateLoadingText(string msg)
    {
        LoadingText.text = msg;
    }

    private void ResetLoadingPannel()
    {
        LoadingText.text = "Loading...";
        BackButton.gameObject.SetActive(false);
    }

    public void BackButtonOn()
    {
        BackButton.gameObject.SetActive(true);
    }

    public void BackButtonCode()
    {
        MenuButtonsPannelOn();
        ResetLoadingPannel();
    }

    #endregion

    #region FindLobbyPannel
    
    public void AddLobbyInList(string LobbyName, int CurrentPlayers, int MaxPlayers, string LobbyId)
    {
        GameObject lobby = Instantiate(LobbyInListPrefab, LobbiesShowArea.transform);

        lobby.GetComponent<InListLobby>().Name.text = LobbyName;
        lobby.GetComponent<InListLobby>().Players.text = CurrentPlayers + "/" + MaxPlayers;

        lobby.GetComponent<InListLobby>().JoinButton.onClick.AddListener(() => {
            lobbyController.JoinLobbyById(LobbyId);
        });
    }

    #endregion
}
