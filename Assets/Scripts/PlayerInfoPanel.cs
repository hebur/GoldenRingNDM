using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 현재 어떤 사람의 턴인지 보여줍니다
/// </summary>
public class PlayerInfoPanel : MonoBehaviour
{

    [SerializeField] private Image Highlight;
    [SerializeField] private List<TextMeshProUGUI> Text;
    
    private List<string> _Name;

    private void Awake()
    {
        _Name = new List<string>();
        _Name.Add("Gold : ");
        _Name.Add("A : ");
        _Name.Add("B : ");
        _Name.Add("C : ");
        _Name.Add("D : ");
        _Name.Add("Score : ");

    }

    public void DrawInfo(bool isHighlight, List<int> text, int score)
    {
        if (isHighlight)
            Highlight.color = Color.red;
        else
            Highlight.color = Color.white;
        for(int i = 0; i < text.Count; i++)
            Text[i].text = _Name[i] + text[i].ToString();
        Text[5].text = _Name[5] + score.ToString();
    }

}
