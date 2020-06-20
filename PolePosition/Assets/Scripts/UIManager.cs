using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool showGUI = true;
    private int nextPos = 1;

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

    [Header("Score Room")] [SerializeField] private GameObject scoreRoom;
    [SerializeField] private Button buttontest;
    [SerializeField] private Text score;

    [Header("In-Game HUD")]
    [SerializeField]
    private GameObject inGameHUD;

    [SerializeField] private Text textSpeed;
    [SerializeField] private Text textLaps;
    [SerializeField] public Text textCountdown;
    [SerializeField] public Text textPosition;
    [SerializeField] private Text EndingMessage;
    [SerializeField] public Text checkPath;

    private void Awake()
    {
        m_NetworkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        inputFieldIP.text = "localhost";
        buttonHost.onClick.AddListener(() => StartHost());          //LLama a las funciones que activan el lobby de cada usuario
        buttonClient.onClick.AddListener(() => StartClient());
        buttonServer.onClick.AddListener(() => StartServer());
        ActivateMainMenu();
    }

    public void UpdateSpeed(int speed)
    {
        textSpeed.text = "Speed " + speed + " Km/h";
    }

    public void UpdatePlayers(int players)  //Actualiza el texto "players connected: " del lobbyRoom
    {
        playersReady.text = "Players connected: " + players;
    }

    public void ShowScore(String jug, float t)      //Actualiza la tabla de puntuación dentro de la interfaz de ScoreRoom
    {
        score.text += nextPos + "º " + jug + "   :       " + t + " seg \n";
        nextPos++;
    }

    private void DestroyButtons()
    {
        buttonReady.enabled = false;
        buttonReturn.enabled = false;
    }

    public void GoToScore()         //Activa la interfaz de puntuaciones
    { 
        buttontest.onClick.AddListener(() => ActivateMainMenu());
        buttontest.onClick.AddListener(() => m_NetworkManager.StopClient());
        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
        scoreRoom.SetActive(true);
    }

    /*public void PlayerEnded()   
    {
        EndingMessage.enabled = true;
        textSpeed.enabled = false;
        textLaps.enabled = false;
        textPosition.enabled = false;
    }*/

    private void ActivateMainMenu()     //Activa interfaz del menu principal
    {
        buttonReady.enabled = true;
        buttonReturn.enabled = true;
        mainMenu.SetActive(true);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
        scoreRoom.SetActive(false);
    }

    private void ReturnToMenu()     
    {
        buttonReady.onClick.RemoveAllListeners();
        mainMenu.SetActive(true);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(false);
        scoreRoom.SetActive(false);
    }

    private void ActivateLobbyRoomHost()        //Activa el lobby del host
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartHost());
        buttonReady.onClick.AddListener(() => DestroyButtons());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());

        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
        scoreRoom.SetActive(false);
    }

    private void ActivateLobbyRoomClient()      //Activa el lobby del cliente
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartClient());
        buttonReady.onClick.AddListener(() => DestroyButtons());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());

        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
        scoreRoom.SetActive(false);
    }

    private void ActivateLobbyRoomServer()      //Activa el lobby del sRoomServer
    {
        buttonReady.onClick.AddListener(() => ActivateInGameHUD());
        buttonReady.onClick.AddListener(() => m_NetworkManager.StartServer());
        buttonReady.onClick.AddListener(() => DestroyButtons());
        buttonReturn.onClick.AddListener(() => ReturnToMenu());

        mainMenu.SetActive(false);
        inGameHUD.SetActive(false);
        lobbyRoom.SetActive(true);
        scoreRoom.SetActive(false);
    }

    public void ActivateInGameHUD()             //Activa la interfaz ingame
    {
        mainMenu.SetActive(false);
        lobbyRoom.SetActive(false);
        inGameHUD.SetActive(true);
        //scoreRoom.SetActive(false);
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
}