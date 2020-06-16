/*using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool showGUI = true;

    private NetworkManager m_NetworkManager;

    [Header("Main Menu")] [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button buttonHost;
    [SerializeField] private Button buttonClient;
    [SerializeField] private Button buttonServer;
    [SerializeField] private InputField inputFieldIP;
    [SerializeField] public InputField insertName;
    [SerializeField] public Dropdown InputColor;

    [Header("Lobby Room")] [SerializeField] private GameObject lobbyRoom;
    [SerializeField] private Button buttonReady;
    [SerializeField] private Button buttonReturn;
    [SerializeField] private Text playersReady;


    [Header("In-Game HUD")]
    [SerializeField]
    private GameObject inGameHUD;

    [SerializeField] private Text textSpeed;
    [SerializeField] private Text textLaps;
    [SerializeField] public Text textPosition;

    private void Awake()
    {
        m_NetworkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        buttonHost.onClick.AddListener(() => StartHost());
        buttonClient.onClick.AddListener(() => StartClient());
        buttonServer.onClick.AddListener(() => StartServer());
        ActivateMainMenu();
    }

    public void UpdateSpeed(int speed)
    {
        textSpeed.text = "Speed " + speed + " Km/h";
    }

    public void UpdatePlayers(int players)
    {
        playersReady.text = "Players connected: " + players;
    }

    private void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
    }

    private void ReturnToMenu()
    {
        buttonReady.onClick.RemoveListener(() => ActivateInGameHUD());
        mainMenu.SetActive(true);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
    }


    private void ActivateLobbyRoomHost()
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartHost());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());
        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
    }

    private void ActivateLobbyRoomClient()
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartClient());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());
        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
    }

    private void ActivateLobbyRoomServer()
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartServer());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());
        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
    }

    private void ActivateInGameHUD()
    {
        mainMenu.SetActive(false);
        lobbyRoom.SetActive(false);
        inGameHUD.SetActive(true);
    }

    private void StartHost()
    {
        ActivateLobbyRoomHost();
    }

    private void StartClient()
    {

        m_NetworkManager.networkAddress = inputFieldIP.text;

        ActivateLobbyRoomClient();
    }

    private void StartServer()
    {
        ActivateLobbyRoomServer();
    }
}*/

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool showGUI = true;

    private NetworkManager m_NetworkManager;

    [Header("Main Menu")] [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button buttonHost;
    [SerializeField] private Button buttonClient;
    [SerializeField] private Button buttonServer;
    [SerializeField] private InputField inputFieldIP;
    [SerializeField] public InputField insertName;
    [SerializeField] public Dropdown InputColor;

    [Header("Lobby Room")] [SerializeField] private GameObject lobbyRoom;
    [SerializeField] private Button buttonReady;
    [SerializeField] private Button buttonReturn;
    [SerializeField] public Text playersReady;

    [Header("In-Game HUD")]
    [SerializeField]
    private GameObject inGameHUD;

    [SerializeField] private Text textSpeed;
    [SerializeField] private Text textLaps;
    [SerializeField] public Text textPosition;

    private void Awake()
    {
        m_NetworkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        buttonHost.onClick.AddListener(() => StartHost());
        buttonClient.onClick.AddListener(() => StartClient());
        buttonServer.onClick.AddListener(() => StartServer());
        ActivateMainMenu();
    }

    public void UpdateSpeed(int speed)
    {
        textSpeed.text = "Speed " + speed + " Km/h";
    }

    private void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
    }

    private void ReturnToMenu()
    {
        buttonReady.onClick.RemoveAllListeners();
        mainMenu.SetActive(true);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
    }

    private void ActivateLobbyRoomHost()
    {
        //buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartHost());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());

        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
    }
    

    private void ActivateLobbyRoomClient()
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartClient());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());
        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
    }

    private void ActivateLobbyRoomServer()
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartServer());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());
        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
    }

    private void ActivateInGameHUD()
    {
        mainMenu.SetActive(false);
        lobbyRoom.SetActive(false);
        inGameHUD.SetActive(true);
    }

    private void StartHost()
    {
        ActivateLobbyRoomHost();
    }

    private void StartClient()
    {

        m_NetworkManager.networkAddress = inputFieldIP.text;
        m_NetworkManager.StartClient();
        mainMenu.SetActive(false);
        inGameHUD.SetActive(true);
        lobbyRoom.SetActive(false);
    }

    private void StartServer()
    {
        ActivateLobbyRoomServer();
    }
}