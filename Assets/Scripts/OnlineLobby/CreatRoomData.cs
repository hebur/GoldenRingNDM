using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomData
{
    public string roomCode;
    public string roomTitle;
    public int maxPlayerCount = 4;
    public int turninfo;

    public List<int> deck;
    public List<Player> players;
}