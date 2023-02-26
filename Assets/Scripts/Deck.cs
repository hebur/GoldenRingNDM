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
    public List<CardData> cards { get { return _cards; } }
    // Start is called before the first frame update
    void Awake()
    {
        LoadCards();
        if (doShuffle)
            // Shuffle(0, _cards.Count);
            Create_Deck();
    }

    void LoadCards()
    {
        //StreamReader fs = new StreamReader(Path.Combine(Application.dataPath, "Resources/Json/cards.json"));
        int version = MainMenuController.instance.Version;

        StreamReader fs;

        if(version == 0)
        {
            fs = new StreamReader(Path.Combine(Application.streamingAssetsPath, "cards_short.json"));
        }
        else if(version == 1)
        {
            fs = new StreamReader(Path.Combine(Application.streamingAssetsPath, "cards_delux.json"));
        }
        else
        {
            fs = new StreamReader(Path.Combine(Application.streamingAssetsPath, "cards_.json"));
        }
        
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

    void Create_Deck()
    {
        int level = 0;
        List<List<int>> num = new List<List<int>>(5); // [단계] [카드 번호]
        for(int i = 0; i < 10; i++) num.Add(new List<int>());
        foreach (var card in _cards)
        {
            level = card.CardNum / 100;
            num[level].Add(card.CardNum); // 단계별 카드
        }

        int start = 0;
        for (int lev = 0; lev < num.Count; lev++)
        {
            if (num[lev].Count != 0)
            {
                Shuffle(start, num[lev].Count);
                start += num[lev].Count;
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