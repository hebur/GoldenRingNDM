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
        //GET ���
        string url = "https://api.neople.co.kr/df/servers?apikey="

        // UnityWebRequest�� ����Ǿ� �ִ� GET �޼ҵ带 ����Ѵ�.
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest(); // ������ �� ������ ���

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
        // POST ���
        string url = "--server url--";
        WWWForm form = new WWWForm();
        string id = "--���̵�--";
        string pw = "--��й�ȣ--";
        form.AddField("Username", id);
        form.AddField("Password", pw);
        UnityWebRequest www = UnityWebRequest.Post(url, form); // ���� �ּҿ� ������

        yield return www.SendWebRequest(); // ���� �� ������ ���

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