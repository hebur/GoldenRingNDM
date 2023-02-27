using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    [SerializeField] private GameObject QuitConfirm;

    [SerializeField] private GameObject RuleBookHolder, title;
    [SerializeField] private List<GameObject> Rules;

    [SerializeField] private bool isOpenRule;
    [SerializeField] private int RuleNowPage;

    private int version = 0;

    public int Version { get { return version; } set => version = value; }

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {

    }


    private void SoundClick()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayAudio(SoundType.UIBtn);
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
}
