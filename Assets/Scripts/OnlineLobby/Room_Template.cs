using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Room_Template : MonoBehaviour
{
    Network network;

    [SerializeField] Button RoomEnter_Btn;
    [SerializeField] TextMeshProUGUI RoomTitle_Txt;

    Network.Room createRoomData;
    private string userNickname;

    void Start()
    {
        network = new Network();

        createRoomData = new Network.Room();
        userNickname = FindObjectOfType<OnlineLobbyController>().userNickname;

        // ÀÓ½Ã °ª ºÎ¿©
        createRoomData.code = "DAZUGA";
    }

    public async void BTN_CallDirectRoomEnter()
    {
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
