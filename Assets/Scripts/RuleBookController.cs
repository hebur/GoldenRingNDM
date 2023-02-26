using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RuleBookController : MonoBehaviour
{
    [SerializeField] private GameObject RuleBookHolder;
    [SerializeField] private Image RulePage;
    [SerializeField] private List<Sprite> Rules;

    private int RuleNowPage = 0;

    private void SoundClick()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayAudio(SoundType.UIBtn);
    }

    /// <summary>
    /// ∑Í∫œ¿ª ø±¥œ¥Ÿ.
    /// </summary>
    public void BTN_CallRuleBook()
    {
        RuleNowPage = 0;
        SoundClick();
        //∑Í∫œ¿ª ø±¥œ¥Ÿ.
        RuleBookHolder.SetActive(true);
        RulePage.sprite = Rules[0];
    }

    public void BTN_NextPage()
    {
        SoundClick();
        RuleNowPage++;
        if(RuleNowPage == Rules.Count )
        {
            RuleBookHolder.SetActive(false);
        }
        RulePage.sprite = Rules[RuleNowPage];
    }

    public void BTN_BackPage()
    {
        SoundClick();
        RuleNowPage--;
        if(RuleNowPage < 0)
        {
            RuleBookHolder.SetActive(false);
        }
        RulePage.sprite = Rules[RuleNowPage];
    }
}
