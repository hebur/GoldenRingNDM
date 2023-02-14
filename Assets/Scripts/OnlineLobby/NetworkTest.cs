/**
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NetworkTest : MonoBehaviour
{
    async void Start()
    {
        string player1 = "Apple";
        string player2 = "Banana";
        string player3 = "Chat";
        string player4 = "Dino";
        string player5 = "Eggsome";

        Debug.Log("Start");
        Network nw = new Network();

        Debug.Log("GetisNNExist Test");
        bool isExist = await nw.GetisNNExist(player1);
        Debug.Log("isExist ¤¡");
        Debug.Log(isExist);
        Debug.Log("GetisNNExist Done");

        Thread.Sleep(3000);

        Debug.Log("PostNewPlayer Test");
        nw.PostNewPlayer(player1);
        Debug.Log("PostNewPlayer Done");

        Thread.Sleep(3000);

        Debug.Log("PostNewRoom Test");
        var newRoomcode = await nw.PostNewRoom(4, "title");
        Debug.Log("newRoomcode ¤¡");
        Debug.Log(newRoomcode);
        Debug.Log("PostNewRoom Done");

        Thread.Sleep(3000);

        Debug.Log("GetisRoomFull Test");
        var isFull = await nw.GetisRoomFull(newRoomcode);
        Debug.Log("isFull ¤¡");
        Debug.Log(isFull);
        Debug.Log("GetisRoomFull Done");

        Thread.Sleep(3000);

        Debug.Log("PutPlayertoRoom Test");
        nw.PutPlayerToRoom(newRoomcode, player1);
        var roominfo1 = await nw.GetRoomInfo(newRoomcode);
        Debug.Log("RoomInfo1 ¤¡");
        Debug.Log(roominfo1);

        Thread.Sleep(3000);

        nw.PostNewPlayer(player2);
        nw.PostNewPlayer(player3);
        nw.PostNewPlayer(player4);

        Thread.Sleep(3000);

        nw.PutPlayerToRoom(newRoomcode, player2);
        nw.PutPlayerToRoom(newRoomcode, player3);
        nw.PutPlayerToRoom(newRoomcode, player4);

        Thread.Sleep(3000);

        var roominfo2 = await nw.GetRoomInfo(newRoomcode);
        Debug.Log("RoomInfo2 ¤¡");
        Debug.Log(roominfo2);
        Debug.Log("PutPlayertoRoom Done");

        Thread.Sleep(3000);

        Debug.Log("GetisRoomFull Test");
        bool isFull2 = await nw.GetisRoomFull(newRoomcode);
        Debug.Log("isFull2 ¤¡");
        Debug.Log(isFull2);
        Debug.Log("GetisRoomFull Done");

        Thread.Sleep(3000);

        nw.PostNewPlayer(player5);
        nw.PutPlayerToRoom(newRoomcode, player5);

        Thread.Sleep(3000);

        Debug.Log("PutTurnEnd Test");
        nw.PutTurnEnd(newRoomcode);
        Debug.Log("PutTurnEnd Done");

        Thread.Sleep(3000);

        Debug.Log("GetTurnInfo Test");
        int turninfo = await nw.GetTurninfo(newRoomcode);
        Debug.Log("turninfo ¤¡");
        Debug.Log(turninfo);
        Debug.Log("GetTurnInfo Done");

        Thread.Sleep(3000);

        int act = 0, card_num = 1401;
        Debug.Log("PutCtoP Test");
        nw.PutCardToPlayer(player1, act, card_num, newRoomcode);
        Debug.Log("PutCtoP Done");

        Thread.Sleep(3000);

        //string newRoomcode = "REYAHO";

        Debug.Log("GetRooms Test");
        var rooms = await nw.GetRooms();
        Debug.Log("rooms ¤¡");
        Debug.Log(rooms);
        Debug.Log("GetRooms Done");

        Thread.Sleep(3000);

        Debug.Log("GetPlayerInfo Test");
        var p_info1 = await nw.GetPlayerInfo(player1);
        Debug.Log("palyer info1 ¤¡");
        Debug.Log(p_info1);
        Debug.Log("GetPlayerInfo Done");

        Thread.Sleep(3000);

        Debug.Log("DeletePlayer Test");
        nw.DeletePlayer(player1);
        var rooms2 = await nw.GetRooms();
        Debug.Log(rooms2);
        Debug.Log("DeletePlayer Done");

        Thread.Sleep(3000);

        Debug.Log("DeleteRoom Test");
        nw.DeleteRoom(newRoomcode);
        var rooms3 = await nw.GetRooms();
        Debug.Log(rooms3);
        Debug.Log("DeleteRoom Done");
    }
}
**/