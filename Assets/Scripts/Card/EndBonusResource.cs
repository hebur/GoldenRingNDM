using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 버튼이 호출하여 오직 자원만을 얻는 스크립트 입니다.
/// </summary>
public class EndBonusResource : MonoBehaviour
{ 
    public List<List<int>> EndBonuslistResource;
    [SerializeField] public bool earn_res = false;
    [SerializeField] private List<TextMeshProUGUI> EarnText;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        EndBonuslistResource = new List<List<int>>();
        for (int i = 0; i < 5; i++)
        {
            EndBonuslistResource.Add(new List<int>());
            for(int j = 0; j < 5; j++)
                EndBonuslistResource[i].Add(0);
        }

        // 플레이어가 Gain할 때 리스트 형태로 받습니다.
        for(int i = 0; i < 5; i++)
        {
            EndBonuslistResource[i][0] = 4;
        }
        EndBonuslistResource[1][1] = 3;
        EndBonuslistResource[2][2] = 3;
        EndBonuslistResource[3][3] = 3;
        EndBonuslistResource[4][4] = 3;
        for(int i = 0; i < EarnText.Count; i++) 
        {
            EarnText[i].text = EndBonuslistResource[i][i].ToString();
        }

    }

    public void BTN_CallButton(int rsh)
    {
        earn_res = true;
        TableManager.instance.Get_NowPlayerScript().Gain(EndBonuslistResource[rsh], 0, true);
        UIManager.instance.Popdown_EndBonusUI();
        // TableManager.instance.End_PlayerTurn();
    }
}
