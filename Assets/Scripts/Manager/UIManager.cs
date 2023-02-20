using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject BuyUIOn;
    [SerializeField] private GameObject BuyUIOff;
    [SerializeField] private GameObject ShoppingPannel;
    [SerializeField] private GameObject ShoppingClickBlocker;
    [SerializeField] private GameObject ShoppingWorldClickBlocker;
    [SerializeField] private GameObject ShoppingBreaker;

    [SerializeField] private GameObject EndBonusUI;

    [SerializeField] private List<Button> ShoppingButton;
    [SerializeField] private List<bool> ShoppingButtonAble;
    [SerializeField] private List<TextMeshProUGUI> PriceText;
    [SerializeField] private List<int> Price;

    [SerializeField] private GameObject GoldPanel;
    
    [SerializeField] private UpScorePopup curUpScorePopup;

    [SerializeField] private int CardNum;

    [SerializeField] private List<bool> testBool;
    [SerializeField] private List<int> testInt;

    private List<int> curPrice = new List<int>();
    private List<int> curPlayRes = new List<int>();

    private bool ispurchasing = false;
    private bool isEndCard = false;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        /* 골드를 다른 자원 대신 사용할 때 썼던 코드
        if (ispurchasing == true)
        {
            bool isAble = isButtonAble();
            if (isAble != ShoppingButtonAble)
            {
                ShoppingButtonAble = isAble;
                for (int i = 0; i < ShoppingButton.Count; i++)
                {
                    ShoppingButton[i].interactable = ShoppingButtonAble[i];
                }
            }
        }
        */
    }

    // 보유 골드와 사용자 자원에 따라 버튼이 사용 가능한 상태인지 리턴
    private bool isButtonAble()
    {
        List<int> selGold = GoldPanel.GetComponent<GoldButton>().getGoldUsed(); // 선택된 골드
        bool isAble = true;
        for (int i = 1; i < 5; i++)
        {
            if (selGold[i] + curPlayRes[i] < curPrice[i - 1])
            { isAble = false; break; }
        }
        return isAble;
    }

    /// <summary>
    /// 시장 카드에 오버레이했을 때 실행, 플레이어의 바뀔 정보 표시
    /// </summary>
    public void ShowAfterBuy(GameObject card)
    {
        // 추가될 효과 표시
        CardScript cs = card.GetComponent<CardScript>();
        Player nowPlayer = TableManager.instance.Get_NowPlayerScript();
        List<int> add = nowPlayer.ShowNextTurn(false);

        for (int i = 0; i < add.Count; i++)
            add[i] += cs.GetEffect()[i];
        // add[add.Count - 1] += cs.GetScore();

        Get_UpScore().DrawText(add, false); // 추가될 자원 표시

        /*//  예상 소비 자원 표시
        List<int> curRes = new List<int>();
        for (int i = 0; i < nowPlayer.Resource.Count; i++)
            curRes.Add(0);

        for (int i = 0; i < curRes.Count; i++)
        {
            curRes[i] = nowPlayer.Resource[i];
            curRes[i] -= cs.GetPrice()[i];
        }

        PlayerInfoPanel playerPanel = TableManager.instance.Get_NowPlayerPanel();
        playerPanel.DrawInfo(true, curRes, nowPlayer.Score);
        */
    }

    /// <summary>
    /// 시장 카드를 클릭했을 때 실행
    /// </summary>
    public void Popup_PurchaseUI(int cardNum, List<bool> Able, List<int> price, List<int> playerResource, bool isendCard)
    {
        //구매 진행 중 -> 가능 여부 Update()
        ispurchasing = true;
        isEndCard = isendCard;

        // 배경 클릭 막기
        ShoppingClickBlocker.SetActive(true);
        ShoppingWorldClickBlocker.SetActive(true);

        CardNum = cardNum;
        ShoppingButtonAble = Able;
        Price = price;
        curPrice = price;
        curPlayRes = playerResource;
        GoldPanel.GetComponent<GoldButton>().setCurCardPrice(curPrice);
        StartCoroutine(corFunc_PopupPurchaseUI());

        // 자원별로 가격 표시
        for (int i = 0; i < ShoppingButton.Count; i++)
        {

            if (Price[i] < 99)
            {
                ShoppingButton[i].gameObject.SetActive(true);
                PriceText[i].text = Price[i].ToString();
            }
            else
                ShoppingButton[i].gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 특정 자원으로 구매하면 실행되는 함수
    /// </summary>
    public void Popdown_PurchaseUI(int res) // 0: gold, 1: 빨강, ~
    {
        //구매 가능 여부 Update() 필요 x
        ispurchasing = false;

        List<int> cost = new List<int>();
        for (int i = 0; i < 5; i++) cost.Add(0);
        cost[res] = Price[res]; // 소모비용


        StartCoroutine(corFunc_PopDownPurchaseUI());

        /* List<int> usedGold = GoldPanel.GetComponent<GoldButton>().getGoldUsed();
        for (int i = 0; i < usedGold.Count; i++)
            cost[i] -= usedGold[i];
        */

        TableManager.instance.Get_NowPlayerScript().AddCard(CardManager.instance.Get_MarketCard(CardNum));
        TableManager.instance.Get_NowPlayerScript().Use(cost);

        if (isEndCard)
        {
            Popup_EndBonusUI();
            return;
        }
        TableManager.instance.End_PlayerTurn();
    }

    public void Popup_EndBonusUI()
    {
        TableManager.instance.CardMouseEffectOn = false;
        EndBonusUI.SetActive(true);
    }

    public void Popdown_EndBonusUI()
    {
        TableManager.instance.CardMouseEffectOn = true;
        EndBonusUI.SetActive(false);
    }

    public void ButtonClose()
    {
        for (int i = 0; i < ShoppingButton.Count; i++)
        {
            ShoppingButton[i].interactable = false;
        }
    }

    public void BTN_CancelShopping()
    {
        StartCoroutine(corFunc_PopDownPurchaseUI());
    }

    /// <summary>
    /// 구매 팝업 창이 내려옴.
    /// 구매 가능할 때 구매 가능한 상태로 보임.
    /// </summary>
    /// <returns></returns>
    private IEnumerator corFunc_PopupPurchaseUI()
    {
        SoundManager.instance.PlayAudio(SoundType.UIOn);
        ButtonClose();

        Vector3 VecOn = BuyUIOn.transform.position;
        Vector3 VecOff = BuyUIOff.transform.position;

        ShoppingPannel.transform.position = VecOff;
        ShoppingPannel.gameObject.SetActive(true);
        ShoppingPannel.transform.DOMove(VecOn, 0.3f).SetEase(Ease.InOutBack);

        /*ShoppingPannel.DOMoveY(1080 * 2, 0f);
        ShoppingPannel.gameObject.SetActive(true);
        ShoppingPannel.DOMoveY(1080 / 2f, 0.5f).SetEase(Ease.InOutBack);*/

        yield return new WaitForSeconds(0.5f);

        ShoppingBreaker.SetActive(true);

        for (int i = 0; i < ShoppingButton.Count; i++)
        {
            ShoppingButton[i].interactable = ShoppingButtonAble[i];
        }
    }

    /// <summary>
    /// 구매 취소 또는 구매 선택 했을 때 팝업창이 사라짐.
    /// </summary>
    /// <returns></returns>
    private IEnumerator corFunc_PopDownPurchaseUI()
    {
        // GoldPanel.GetComponent<GoldButton>().resetGold();
        SoundManager.instance.PlayAudio(SoundType.UIOff);
        ButtonClose();

        Vector3 VecOff = BuyUIOff.transform.position;
        ShoppingPannel.transform.DOMove(VecOff, 0.5f).SetEase(Ease.InOutBack);
        ShoppingBreaker.SetActive(false);
        ShoppingPannel.gameObject.SetActive(true);
        //ShoppingPannel.DOMoveY(1080 * 2f, 0.5f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(0.5f);

        ShoppingPannel.gameObject.SetActive(false);
        //ShoppingClickBlocker.SetActive(false);
        ShoppingWorldClickBlocker.SetActive(false);
    }

    public UpScorePopup Get_UpScore()
    {
        return curUpScorePopup;
    }
}
