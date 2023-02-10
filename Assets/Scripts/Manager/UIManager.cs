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

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            Popup_PurchaseUI(0, testBool, testInt);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Popdown_PurchaseUI(0);
        }*/
    }

    public void Popup_PurchaseUI(int cardNum, bool Able, List<int> resource) // resource == price
    {
        // 배경 클릭 막기
        ShoppingClickBlocker.SetActive(true);
        ShoppingWorldClickBlocker.SetActive(true);

        CardNum = cardNum;
        ShoppingButtonAble = Able;
        ShoppingTextResource = resource.GetRange(1,resource.Count-1);
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
