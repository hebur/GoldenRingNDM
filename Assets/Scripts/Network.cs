using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
using System.IO;
using System.Threading.Tasks;

public class Network
{
    private readonly string address = Network_Secret.getserveraddress();
    
    // POST
    public async void PostNewRoom(string nickname)
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);
        UnityWebRequest request = UnityWebRequest.Post(address+"/rooms/", form);
        
        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        { Debug.Log(request.downloadHandler.text); }
        else
        { Debug.Log("error"); }
    }
    public async void PostNewPlayer(string nickname)
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);
        UnityWebRequest request = UnityWebRequest.Post(address + "/players/", form);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        { Debug.Log(request.downloadHandler.text); }
        else
        { Debug.Log("error"); }
    }

    // GET
    public async Task<string> GetNewRoomCode(string nickname)
    {
        string room_code = null;
        UnityWebRequest request = UnityWebRequest.Get(address + "/players/" + nickname); // 에러 시 주소 확인

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            room_code = (string)dict["room_code"];
            //File.WriteAllText("./data/data.txt", room_code); // 파일 입출력 확인 필요
        }
        else
        {
            Debug.Log("error");
        }

        return room_code;
    }
    public async Task<Dictionary<string, object>> GetRooms()
    {
        var dict = new Dictionary<string, object>();

        UnityWebRequest request = UnityWebRequest.Get(address + "/rooms/");

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
        }
        else
        {
            Debug.Log("error");
        }

        return dict;
    }
    public async Task<int> GetTurninfo(string room_code)
    {
        int turninfo = 0;

        UnityWebRequest request = UnityWebRequest.Get(address + "/rooms/" + room_code + "/turninfo/"); // 에러 시 주소 확인

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            turninfo = (int)dict["turninfo"];
        }
        else
        {
            Debug.Log("error");
        }

        return turninfo;
    }
    public async Task<bool> GetisRoomFull(string room_code)
    {
        bool isfull = false;

        UnityWebRequest request = UnityWebRequest.Get(address + "/rooms/" + room_code);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            int player_num = (int)dict["player_num"];
            if(player_num == 4)
            { isfull = true; }
        }
        else
        {
            Debug.Log("error");
        }

        return isfull;
    }
    public async Task<bool> GetisNNExist(string nickname)
    {
        bool isexist = false;

        UnityWebRequest request = UnityWebRequest.Get(address + "/players/" + nickname );

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            isexist = (bool)dict["isexist"];
        }
        else
        {
            Debug.Log("error");
        }

        return isexist;
    }
    
    // PUT
    public async void PutPlayerToRoom(string room_code, string nickname)
    {
        UnityWebRequest request = UnityWebRequest.Put(address + "/rooms/" + room_code, nickname);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
    public async void PutTurnEnd(string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Put(address + "/" + room_code, room_code);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
    public async void PutCardToPlayer(string nickname, int act, int card)  // str 추가 예정
    {
        string str = "";
        UnityWebRequest request = UnityWebRequest.Put(address, str);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
    
    // DELETE
    public async void DeleteRoom(string room_code)
    {
        UnityWebRequest request = UnityWebRequest.Delete(address + "/rooms/" + room_code);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
    public async void DeletePlayer(string nickname)
    {
        UnityWebRequest request = UnityWebRequest.Delete(address + "/players/" + nickname);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
}