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
    [SerializeField] GameObject DirectRoomEnter_UI;

    void Start()
    {
        
    }

    public void Btn_CallDirectRoomEnter()
    {
        DirectRoomEnter_UI.SetActive(true);
    }

    public void BTN_CallCloseParentUI()
    {
        GameObject clickedGO = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"ClickedGO Name: {clickedGO.name}");
        clickedGO.transform.parent.gameObject.SetActive(false);
    }
}
