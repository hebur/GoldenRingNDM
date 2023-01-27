using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnlineLobbyController : MonoBehaviour
{
    [SerializeField] private GameObject CreateRoom_UI, EnterCode_UI;

    void Start()
    {

    }

    public void BTN_CallBack()
    {
        SceneManager.LoadScene(1);
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
