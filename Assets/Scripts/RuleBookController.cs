using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBookController : MonoBehaviour
{
    [SerializeField] private GameObject RuleBookHolder;
    [SerializeField] private List<GameObject> Rules;

    [SerializeField] private bool isOpenRule;
    [SerializeField] private int RuleNowPage;

    private void Start()
    {
        isOpenRule = false;
        RuleBookHolder.SetActive(false);
        RuleNowPage = 0;
        for (int i = 0; i < Rules.Count; i++)
            Rules[i].SetActive(false);
    }

    private void SoundClick()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayAudio(SoundType.UIBtn);
    }

    public void BTN_CallRuleBook()
    {
        SoundClick();
        isOpenRule = true;
        //·êºÏÀ» ¿±´Ï´Ù.
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
                isOpenRule = false;
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
                isOpenRule = false;
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
