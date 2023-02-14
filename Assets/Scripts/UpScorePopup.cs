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

    private void Awake()
    {
        ScoreUp.SetActive(false);
        tempUp.SetActive(false);
    }

    private void ShowText(List<TextMeshProUGUI> text, List<int> rsh)
    {
        if (text.Count == 5) // tempUp_text (자원만 얻거나 골드를 얻었을 때)
        {
            for (int i = 0; i < text.Count; i++)
            {
                if (rsh[i] != 0)
                    text[i].text = ColorCode + " + " + rsh[i].ToString() + "</color>";
                else
                    text[i].text = "";
            }
        }
        else // ScoreUp_text
        {
            for (int i = 1; i < text.Count; i++)
                text[i].text = ColorCode + " + " + rsh[i].ToString() + "</color>";
        }
    }

    public void DrawText(List<int> rsh, bool is_res)
    {
        if (is_res) // 자원만 얻는다면
        {
            ScoreUp.SetActive(false);
            tempUp.SetActive(true);
            ShowText(tempUp_text, rsh);
            StartCoroutine(corFunc_SelfOff());  // 몇 초 후에 꺼짐
        }
        else         // 카드 효과
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
        nowPlayer.ShowNextTurn();     // 다음 턴에 얻을 자원 표시
    }
}
