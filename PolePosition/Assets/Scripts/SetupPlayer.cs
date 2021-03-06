﻿using System;
using Mirror;
using UnityEngine;
using Random = System.Random;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class SetupPlayer : NetworkBehaviour
{
    [SyncVar] private int m_ID;
    [SyncVar] private string m_Name;
    [SyncVar] private Color m_Color;
    //[SyncVar] private int m_playersReady;


    private Color[] PLAYER_COLORS =
    {
        new Color(0.8F, 0.1F, 0.1F), //RED
        new Color(0.1F, 0.8F, 0.1F), //GREEN
        new Color(0.1F, 0.1F, 0.8F), //BLUE
        new Color(0.1F, 0.1F, 0.1F), //BLACK
        new Color(0.7F, 0.3F, 0.2F), //ORANGE
        new Color(0.8F, 0.8F, 0.8F), //WHITE
    };

    private UIManager m_UIManager;
    private NetworkManager m_NetworkManager;
    private PlayerController m_PlayerController;
    private PlayerInfo m_PlayerInfo;
    private PolePositionManager m_PolePositionManager;
    

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();
        m_ID = connectionToClient.connectionId;        
    }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
        m_PlayerInfo.ID = m_ID;
        m_PlayerInfo.CurrentLap = 0;
        m_PlayerInfo.Name = m_Name;
        m_PlayerInfo.Color = m_Color;

        GetComponentInChildren<Renderer>().materials[1].color = m_Color;
        m_PolePositionManager.AddPlayer(m_PlayerInfo);

    }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        CmdProvideName(m_UIManager.insertName.text);
        CmdProvideColor(m_UIManager.InputColor.value);
        CmdNumPlayers();
        if (m_PolePositionManager.numPlayers > 0)
        {
            CmdCanStartCar();
        }
    }

    [Command]
    void CmdCanStartCar()
    {
        RpcCanStartCar();
    }

    [Command]
    void CmdNumPlayers()
    {
        //Debug.Log("m_playersReady CMD" + m_playersReady);
        RpcNumPlayers();
    }

    [Command]
    void CmdProvideName(String name)
    {
        m_Name = name;
        RpcPlayerName(name);
        
    }

    [Command]
    void CmdProvideColor(int colorId)
    {
        m_Color = PLAYER_COLORS[colorId];
        RpcPlayerColor(colorId);
    }

    [ClientRpc]
    void RpcPlayerName(string name)
    {        
        m_PlayerInfo.Name = name;
        m_UIManager.textPosition.text += name + "\n";
        //CmdUpdateUI();
    }

    [ClientRpc]
    void RpcPlayerColor(int colorId)
    {
        m_PlayerInfo.Color = PLAYER_COLORS[colorId];
        GetComponentInChildren<Renderer>().materials[1].color = PLAYER_COLORS[colorId];
    }

    [ClientRpc]
    void RpcCanStartCar()
    {
        m_PolePositionManager.AllStartTrue();
    }

    [ClientRpc]
    void RpcNumPlayers()
    {
        m_UIManager.playersReady.text = "Players ready: " + m_PolePositionManager.numPlayers;
        if (m_PolePositionManager.numPlayers > 0)
        {
            m_UIManager.ActivateInGameHUD();
        }
        //Debug.Log("m_playersReady RPC" + m_playersReady);
    }

    #endregion

    private void Awake()
    {
        m_PlayerInfo = GetComponent<PlayerInfo>();
        m_PlayerController = GetComponent<PlayerController>();
        m_NetworkManager = FindObjectOfType<NetworkManager>();
        m_PolePositionManager = FindObjectOfType<PolePositionManager>();
        m_UIManager = FindObjectOfType<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            m_PlayerController.enabled = true;
            m_PlayerController.OnSpeedChangeEvent += OnSpeedChangeEventHandler;
            ConfigureCamera();
        }
    }


    void OnSpeedChangeEventHandler(float speed)
    {
        m_UIManager.UpdateSpeed((int)speed * 5); // 5 for visualization purpose (km/h)
    }

    void ConfigureCamera()
    {
        if (Camera.main != null) Camera.main.gameObject.GetComponent<CameraController>().m_Focus = this.gameObject;
    }

    private void Update()
    {
        m_UIManager.textPosition.text = m_PolePositionManager.UpdateUI();
        if (m_PlayerController.checkProblems())
        {
            Quaternion rot = new Quaternion(0.0f, m_PlayerInfo.transform.rotation.y, m_PlayerInfo.transform.rotation.z, m_PlayerInfo.transform.rotation.w);
            Vector3 pos;
            pos = m_PolePositionManager.SpherePosition(m_PlayerInfo.CurrentPosition);
            pos.y += 5.0f;
            m_PlayerInfo.transform.SetPositionAndRotation(pos, rot);
        }
    }
}