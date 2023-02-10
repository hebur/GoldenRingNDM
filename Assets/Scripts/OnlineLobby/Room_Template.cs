using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Room_Template : MonoBehaviour
{
    [SerializeField] Button RoomEnter_Btn;
    [SerializeField] TextMeshProUGUI RoomTitle_Txt;

    CreateRoomData createRoomData;
    Network network;

    private string userNickname;

    void Start()
    {
        network = new Network();
        userNickname = FindObjectOfType<OnlineLobbyController>().userNickname;
    }

    public async void BTN_CallDirectRoomEnter()
    {
        // ·ë ¸¸¿ø Ã¼Å©
        bool isRoomFull = await network.GetisRoomFull(createRoomData.roomCode);
        if (isRoomFull)
        {
            // ·ë ÀÎ¿ø ´ÙÃ¡´Ù´Â ÆË¾÷ ¶ç¿ì±â - TODO
            Debug.Log("The Room is Full");
            return;
        }
        network.PutPlayerToRoom(userNickname, createRoomData.roomCode);

        var RoomInfo = await network.GetRoomInfo(createRoomData.roomCode);
        Debug.Log(RoomInfo);
    }

    public void BTN_CallCloseParentUI()
    {
        GameObject clickedGO = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"ClickedGO Name: {clickedGO.name}");
        clickedGO.transform.parent.gameObject.SetActive(false);
    }
}
