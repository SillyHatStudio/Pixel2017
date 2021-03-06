﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
* List of Enums usable from any script
**/
public class EnumTypes
{
    //Time management
    public enum TimerTypeEnum
    {
        Countdown = 0, //Timer that starts from XX:XX ends at 00:00
        Normal = 1 // Timer that starts from 00:00
    }

    //Player Numbers
    public enum PlayerEnum
    {
        Unassigned = -1,
        P1 = 0,
        P2 = 1,
        P3 = 2,
        P4 = 3
    }

    //Player Colors
    public static class PlayerColors
    {
        public static Color ColorP1 = Color.white;
        public static Color ColorP2 = Color.black;
        public static Color ColorNone = Color.gray;
    }

    //PlayerControlTypes
    public enum PlayerControlKeys
    {
        P1_Up = KeyCode.W,
        P1_Down = KeyCode.S,
        P1_Left = KeyCode.A,
        P1_Right = KeyCode.D,
        P2_Up = KeyCode.UpArrow,
        P2_Down = KeyCode.DownArrow,
        P2_Left = KeyCode.LeftArrow,
        P2_Right = KeyCode.RightArrow,
        P3_Up = KeyCode.Y,
        P3_Down = KeyCode.H,
        P3_Left = KeyCode.G,
        P3_Right = KeyCode.J,
        P4_Up = KeyCode.O,
        P4_Down = KeyCode.L,
        P4_Left = KeyCode.K,
        P4_Right = KeyCode.Semicolon
    }

    //PlayerTypes
    public static class PlayerType
    {
        //public static int Pacman = 0;
        //public static int Ghost = 1;
    }

    [System.Serializable]
    public struct PlayerSpawnPoint
    {
        public Vector2 position;
        public Vector2 size;
        public int owner;
    }

    [System.Serializable]
    public struct WinZone
    {
        public Vector2 position;
        public AllowedPlayer playerAllowed;

        public enum AllowedPlayer
        {
            Any,
            P1,
            P2
        }
    }
}


