using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Room_Template : MonoBehaviour
{
    Network network;

    [SerializeField] Button RoomEnter_Btn;
    [SerializeField] TextMeshProUGUI RoomTitle_Txt;

    public Network.Room createRoomData;
    private string userNickname;

    void Start()
    {
        network = new Network();
        createRoomData = new Network.Room();
        userNickname = FindObjectOfType<OnlineLobbyController>().userNickname;
        OnlineLobbyController lobbyController = FindObjectOfType<OnlineLobbyController>();

        lobbyController.reloadAction -= updateRoomTemplate;
        lobbyController.reloadAction += updateRoomTemplate;
        
        //// ÀÓ½Ã °ª ºÎ¿©
        //createRoomData.code = "DAZUGA";
    }

    public void updateRoomTemplate()
    {
        if (createRoomData.title == null)
        {
            gameObject.SetActive(false);
            return;
        }

        RoomTitle_Txt.text = createRoomData.title;
        Debug.Log(createRoomData.title + createRoomData.code);
    }

    public async void BTN_CallDirectRoomEnter()
    {
        Debug.Log($"roomCode is {createRoomData.code}");
        // ·ë ¸¸¿ø Ã¼Å©
        bool isRoomFull = await network.GetisRoomFull(createRoomData.code);
        if (isRoomFull)
        {
            // ·ë ÀÎ¿ø ´ÙÃ¡´Ù´Â ÆË¾÷ ¶ç¿ì±â - TODO
            Debug.Log("The Room is Full");
            return;
        }
        network.PutPlayerToRoom(userNickname, createRoomData.code);

        var RoomInfo = await network.GetRoomInfo(createRoomData.code);
    }

    public void BTN_CallCloseParentUI()
    {
        GameObject clickedGO = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"ClickedGO Name: {clickedGO.name}");
        clickedGO.transform.parent.gameObject.SetActive(false);
    }
}
