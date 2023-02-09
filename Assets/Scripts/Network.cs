using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
using System.IO;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using System;

public class Network
{
    private readonly string address = Network_Secret.getserveraddress();

    public class NN
    {
        public string nickname;
    }

    // POST
    public async Task<string> PostNewRoom(int player_num)
    {
        string room_code = "";

        WWWForm form = new WWWForm();
        form.headers["Accept"] = "application/json";
        string url = address + "/rooms/" + player_num;

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            room_code = request.downloadHandler.text.Replace("\"", "");
        }
        else
        { Debug.Log("error"); }

        return room_code;
    }
    public async void PostNewPlayer(string nickname)
    {
        WWWForm form = new WWWForm();
        form.headers["Accept"] = "application/json";
        string url = address + "/players/" + nickname;

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        { Debug.Log(request.downloadHandler.text); }
        else
        { Debug.Log(request.error); }
    }

    // GET
    /*public async Task<string> GetNewRoomCode(string nickname)
    {
        string room_code = null;
        string url = address + "/rooms/players/?nickname=" + nickname;
        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            room_code = request.downloadHandler.text;
            //var jsonString = request.downloadHandler.text;
            //var dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
            //room_code = (string)dict["room_code"];
            //File.WriteAllText("./data/data.txt", room_code); // 파일 입출력 확인 필요
        }
        else
        {
            Debug.Log("error");
        }

        return room_code;
    }*/
    public async Task<Dictionary<string, object>> GetRooms()
    {
        var dict = new Dictionary<string, object>();
        string url = address + "/rooms/";

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            string jsonString = request.downloadHandler.text;
            /*int firstIndex = jsonString.IndexOf("\"");            
            jsonString = jsonString.Remove(firstIndex, "\"".Length);
            int lastIndex = jsonString.IndexOf("\"");
            jsonString = jsonString.Remove(lastIndex, "\"".Length);*/
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
        int turninfo = -1;
        string url = address + "/rooms/" + room_code + "/turninfo/";

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            turninfo = Int32.Parse(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }

        return turninfo;
    }
    public async Task<Dictionary<string, object>> GetRoomInfo(string room_code)
    {
        var dict = new Dictionary<string, object>();
        string url = address + "/rooms/" + room_code;

        UnityWebRequest request = UnityWebRequest.Get(url);

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
    public async Task<Dictionary<string, object>> GetPlayerInfo(string nickname)
    {
        var dict = new Dictionary<string, object>();
        string url = address + "/players/" + nickname;

        UnityWebRequest request = UnityWebRequest.Get(url);

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
    public async Task<Dictionary<string, object>> GetRoomInfo(string room_code)
    {
        var dict = new Dictionary<string, object>();

        UnityWebRequest request = UnityWebRequest.Get(address + "/rooms/" + room_code);

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
    public async Task<Dictionary<string, object>> GetPlayerInfo(string nickname)
    {
        var dict = new Dictionary<string, object>();

        UnityWebRequest request = UnityWebRequest.Get(address + "/players/" + nickname);

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
    public async Task<bool> GetisRoomFull(string room_code)
    {
        bool isfull = false;
        string url = address + "/rooms/" + room_code + "/isfull/";

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            string result = request.downloadHandler.text;
            if (result == "true")
            { isfull = true; }
            else
            { isfull = false; }
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
        string url = address + "/players/" + nickname;

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();

        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log("success");
            Debug.Log(request.downloadHandler.text);
            if (request.downloadHandler.text == "true")
            { isexist = true; }
            else
            { isexist = false; }
        }
        else
        {
            Debug.Log(request.error);
        }

        return isexist;
    }

    // PUT
    public async void PutPlayerToRoom(string room_code, string nickname)
    {
        string url = address + "/rooms/" + room_code + "/?nickname=" + nickname;
        string str = "";
        UnityWebRequest request = UnityWebRequest.Put(url, str);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
    public async void PutTurnEnd(string room_code)
    {
        string str = "";
        string url = address + "/rooms/" + room_code + "/turnend/";

        UnityWebRequest request = UnityWebRequest.Put(url, str);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }
    }
    public async void PutCardToPlayer(string nickname, int act, int card_num, string room_code)
    {
        string str = "";
        string url = address + "/players/" + nickname + "/cards/"
            + "?act=" + act + "&card_num=" + card_num + "&room_code=" + room_code;
        UnityWebRequest request = UnityWebRequest.Put(url, str);

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