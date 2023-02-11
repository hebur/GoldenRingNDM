using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

/// <summary>
/// 골드를 사용하는 버튼을 눌렀을 때 작동합니다.
/// </summary>
public class GoldButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI GoldSelect;

    private List<List<bool>> PressedButton = new List<List<bool>>();

    private int playerGold = 0;
    List<int> goldUsed = new List<int>(new int[5]);
    int goldUsedSum = 0;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        PressedButton = new List<List<bool>>();
        for(int i = 0; i < 4; i++)
        {
            PressedButton.Add(new List<bool>());
            for (int j = 0; j < 3; j++)
                PressedButton[i].Add(false);
        }
    }

    public void setCurGold(int gold)
    {
        playerGold = gold;
        resetGold();
    }
    public void resetGold()
    {
        GoldSelect.text = "Reset!";
        goldUsedSum = 0;
        for(int i = 0; i < 5; i++)
        {
            goldUsed[i] = 0;
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                PressedButton[i][j] = false;
            }
        }
    }

    public List<int> getGoldUsed()
    { return goldUsed; }  
    
    public int getGoldUsedSum() 
    { return goldUsedSum; }

    public void BTN_CallButton(int rsh)
    {
        if(rsh == -1)
        {
            resetGold();
            return;
        }

        int resource = rsh / 10;
        int num = rsh % 10;

        int usedGoldSum = 0;
        List<int> usedGold = new List<int>(new int[5]);

        for(int i = 0; i < 3; i++) // 같은 자원에 대해 중복 제거
        {
            PressedButton[resource][i] = false;
        }

        // 골드를 더 사용할 수 있는지 확인하기 위해 사용한 골드 합 구하기
        for (int i = 0; i < 4; i++) 
        {
            for (int j = 0; j < 3; j++)
            {
                if (PressedButton[i][j] == true)
                {
                    usedGoldSum += j + 1;
                    usedGold[i + 1] = j + 1;
                }
            }
        }

        // 골드가 부족한 상황
        if (usedGoldSum + num + 1 > playerGold)
        {
            Debug.Log("Not Enough Gold!"); 
        }
        else
        {
            PressedButton[resource][num] = true;
        }

        usedGoldSum = 0;
        for (int i = 0; i < 5; i++)
            usedGold[i] = 0;
        string message = "";
        for (int i = 0; i < 4; i++)
        {
            bool isAny = false;
            for (int j = 0; j < 3; j++)
            {
                if (PressedButton[i][j] == true)
                {
                    isAny = true;
                    message = message + (i + 1).ToString() + ": " + (j + 1).ToString() + ",  ";
                    usedGoldSum += j + 1;
                    usedGold[i + 1] = j + 1;
                }      
            }
            if (isAny == false)
                message = message + (i + 1).ToString() + ": " + (0).ToString() + ",  ";
        }
        int lastIndex = message.LastIndexOf(",");
        message = message.Remove(lastIndex, ",".Length);
        GoldSelect.text = message;

        goldUsed = usedGold;
        goldUsedSum = usedGoldSum;
    }
}
