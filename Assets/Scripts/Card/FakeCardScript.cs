using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class FakeCardScript : MonoBehaviour
{
    public GameObject ReqHolder;
    public GameObject[] Reqs;
    public TextMeshPro[] ReqTexts;
    public List<GameObject> slash;
    public TextMeshPro[] EffectTexts;
    public GameObject endObject, returnedObject, attackObject;
    public int MaxSlot;
    public List<GameObject> slots;
    public CardData _cardData;
    int turnLeft;
    Vector3 v1, v2;

    public SpriteRenderer background;
    public List<Sprite> BackGrounds;

    public float scaleMultiplier;
    private float targetScale, originScale;
    private float targetZ, originZ;

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
    /// 마우스 올리면 확대
    /// </summary>
    public void OnMouseEnter()
    {
            targetScale = originScale * scaleMultiplier;
            targetZ = originZ - 0.25f;
            Vector3 before = transform.localPosition;
            before.z = targetZ;
            transform.localPosition = before;
    }

    /// <summary>
    /// 마우스 때면 원래대로
    /// </summary>
    public void OnMouseExit()
    {
            targetScale = originScale;
            targetZ = originZ;
    }


    /// <summary>
    /// 카드 정보 받아와 카드 생성
    /// </summary>
    public void Initalize(CardData from)
    {
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

        //턴 정보 작성
        turnLeft = _cardData.Turn;
        background.sprite = BackGrounds[turnLeft];

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
}
