using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;

public class PolePositionManager : NetworkBehaviour
{
    public int numPlayers;
    public NetworkManager networkManager;

    private readonly List<PlayerInfo> m_Players = new List<PlayerInfo>(4);
    private CircuitController m_CircuitController;
    private GameObject[] m_DebuggingSpheres;

    private GameObject[] m_Checkpoints = new GameObject[8];

    private void Awake()
    {
        if (networkManager == null) networkManager = FindObjectOfType<NetworkManager>();
        if (m_CircuitController == null) m_CircuitController = FindObjectOfType<CircuitController>();

        m_DebuggingSpheres = new GameObject[networkManager.maxConnections];
        for (int i = 0; i < networkManager.maxConnections; ++i)
        {
            m_DebuggingSpheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_DebuggingSpheres[i].GetComponent<SphereCollider>().enabled = false;
        }

        m_Checkpoints[0] = GameObject.Find("Waypoint 0");
        m_Checkpoints[1] = GameObject.Find("Waypoint 1");
        m_Checkpoints[2] = GameObject.Find("Waypoint 2");
        m_Checkpoints[3] = GameObject.Find("Waypoint 3");
        m_Checkpoints[4] = GameObject.Find("Waypoint 4");
        m_Checkpoints[5] = GameObject.Find("Waypoint 5");
        m_Checkpoints[6] = GameObject.Find("Waypoint 6");
        m_Checkpoints[7] = GameObject.Find("Waypoint 7");
    }

    private void Update()
    {
        if (m_Players.Count == 0)
            return;

        UpdateRaceProgress();
    }

    public Vector3 SpherePosition(int ID)
    {
        return m_DebuggingSpheres[ID].transform.position;
    }

    public Vector3 getCurrentWaypoint(int ID)
    {
        return m_Checkpoints[ID].transform.position;
    }


    public void AddPlayer(PlayerInfo player)
    {
        m_Players.Add(player);
        player.CurrentPosition = m_Players.Count - 1;
        numPlayers++;
    }

    public void DeletePlayer(PlayerInfo player)
    {
        m_Players.Remove(player);
        numPlayers--;
    }

    public void AllStartTrue()
    {
        foreach (var item in m_Players)
        {
            item.StartCar = true;
        }
    }

    private class PlayerInfoComparer : Comparer<PlayerInfo>
    {
        float[] m_ArcLengths;

        public PlayerInfoComparer(float[] arcLengths)
        {
            m_ArcLengths = arcLengths;
        }

        public override int Compare(PlayerInfo x, PlayerInfo y)
        {
            if (this.m_ArcLengths[x.CurrentPosition] < m_ArcLengths[y.CurrentPosition])
                return 1;
            else return -1;
        }
    }

    public void UpdateRaceProgress()
    {
        // Update car arc-lengths
        float[] arcLengths = new float[m_Players.Count];

        for (int i = 0; i < m_Players.Count; ++i)
        {
            arcLengths[i] = ComputeCarArcLength(i);
        }
        
        m_Players.Sort(new PlayerInfoComparer(arcLengths));

        for (int i = 0; i < m_Players.Count; ++i)
        {
            m_Players[i].CurrentPosition = i;
        }

        string myRaceOrder = "";
        foreach (var _player in m_Players)
        {
            myRaceOrder += _player.Name + " " + _player.CurrentPosition + " " + _player.ID;
        }

        //Debug.Log("El orden de carrera es: " + myRaceOrder);
    }

    public string UpdateUI()
    {
        string UpdatedPositions = "";
        foreach (var _player in m_Players)
        {
            UpdatedPositions += _player.Name + " " + _player.CurrentPosition + "º" + "\n";
        }
        return UpdatedPositions;
    }

    float ComputeCarArcLength(int ID)
    {
        // Compute the projection of the car position to the closest circuit 
        // path segment and accumulate the arc-length along of the car along
        // the circuit.
        Vector3 carPos = this.m_Players[ID].transform.position;

        int segIdx;
        float carDist;
        Vector3 carProj;

        float minArcL =
            this.m_CircuitController.ComputeClosestPointArcLength(carPos, out segIdx, out carProj, out carDist);

        this.m_DebuggingSpheres[ID].transform.position = carProj;

        if (this.m_Players[ID].CurrentLap == 0)
        {
            minArcL -= m_CircuitController.CircuitLength;
        }
        else
        {
            minArcL += m_CircuitController.CircuitLength *
                       (m_Players[ID].CurrentLap - 1);
        }

        return minArcL;
    }
}