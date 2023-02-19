using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject Collectholder;

    public static CardManager instance;

    private List<GameObject> listGenCard;   //Game Object 형태로 생성된 카드를 보관

    [SerializeField] private int marketMax; //마켓에 최대로 들어갈 수 있는 카드의 갯수
    private List<GameObject> listMarketCardGO;//마켓 카드 리스트
    private List<CardScript> listMarketCardCS;//마켓 카드 리스트
    private List<CardData> listOrgData;

    private List<List<GameObject>> listPlayerCard;  //플레이어들이 가진 카드 리스트

    [SerializeField] private List<Transform> listMarketHolder;   //마켓에 카드가 들어갈 홀더
    [SerializeField] private Deck deck;     //덱...
    [SerializeField] private bool CheckBuyFst;

    [SerializeField] private GameObject CardPrefab;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {

    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void Initialize()
    {
        marketMax = 8;
        listGenCard = new List<GameObject>();
        listMarketCardGO = new List<GameObject>();
        listMarketCardCS = new List<CardScript>();
        CheckBuyFst = false;
        GenerationCardList();
    }

    /// <summary>
    /// 카드 리스트를 만들어 혹은 카드 리스트들을 받아와 마켓에 추가합니다.
    /// </summary>
    private void GenerationCardList()
    {
        //테스트 생성
        //TestCardMake();
        GenerateCardGameObject();

        //listGenCard에 생성된 카드를 가져와 저장함.

        //마켓에 추가
        Add_Market();

        //마켓 카드 홀더를 우선 초기화 해야함.
        //인스펙터에 넣을지 동적으로 생성되는 홀더를 가져오게 할지
        //...

        //1번칸의 구매 여부를 확인
        

        RePosition_MarketCard();

    }

    /// <summary>
    /// 덱에서 카드 정보를 받아와 게임오브젝트를 만듭니다.
    /// </summary>
    private void GenerateCardGameObject()
    {
        listOrgData = new List<CardData>();
        listOrgData = deck.cards;
        int LoopAmount = listOrgData.Count;

        Debug.Log("Count" + listOrgData.Count);
        for (int i = 0; i < LoopAmount; i++)
        {
            listGenCard.Add(Instantiate(CardPrefab));
            listGenCard[i].GetComponent<CardScript>().Initalize(listOrgData[i]);
        }
    }
    
    /// <summary>
    /// 덱(남은 카드 리스트)에서 가장 첫번째 인덱스의 카드를 마켓에 추가합니다.
    /// </summary>
    public void Add_Market()
    {
        int tmp = marketMax - listMarketCardGO.Count;
        GameObject tmpG;
        if (listMarketCardGO.Count < marketMax)
            for (int i = 0; i < tmp; i++)
            {
                tmpG = Get_Card();
                if (tmpG != null)
                {
                    listMarketCardGO.Add(tmpG);
                    listMarketCardCS.Add(tmpG.GetComponent<CardScript>());
                }
            }
    }

    /// <summary>
    /// 마켓에서부터 카드를 구매하여 플레이어로 가져옵니다.
    /// </summary>
    /// <param name="cardNum">카드번호 입니다.</param>
    public GameObject Get_MarketCard(int cardNum)
    {
        GameObject tmpCard;
        int tmpindex = 99999;

        //선택한 카드 번호를 list에 존재하는지 찾고 해당 리스트 인덱스를 저장
        for (int i = 0; i < listMarketCardGO.Count; i++)
            if (listMarketCardCS[i].GetCardNum() == cardNum)
                tmpindex = i; //발견시 저장

        if (tmpindex == 0)
            CheckBuyFst = true;

        if (tmpindex == 99999)
        {
            Debug.LogError("Get_MarketCard : NO IN HAS MARKET!!");
            return null;
        }

        //발견한 인덱스의 카드를 마켓에서 제거하고 리턴
        tmpCard = listMarketCardGO[tmpindex];
        listMarketCardGO.RemoveAt(tmpindex);
        listMarketCardCS[tmpindex].IsPurchased = true;

        //종료 카드일 시 TableManager에 ThisEndCard 라는 것을 알리는 함수를 호출
        if (listMarketCardCS[tmpindex].GetEffect()[5] == 1)
        {
            TableManager.instance.increaseCEC();
        }
        listMarketCardCS.RemoveAt(tmpindex);

        Add_Market();
        RePosition_MarketCard();
        SoundManager.instance.PlayAudio(SoundType.LoadDeck);
        //임시 카드 이동
        //tmpCard.transform.DOMove(new Vector2(100, 0),1f);

        return tmpCard;
    }

    /// <summary>
    /// 마켓 카드 포지션 재정렬
    /// </summary>
    private void RePosition_MarketCard()
    {

        SoundManager.instance.PlayAudio(SoundType.LoadDeck);

        //리포지션 변경
        //listMarketHolder의 첫 인덱스와 끝 인덱스를 참조하여 카드들을 내부에 자동 정렬.

        float Length = ((listMarketHolder[listMarketHolder.Count - 1].transform.position.x - listMarketHolder[0].transform.position.x) / 8f);
        Vector3 VecOrg = listMarketHolder[0].transform.position;
        Vector3 tmpVec = listMarketHolder[0].transform.position;
        for (int i = 0; i < listMarketCardGO.Count; i++)
        {
            //Transform 을 기준으로  + ( ( ( (2)B-(1)A ) / (Max - 1) ) * i )
            tmpVec = VecOrg;
            tmpVec.x += (Length * (float)i);
            listMarketCardGO[i].transform.DOMove((tmpVec), 0.3f);
        }
       /* for (int i = 0; i < listMarketCardCS.Count; i++)
        {
            if (i < 2)
                listMarketCardCS[i].UpdateSaleInfo(-1);
            else
                listMarketCardCS[i].UpdateSaleInfo(1);
        }*/ 
        
    }


    /// <summary>
    /// 마켓 카드 홀더 Transform 을 지역변수로 저장하게.
    /// </summary>
    /// <param name="position"></param>
    public void Set_MarketHolderList(List<Transform> position)
    {
        listMarketHolder = position;
    }

    /// <summary>
    /// 외부에서 호출하여 카드 리스트 중 가장 첫번째 인덱스의 카드를 가져가게 됨.
    /// </summary>
    /// <returns></returns>
    public GameObject Get_Card()
    {
        if (listGenCard.Count > 0)
        {
            GameObject card = listGenCard[0];
            listGenCard.RemoveAt(0);
            return card;
        }
        else
            return null;
    }

    /// <summary>
    /// 선택한 카드를 플레이어가 구매할 수 있는지 확인
    /// </summary>
    /// <param name="cardNum">구매하려는 카드 번호</param>
    /// <param name="resource">플레이어가 가진 자원</param>
    public List<bool> Is_Buyable(int cardNum, List<int> resource)
    {
        List<bool> buyAble = new List<bool>();
        for (int i = 0; i < 5; i++) buyAble.Add(false);
        List<int> price;
        List<int> curRes = TableManager.instance.Get_NowPlayerResource();

        // 마켓에서 카드 번호 순회
        for (int i = 0; i < listMarketCardGO.Count; i++)
            if (listMarketCardCS[i].GetCardNum() == cardNum) // 카드 번호 동일. 구매 가능 여부 확인
            {
                price = listMarketCardCS[i].GetPrice();
                for (int j = 0; j < 5; j++)
                {
                    if (price[j] <= curRes[j])
                        buyAble[j] = true;
                }
                break;
           }
        return buyAble;

        /*
        bool buyAble = false;
        int check = 1;
        for (int i = 0; i < listMarketCardGO.Count; i++)
            if (listMarketCardCS[i].GetCardNum() == cardNum)
            {
                List<int> price = listMarketCardCS[i].GetPrice();
                for (int j = 0; j < 4; j++)
                {
                    if (price[j] > resource[j + 1])
                        check *= 0;
                }
                break;
            }
        if (check == 1)
            buyAble = true;
        return buyAble;
        */
    }

    /*
    /// <summary>
    /// 카드 구매 가능 여부를 리턴합니다
    /// </summary>
    /// <param name="cardNum">선택한 카드 번호</param>
    /// <param name="Happy">플레이어측 재화</param>
    /// <returns>5가지 항목중 어떤 품목이 구매 가능한지 bool 변수로 전달.</returns>
    public List<bool> Check_BuyThisCard(int cardNum, List<int> Happy)
    {
        List<int> tmpPrice;
        List<bool> buyAble = new List<bool>(); //5가지 항목
        for (int i = 0; i < 5; i++) buyAble.Add(false);
        Debug.Log(buyAble.Count);
        //list Market Card 에서 카드 번호를 순회한 뒤 해당하는 항목과 가격비교
        for (int i = 0; i < listMarketCardGO.Count; i++)
            //카드 번호가 동일함. 구매 가능 여부 확인
            if (listMarketCardCS[i].GetCardNum() == cardNum)
            {
                tmpPrice = listMarketCardCS[i].GetPrice();
                for (int j = 0; j < 5; j++)
                {
                    if (tmpPrice[j] + tmpPrice[j + 5] <= Happy[j])
                        buyAble[j] = true;
                }
                break;
            }
        return buyAble;
    }
    */

    /// <summary>
    /// 첫 번째 카드를 구매했는지 확인하고 처리합니다.
    /// </summary>
    public void CheckBuyFirst()
    {
        if(!CheckBuyFst) //첫 카드 구매 안 했을 때
        {
            if (listMarketCardCS[0].GetEffect()[5] == 1)//종료 카드라면
            {
                TableManager.instance.Get_NowPlayerScript().ApplyEndEffect(0);
                int nowTurn = TableManager.instance.Get_NowPlayerTurn();
                int prevTurn = nowTurn - 1;
                if (prevTurn < 0) prevTurn = TableManager.instance.Get_MaxPlayer() - 1;
                TableManager.instance.Get_PlayerScript(prevTurn).ApplyEndEffect(1);

                if (listMarketCardCS[0].IsReturned)//이미 한 번 돌아온 카드라면
                {
                    //TableManager에 ThisEndCard 라는 것을 알리는 함수를 호출
                    TableManager.instance.increaseCEC();

                    //Market에서 제거
                    GameObject tmpCard;
                    tmpCard = listMarketCardGO[0];
                    listMarketCardGO.RemoveAt(0);
                    Destroy(tmpCard);
                    listMarketCardCS[0].IsPurchased = true;
                    listMarketCardCS.RemoveAt(0);
                }
                else//처음 버려지는 카드라면
                {
                    //한 번 버려졌다고 표시
                    listMarketCardGO[0].transform.DOMove(Collectholder.transform.position, 0.3f);
                    listMarketCardCS[0].IsReturned = true;
                    listMarketCardCS[0].returnedObject.SetActive(true);

                    // 시장에서 제거
                    // 덱으로 다시 넣기
                    listGenCard.Insert(0, listMarketCardGO[0]);
                    listMarketCardGO.RemoveAt(0);
                    listMarketCardCS.RemoveAt(0);
                    /*
                    //덱으로 다시 넣기
                    int len = listMarketCardCS.Count; 
                    for (int i = len - 1; i >= 0; i--)
                    {
                        listGenCard.Insert(0, listMarketCardGO[i]);
                    }

                    //시장 비우기
                    listMarketCardCS.Clear();
                    listMarketCardGO.Clear();
                    */

                    int end = listGenCard.Count - 1; //셔플
                    for (int i = 0; i < end; i++)
                    {
                        // Pick random Element
                        int j = UnityEngine.Random.Range(i, end + 1);

                        // Swap Elements
                        GameObject tmp = listGenCard[i];
                        listGenCard[i] = listGenCard[j];
                        listGenCard[j] = tmp;
                    }
                }
            }
            else //종료 카드가 아니라면
            {
                //Market에서 제거
                GameObject tmpCard;
                tmpCard = listMarketCardGO[0];
                listMarketCardGO.RemoveAt(0);
                Destroy(tmpCard);
                listMarketCardCS[0].IsPurchased = true;
                listMarketCardCS.RemoveAt(0);
            }
        }

        //시장 채우기, 카드 배치
        Add_Market();
        RePosition_MarketCard();

        CheckBuyFst = false;

    }

    /// <summary>
    /// SaleObject 배치를 결정합니다.
    /// </summary>
    public void UpdateSaleInfo()
    {
        for (int i = 0; i < listMarketCardCS.Count; i++)
        {
            if(i < 4)
                listMarketCardCS[i].UpdateResourceSaleInfo();
        }
            
    }
}
