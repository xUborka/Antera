using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class SteamLobby : MonoBehaviour
{

    protected static SteamLobby s_instance;
    public static SteamLobby Instance
    {
        get
        {
                return s_instance;
        }
    }

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<LobbyChatUpdate_t> LobbyChatUpdate;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEnter;

    protected Callback<LobbyMatchList_t> LobbyList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;

    public List<CSteamID> lobbyIDs = new List<CSteamID>();

    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";

    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager == null)
            {
                manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
            return manager;
        }
    }

    private void Awake()
    {
        if (Instance == null) { s_instance = this; }
        if (!SteamManager.Initialized) { return; }

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    private void OnDestroy(){
        LobbyCreated.Unregister();
        LobbyChatUpdate.Unregister();
        JoinRequest.Unregister();
        LobbyEnter.Unregister();
        LobbyList.Unregister();
        LobbyDataUpdated.Unregister();
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, Manager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }
        Manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName().ToString() + "'s Lobby");
        // SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "game_type", "low_polly_stuff");
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
    {

    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        // Everyone
        CurrentLobbyID = callback.m_ulSteamIDLobby;

        // Client
        if (NetworkServer.active) { return; }

        Manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        Manager.StartClient();
    }

    public void JoinLobby(CSteamID lobbyID)
    {
        SteamMatchmaking.JoinLobby(lobbyID);
    }

    public void GetLobbiesList()
    {
        if (lobbyIDs.Count > 0) { lobbyIDs.Clear(); }

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
        // SteamMatchmaking.AddRequestLobbyListStringFilter("game_type", "low_polly_stuff", ELobbyComparison.k_ELobbyComparisonEqual);
        SteamMatchmaking.RequestLobbyList();
    }

    void OnGetLobbyList(LobbyMatchList_t result)
    {
        if (LobbiesListManager.instance.listOfLobbies.Count > 0) { LobbiesListManager.instance.DestroyLobbies(); }

        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    }

    void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbiesListManager.instance.DisplayLobbies(lobbyIDs, result);
    }

}
