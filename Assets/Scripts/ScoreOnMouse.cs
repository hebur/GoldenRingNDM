using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnMouse : MonoBehaviour
{
    [SerializeField] private GameObject ScoreExp;
    [SerializeField] private GameObject BuyUIBlocker;

    public void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        Debug.Log("OnMouse!");
        if(!BuyUIBlocker.activeSelf)
            ScoreExp.GetComponent<ScoreExplanation>().PopUp();
    }

    public void OnMouseExit()
    {
        ScoreExp.GetComponent<ScoreExplanation>().PopDown();
    }
}
