using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTest : MonoBehaviour
{
    private void Start()
    {
        //StartCoroutine(WWWGETTest());
        //StartCoroutine(WWWPOSTTest());
        StartCoroutine(UnityWebRequestGETTest());
    }

    IEnumerator UnityWebRequestGETTest()
    {
        //GET 방식
        string url = "https://api.neople.co.kr/df/servers?apikey=";


        // UnityWebRequest에 내장되어 있는 GET 메소드를 사용한다.
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest(); // 응답이 올 때까지 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }

    IEnumerator UnityWebRequestPOSTTest()
    {
        // POST 방식
        string url = "--server url--";
        WWWForm form = new WWWForm();
        string id = "--아이디--";
        string pw = "--비밀번호--";
        form.AddField("Username", id);
        form.AddField("Password", pw);
        UnityWebRequest www = UnityWebRequest.Post(url, form); // 보낼 주소와 데이터

        yield return www.SendWebRequest(); // 응답 올 때까지 대기

        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }

    //IEnumerator U
}