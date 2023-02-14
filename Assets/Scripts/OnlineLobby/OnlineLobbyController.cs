using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Networking;
using System.Threading;
using System;
using System.Threading.Tasks;

public class OnlineLobbyController : MonoBehaviour
{
    Network network;
    Network.Room createRoomData;

    [SerializeField] private GameObject CreateRoom_UI, EnterCode_UI, PlayerNickname_UI;
    [SerializeField] private TMP_InputField nicknameInputField, roomTitleInputField;
    [SerializeField] private TextMeshProUGUI playerNickname_txt;
    [SerializeField] private List<Button> maxPlayerCountBtnList;

    [SerializeField] private GameObject roomTemplateGO;
    [SerializeField] private List<Room_Template> roomTemplates;
    [SerializeField] private Transform roomContent;

    public event Action reloadAction;
    public string userNickname;

    void Awake()
    {
        network= new Network();
        createRoomData= new Network.Room();

        // if(!PlayerNickname_UI.activeSelf)
        //      PlayerNickname_UI.SetActive(true);

    }

    // MainMenu�� ���ư���
    public async void BTN_CallBack()
    {
        if(userNickname != null)
        {
            Debug.Log("UserNickname: " + userNickname);
            network.DeletePlayer(userNickname);

            bool isExist = await network.GetisNNExist(userNickname);
            if (isExist)
            {
                Debug.LogError($"{userNickname} is not deleted!");
                return;
            }
        }
        SceneManager.LoadScene("MainMenu");
    }

    // �÷��̾� Post
    public async void BTN_CallPlayerNicknameCheck()
    {
        if (nicknameInputField.text == "")
        {
            // �г��� ���� �Ұ� �˾� ����ֱ� - TODO
            Debug.Log("Nickname InputField is Emty!");
            return;
        }
        
        // �̹� �����ϴ� �г������� üũ
        string user_nickname = nicknameInputField.text;
        bool isExist = await network.GetisNNExist(user_nickname);
        Debug.Log("isExist: " + isExist);

        if (isExist)
        {
            // �г��� ���� �Ұ� �˾� ����ֱ� - TODO
            Debug.Log("Nickname already Exists!");
            return;
        }
            
        network.PostNewPlayer(user_nickname);
        userNickname = user_nickname;
        playerNickname_txt.text = "User Name: " + nicknameInputField.text;
        PlayerNickname_UI.SetActive(false);

        BTN_CallLobbyReLoad();
    }

    // Room ����
    public async void BTN_CallCreateRoomEnter()
    {
        string roomTitle = roomTitleInputField.text;
        var newRoomcode = await network.PostNewRoom(createRoomData.player_num, roomTitle);
        createRoomData.code = newRoomcode;
        Debug.Log($"NewRoomCode: {newRoomcode}");

        BTN_CallLobbyReLoad();
        BTN_CallCloseParentUI();
    }

    // maxPlayerCount ������Ʈ �Լ�
    public void UpdateMaxPlayerCount(int playerMaxCount)
    {
        createRoomData.player_num = playerMaxCount;
        for (int i = 0; i < maxPlayerCountBtnList.Count; i++)
        {
            if(i == playerMaxCount - 3)
            {
                maxPlayerCountBtnList[i].image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                maxPlayerCountBtnList[i].image.color = new Color(1.0f, 1.0f, 1.0f, 0f);
            }
        }
    }

    // roomCode�� �̿��� Room�� �����ϴ� �Լ�
    public async void BTN_CallRoomCodeEnter()
    {
        TMP_InputField roomcodeInputField = EnterCode_UI.GetComponentInChildren<TMP_InputField>();

        // �� ���� üũ
        bool isRoomFull = await network.GetisRoomFull(roomcodeInputField.text);
        if(isRoomFull)
        {
            // �� �ο� ��á�ٴ� �˾� ���� - TODO
            Debug.Log("The Room is Full");
            return;
        }
        network.PutPlayerToRoom(userNickname, roomcodeInputField.text);

        var RoomInfo = await network.GetRoomInfo(roomcodeInputField.text);
    }

    public async void BTN_CallLobbyReLoad()
    {
        List<Network.Room> rooms = await network.GetRooms();

        // roomTemplateGO�� ���� room�� ������ ���� �ʵ��� �߰�
        while (roomTemplates.Count < rooms.Count)
        {
            GameObject newRoomTemplateGO = Instantiate(roomTemplateGO, roomContent);
            roomTemplates.Add(newRoomTemplateGO.GetComponent<Room_Template>());
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            roomTemplates[i].createRoomData = rooms[i];
        }
        reloadAction.Invoke();
    }

    public void BTN_CallCloseParentUI()
    {
        GameObject clickedGO = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"ClickedGO Name: {clickedGO.name}");
        clickedGO.transform.parent.gameObject.SetActive(false);
    }

    public void BTN_CallPopupUI(string value)
    {
        switch (value)
        {
            case "CreateRoom_UI":
                CreateRoom_UI.SetActive(true);
                break;
            case "EnterCode_UI":
                EnterCode_UI.SetActive(true);
                break;
        }
    }
}