using UnityEngine;
// using UnityEngine.InputSystem;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    // Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool PlayerReady;

    private CharacterController2D player_input;

    private CustomNetworkManager manager;

    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartLocalPlayer()
    {
        player_input = GetComponent<CharacterController2D>();
        player_input.enabled = true;
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer(this.gameObject);
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayerName)
    {
        PlayerNameUpdate(this.PlayerName, PlayerName);
    }

    [Command]
    private void CMDSetPlayerReady()
    {
        PlayerReadyUpdate(PlayerReady, !PlayerReady);
    }

    public void ChangeReady()
    {
        if (isOwned)
        {
            CMDSetPlayerReady();
        }
    }

    public void PlayerNameUpdate(string OldValue, string NewValue)
    {
        if (isServer)
        {
            PlayerName = NewValue;
        }
        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    private void PlayerReadyUpdate(bool OldValue, bool NewValue)
    {
        if (isServer)
        {
            PlayerReady = NewValue;
        }
        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }
    
    public void CanStartGame(string SceneName)
    {
        if(isOwned)
        {
            CMDCanStartGame(SceneName);
        }
    }

    [Command]
    public void CMDCanStartGame(string SceneName)
    {
        Manager.StartGame(SceneName);
    }
}
