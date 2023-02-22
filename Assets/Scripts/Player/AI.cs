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
    int choice = -1; // 구매할 카드
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
        pref = nowTurn + 1; // 선호자원
        resource = TableManager.instance.Get_NowPlayerResource();
        listMarketCardGO = CardManager.instance.listMarketCardGO; // 마켓 카드 리스트
        listMarketCardCS = CardManager.instance.listMarketCardCS;

        Debug.Log("플레이어의 남은 슬롯 "+TableManager.instance.Get_NowPlayerScript().SlotLeft);

        // 마켓 카드를 순회하면서 구매 가능한 카드 리스트에 저장
        for (int i = 0; i < listMarketCardGO.Count; i++)
        {
            int cardNum = listMarketCardCS[i].GetCardNum();
            price = listMarketCardCS[i].GetPrice();

            buyAble = CardManager.instance.Is_Buyable(cardNum, resource);
            bool check = false; // 구매 가능 여부
            for (int j = 0; j < buyAble.Count; j++)
                if (buyAble[j])
                    check = true;
            if (TableManager.instance.Get_NowPlayerScript().SlotLeft >= listMarketCardCS[i]._cardData.Slot) // 슬롯이 남아있고
                if (check && Check_Price() && listMarketCardCS[i].GetEffect()[5] == 0) // 구매할 카드: 선호자원 가격 조건 만족하는 카드 중 종료 카드, 공격 카드 제외
                { 
                    options.Add(i);
                    cards.Add(listMarketCardCS[i]);
                }
        }

        if (!options.Any())  // 구매할 카드가 없으면 자원을 얻음: 가장 적은 자원
        {
            Gain_MinResource();
        }
        else // 구매할 카드가 있다면 카드 구매
        {
            if (TableManager.instance.tableTurn <= 4) // 초반에는 골드를 모으는 전략
            {
                if(Check_Gold())
                {
                    BuyCard_AI(choice);
                }
                else if (Check_First())
                {
                    BuyCard_AI(choice); // 선호자원을 보상으로 주는 카드 중 가장 많이 주는 카드
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
            // 우선순위: 1. 보상이 선호자원 2. 보상이 가장 적은 자원  3. 그 외
            else if (Check_First())
            {
                BuyCard_AI(choice); // 선호자원을 보상으로 주는 카드 중 가장 많이 주는 카드
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
        max = Get_Max(0); // 골드를 가장 많이 주는 카드
        if (max > 0)
            return true;
        else
            return false;
    }

    bool Check_Price() // 선호자원 제외 다른 자원으로 살 수 있는지 || 현재 보유한 선호자원/2 보다 가격이 낮은지
    {
        bool otherBuyAble = false;
        for (int i = 0; i < 5; i++)
        {
            if (buyAble[i] && i != pref)
                otherBuyAble = true;
        }
        return otherBuyAble || price[pref] < resource[pref] / 3;
    }

    private void Gain_MinResource() // 가장 적게 보유한 자원을 얻음
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

    bool Check_First() // 첫 번째 조건: 선호자원을 보상으로 줌
    {
        int max = 0;
        max = Get_Max(pref); 
        if (max > 0) 
            return true;
        else
            return false;
    }
    int Get_Max(int target) // target 자원을 가장 많이 생산하는 카드의 생산량 반환, choice 업데이트
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

    bool Check_Second() // 두 번째 조건: 가장 적은 자원을 보상으로 줌
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

    private void BuyCard_AI(int choice) // 선택한 카드 구매
    {
        int index = 0;
        //선택한 카드 번호를 list에 존재하는지 찾고 해당 리스트 인덱스를 저장
        for (int i = 0; i < listMarketCardGO.Count; i++)
            if (listMarketCardCS[i].GetCardNum() == choice)
                index = i; //발견시 저장

        cs = listMarketCardCS[index];

        // if (cs.GetEffect()[5] == 1) isEndCard = true;
        price = cs.GetPrice();
        buyAble = CardManager.instance.Is_Buyable(cs.GetCardNum(), resource);

        cs.OnMouseEnter();

        cs.Invoke("OnMouseExit", 3f);


        res = -1;
        for (int i = 0; i < 5; i++) // 구매할 자원 고르기
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



