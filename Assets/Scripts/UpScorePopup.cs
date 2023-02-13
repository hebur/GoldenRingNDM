using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpScorePopup : MonoBehaviour
{
    [SerializeField] private string ColorCode;
    [SerializeField] GameObject ScoreUp;
    [SerializeField] GameObject tempUp;
    [SerializeField] List<TextMeshProUGUI> text;
    [SerializeField] List<TextMeshProUGUI> tempUp_text;

    private List<Player> listPlayer;
    private void Awake()
    {
        ScoreUp.SetActive(false);
        tempUp.SetActive(false);
    }

    private void ShowText(List<TextMeshProUGUI> text, List<int> rsh)
    {
        if (text.Count == 5)
        {
            for (int i = 0; i < text.Count; i++)
            {
                if (rsh[i] != 0)
                    text[i].text = ColorCode + " + " + rsh[i].ToString() + "</color>";
                else
                    text[i].text = "";
            }
        }
        else
        {
            for (int i = 0; i < text.Count; i++)
                text[i].text = ColorCode + " + " + rsh[i + 1].ToString() + "</color>";
        }
    }

    public void DrawText(List<int> rsh, bool is_res)
    {
        if (is_res)
        {
            ScoreUp.SetActive(false);
            tempUp.SetActive(true);
            ShowText(tempUp_text, rsh);
            StartCoroutine(corFunc_SelfOff());  // 자원만 얻는다면 몇 초 후에 꺼짐
        }
        else
        {
            ScoreUp.SetActive(true);
            ShowText(text, rsh);
        } 
    }

    private IEnumerator corFunc_SelfOff()
    {  
        yield return new WaitForSeconds(1f);
        tempUp.SetActive(false);
        Player nowPlayer = TableManager.instance.Get_NowPlayerScript();
        nowPlayer.ShowNextTurn();
    }
}
