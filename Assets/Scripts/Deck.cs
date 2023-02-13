using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<CardData> _cards;
    public bool doShuffle;
    // Start is called before the first frame update
    void Awake()
    {
        LoadCards();
        if (doShuffle)
            //Shuffle(_cards.Count);
            Create_Deck();
    }

    void LoadCards()
    {
        //StreamReader fs = new StreamReader(Path.Combine(Application.dataPath, "Resources/Json/cards.json"));
        StreamReader fs = new StreamReader(Path.Combine(Application.streamingAssetsPath ,"cards_.json"));
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

    //void Shuffle(int num)
    //{
    //    for(int i = 0; i < num - 1 - 4; i++)
    //    { 
    //        int rnd = UnityEngine.Random.Range(i, num - 4);
    //        exchange(i, rnd);
    //    }
    //    for(int i = 1; i <= 4; i++)
    //    {
    //        int rnd = UnityEngine.Random.Range((num - 4)/3, num - 4 + i);
    //        exchange(num - 1 - 4 + i, rnd);
    //    }
    //}

    // 바뀔 카드 셔플: 프로토타입에서만 임시로 사용
    void Create_Deck()
    {
        int[] num = new int[] { 0, 14, 7, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 단계별 카드
        int start = 0;
        for (int i = 1; i < num.Length; i++)
        {
            if (num[i] != 0)
            {
                int[] level_deck = new int[num[i]];
                Shuffle(start, num[i]);
                start += num[i];
            }
        }
    }

    void Shuffle(int start, int num)
    {
        int end = start + num - 1;
        for (int i = start; i < end; i++)
        {
            // Pick random Element
            int j = UnityEngine.Random.Range(i, end + 1);

            // Swap Elements
            CardData tmp = _cards[i];
            _cards[i] = _cards[j];
            _cards[j] = tmp;
        }
    }

    public List<CardData> cards { get { return _cards; } }
}

public class CardLoadData
{
    public List<CardFromJson> cards;
}

[System.Serializable]
public class CardFromJson
{
    public int id;
    public List<int> price;
    public List<int> effect;
    public int turn;
    public int slot;
    public int score;
}