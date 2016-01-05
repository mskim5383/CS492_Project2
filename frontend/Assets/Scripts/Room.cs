using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class Room : MonoBehaviour {
    GameStatus gameStatus;
    WWWRequest request;
    ArrayList rooms; // list of room numbers (integer)
    ArrayList roomMemCounts;

    ArrayList roomButtons;
    ArrayList roomLabels;
    
    public GUIStyle titleStyle;
    public GUIStyle pageNumStyle;

    public bool refreshedFirstTime = false;
    public int roomPageNum = 0;
    public int roomCount = 0;
    List<string> roomTitles;

    public int pageSize = 0;

    public bool goingToNextScene = false;
    public Texture image;

    void OnGUI ()
    {
        GUI.Label(new Rect(10, 10, 200, 50), new GUIContent(image));
        if (GUI.Button(new Rect(210, 15, 70, 40), "새로고침"))
        {
            refreshRoomNumbers();
        }
        if (GUI.Button(new Rect(290, 15, 70, 40), "방만들기"))
        {
            if (!goingToNextScene)
            {
                goingToNextScene = true;
                StartCoroutine(request.createRoom());
            }
        }

        for (int t = 0; t < pageSize; t++)
        {
            if (GUI.Button(new Rect(20, 80 + 40 * t, Screen.width - 40, 30), roomTitles[t]) && (roomPageNum * pageSize + t < roomCount))
            {
                if (!goingToNextScene)
                {
                    goingToNextScene = true;
                    gameStatus.room_id = gameStatus.rooms[roomPageNum * pageSize + t].ToString();
                    StartCoroutine(request.participateRoom());
                }
            }
        }

        GUILayout.BeginArea(new Rect(Screen.width - 100, 20, 80, 40));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<") && roomPageNum > 0)
        {
            roomPageNum -= 1;
        }
        GUILayout.Label("" + (roomPageNum + 1) + " / " + ((roomCount-1) / pageSize + 1), pageNumStyle);
        if (GUILayout.Button(">") && ((roomCount-1) / pageSize) > roomPageNum)
        {
            roomPageNum += 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
	// Use this for initialization
	void Start () {
        gameStatus = GameStatus.getInstance();
        request = new WWWRequest();
        rooms = new ArrayList();
        roomButtons = new ArrayList();
        roomLabels = new ArrayList();

        pageSize = (Screen.height - 90) / 40;
        roomTitles = new List<string>();
        for (int t = 0; t < pageSize; t++)
        {
            roomTitles.Add("");
        }
        StartCoroutine(request.RequestUser());

        /* */
        /* */
        //GameObject obj = new GameObject("Button");
        //obj.layer = layerUI;
        //Button button1 = obj.AddComponent<Button>();
        //GameObject Button1 = (GameObject) Instantiate(prefab);
        //Button1.transform.SetParent(parentPanel, false);
        //Button1.transform.localScale = new Vector3(1, 1, 1);

        //Button Button = Button1.GetComponent<Button>();
        //Button.name = "Button1";
        //Button.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        //Button.onClick.AddListener(listener);
        //roomButtons.Add(Button);
    }

    // Update is called once per frame
    void listener ()
    {
        Debug.Log("Button1");
    }
    void Update () {
        if (!refreshedFirstTime && !string.IsNullOrEmpty(gameStatus.id))
        {
            refreshedFirstTime = true;
            refreshRoomNumbers();
        }
        if (goingToNextScene == true)
            return;
        ArrayList newRooms = gameStatus.rooms;
        ArrayList newMemCounts = gameStatus.roomMemCounts;
        if (newRooms != null && newMemCounts != null && (newRooms != rooms || newMemCounts != roomMemCounts))
        {
            roomCount = newRooms.Count;
            if (roomCount != newMemCounts.Count)
                Debug.Log("[Error] roomCount " + roomCount + "!=" + newMemCounts.Count);
            for (int j = 0; j < pageSize; j++)
            {
                if (roomPageNum * pageSize + j >= roomCount)
                {
                    roomTitles[j] = "";
                }
                else
                {
                    roomTitles[j] = "방 #" + gameStatus.rooms[roomPageNum * pageSize + j] + "   인원: " + gameStatus.roomMemCounts[roomPageNum * pageSize + j] + "/5";
                }
            }
            rooms = newRooms;
        }
	}

    void refreshRoomNumbers ()
    {
        StartCoroutine(request.RequestRooms());
    }
}
