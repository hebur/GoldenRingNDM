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

        // �ӽ� �� �ο�
        createRoomData.code = "DAZUGA";
    }

    public async void BTN_CallDirectRoomEnter()
    {
        // �� ���� üũ
        bool isRoomFull = await network.GetisRoomFull(createRoomData.code);
        if (isRoomFull)
        {
            // �� �ο� ��á�ٴ� �˾� ���� - TODO
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
