using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Mission
{
    public int missionNum;
    public int roomNum;
    public string mission;
    public bool isSolved;

    public Mission(int missionNum, int roomNum, string mission, bool isSolved)
    {
        this.missionNum = missionNum;
        this.roomNum = roomNum;
        this.mission = mission;
        this.isSolved = isSolved;
    }

    public void missionSolved()
    {
        this.isSolved = true;
    }

    public int getMissionNum()
    {
        return missionNum;
    }

    public int getRoomNum()
    {
        return roomNum;
    }

    public string getMission()
    {
        return mission;
    }

    public bool getIsSolved()
    {
        return isSolved;
    }

    public static string getRoomName(int _missionNum)
    {
        if(_missionNum == 0) return "Lecture1";
        else if(_missionNum == 1) return "Lecture2";
        else if(_missionNum == 2) return "Lecture3";
        else if(_missionNum == 3) return "Lecture4";
        else if(_missionNum == 4) return "Lecture5";
        else if(_missionNum == 5) return "Lecture6";
        else if(6 <= _missionNum && _missionNum <= 8) return "Entrance";
        else if(9 <= _missionNum && _missionNum <= 10) return "Spawn";
        else if(11 <= _missionNum && _missionNum <= 12) return "StudyRoom";
        else if(_missionNum == 13 || _missionNum == 26) return "WomanToilet";
        else if(_missionNum == 14) return "ManToilet";
        else if(15 <= _missionNum && _missionNum <= 17) return "Store";
        else if(18 <= _missionNum && _missionNum <= 20) return "Kitchen";
        else if(21 <= _missionNum && _missionNum <= 23) return "DiningRoom";
        else if(24 <= _missionNum && _missionNum <= 25) return "Garden";
        else return "";
    }
}
