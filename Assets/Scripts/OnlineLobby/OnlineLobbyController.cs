using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class OnlineLobbyController : MonoBehaviour
{
    Network network;

    [SerializeField] private GameObject CreateRoom_UI, EnterCode_UI, PlayerNickname_UI;
    [SerializeField] private TMP_InputField nicknameInputField;

    void Start()
    {
        network= new Network();
    }

    public void BTN_CallBack()
    {
        SceneManager.LoadScene(1);
    }

    public async void BTN_CallPlayerNicknameCheck()
    {
        if (nicknameInputField.text == "")
        {
            // �˾� ����ֱ� TODO
            Debug.Log("Nickname InputField is Emty!");
            return;
        }

        // �̹� �����ϴ� �г������� üũ
        string user_nickname = nicknameInputField.text;
        var isExist = await network.GetisNNExist(user_nickname);

        if (isExist)
        {
            // �˾� ����ֱ� TODO
            Debug.Log("Nickname already Exists!");
            return;
        }
            
        network.PostNewPlayer(user_nickname);
        PlayerNickname_UI.SetActive(false);

        // �г����׽�Ʈ
        // nicknameTest1

    }

    public void BTN_CallCreateRoomEnter()
    {

    }

    public void BTN_CallRoomCodeEnter()
    {

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
