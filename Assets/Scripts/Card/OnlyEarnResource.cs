using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 버튼이 호출하여 오직 자원만을 얻는 스크립트 입니다.
/// </summary>
public class OnlyEarnResource : MonoBehaviour
{ 
    public static List<List<int>> listResource;
    [SerializeField] public bool earn_res = false;
    [SerializeField] private List<TextMeshProUGUI> EarnText;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        listResource = new List<List<int>>();
        for (int i = 0; i < 5; i++)
        {
            listResource.Add(new List<int>());
            for(int j = 0; j < 5; j++)
                listResource[i].Add(0);
        }

        // 플레이어가 Gain할 때 리스트 형태로 받습니다.
        listResource[0][0] = 3;
        listResource[1][1] = 3; 
        listResource[2][2] = 3;
        listResource[3][3] = 3;
        listResource[4][4] = 3;
        for(int i = 0; i < EarnText.Count; i++) 
        {
            EarnText[i].text = "3";
        }

    }

    /// <summary>
    /// 턴에 따라 주는 자원의 양을 변화시킵니다.
    /// </summary>
    /// <param name="turn"></param>
    public void TurnCheck(int turn)
    {
        if (turn > 20)
        {
            listResource[0][0] = 7;
            listResource[1][1] = 7;
            listResource[2][2] = 7;
            listResource[3][3] = 7;
            listResource[4][4] = 7;
        }
        else if (turn > 15)
        {
            listResource[0][0] = 6;
            listResource[1][1] = 6;
            listResource[2][2] = 6;
            listResource[3][3] = 6;
            listResource[4][4] = 6;
        }
        else if (turn > 10)
        {
            listResource[0][0] = 5;
            listResource[1][1] = 5;
            listResource[2][2] = 5;
            listResource[3][3] = 5;
            listResource[4][4] = 5;
        }
        else if(turn > 5)
        {
            listResource[0][0] = 4;
            listResource[1][1] = 4;
            listResource[2][2] = 4;
            listResource[3][3] = 4;
            listResource[4][4] = 4;
        }
        else
        {
            listResource[0][0] = 3;
            listResource[1][1] = 3;
            listResource[2][2] = 3;
            listResource[3][3] = 3;
            listResource[4][4] = 3;
        }

        for(int i = 0; i < EarnText.Count; i++)
        {
            int x = listResource[i][i];
            EarnText[i].text = x.ToString();
        }
    }

    public void BTN_CallButton(int rsh)
    {
        earn_res = true;
        TableManager.instance.Get_NowPlayerScript().Gain(listResource[rsh], 0, true);
        TableManager.instance.End_PlayerTurn();
    }
}
