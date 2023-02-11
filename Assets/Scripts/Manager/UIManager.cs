using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private RectTransform ShoppingPannel;
    [SerializeField] private GameObject ShoppingClickBlocker;
    [SerializeField] private GameObject ShoppingWorldClickBlocker;
    [SerializeField] private GameObject ShoppingBreaker;

    [SerializeField] private List<Button> ShoppingButton;
    [SerializeField] private bool ShoppingButtonAble;
    [SerializeField] private List<TextMeshProUGUI> ShoppingText;
    [SerializeField] private List<int> ShoppingTextResource;

    [SerializeField] private GameObject GoldPanel;
    
    [SerializeField] private List<UpScorePopup> upScorePopup;

    [SerializeField] private int CardNum;

    [SerializeField] private List<bool> testBool;
    [SerializeField] private List<int> testInt;

    private List<int> curPrice = new List<int>();
    private List<int> curPlayRes = new List<int>();

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        bool isAble = ButtonisAble();
        if(isAble != ShoppingButtonAble)
        {
            ShoppingButtonAble = isAble;
            for (int i = 0; i < ShoppingButton.Count; i++)
            {
                ShoppingButton[i].interactable = ShoppingButtonAble;
            }
        }

    }

    private bool ButtonisAble()
    {
        List<int> selGold = GoldPanel.GetComponent<GoldButton>().getGoldUsed(); // 선택된 골드
        bool isAble = true;
        for (int i = 1; i < 5; i++)
        {
            if (selGold[i] + curPlayRes[i] < curPrice[i])
            { isAble = false; break; }
        }

        return isAble;
    }

    /// <summary>
    /// 시장 카드를 클릭했을 때 실행. 
    /// </summary>
    /// <param name="cardNum"></param>
    /// <param name="Able"></param>
    /// <param name="resource"></param>
    public void Popup_PurchaseUI(int cardNum, bool Able, List<int> resource, List<int> playerResource) // resource == price
    {
        // 배경 클릭 막기
        ShoppingClickBlocker.SetActive(true);
        ShoppingWorldClickBlocker.SetActive(true);

        CardNum = cardNum;
        ShoppingButtonAble = Able;
        ShoppingTextResource = resource.GetRange(1,resource.Count-1);
        curPrice = resource;
        curPlayRes = playerResource;
        StartCoroutine(corFunc_PopupPurchaseUI());

        // 자원별로 가격 표시
        for (int i = 0; i < ShoppingButton.Count; i++)
        {
            if (ShoppingTextResource[i] < 99999)
                ShoppingText[i].text = ShoppingTextResource[i].ToString();
            else
            {
                ShoppingButton[i].gameObject.SetActive(false);
                // ShoppingText[i].text = "X";
            }
                
        }
    }

    // 특정 자원으로 구매하면 실행되는 함수
    public void Popdown_PurchaseUI() // 0: gold, 1: 빨강, ~
    {
        List<int> cost = new List<int>();
        cost.Add(0);
        for (int i = 1; i < 5; i++)
        {
                cost.Add(0);
                cost[i] = ShoppingTextResource[i-1];  // 소모비용
        }
        StartCoroutine(corFunc_PopDownPurchaseUI());

        List<int> usedGold = GoldPanel.GetComponent<GoldButton>().getGoldUsed();
        for (int i = 0; i < usedGold.Count; i++)
            cost[i] -= usedGold[i];
        Debug.Log("cost: " + cost);
        TableManager.instance.Get_NowPlayerScript().AddCard(CardManager.instance.Get_MarketCard(CardNum));
        TableManager.instance.Get_NowPlayerScript().Use(cost);
        TableManager.instance.End_PlayerTurn();
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
        ShoppingPannel.DOMoveY(1080 * 2, 0f);
        ShoppingPannel.gameObject.SetActive(true);
        ShoppingPannel.DOMoveY(1080 / 2f, 0.5f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(0.5f);

        ShoppingBreaker.SetActive(true);

        for (int i = 0; i < ShoppingButton.Count; i++)
        {
            ShoppingButton[i].interactable = ShoppingButtonAble;
        }
    }

    private IEnumerator corFunc_PopDownPurchaseUI()
    {
        GoldPanel.GetComponent<GoldButton>().resetGold();
        SoundManager.instance.PlayAudio(SoundType.UIOff);
        ButtonClose();
        ShoppingBreaker.SetActive(false);
        ShoppingPannel.gameObject.SetActive(true);
        ShoppingPannel.DOMoveY(1080 * 2f, 0.5f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(0.5f);

        ShoppingPannel.gameObject.SetActive(false);
        ShoppingClickBlocker.SetActive(false);
        ShoppingWorldClickBlocker.SetActive(false);
    }

    public UpScorePopup Get_UpScore(int rsh)
    {
        return upScorePopup[rsh];
    }

}
