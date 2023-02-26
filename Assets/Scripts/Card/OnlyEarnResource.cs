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

        int version = MainMenuController.instance.Version;

        if(version == 0)
        {
            // 플레이어가 Gain할 때 리스트 형태로 받습니다.
            listResource[0][0] = 5;
            listResource[1][1] = 2; 
            listResource[2][2] = 2;
            listResource[3][3] = 2;
            listResource[4][4] = 2;
        }
        else if(version == 1)
        {
            listResource[0][0] = 7;
            listResource[1][1] = 3;
            listResource[2][2] = 3;
            listResource[3][3] = 3;
            listResource[4][4] = 3;
        }

        for(int i = 0; i < EarnText.Count; i++) 
        {
            EarnText[i].text = listResource[i][i].ToString();
        }

    }

    /// <summary>
    /// 무료 자원 버튼을 클릭하였을 때 호출
    /// </summary>
    /// <param name="rsh">자원 번호(골드 포함)</param>
    public void BTN_CallButton(int rsh)
    {
        earn_res = true;
        TableManager.instance.Get_NowPlayerScript().Gain(listResource[rsh], 0, true);
        TableManager.instance.End_PlayerTurn();
        earn_res = false;
    }
}
