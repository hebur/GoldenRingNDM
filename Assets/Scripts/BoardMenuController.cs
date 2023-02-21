using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardMenuController : MonoBehaviour
{
    [SerializeField] GameObject BoardMenuCanvas;

    public void Btn_CallMenuPopup()
    {
        BoardMenuCanvas.SetActive(true);
        TableManager.instance.CardMouseEffectOn = false;
    }

    public void Btn_Apsolutely()
    {
        SceneManager.LoadScene("MainMenu");
        TableManager.instance.CardMouseEffectOn = true;
    }

    public void Btn_Noway()
    {
        BoardMenuCanvas.SetActive(false);
        TableManager.instance.CardMouseEffectOn = true;
    }

}
