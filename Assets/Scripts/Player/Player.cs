using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    private int _order;            // �÷��� ����
    [SerializeField] private int _scorehappy;       // ������ �� �ڿ�
    [SerializeField] private List<int> _resource;   // ���� �ִ� ���� �ڿ�
    //private List<Card> _hands;   // �ڵ忡 �ִ� ī�� ����Ʈ
    private List<GameObject> _fields;    // �ʵ忡 �ִ� ī�� ����Ʈ
    private int slotUsed;
    public const int maxSlot = 4;
    public float cardGap;  // ī�� ������ ����
    public TextMeshProUGUI slotText;
    public int Order { get => _order; set => _order = value; }
    public int Scorehappy { get => _scorehappy; set => _scorehappy = value; }
    public List<int> Resource { get => _resource; set => _resource = value; }
    public List<GameObject> Fields { get => _fields; set => _fields = value; }
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
    /// _resource�� _fields�� �ʱ�ȭ
    /// </summary>
    public void Initialize()
    {
        //_resource = new List<int>();
        _fields = new List<GameObject>();
    }

    /// <summary>
    /// �� ���� �� ī�� ȿ���� ���� ��,�ڿ��� ������ �ݿ��մϴ�.
    /// </summary>
    /// <param name="gain">�󸶳� �������</param>
    public void Gain(List<int> gain)    // 
    {
        for (int i = 0; i < _resource.Count; i++)
        {
            Debug.Log(_resource.Count + " " + gain.Count);
            _resource[i] += gain[i];

            UIManager.instance.Get_UpScore(_order).gameObject.SetActive(true);
            UIManager.instance.Get_UpScore(_order).DrawText(gain);

        }
    }

    /// <summary>
    /// ī�� ���ſ� ����� ��,�ڿ��� ���Ҹ� �ݿ��մϴ�.
    /// </summary>
    /// <param name="used">�󸶳� �����ߴ���</param>
    public void Use(List<int> used)     // 
    {
        for (int i = 0; i < used.Count; i++)
        {
            _resource[i] -= used[i];
        }
    }

    /// <summary>
    /// ����� �ʵ忡 ī��(newcard) �߰�
    /// </summary>
    /// <param name="newcard">�߰��� ī��</param>
    public void AddCard(GameObject newcard) 
    {
        CardScript card = newcard.GetComponent<CardScript>();
        _fields.Add(newcard);
        SlotUsed += card.GetSlot();
        card.IsPurchased = true;
        int index = _fields.Count-1;
        _fields[index].transform.parent = transform;
        _fields[index].transform.DOLocalMove(Vector3.right * cardGap * index, 0.5f);
        //Debug.Log(_fields[index].transform.localPosition);
    }

    /// <summary>
    /// ����� �ʵ忡�� ī�� ����
    /// </summary>
    /// <param name="card">������ ī��</param>
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
    /// �� ���� ���� ������ �ʿ��� �۾�
    /// �ʵ忡 �ִ� ī�� ȿ�� 
    /// �ʵ忡 �ִ� ī�帶�� �� ���̱�
    /// ���Ÿ� ���� �ʾ��� �� �� �Ǵ� �ڿ��� ȹ��
    /// </summary>
    public void EndTurn()
    {
        List<GameObject> deleteTargets = new List<GameObject>();
        foreach(var card in _fields)
        {
            CardScript cs = card.GetComponent<CardScript>();
            this.Gain(cs.GetEffect());
            if (--(cs.TurnLeft) == 0)
                deleteTargets.Add(card);
        }
        foreach (var target in deleteTargets)
            RemoveCard(target);
    }

    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    /// <returns>�ڿ� �� �÷��̾�� ������ �Ǵ� ��</returns>
    public int GetScore()
    {
        return _resource[_scorehappy];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}