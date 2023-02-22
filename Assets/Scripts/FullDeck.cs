using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FullDeck : MonoBehaviour
{
    [SerializeField] private GameObject onAnchor, offAnchor;
    [SerializeField] private RectTransform FullDeckPanel;
    [SerializeField] private GameObject FakeCardPrefab;
    [SerializeField] private List<GameObject> holders;
    [SerializeField] private List<GameObject> collectHolders;
    [SerializeField] private TextMeshProUGUI pageText;
    [SerializeField] private GameObject Blocker;

    private List<CardData> _cards;
    private List<GameObject> listCard;

    private bool isFirst = true;
    private int cardNum = 0;
    private int page = -1;
    private int Maxpage = -1;

    private void First()
    {
        LoadCardsData();
        GenerateCardGameObject();
        cardNum = _cards.Count;
        page = 0;
        Maxpage = cardNum / 40 + ((cardNum % 40 == 0) ? 0 : 1);
        pageText.text = "page " + (page + 1).ToString() + "/" + Maxpage.ToString();
    }

    /// <summary>
    /// 카드 정보 불러 옴.
    /// </summary>
    private void LoadCardsData()
    {
        StreamReader fs = new StreamReader(Path.Combine(Application.streamingAssetsPath, "cards_.json"));
        string str = fs.ReadToEnd();
        var json = JsonUtility.FromJson<CardLoadData>(str);
        _cards = new List<CardData>();
        foreach (var card in json.cards)
        {
            CardData newCard = new CardData();
            newCard.CardNum = card.id;
            newCard.Price = card.price;
            newCard.Effect = card.effect;
            newCard.Turn = card.turn;
            newCard.Slot = card.slot;
            newCard.Score = card.score;

            _cards.Add(newCard);
        }
    }

    /// <summary>
    /// 카드 정보를 받아와 게임오브젝트를 만듭니다.
    /// </summary>
    private void GenerateCardGameObject()
    {
        int LoopAmount = _cards.Count;
        listCard = new List<GameObject>();

        for (int i = 0; i < LoopAmount; i++)
        {
            listCard.Add(Instantiate(FakeCardPrefab));
            listCard[i].GetComponent<FakeCardScript>().Initalize(_cards[i]);
        }
    }

    /// <summary>
    /// FullDeck 오른쪽에서 나타남
    /// </summary>
    private IEnumerator corFunc_PopUpFDP()
    {
        SoundManager.instance.PlayAudio(SoundType.UIOn);

        Vector3 VecOn = onAnchor.transform.position;
        Vector3 VecOff = offAnchor.transform.position;

        FullDeckPanel.transform.position = VecOff;
        FullDeckPanel.gameObject.SetActive(true);
        FullDeckPanel.transform.DOMove(VecOn, 0.5f).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(0.5f);

        ShowCards();

    }

    /// <summary>
    /// FullDeck 오른쪽으로 나감
    /// </summary>
    private IEnumerator corFunc_PopDownFDP()
    {
        collectCards();
        yield return new WaitForSeconds(0.3f);
        
        SoundManager.instance.PlayAudio(SoundType.UIOff);

        Vector3 VecOn = onAnchor.transform.position;
        Vector3 VecOff = offAnchor.transform.position;

        FullDeckPanel.transform.DOMove(VecOff, 0.5f).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(0.5f);

        FullDeckPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 전체 카드 창 보여주기
    /// </summary>
    public void OnClickShowButton()
    {
        Blocker.SetActive(true);
        if(isFirst)
        { First(); isFirst = false; }
        FullDeckPanel.gameObject.SetActive(true);
        StartCoroutine(corFunc_PopUpFDP());
    }

    /// <summary>
    /// 돌아가기 버튼 클릭
    /// </summary>
    public void OnClickBackButton()
    {
        Blocker.SetActive(false);
        StartCoroutine(corFunc_PopDownFDP());
    }

    /// <summary>
    /// 화면에 카드 배치
    /// </summary>
    private void ShowCards()
    {
        bool stop = false;
        for (int i = 0; i < 4; i++)
        {
            float Length = ((holders[i * 2 + 1].transform.position.x - holders[i * 2 + 0].transform.position.x) / 9f);
            Vector3 cardVec = holders[i * 2].transform.position;
            for (int j = 0; j < 10; j++)
            {
                if(page * 40 + i * 10 + j >=cardNum)
                { stop = true; break; }

                listCard[page * 40 + i * 10 + j].transform.DOMove(cardVec, 0.3f);
                cardVec.x += Length;
            }
            if(stop)
            { break; }
        }
    }

    /// <summary>
    /// 카드 화면 밖으로 빼기
    /// </summary>
    private void collectCards()
    {
        bool stop = false;
        for (int i = 0; i < 4; i++)
        {
            Vector3 cardVec = collectHolders[i].transform.position;
            for (int j = 0; j < 10; j++)
            {
                if (page * 40 + i * 10 + j >= cardNum)
                { stop = true; break; }

                listCard[page * 40 + i * 10 + j].transform.DOMove(cardVec, 0.3f);
            }
            if (stop)
            { break; }
        }
    }

    /// <summary>
    /// 몇 번째 카드 페이지를 보여줄 건지 선택
    /// </summary>
    /// <param name="gnum"></param>
    public void OnClickPageButton(int num)
    {
        int act = 0;

        if (page + num < 0 || page + num >= Maxpage)
        { act = 0; }
        else if (num < 0)
        { act = -1; }
        else if (num > 0)
        { act = 1; }

        if(act != 0)
        {
            collectCards();
            page += num;
            ShowCards();
        }

        pageText.text = "page " + (page + 1).ToString() + "/" + Maxpage.ToString();
    }
}
