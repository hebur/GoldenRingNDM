using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

/// <summary>
/// ��带 ����ϴ� ��ư�� ������ �� �۵��մϴ�.
/// </summary>
public class GoldButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI GoldSelect;

    private List<List<bool>> PressedButton = new List<List<bool>>();

    private int playerGold = 0;
    List<int> goldUsed = new List<int>(new int[5]); // ����� ��� ����
    int goldUsedSum = 0; // ����� ��� ����

    List<int> curCardPrice = new List<int>();

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

    //���� �÷��̾ ���� �ִ� ��� ��
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

    //���� ���� �ִ� ī���� ����
    public void setCurCardPrice(List<int> price) 
    { curCardPrice = price; }

    //���ݱ����� ��� ��뷮
    public List<int> getGoldUsed()
    { return goldUsed; }  
    public int getGoldUsedSum() 
    { return goldUsedSum; }

    /// <summary>
    /// ��� ��� ��ư�� ������ �� �۵�
    /// </summary>
    /// <param name="rsh">
    /// -1�� ���� Reset Button
    /// ���ڸ� ������ ����. �����ڸ�+1 �� �ڿ���ȣ-1�� ��Ÿ���� �����ڸ�+1�� ��뷮-1�� ��Ÿ��
    /// </param>
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

        if (num + 1 > curCardPrice[resource]) //����Ϸ��� ���� ī�� ���� ��
        {
            Debug.Log("Can't use more gold than price. try to use: " + (num+1) + "price: " + curCardPrice[resource]);
        }
        else //����Ϸ��� ���� ������� ��� ������ ��
        {
            for (int i = 0; i < 3; i++) // ���� �ڿ��� ���� �ߺ� ����
            {
                PressedButton[resource][i] = false;
            }

            // ��带 �� ����� �� �ִ��� Ȯ���ϱ� ���� ����� ��� �� ���ϱ�
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

            // ��尡 ������ ��Ȳ
            if (usedGoldSum + num + 1 > playerGold)
            {
                Debug.Log("Not Enough Gold!");
            }
            else
            {
                PressedButton[resource][num] = true;
            }
        }

        // �� ��뷮 �����ִ� �޼��� �ۼ�
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
        int lastIndex = message.LastIndexOf(",  ");
        message = message.Remove(lastIndex, ",  ".Length);
        GoldSelect.text = message;

        goldUsed = usedGold;
        goldUsedSum = usedGoldSum;
    }
}
