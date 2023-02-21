using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBookController : MonoBehaviour
{
    [SerializeField] private GameObject RuleBookHolder;
    [SerializeField] private List<GameObject> Rules;

    [SerializeField] private int RuleNowPage;

    private void SoundClick()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayAudio(SoundType.UIBtn);
    }

    public void BTN_CallRuleBook()
    {
        RuleNowPage = 0;
        SoundClick();
        //∑Í∫œ¿ª ø±¥œ¥Ÿ.
        RuleBookHolder.SetActive(true);
        Rules[0].SetActive(true);
    }

    public void BTN_ContinueRule()
    {
        SoundClick();
        switch (RuleNowPage)
        {
            case 0:
                Rules[0].SetActive(false);
                Rules[1].SetActive(true);
                RuleNowPage++;
                break;
            case 1:
                Rules[1].SetActive(false);
                Rules[2].SetActive(true);
                RuleNowPage++;
                break;
            case 2:
                Rules[2].SetActive(false);
                RuleNowPage = 0;
                RuleBookHolder.SetActive(false);
                return;

            default:
                break;
        }
    }

    public void BTN_BackRulePage()
    {
        SoundClick();
        switch (RuleNowPage)
        {
            case 0:
                Rules[0].SetActive(false);
                RuleNowPage = 0;
                RuleBookHolder.SetActive(false);
                return;
            case 1:
                Rules[1].SetActive(false);
                Rules[0].SetActive(true);
                RuleNowPage--;
                break;
            case 2:
                Rules[2].SetActive(false);
                Rules[1].SetActive(true);
                RuleNowPage--;
                break;
            default:
                break;
        }
    }
}
