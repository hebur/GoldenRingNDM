// Reference- https://codefinder.janndk.com/22
// https://sagacityjang.tistory.com/83 - Canvas Camera

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreExplanation : MonoBehaviour
{
    [SerializeField] private GameObject ScoExp;
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private RectTransform transform_icon;

    private Vector2 screenPoint;

    private void Start()
    {
        ScoExp.SetActive(false);
        transform_icon = GetComponent<RectTransform>();
        uiCamera = Camera.main;
        Init_Cursor();
    }

    private void Update()
    {
        if(ScoExp.activeSelf == true)
            Update_MousePosition();
    }

    private void Init_Cursor()
    {
        if(transform_icon.GetComponent<Graphic>())
            transform_icon.GetComponent<Graphic>().raycastTarget = false;
    }

    private void Update_MousePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, Input.mousePosition, uiCamera, out screenPoint);
        float w = transform_icon.rect.width;
        float h = transform_icon.rect.height;
        transform_icon.localPosition = screenPoint - (new Vector2(w, h) * 0.5f);


    }

    public void PopUp()
    {
        ScoExp.SetActive(true);
    }

    public void PopDown()
    {
        ScoExp.SetActive(false);
    }

}
