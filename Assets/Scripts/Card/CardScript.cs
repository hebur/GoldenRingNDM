using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public TextMeshPro[] ReqTexts;
    public TextMeshPro[] EffectTexts;
    public TextMeshPro TurnText, SaleText;
    public GameObject slotObject, slotPrefab, SaleObject, exitObject, playerSaleObject;
    public float scaleMultiplier;
    private CardData _cardData;
    bool isPurchased;
    int turnLeft;
    float targetScale, originScale;
    float targetZ, originZ;
    List<int> originGoldCosts;
    Vector3 v1, v2;
    // Start is called before the first frame update
    void Start()
    {
        targetScale = originScale = transform.localScale.x;
        targetZ = originZ = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.SmoothDamp(transform.localScale, new Vector3(targetScale, targetScale, 1.0f), ref v1, 0.3f);
        Vector3 targetPos = transform.localPosition;
        targetPos.z = targetZ;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref v2, 0.3f);
    }

    /// <summary>
    /// 카드 정보 받아와 카드 생성
    /// </summary>
    /// <param name="from"></param>
    public void Initalize(CardData from)
    {
        this._cardData = from;
        for (int i = 0; i < 5; i++)
        {
            ReqTexts[i].text = _cardData.Price[i] > 1000000 ? "X" : _cardData.Price[i].ToString();
            EffectTexts[i].text = _cardData.Effect[i].ToString();
        }
        TurnText.text = _cardData.Turn.ToString();
        for(int i = 0; i < _cardData.Slot; i++)
        {
            var newObj = Instantiate(slotPrefab);
            newObj.transform.parent = slotObject.transform;
            newObj.transform.localPosition = Vector3.down * 0.15f * i;
        }
        isPurchased = false;
        turnLeft = _cardData.Turn;
        originGoldCosts = new List<int>(_cardData.Price);
        exitObject.SetActive(_cardData.Effect[5] != 0);
    }
    
    /// <summary>
    /// 마우스 올리면 확대
    /// </summary>
    public void OnMouseEnter()
    {
        //if (!isControlAble) return;
        targetScale = originScale * scaleMultiplier;
        targetZ = originZ - 0.25f;
        Vector3 before = transform.localPosition;
        before.z = targetZ;
        transform.localPosition = before;
    }
    
    /// <summary>
    /// 마우스 때면 원래대로
    /// </summary>
    public void OnMouseExit()
    {
        targetScale = originScale;
        targetZ = originZ;
    }

    /// <summary>
    /// 클릭 했을 때
    /// 구매하거나 버리거나
    /// </summary>
    public void OnMouseDown()
    {
        if (!isPurchased)
        {
            if (TableManager.instance.Get_NowPlayerScript().SlotLeft >= this._cardData.Slot)
                UIManager.instance.Popup_PurchaseUI(this._cardData.CardNum, CardManager.instance.Is_Buyable(this._cardData.CardNum, TableManager.instance.Get_NowPlayerResource()), this._cardData.Price);
            else
                TableManager.instance.Get_NowPlayerScript().FlashRed();
        }
        else
            TableManager.instance.Get_NowPlayerScript().RemoveCard(gameObject);
    }
   
    /// <summary>
    /// Get 함수들
    /// </summary>
    /// <returns></returns>
    public int GetCardNum()
    {
        return _cardData.CardNum;
    }
    public List<int> GetPrice()
    {
        return _cardData.Price;
    }
    public List<int> GetEffect()
    {
        return _cardData.Effect;
    }
    public int TurnLeft
    {
        get => turnLeft; 
        set
        {
            turnLeft = value;
            TurnText.text = turnLeft.ToString();
        }
    }
    public int GetSlot()
    {
        return _cardData.Slot;
    }
    public bool IsPurchased { get { return isPurchased; } set { isPurchased = value; } }


    // 카드의 골드 세일 정보 갱신 더 이상 필요 없음.
    /*public void UpdateSaleInfo(int n)

    {
        SaleObject.SetActive(n != 0);
        _cardData.Price[0] = originGoldCosts[0];
        if (n > 0)
        {
            SaleText.text = "+1";
            _cardData.Price[0]++;
            SaleText.color = Color.red;
        }
        else if (n < 0)
        {
            SaleText.text = "-1";
            _cardData.Price[0]--;
            SaleText.color = Color.blue;
        }
    }*/


    /// <summary>
    /// 매 턴마다 사용자 순서에 따라 해당 턴 사용자 자원 할인 정보 갱신
    /// </summary>
    /// <param name="curPlayer"></param>
    public void UpdateResourceSaleInfo()
    {
        int maxRes = 0; // 할인 적용할 자원
        int maxCost = 0; // 최고 비용

        for(int i = 1; i < 5; i++)
        {
            if (maxCost < _cardData.Price[i])
            {
                maxCost = _cardData.Price[i];
                maxRes = i;
            }
        }

        if(maxCost == 0)
        {
            playerSaleObject.gameObject.SetActive(false);
        }
        else
        {
            playerSaleObject.gameObject.SetActive(true);
            SaleText.text = "-1";
            playerSaleObject.transform.parent = ReqTexts[maxRes].gameObject.transform;
            playerSaleObject.transform.localPosition = Vector3.left * 0.62f;
            playerSaleObject.GetComponent<SpriteRenderer>().color = ReqTexts[maxRes].transform.parent.gameObject.GetComponent<SpriteRenderer>().color;
            for (int i = 1; i < 5; i++)
                _cardData.Price[i] = originGoldCosts[i];
            _cardData.Price[maxRes]--;
        }
        
    }
}
