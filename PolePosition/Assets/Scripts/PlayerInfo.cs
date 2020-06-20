﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string Name { get; set; }

    public int ID { get; set; }
    
    public Color Color { get; set; }

    public int CurrentPosition { get; set; }

    public int CurrentLap { get; set; }

    public float raceTime { get; set; }

    //public bool StartCar = false; //{ get; set; }
    public bool StartCar { get; set; }

    public bool CanWin = true;

    public override string ToString()
    {
        return Name;
    }
}