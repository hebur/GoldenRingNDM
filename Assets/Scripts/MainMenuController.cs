using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject ExitText, QuitConfirm;

    [SerializeField] private GameObject RuleBookHolder, title;
    [SerializeField] private List<GameObject> Rules;

    [SerializeField] private bool isOpenRule;
    [SerializeField] private int RuleNowPage;

    private void Start()
    {
        ExitText.SetActive(false);
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

    public void BTN_CallExit()
    {
        StartCoroutine(ExitAuto());
        SoundClick();
    }

    public void BTN_CallStart(string value)
    {
        switch (value)
        {
            case "LOCAL":
                SceneManager.LoadScene("Board");
                SoundClick();
                break;
            case "ONLINE":
                // SceneManager.LoadScene("OnlineLobby");
                SoundClick();
                break;
        }
    }

    public void BTN_CallQuit()
    {
        QuitConfirm.SetActive(true);
        SoundClick();
    }

    public void BTN_QuitOrCancel(string value)
    {
        switch (value)
        {
            case "QUIT":
                SoundClick();
                Application.Quit();
                break;
            case "CANCEL":
                QuitConfirm.SetActive(false);
                SoundClick();
                break;
        }
    }

    private IEnumerator ExitAuto()
    {
        ExitText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 20; i++)
        {
            if (i % 2 != 0)
                ExitText.gameObject.SetActive(true);
            else
                ExitText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.05f);

        }

        ExitText.gameObject.SetActive(false);
    }

    public void BTN_CallRuleBook()
    {
        SoundClick();
        isOpenRule = true;
        //·êºÏÀ» ¿±´Ï´Ù.
        RuleBookHolder.SetActive(true);
        Rules[0].SetActive(true);
        title.SetActive(false);
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
