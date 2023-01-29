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

    void Start()
    {
        
    }

    public void BTN_CallDirectRoomEnter()
    {
        
    }

    public void BTN_CallCloseParentUI()
    {
        GameObject clickedGO = EventSystem.current.currentSelectedGameObject;
        Debug.Log($"ClickedGO Name: {clickedGO.name}");
        clickedGO.transform.parent.gameObject.SetActive(false);
    }
}
