using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField] private GameObject Blocker, UIBlocker;
    [SerializeField] private GameObject ReqHolder;
    public GameObject[] Reqs;
    public TextMeshPro[] ReqTexts;
    [SerializeField] private List<GameObject> slash;
    public TextMeshPro[] EffectTexts;
    public GameObject endObject, returnedObject, attackObject;
    public int MaxSlot;
    [SerializeField] private List<GameObject> slots;
    public float scaleMultiplier;
    private CardData _cardData;
    bool isPurchased; //사용자에게 속해 있는지 여부
    bool isReturned; //종료카드에면서 한 번 버려졌는지 여부
    bool isEndCard; //종료카드인지 여부
    int turnLeft;
    float targetScale, originScale;
    float targetZ, originZ;
    List<int> originGoldCosts;
    Vector3 v1, v2;

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private List<Sprite> BackGrounds;

    public bool IsReturned { get { return isReturned; } set => isReturned = value; }

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
    public void Initalize(CardData from)
    {
        UIBlocker = GameObject.Find("EndBonusCanvas").transform.GetChild(0).gameObject;
        this._cardData = from;

        // 소비 자원 선택지 작성
        List<GameObject> cardReqs = new List<GameObject>();
        for (int i = 0; i < ReqTexts.Length; i++)
        {
            ReqTexts[i].text = _cardData.Price[i].ToString();
            if (_cardData.Price[i] > 99) // || _cardData.Price[i] == 0
                Reqs[i].gameObject.SetActive(false);
            else
                cardReqs.Add(Reqs[i]);
        }
        DrawCardReq(cardReqs);

        // 효과 설명 작성
        for (int i = 0; i < 5; i++)
        {
            EffectTexts[i].text = _cardData.Effect[i].ToString();
        }

        // 슬롯 정보 작성
        for (int i = 0; i < MaxSlot; i++)
        {
            if (i < _cardData.Slot)
            { slots[i].SetActive(true); }
            else
            { slots[i].SetActive(false); }
        }

        //상태정보 초기화
        isPurchased = false;
        isReturned = false;
        if (_cardData.Effect[5] == 1)
            isEndCard = true;
        else
            isEndCard = false;

        //턴 정보 작성
        turnLeft = _cardData.Turn;
        background.sprite = BackGrounds[turnLeft];

        originGoldCosts = new List<int>(_cardData.Price);

        //특수 카드 이미지 설정
        endObject.SetActive(_cardData.Effect[5] == 1);
        returnedObject.SetActive(false);
        attackObject.SetActive(_cardData.Effect[5] == 2);
    }

    /// <summary>
    /// 선택 가능한 소비 자원만 카드에 표시
    /// </summary>
    void DrawCardReq(List<GameObject> cardReqs)
    {
        Vector3 firstPos = ReqHolder.transform.position;
        Vector3 pos = firstPos;
        for (int i = 0; i < cardReqs.Count; i++)
        {
            cardReqs[i].transform.position = pos;
            pos.y -= 0.26f;
        }
        for(int i = 0; i < MaxSlot - 1; i++)
        {
            if(i<cardReqs.Count - 1) 
            { slash[i].SetActive(true); }
            else
            { slash[i].SetActive(false); }
        }
    }

    /// <summary>
    /// 한 턴이 지날 때 마다 TurnLeft 줄이고 배경 바꿈.
    /// </summary>
    public void TurnDecrese()
    {
        turnLeft--;
        if(turnLeft != 0)
        {
            background.sprite = BackGrounds[turnLeft];
        }
    }

    /// <summary>
    /// 마우스 올리면 확대
    /// </summary>
    public void OnMouseEnter()
    {
        if (!Blocker.gameObject.activeSelf && TableManager.instance.CardMouseEffectOn)
        {
            targetScale = originScale * scaleMultiplier;
            targetZ = originZ - 0.25f;
            Vector3 before = transform.localPosition;
            before.z = targetZ;
            transform.localPosition = before;

            if(!isPurchased)
                UIManager.instance.ShowAfterBuy(this.gameObject);
        }
    }
    
    /// <summary>
    /// 마우스 때면 원래대로
    /// </summary>
    public void OnMouseExit()
    {
        if (!Blocker.gameObject.activeSelf && TableManager.instance.CardMouseEffectOn)
        {
            targetScale = originScale;
            targetZ = originZ;

            Player nowPlayer = TableManager.instance.Get_NowPlayerScript();
            PlayerInfoPanel playerPanel = TableManager.instance.Get_NowPlayerPanel();

            nowPlayer.ShowNextTurn(false);
            playerPanel.DrawInfo(true, nowPlayer.Resource, nowPlayer.Score);
        }
    }

    /// <summary>
    /// 클릭 했을 때
    /// 구매하거나 버리거나
    /// </summary>
    public void OnMouseDown()
    {
        OnMouseExit();
        if (!Blocker.gameObject.activeSelf && TableManager.instance.CardMouseEffectOn)
        {
            if (!isPurchased)
            {
                if (TableManager.instance.Get_NowPlayerScript().SlotLeft >= this._cardData.Slot)
                {
                    List<int> playerResource = TableManager.instance.Get_NowPlayerResource();
                    UIManager.instance.Popup_PurchaseUI(this._cardData.CardNum, CardManager.instance.Is_Buyable(this._cardData.CardNum, playerResource),
                        this._cardData.Price, playerResource, isEndCard);
                }
                else
                    TableManager.instance.FlashRed();
            }
            /*else //플레이어 필드에서 카드를 버리는 코드
                TableManager.instance.Get_NowPlayerScript().RemoveCard(gameObject);*/
        }
    }
   
    // Get 함수들
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
            //TurnText.text = turnLeft.ToString();
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
    /*public void UpdateResourceSaleInfo()
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
        
    }*/
}
