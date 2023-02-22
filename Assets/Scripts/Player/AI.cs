using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    private List<int> resource = new List<int>();
    private List<int> options = new List<int>();
    private List<GameObject> listMarketCardGO = new List<GameObject>();
    private List<CardScript> listMarketCardCS = new List<CardScript>();
    List<CardScript> cards = new List<CardScript>();
    List<Button> ShoppingButton = new List<Button>();
    List<Button> Resource = new List<Button>();
    List<int> price = new List<int>();
    List<bool> buyAble = new List<bool>();
    CardScript cs = new CardScript();
    private bool isEndCard = false;
    int pref = 0;
    int choice = -1; // ������ ī��
    int res = -1;

    // Start is called before the first frame update
    void Start()
    {
        ShoppingButton = UIManager.instance.ShoppingButton;
        Resource = UIManager.instance.Resource;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Run_AI()
    {
        choice = -1;
        options.Clear();
        cards.Clear();
        int nowTurn = TableManager.instance.Get_NowPlayerTurn();
        pref = nowTurn + 1; // ��ȣ�ڿ�
        resource = TableManager.instance.Get_NowPlayerResource();
        listMarketCardGO = CardManager.instance.listMarketCardGO; // ���� ī�� ����Ʈ
        listMarketCardCS = CardManager.instance.listMarketCardCS;

        Debug.Log("�÷��̾��� ���� ���� "+TableManager.instance.Get_NowPlayerScript().SlotLeft);

        // ���� ī�带 ��ȸ�ϸ鼭 ���� ������ ī�� ����Ʈ�� ����
        for (int i = 0; i < listMarketCardGO.Count; i++)
        {
            int cardNum = listMarketCardCS[i].GetCardNum();
            price = listMarketCardCS[i].GetPrice();

            buyAble = CardManager.instance.Is_Buyable(cardNum, resource);
            bool check = false; // ���� ���� ����
            for (int j = 0; j < buyAble.Count; j++)
                if (buyAble[j])
                    check = true;
            if (TableManager.instance.Get_NowPlayerScript().SlotLeft >= listMarketCardCS[i]._cardData.Slot) // ������ �����ְ�
                if (check && Check_Price() && listMarketCardCS[i].GetEffect()[5] == 0) // ������ ī��: ��ȣ�ڿ� ���� ���� �����ϴ� ī�� �� ���� ī��, ���� ī�� ����
                { 
                    options.Add(i);
                    cards.Add(listMarketCardCS[i]);
                }
        }

        if (!options.Any())  // ������ ī�尡 ������ �ڿ��� ����: ���� ���� �ڿ�
        {
            Gain_MinResource();
        }
        else // ������ ī�尡 �ִٸ� ī�� ����
        {
            if (TableManager.instance.tableTurn <= 4) // �ʹݿ��� ��带 ������ ����
            {
                if(Check_Gold())
                {
                    BuyCard_AI(choice);
                }
                else if (Check_First())
                {
                    BuyCard_AI(choice); // ��ȣ�ڿ��� �������� �ִ� ī�� �� ���� ���� �ִ� ī��
                }
                else if (Check_Second())
                {
                    BuyCard_AI(choice);
                }
                else
                {
                    choice = UnityEngine.Random.Range(0, options.Count);
                    BuyCard_AI(choice);
                }
            }
            // �켱����: 1. ������ ��ȣ�ڿ� 2. ������ ���� ���� �ڿ�  3. �� ��
            else if (Check_First())
            {
                BuyCard_AI(choice); // ��ȣ�ڿ��� �������� �ִ� ī�� �� ���� ���� �ִ� ī��
            }
            else if (Check_Second())
            {
                BuyCard_AI(choice);
            }
            else 
            {
                choice = UnityEngine.Random.Range(0, options.Count);
                BuyCard_AI(choice);
            }
        }
    }

    bool Check_Gold()
    {
        int max = 0;
        max = Get_Max(0); // ��带 ���� ���� �ִ� ī��
        if (max > 0)
            return true;
        else
            return false;
    }

    bool Check_Price() // ��ȣ�ڿ� ���� �ٸ� �ڿ����� �� �� �ִ��� || ���� ������ ��ȣ�ڿ�/2 ���� ������ ������
    {
        bool otherBuyAble = false;
        for (int i = 0; i < 5; i++)
        {
            if (buyAble[i] && i != pref)
                otherBuyAble = true;
        }
        return otherBuyAble || price[pref] < resource[pref] / 3;
    }

    private void Gain_MinResource() // ���� ���� ������ �ڿ��� ����
    {
        int min = resource[0];
        int minIdx = 0;
        for (int i = 0; i < resource.Count; i++)
        {
            if (resource[i] < min)
            {
                min = resource[i];
                minIdx = i;
            }
        }

        GameObject.Find("EarnResource").GetComponent<OnlyEarnResource>().BTN_CallButton(minIdx);
    }

    bool Check_First() // ù ��° ����: ��ȣ�ڿ��� �������� ��
    {
        int max = 0;
        max = Get_Max(pref); 
        if (max > 0) 
            return true;
        else
            return false;
    }
    int Get_Max(int target) // target �ڿ��� ���� ���� �����ϴ� ī���� ���귮 ��ȯ, choice ������Ʈ
    {
        int max = 0;
        foreach (var card in cards)
        {
            int targetGain = card.GetEffect()[target];
            if (targetGain > max)
            {
                max = targetGain;
                choice = card.GetCardNum();
            }
        }
        return max;
    }

    bool Check_Second() // �� ��° ����: ���� ���� �ڿ��� �������� ��
    {
        int min = resource[0];
        int minIdx = 0;
        for (int i = 0; i < resource.Count; i++)
        {
            if (resource[i] < min)
            {
                min = resource[i];
                minIdx = i;
            }
        }
        foreach (var card in cards)
        {
            int minGain = card.GetEffect()[minIdx];
            if (minGain > 0)
            {
                choice = card.GetCardNum();
                break;
            }
        }
        if (choice != -1)
            return true;
        else
            return false;
    }

    private void BuyCard_AI(int choice) // ������ ī�� ����
    {
        int index = 0;
        //������ ī�� ��ȣ�� list�� �����ϴ��� ã�� �ش� ����Ʈ �ε����� ����
        for (int i = 0; i < listMarketCardGO.Count; i++)
            if (listMarketCardCS[i].GetCardNum() == choice)
                index = i; //�߽߰� ����

        cs = listMarketCardCS[index];

        // if (cs.GetEffect()[5] == 1) isEndCard = true;
        price = cs.GetPrice();
        buyAble = CardManager.instance.Is_Buyable(cs.GetCardNum(), resource);

        cs.OnMouseEnter();

        cs.Invoke("OnMouseExit", 3f);


        res = -1;
        for (int i = 0; i < 5; i++) // ������ �ڿ� ����
        {
            if (buyAble[i])
            {
                //if (cs.GetPrice()[i] == 0 || resource[i] > cs.GetPrice()[i])
                    res = i;
            }
        }
        if (res == -1) Gain_MinResource();
        else 
        {
            Invoke("Invoke_PopUp", 3f);
        }
        UIManager.instance.ShoppingBreaker.gameObject.SetActive(false);
    }

    private void Invoke_PopUp()
    {
        UIManager.instance.Popup_PurchaseUI(cs.GetCardNum(), buyAble, price, resource, isEndCard);
        UIManager.instance.Popdown_PurchaseUI(res);
    }
}



