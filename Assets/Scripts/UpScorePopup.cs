using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class UpScorePopup : MonoBehaviour
{
    private string ColorCode;
    [SerializeField] GameObject ScoreUp;
    [SerializeField] GameObject tempUp;
    [SerializeField] List<TextMeshProUGUI> Uptext;
    [SerializeField] List<TextMeshProUGUI> tempUp_text;
    private List<string> temp;
    [SerializeField] private List<Color> colors = new List<Color>();

    private void Awake()
    {
        ScoreUp.SetActive(false);
        tempUp.SetActive(false);
    }

    /// <summary>
    /// UpScore 텍스트를 설정함.
    /// </summary>
    public void SetText(List<int> gain, int player_num)
    {
        for(int i = 0; i < gain.Count; i++)
        {
            Uptext[i].text = "+" + gain[i].ToString();
            Uptext[i].color = Color.red;
        }
    }

    /// <summary>
    /// 카드를 올렸을 때 일시적으로 텍스트를 표시함.
    /// </summary>
    public void TempText(List<int> gain, int player_num)
    {
        temp.Clear();
        for(int i=0;i<gain.Count;i++)
        {
            temp.Add(Uptext[i].text);
            Uptext[i].text = "+" + (Int32.Parse(Uptext[i].text) + gain[i]).ToString();
        }
    }

    /// <summary>
    /// 원래 텍스트로 돌아감.
    /// </summary>
    public void ReturnText()
    {
        for(int i = 0; i < temp.Count; i++)
        {
            Uptext[i].text = "+" + temp[i];
        }
    }

    /// <summary>
    /// 무료 자원을 얻었을 때 일시적으로 표시함.
    /// </summary>
    public IEnumerator EarnFreeText(List<int> gain, int player_num)
    {
        temp.Clear();
        for (int i = 0; i < gain.Count; i++)
        {
            temp.Add(Uptext[i].text);
            Uptext[i].text = (Int32.Parse(Uptext[i].text) + gain[i]).ToString();
        }
        yield return new WaitForSeconds(1f);
        ReturnText();
    }

    private void ShowText(List<TextMeshProUGUI> text, List<int> rsh)
    {
        if (text.Count == 5) // tempUp_text (자원만 얻었을 때)
        {
            for (int i = 0; i < text.Count; i++)
            {
                if (rsh[i] != 0)
                {
                    text[i].text = "+" + rsh[i].ToString();
                    text[i].color = Color.red;
                }
                else
                    text[i].text = "";
            }
        }
        else // ScoreUp_text
        {
            for (int i = 0; i < text.Count; i++)
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
        else         // 카드 효과라면
        {
            ScoreUp.SetActive(true);
            ShowText(Uptext, rsh);
        } 
    }

    private IEnumerator corFunc_SelfOff()
    {  
        yield return new WaitForSeconds(1f);
        tempUp.SetActive(false);
        Player nowPlayer = TableManager.instance.Get_NowPlayerScript();
        nowPlayer.ShowNextTurn(false);     // 다음 턴에 얻을 자원 표시
    }
}
