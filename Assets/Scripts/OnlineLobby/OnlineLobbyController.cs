using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;
using static UnityEditor.Experimental.GraphView.GraphView;

public class OnlineLobbyController : MonoBehaviour
{
    Network network;

    [SerializeField] private GameObject CreateRoom_UI, EnterCode_UI, PlayerNickname_UI;
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TextMeshProUGUI playerNickname_txt;

    private string userNickname;

    void Start()
    {
        network= new Network();
    }

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
            }

            SceneManager.LoadScene("MainMenu");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public async void BTN_CallPlayerNicknameCheck()
    {
        if (nicknameInputField.text == "")
        {
            // �г��� ���� �Ұ� �˾� ����ֱ� TODO
            Debug.Log("Nickname InputField is Emty!");
            return;
        }
        
        // �̹� �����ϴ� �г������� üũ
        string user_nickname = nicknameInputField.text;
        bool isExist = await network.GetisNNExist(user_nickname);
        Debug.Log("isExist: " + isExist);
        Thread.Sleep(3000);

        if (isExist)
        {
            // �г��� ���� �Ұ� �˾� ����ֱ� TODO
            Debug.Log("Nickname already Exists!");
            return;
        }
            
        network.PostNewPlayer(user_nickname);
        userNickname = user_nickname;
        playerNickname_txt.text = "User Name: " + nicknameInputField.text;
        PlayerNickname_UI.SetActive(false);
        Thread.Sleep(3000);
    }

    public async void BTN_CallCreateRoomEnter()
    {

    }

    public async void BTN_CallRoomCodeEnter()
    {

    }

    public async void BTN_CallLobbyReLoad()
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
