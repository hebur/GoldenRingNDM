using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
using System.IO;

public class NetworkTest : MonoBehaviour
{
    // StartCoroutine(UnityWebRequestGETTest());

    private readonly string address = Network_Secret.getserveraddress();
    
    // POST
    IEnumerator PostNewRoom(int player_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_id", player_id);
        UnityWebRequest request = UnityWebRequest.Post(address + "/rooms/",form);
    
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }
    IEnumerator PostNewPlayer(string nickname)
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);
        UnityWebRequest request = UnityWebRequest.Post(address + "/players/", form);

        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }

    // GET
    IEnumerator GetNewRoomCode(int player_id, string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Get(address + "/players/" + player_id); // 에러 시 주소 확인
        
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            room_code = (string)dict["room_code"];
            File.WriteAllText("./data/data.txt", room_code); // 파일 입출력 확인 필요
        }
        else
        {
            Debug.Log("error");
        }
    }
    IEnumerator GetRooms(int player_id, string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Get(address + "/players/" + player_id); // 에러 시 주소 확인

        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            room_code = (string)dict["room_code"];
            File.WriteAllText("./data/data.txt", room_code); // 파일 입출력 확인 필요
        }
        else
        {
            Debug.Log("error");
        }
    }
    IEnumerator GetTurninfo(int player_id, string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Get(address + "/players/" + player_id); // 에러 시 주소 확인

        yield return request.SendWebRequest();

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            room_code = (string)dict["room_code"];
            File.WriteAllText("./data/data.txt", room_code); // 파일 입출력 확인 필요
        }
        else
        {
            Debug.Log("error");
        }
    }

    // PUT
    IEnumerator PutPlayerToRoom(string room_code)
    {
        byte[] bytes = new byte[10];
        WWWForm form = new WWWForm();
        form.AddField("room_code", room_code);
        UnityWebRequest request = UnityWebRequest.Put(address, bytes);

        yield return request.SendWebRequest();
    }
    IEnumerator PutTurnEnd(string room_code)
    {
        byte[] bytes = new byte[10];
        WWWForm form = new WWWForm();
        form.AddField("room_code", room_code);
        UnityWebRequest request = UnityWebRequest.Put(address, bytes);

        yield return request.SendWebRequest();
    }
    IEnumerator PutCardToPlayer(string nickname)
    {
        byte[] bytes = new byte[10];
        UnityWebRequest request = UnityWebRequest.Put(address, bytes);

        yield return request.SendWebRequest();
    }

    // DELETE
    IEnumerator DeleteRoom(string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Delete(address);

        yield return request.SendWebRequest();
    }
    IEnumerator DeletePlayer(string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Delete(address);

        yield return request.SendWebRequest();
    }
}