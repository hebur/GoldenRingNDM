using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���� � ����� ������ �����ݴϴ�
/// </summary>
public class PlayerInfoPanel : MonoBehaviour
{

    [SerializeField] private Image Highlight;
    [SerializeField] private List<TextMeshProUGUI> Text;
    
    private List<string> _Name;

    private void Awake()
    {
        _Name = new List<string>();
        _Name.Add(": ");
        _Name.Add(": ");
        _Name.Add(": ");
        _Name.Add(": ");
        _Name.Add(": ");
        _Name.Add("");

    }

    /// <summary>
    /// �÷��̾��� ���� ���� �ؽ�Ʈ�� ������Ʈ �մϴ�.
    /// ���� ������ �÷��̾�� ���������� ǥ��
    /// </summary>
    /// <param name="isHighlight"></param>
    /// <param name="text"></param>
    /// <param name="score"></param>
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
