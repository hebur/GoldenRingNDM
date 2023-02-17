using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    public int _order;            // 플레이 순서
    [SerializeField] private int _scorehappy;       // 점수가 될 자원
    [SerializeField] private List<int> _resource;   // 갖고 있는 돈과 자원
    //private List<Card> _hands;   // 핸드에 있는 카드 리스트
    private List<GameObject> _fields;    // 필드에 있는 카드 리스트
    private int slotUsed;
    public const int maxSlot = 4;
    public float cardGap;  // 카드 사이의 간격
    public TextMeshProUGUI slotText;
    private int _score;
    public int Order { get => _order; set => _order = value; }
    public int Scorehappy { get => _scorehappy; set => _scorehappy = value; }
    public List<int> Resource { get => _resource; set => _resource = value; }
    public List<GameObject> Fields { get => _fields; set => _fields = value; }
    public int Score { get => _score; set => _score = value; }
    public int SlotUsed 
    { 
        get => slotUsed; 
        set
        {
            slotUsed = value;
            slotText.text = string.Format("Slots : {0}/{1}", slotUsed, maxSlot);
        }
    }
    public int SlotLeft
    {
        get => (maxSlot - slotUsed);
    }
    private void Awake()
    {
        Initialize();
        SlotUsed = 0;        
    }

    public void FlashRed()
    {
        StopAllCoroutines();
        StartCoroutine(RunFlashRed());
    }

    IEnumerator RunFlashRed()
    {
        slotText.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        slotText.color = Color.white;
    }
    
    /// <summary>
    /// _resource와 _fields를 초기화
    /// </summary>
    public void Initialize()
    {
        //_resource = new List<int>();
        _fields = new List<GameObject>();
    }

    /// <summary>
    /// 턴 종류 후 카드 효과에 의한 돈,자원의 증가를 반영합니다.
    /// </summary>
    /// <param name="gain">얼마나 얻었는지</param>
    public void Gain(List<int> gain, int score, bool is_res)  
    {
        //_score += score;
        for (int i = 0; i < _resource.Count; i++)
        {
            _resource[i] += gain[i];
        }
         score = 0;
        gain.Add(score);
        UIManager.instance.Get_UpScore(_order).DrawText(gain, is_res);
    }

    /// <summary>
    /// 카드 구매에 사용한 돈,자원의 감소를 반영합니다.
    /// </summary>
    /// <param name="used">얼마나 지불했는지</param>
    public void Use(List<int> used)
    {
        for (int i = 0; i < used.Count; i++)
        {
            _resource[i] -= used[i];
        }
    }

    /// <summary>
    /// 사용자 필드에 카드(newcard) 추가
    /// </summary>
    /// <param name="newcard">추가할 카드</param>
    public void AddCard(GameObject newcard) 
    {
        CardScript card = newcard.GetComponent<CardScript>();
        _fields.Add(newcard);
        SlotUsed += card.GetSlot();
        card.IsPurchased = true;
        int index = _fields.Count-1;
        _fields[index].transform.parent = transform;
        _fields[index].transform.DOLocalMove(Vector3.right * cardGap * index, 0.5f);
    }

    /// <summary>
    /// 사용자 필드에서 카드 제거
    /// </summary>
    /// <param name="card">제거할 카드</param>
    public void RemoveCard(GameObject card) // 
    {
        int cardNum = card.GetComponent<CardScript>().GetCardNum();
        int i;
        bool found = false;
        for(i=0;i<_fields.Count;i++)
        {
            if (_fields[i].GetComponent<CardScript>().GetCardNum() == cardNum)
            {
                _fields.RemoveAt(i);
                found = true;
                break;
            }
        }
        if (!found) return;
        for(; i<_fields.Count;i++)
        {
            _fields[i].transform.DOLocalMove(Vector3.right * cardGap * i, 0.5f);
        }
        card.gameObject.SetActive(false);
        SlotUsed -= card.GetComponent<CardScript>().GetSlot();
        Destroy(card);
    }

    /// <summary>
    /// 한 턴이 끝날 때마다 필요한 작업.
    /// 필드에 있는 카드 효과(자원 얻기).
    /// 필드에 있는 카드마다 턴 줄이기.
    /// 구매를 하지 않았을 때 돈 또는 자원을 획득.
    /// </summary>
    public void EndTurn()
    {
        List<int> add = new List<int>(new int[5]);
        int score = 0;
        foreach(var card in _fields)
        {
            CardScript cs = card.GetComponent<CardScript>();

            for (int i = 0; i < _resource.Count; i++)
                add[i] += cs.GetEffect()[i];
            
            // score += cs.GetScore();
        }
         this.Gain(add, score, false);

        bool earn_res = GameObject.Find("EarnResource").GetComponent<OnlyEarnResource>().earn_res;
        if (!earn_res)
           ShowNextTurn(true);
    }

    /// <summary>
    /// 자원 표시기: 다음 턴에 추가될 자원 표시
    /// </summary>
    public List<int> ShowNextTurn(bool turnEnd) 
    {
        List<GameObject> deleteTargets = new List<GameObject>();
        List<int> add = new List<int>(new int[6]);
        for (int i = 0; i < add.Count; i++) add[i] = 0;

        foreach (var card in _fields)
        {
            CardScript cs = card.GetComponent<CardScript>();
            if (turnEnd)
            {
                if (--(cs.TurnLeft) == 0)
                    deleteTargets.Add(card);
            }
            if (cs.TurnLeft > 0)
            {
                for (int i = 0; i < _resource.Count; i++)
                    add[i] += cs.GetEffect()[i];

                // add[_resource.Count] += cs.GetScore();
                add[_resource.Count] += 0;
            }
        }
        if (turnEnd)
        { 
            foreach (var target in deleteTargets)
            RemoveCard(target);
        }
       
        UIManager.instance.Get_UpScore(_order).DrawText(add, false);
        return add;
    }

    /// <summary>
    /// 사용자 턴이 끝났을 때 점수 업데이트
    /// </summary>
    public void ScoreUpdate()
    {
        int min = 9999;
        for(int i = 1; i < _resource.Count; i++)
        {
            if (_resource[i] < min)
            { min = _resource[i]; }
        }
        int pref = _resource[Order+1];
        int gold = _resource[0];

        _score = min * 8 + pref * 3 + gold;
        Debug.Log("ScoreUpdate: order-"+ _order.ToString() + ", _score: " + _score.ToString());
    }

    /// <summary>
    /// 최종 점수 반환
    /// </summary>
    /// <returns>자원 중 플레이어에게 점수가 되는 것</returns>
    public int GetScore()
    {
        return _score;
    }

    public int GetGoldNum()
    {
        return _resource[0];
    }
}
