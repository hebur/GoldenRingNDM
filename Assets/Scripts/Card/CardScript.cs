using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public TextMeshPro[] ReqTexts;
    public TextMeshPro[] EffectTexts;
    public TextMeshPro ScoreText;
    public TextMeshPro TurnText;
    public GameObject slotObject, slotPrefab, exitObject, playerSaleObject;
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
        Debug.Log(ReqTexts.Length);
        for(int i = 0; i < ReqTexts.Length; i++)
        {
            ReqTexts[i].text = _cardData.Price[i] > 1000000 ? "X" : _cardData.Price[i].ToString();
        }
        for (int i = 0; i < 5; i++)
        {
            EffectTexts[i].text = _cardData.Effect[i].ToString();
        }
        ScoreText.text = _cardData.Score.ToString();
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
        targetScale = originScale * scaleMultiplier;
        targetZ = originZ - 0.25f;
        Vector3 before = transform.localPosition;
        before.z = targetZ;
        transform.localPosition = before;

        UIManager.instance.ShowAfterBuy(this.gameObject);
    }
    
    /// <summary>
    /// 마우스 때면 원래대로
    /// </summary>
    public void OnMouseExit()
    {
        targetScale = originScale;
        targetZ = originZ;

        Player nowPlayer = TableManager.instance.Get_NowPlayerScript();
        PlayerInfoPanel playerPanel = TableManager.instance.Get_NowPlayerPanel();

        nowPlayer.ShowNextTurn(false);
        playerPanel.DrawInfo(true, nowPlayer.Resource, nowPlayer.Score);
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
            {
                List<int> playerResource = TableManager.instance.Get_NowPlayerResource();
                UIManager.instance.Popup_PurchaseUI(this._cardData.CardNum, CardManager.instance.Is_Buyable(this._cardData.CardNum, playerResource), 
                    this._cardData.Price, playerResource);
            }
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
    public int GetScore()
    {
        return _cardData.Score;
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

    /// <summary>
    /// 매 턴마다 사용자 순서에 따라 해당 턴 사용자 자원 할인 정보 갱신
    /// </summary>
    /// <param name="curPlayer"></param>
    public void UpdateResourceSaleInfo()
    {
        int maxRes = 0; // 할인 적용할 자원
        int maxCost = 0; // 최고 비용

        for (int i = 0; i < 4; i++)
            _cardData.Price[i] = originGoldCosts[i];

        for (int i = 0; i < 4; i++)
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
            playerSaleObject.transform.parent = ReqTexts[maxRes].gameObject.transform;
            playerSaleObject.transform.localPosition = Vector3.left * 1f;
            playerSaleObject.GetComponent<SpriteRenderer>().color = new Color(1, 215 / 255f, 0);
            for (int i = 0; i < 4; i++)
                _cardData.Price[i] = originGoldCosts[i];
            _cardData.Price[maxRes]--;
            ReqTexts[maxRes].text = _cardData.Price[maxRes].ToString();
        }
        
    }
}
