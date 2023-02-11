using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class Network
{
    private readonly string address = Network_Secret.getserveraddress();

    public struct Player
    {
        public string nickname;
        public string room_code;
        public List<int> cards;

        public Player(string nickname, string room_code, List<int> cards)
        {
            this.nickname = nickname;
            this.room_code = room_code;
            this.cards = cards;
        }
    }
    public struct Room
    {
        public string code;
        public string title;
        public List<int> deck;
        public int player_num;
        public int turninfo;
        public List<Player> players;

        public Room(string code, string title, List<int> deck, int player_num, int turninfo, List<Player> players)
        {
            this.code = code;
            this.title = title;
            this.deck = deck;
            this.player_num = player_num;
            this.turninfo = turninfo;
            this.players = players;
        }
    }

    // POST
    public async Task<string> PostNewRoom(int player_num, string room_title)
    {
        string room_code = "";

        WWWForm form = new WWWForm();
        form.headers["Accept"] = "application/json";
        string url = address + "/rooms/" + player_num + "/?room_title=" + room_title;

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
    public async Task<List<Room>> GetRooms()
    {
        List<Room> rooms = new List<Room>();
        string url = address + "/rooms/";

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);

            string jsonString = request.downloadHandler.text;
            int firstIndex = jsonString.IndexOf("\"");
            jsonString = jsonString.Remove(firstIndex, "\"".Length);
            int lastIndex = jsonString.LastIndexOf("\"");
            jsonString = jsonString.Remove(lastIndex, "\"".Length);

            rooms = StoJRooms(jsonString);
        }
        else
        {
            Debug.Log("error");
        }

        return rooms;
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
    public async Task<Room> GetRoomInfo(string room_code)
    {
        Room room = new Room();
        string url = address + "/rooms/" + room_code;

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            room = StoJRoom(jsonString);
        }
        else
        {
            Debug.Log("error");
        }

        return room;
    }
    public async Task<List<Player>> GetPlayers()
    {
        List<Player> players = new List<Player>();
        string url = address + "/players/";

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);

            string jsonString = request.downloadHandler.text;

            players = StoJPlayers(jsonString);
        }
        else
        {
            Debug.Log("error");
        }

        return players;
    }
    public async Task<Player> GetPlayerInfo(string nickname)
    {
        Player player = new Player();
        string url = address + "/players/" + nickname;

        UnityWebRequest request = UnityWebRequest.Get(url);

        var op = request.SendWebRequest();
        while (!op.isDone)
        { await Task.Yield(); }

        if (request.error == null)
        {
            Debug.Log(request.downloadHandler.text);
            var jsonString = request.downloadHandler.text;
            //player = JsonUtility.FromJson<Player>(jsonString);
        }
        else
        {
            Debug.Log("error");
        }

        return player;
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


    // JsonString to struct
    private List<Room> StoJRooms(string str)
    {
        if (str.Length == 2)
        {
            return new List<Room>();
        }

        List<Room> rooms = new List<Room>();

        int firstindex = -1, lastindex = -1;
        int count = 0;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '{')
            {
                count++;
                if (firstindex == -1)
                {
                    firstindex = i;
                }
            }
            else if (str[i] == '}')
            {
                count--;
                if (count == 0)
                {
                    lastindex = i;
                    rooms.Add(StoJRoom(str.Substring(firstindex, lastindex - firstindex + 1)));
                    firstindex = lastindex = -1;
                }
            }
        }

        return rooms;
    }
    private Room StoJRoom(string str)
    {
        string temp;
        int firstindex, lastindex;

        string code;
        string title;
        List<int> deck;
        int player_num;
        int turninfo;
        List<Player> players;

        //deck
        firstindex = str.IndexOf("[");
        lastindex = str.IndexOf(']');
        temp = str.Substring(firstindex + 1, lastindex - firstindex - 1);
        deck = new List<int>();
        if (temp.Length > 0)
        {
            string[] deckS = temp.Split(",");
            for (int i = 0; i < deckS.Length; i++)
            {
                deck.Add(Int32.Parse(deckS[i]));
            }
        }

        str = str.Substring(lastindex + 2);

        //code
        code = str.Substring(8, 6);

        str = str.Substring(str.IndexOf(",") + 1);

        //title
        firstindex = str.IndexOf(",");
        title = str.Substring(9, firstindex - 10);

        str = str.Substring(firstindex + 1);

        //player_num
        firstindex = str.IndexOf(",");
        player_num = Int32.Parse(str.Substring(13, firstindex - 13));

        str = str.Substring(firstindex + 1);

        //turninfo
        firstindex = str.IndexOf(",");
        turninfo = Int32.Parse(str.Substring(11, firstindex - 11));

        firstindex = str.IndexOf("[");
        str = str.Substring(firstindex);
        firstindex = str.LastIndexOf("}");
        str = str.Remove(firstindex);

        //players
        players = new List<Player>();
        if (str.Length != 2)
        {
            firstindex = str.IndexOf("{");
            while (firstindex >= 0)
            {
                firstindex = str.IndexOf("{");
                lastindex = str.IndexOf("}");
                if (firstindex < 0)
                    break;
                players.Add(StoJPlayer(str.Substring(firstindex, lastindex - firstindex + 1)));
                str = str.Substring(lastindex + 1);
            }
        }

        //create room with above data
        Room room = new Room(code, title, deck, player_num, turninfo, players);

        return room;
    }
    private List<Player> StoJPlayers(string str)
    {
        if (str.Length == 2)
        {
            return new List<Player>();
        }

        List<Player> players = new List<Player>();

        int firstindex = -1, lastindex = -1;

        firstindex = str.IndexOf("{");
        while (firstindex >= 0)
        {
            firstindex = str.IndexOf("{");
            lastindex = str.IndexOf("}");
            if (firstindex < 0)
                break;
            Debug.Log(str.Substring(firstindex, lastindex - firstindex + 1));
            players.Add(StoJPlayer(str.Substring(firstindex, lastindex - firstindex + 1)));
            str = str.Substring(lastindex + 1);
        }

        return players;
    }
    private Player StoJPlayer(string str)
    {
        string temp;
        int firstindex, lastindex;

        string nickname;
        string room_code;
        List<int> cards;

        //nickname
        firstindex = str.IndexOf(",");
        nickname = str.Substring(13, firstindex - 14);

        str = str.Substring(firstindex + 1);

        //room_code
        room_code = str.Substring(13, 6);

        str = str.Substring(str.IndexOf(",") + 1);

        //cards
        firstindex = str.IndexOf("[");
        lastindex = str.IndexOf(']');
        temp = str.Substring(firstindex + 1, lastindex - firstindex - 1);
        cards = new List<int>();
        if (temp.Length > 0)
        {
            string[] deckS = temp.Split(",");
            for (int i = 0; i < deckS.Length; i++)
            {
                cards.Add(Int32.Parse(deckS[i]));
            }
        }
        Player player = new Player(nickname, room_code, cards);

        return player;
    }
}