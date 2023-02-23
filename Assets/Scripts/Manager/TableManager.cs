using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.X86.Avx;
using System.Runtime.ExceptionServices;
using UnityEditor;

/// <summary>
/// 테이블의 진행을 관리하는 메니저입니다.
/// </summary>
public class TableManager : MonoBehaviour
{
    
    public static TableManager instance;

    public bool IsDebuging; //TurnEnd 메세지 짧게 띄움.

    [Tooltip("최대 턴 횟수 입니다.")]
    [SerializeField] private int maxTurn;

    [Tooltip("4명을 ? 혹은 2인을 기준으로 제작")]
    [SerializeField] private int maxPlayer;
    [SerializeField] private int nowPlayerTurn;

    [SerializeField] private GameObject EarnResource;
    [SerializeField] private GameObject GoldPanel;

    [SerializeField] private bool playerTurnEnd;
    [SerializeField] private bool playerAfterTurnEnd;
    [SerializeField] private bool TableTurnEnd;
    [SerializeField] private bool TableAfterTurnEnd;

    [SerializeField] private List<Player> listPlayer;
    [SerializeField] private GameObject RightButton;

    [SerializeField] private TextMeshProUGUI tmpSpendTurn;  //지난 턴 TODO
    [SerializeField] private TextMeshProUGUI tmpLimitTurn;  //최대 턴
    [SerializeField] private TextMeshProUGUI tmpNowTurn; //현재 턴

    [SerializeField] private List<PlayerInfoPanel> playerInfoPanel; //우측 플레이어 정보
    [SerializeField] private List<Image> Slots;               //우측 슬롯 정보
    [SerializeField] private Sprite voidSlotImage;
    [SerializeField] private Sprite SlotImage;

    [SerializeField] private TextMeshProUGUI TurnEndMessage; //턴 끝날 때 마다 나옴
    [SerializeField] public GameObject TurnEndBlock;        //클릭 방지
    [SerializeField] private GameObject TurnEndBlockImg;

    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject WinnerPanel;
    [SerializeField] private Image WinnerFill;
    [SerializeField] private List<TextMeshProUGUI> PlayerName;
    [SerializeField] private List<TextMeshProUGUI> PlayerScore;
    [SerializeField] private Button ReturnButton;

    private int CountEndCards = 0;
    private bool hasInit = false;

    public bool CardMouseEffectOn;
    public int tableTurn = 0;
    public GameObject AI;

    private void OnEnable()
    {
        if(instance == null)
            instance = this;
    }

    private void Awake()
    {
        // 턴이 끝났을 때 클릭 방지 (흐린 화면 뜸)
        TurnEndBlock.SetActive(false);
        TurnEndBlockImg.SetActive(false);
        TurnEndMessage.gameObject.SetActive(false);
        // 게임이 끝났을 때 클릭 방지 (흐린 화면 뜸)
        GameOverCanvas.SetActive(false);

        if (!hasInit)
            Initialize();
        for (int i = 0; i < maxPlayer; i++)
        {
            listPlayer[i].Order = i;
        }
    }

    private void Start()
    {
        StartTable();
    }

    private void Initialize()
    {
        hasInit = true;

        nowPlayerTurn = 0;
        playerTurnEnd = false;
        playerAfterTurnEnd = false;
        TableTurnEnd = false;
        TableAfterTurnEnd = false;
        CardMouseEffectOn = true;

        WinnerPanel.gameObject.SetActive(false);
        ReturnButton.interactable = false;
    }

    /// <summary>
    /// 테이블을 시작합니다
    /// </summary>
    public void StartTable()
    {
        StartCoroutine(corFunc_RollTable());
        UIManager.instance.Popup_GameStart();
    }

    private void Update()
    {

    }

    /// <summary>
    /// 턴을 진행시킵니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator corFunc_RollTable()
    {
        DrawPannel();

        for (int i = 0; i < maxTurn; i++) // 라운드를 나타냄 (4 턴 = 1 라운드)
        {
            tableTurn = i;
            // Debug.Log("Now Round : " + i);

            //EarnResource.GetComponent<OnlyEarnResource>().TurnCheck(i + 1);

            listPlayer[0].IsAI = true;            ///////////////////////////////////////
            listPlayer[1].IsAI = false;            // AI 설정 부분
            listPlayer[2].IsAI = false;
            listPlayer[3].IsAI = false;

            for (int j = 0; j < maxPlayer; j++) // 턴을 나타냄 (1 턴 = 1 행동)
            {
                Debug.Log("Now (Round, Turn): (" + i.ToString() + ", " + j.ToString() + ")" );
                nowPlayerTurn = j;
                DrawPannel();
                //CardManager.instance.UpdateSaleInfo();

                //플레이어 턴 시작 전 작업
                Run_BeforePlayerTurn(j);


                //플레이어 턴 중 작업
                Run_PlayerTurn(j);

                yield return new WaitUntil(() => playerTurnEnd == true);
                playerTurnEnd = false;

                //턴 종료 이후 작업 실행
                Run_AfterPlayerTurn(j);

                yield return new WaitUntil(() => playerAfterTurnEnd == true);
                playerAfterTurnEnd = false;

                yield return new WaitForSeconds(0.5f);

                //턴 마무리 작업
                SoundManager.instance.PlayAudio(SoundType.Bell);

                Run_AtTurnEnd();

                yield return new WaitForSeconds(0.5f);

                DrawPannel();
            }

            //테이블 자체에 어떠한 효과가 나와야 한다면 호출
            Run_TableTurn();

            yield return new WaitUntil(() => TableTurnEnd == true);
            TableTurnEnd = false;

            //테이블 턴이 끝날시에 호출
            Run_AfterTableTurn();

            // 게임 종료 조건 확인하기
            CheckGameOver();

            yield return new WaitUntil(() => TableAfterTurnEnd == true);
            TableTurnEnd = false;

        }
    }

    // 게임 로직
    // 시장에 카드가 들어온다
    // 카드를 구매 - 사용한다
    // 다음 플레이어에게 넘긴다.
    // 반복

    /// <summary>
    /// 사용자에 따라 정보 텍스트를 업데이트 합니다.
    /// </summary>
    private void DrawPannel()
    {
        bool tmp = false;
        for (int i = 0; i < listPlayer.Count; i++)
        {
            if (i == nowPlayerTurn)
                tmp = true;
            else
                tmp = false;
            playerInfoPanel[i].DrawInfo(tmp, listPlayer[i].Resource, listPlayer[i].Score);
            for (int j = 0; j < 4; j++)
                Slots[i * 4 + j].sprite = voidSlotImage;
            for (int j = 0; j < listPlayer[i].SlotUsed; j++)
                Slots[i * 4 + j].sprite = SlotImage;
        }
    }

    /// <summary>
    /// 플레이어가 턴을 시작하기 전에 필요한 정보를 필드에 입력합니다.
    /// 필드에 있는 카드, 정보, 슬롯.
    /// </summary>
    /// <param name="rsh"></param>
    private void Run_BeforePlayerTurn(int cur)
    {
        RightButton.GetComponent<Field>().UpdateTurn(cur);
    }

    /// <summary>
    /// 해당하는 플레이어 턴을 실행한다.
    /// </summary>
    private void Run_PlayerTurn(int cur)
    {
        //실행 부분
        //플레이어의 제어권 확보
        if (listPlayer[cur].IsAI)
            AI.GetComponent<AI>().Run_AI();
        Get_NowPlayerScript().ShowNextTurn(false);
    }

    /// <summary>
    /// 플레이어의 턴이 종료된 뒤 실행됩니다.
    /// 카드의 정렬 추가 필요.
    /// </summary>
    /// <param name="rsh">현재 플레이어 번호</param>
    private void Run_AfterPlayerTurn(int rsh)
    {
        Player nowPlayer = listPlayer[rsh];

        //플레이어의 재화 확보
        bool earn_res = GameObject.Find("EarnResource").GetComponent<OnlyEarnResource>().earn_res;
        if (!earn_res)
            nowPlayer.EndTurn();         
        else
            nowPlayer.Invoke("EndTurn", 1f);

        nowPlayer.ScoreUpdate();

        //첫 카드 구매하지 않았으면 제거
        CardManager.instance.CheckBuyFirst();

        // 턴 종료 메세지 띄우기
        // Debug.Log("player GoldNum: " + nowPlayer.GetGoldNum());
        StartCoroutine(EndMessage());

        End_AfterPlayerTurn();
    }

    /// <summary>
    /// 턴 종료 메세지를 띄운다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndMessage()
    {
        TurnEndMessage.gameObject.SetActive(true);
        TurnEndBlock.SetActive(true);
        TurnEndBlockImg.SetActive(true);
        if (IsDebuging)
            yield return new WaitForSeconds(0.1f);
        else
            yield return new WaitForSeconds(1f);

        TurnEndMessage.gameObject.SetActive(false);
        TurnEndBlock.SetActive(false);
        TurnEndBlockImg.SetActive(false);
    }

    /// <summary>
    /// 게임 종료 조건을 확인한다. 
    /// 조건이 맞으면 게임 종료 문구를 출력한다.
    /// </summary>
    private void CheckGameOver()
    {
        if (CountEndCards >= 4)
        {
            StartCoroutine(OverMessage());
        }
    }

    /// <summary>
    /// 게임 종료 문구를 출력한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OverMessage()
    {
        GameOverCanvas.SetActive(true);
        CardMouseEffectOn = false;

        //스코어 계산
        List<int> Score = new List<int>();
        List<int> Player = new List<int>();
        for (int i = 0; i < maxPlayer; i++)
        {
            Score.Add(listPlayer[i].Score);
            // Score.Add(listPlayer[i].Resource[i + 1]);
            Player.Add(i);
        }
        int tmpint;
        for (int i = 0; i < maxPlayer - 1; i++)
        {
            for (int j = 0; j < maxPlayer - 1; j++)
            {
                if (Score[j] < Score[j + 1])
                {
                    tmpint = Score[j + 1];
                    Score[j + 1] = Score[j];
                    Score[j] = tmpint;

                    tmpint = Player[j + 1];
                    Player[j + 1] = Player[j];
                    Player[j] = tmpint;
                }
            }
        }

        yield return new WaitForSeconds(3f);

        WinnerPanel.gameObject.SetActive(true);

        WinnerFill.DOFillAmount(1f, 10f);

        for (int i = 0; i < maxPlayer; i++)
        {
            PlayerName[i].text = listPlayer[Player[i]].nickname;
            PlayerScore[i].text = Score[i].ToString();
        }

        yield return new WaitForSeconds(10f);

        // ReturnButton.interactable = true;

        //종료 확인 버튼 -> 메인으로 돌아감

    }

    public void BTN_ReturnMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Cardmanager에서 호출 가능하다. 종료 카드 수를 증가시킨다.
    /// </summary>
    public void increaseCEC()
    {
        CountEndCards++;
        Debug.Log("EndCard Stack: " + CountEndCards.ToString());
    }

    /// <summary>
    /// 공격 카드가 구매되었을 때 CardManager에서 호출됨.
    /// </summary>
    public void UseAttackCard(int resource) 
    {
        // resource = 몇 번 자원으로 사용
        int num;
        int first, second, third;
        int max = -1, secondmax = -1;
        first = second = third = -1;

        num = nowPlayerTurn;

        //전 플레이어
        num = (num + listPlayer.Count - 1) % listPlayer.Count;
        max = listPlayer[num].Resource[resource];
        first = num;

        //전전 플레이어
        num = (num + listPlayer.Count - 1) % listPlayer.Count;
        if (max < listPlayer[num].Resource[resource])
        {
            secondmax = max;
            max = listPlayer[num].Resource[resource];
            second = first;
            first = num;
        }
        else
        {
            second = num;
            secondmax = listPlayer[num].Resource[resource];
        }

        //전전전 플레이어
        num = (num + listPlayer.Count - 1) % listPlayer.Count;
        if(max < listPlayer[num].Resource[resource])
        {
            third = second;
            second = first;
            first = num;
        }
        else if(secondmax < listPlayer[num].Resource[resource])
        {
            third = second;
            second = num;
        }
        else
        {
            third = num;
        }

        List<int> useResource = new List<int>();
        if (resource == 0) // 골드를 사용했을 경우 - 9, 6, 3
        {
            useResource.Add(9);
            useResource.Add(9);
            useResource.Add(9);
        }
        else // 다른 자원 - 3, 2, 1
        {
            useResource.Add(3);
            useResource.Add(3);
            useResource.Add(3);
        }

        List<int> use = new List<int>(new int[5]);

        if (listPlayer[first].Resource[resource] <= useResource[0])
        {
            use[resource] = listPlayer[first].Resource[resource];
        }
        else { use[resource] = useResource[0]; }
        listPlayer[first].Use(use);

        use.Clear();
        use = new List<int>(new int[5]);
        if (listPlayer[second].Resource[resource] <= useResource[1])
        {
            use[resource] = listPlayer[second].Resource[resource];
        }
        else { use[resource] = useResource[1]; }
        listPlayer[second].Use(use);

        use.Clear();
        use = new List<int>(new int[5]);
        if (listPlayer[third].Resource[resource] <= useResource[2])
        {
            use[resource] = listPlayer[third].Resource[resource];
        }
        else { use[resource] = useResource[2]; }
        listPlayer[third].Use(use);
    }

    /// <summary>
    /// 한 라운드가 끝났을 때 호출
    /// </summary>
    private void Run_TableTurn()
    {
        End_TableTurn();
    }

    /// <summary>
    /// 테이블 턴이 종료될 떄 호출
    /// </summary>
    private void Run_AfterTableTurn()
    {
        //테이블 턴 종료시의 함수 호출
        End_AfterTableTurn();
    }

    private void Run_AtTurnEnd()
    {
        for(int i = 0; i < listPlayer.Count; i++)
        {
            listPlayer[i].CollectCard();
        }
    }

    public void End_PlayerSelfEnd(int rsh)
    {
        playerTurnEnd = true;
    }

    /// <summary>
    /// 플레이어의 턴 종료시 호출해야 합니다.
    /// </summary>
    public void End_PlayerTurn()
    {
        playerTurnEnd = true;
    }

    public void End_AfterPlayerTurn()
    {
        playerAfterTurnEnd = true;
    }

    /// <summary>
    /// 테이블의 턴 종료시 호출되어야 합니다.
    /// </summary>
    public void End_TableTurn()
    {
        TableTurnEnd = true;
    }

    public void End_AfterTableTurn()
    {
        TableAfterTurnEnd = true;
    }
    public int Get_MaxPlayer()
    {
        return maxPlayer;
    }
    public int Get_NowPlayerTurn()
    {
        return nowPlayerTurn;
    }

    public List<int> Get_NowPlayerResource()
    {
        return listPlayer[nowPlayerTurn].Resource;
    }

    public Player Get_NowPlayerScript()
    {
        return listPlayer[nowPlayerTurn];
    }

    public Player Get_PlayerScript(int turn)
    {
        return listPlayer[turn];
    }

    public PlayerInfoPanel Get_NowPlayerPanel()
    {
        return playerInfoPanel[nowPlayerTurn];
    }

    /// <summary>
    /// 슬롯이 부족할 때 CS에서 호출. Field의 FlashRed를 호출.
    /// </summary>
    public void FlashRed()
    {
        //StopAllCoroutines();
        StartCoroutine(RunFlashRed(nowPlayerTurn));
        RightButton.GetComponent<Field>().FlashRed();
    }

    private IEnumerator RunFlashRed(int rsh)
    {
        for (int i = 0; i < 4; i++)
        {
            Slots[rsh * 4 + i].sprite = voidSlotImage;
            Slots[rsh * 4 + i].color = Color.red;
        }
        yield return new WaitForSeconds(1.0f);
        for (int j = 0; j < 4; j++)
            Slots[rsh * 4 + j].sprite = voidSlotImage;
        for (int j = 0; j < listPlayer[rsh].SlotUsed; j++)
            Slots[rsh * 4 + j].sprite = SlotImage;
        for(int j = 0; j < 4; j++)
            Slots[rsh * 4 + j].color = Color.white;
    }
}
