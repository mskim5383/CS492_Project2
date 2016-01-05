using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour {
    GameStatus gameStatus;
    WWWRequest request;
    ArrayList rooms; // list of room numbers (integer)
    int i = 0;

    ArrayList roomButtons;
    ArrayList roomLabels;
    
    public GUIStyle titleStyle;
    public GUIStyle pageNumStyle;

    public int roomPageNum = 0;
    public int roomCount = 0;
    string []roomTitles = new string[4] { "", "", "", "" };
    

    void OnGUI ()
    {
        GUI.Label(new Rect(30, 20, 200, 30), "Mighty", titleStyle);
        if (GUI.Button(new Rect(Screen.width-90, 10, 80, 30), "Refresh"))
        {
            Debug.Log("refresh with " + i + " times");
            i++;
            refreshRoomNumbers();
        }

        GUILayout.BeginArea(new Rect(30, 50, 600, 400));
        if (GUILayout.Button(roomTitles[0]) && (roomPageNum * 4 + 0 < roomCount))
        {
            gameStatus.room_id = gameStatus.rooms[roomPageNum * 4 + 0].ToString();
            SceneManager.LoadScene("Main");
        }
        if (GUILayout.Button(roomTitles[1]) && (roomPageNum * 4 + 1 < roomCount))
        {
            gameStatus.room_id = gameStatus.rooms[roomPageNum * 4 + 1].ToString();
            SceneManager.LoadScene("Main");

        }
        if (GUILayout.Button(roomTitles[2]) && (roomPageNum * 4 + 2 < roomCount))
        {
            gameStatus.room_id = gameStatus.rooms[roomPageNum * 4 + 2].ToString();
            SceneManager.LoadScene("Main");

        }
        if (GUILayout.Button(roomTitles[3]) && (roomPageNum * 4 + 3 < roomCount))
        {
            gameStatus.room_id = gameStatus.rooms[roomPageNum * 4 + 3].ToString();
            SceneManager.LoadScene("Main");
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(100, 20, 300, 40));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<") && roomPageNum > 0)
        {
            roomPageNum -= 1;
        }
        GUILayout.Label("" + (roomPageNum + 1) + " / " + (roomCount / 4 + 1));
        if (GUILayout.Button(">") && (roomCount / 4) > roomPageNum)
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
        StartCoroutine(request.RequestUser());
        refreshRoomNumbers();

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
        ArrayList newRooms = gameStatus.rooms;
        if (newRooms != rooms)
        {
            Debug.Log("Update happened!");
            roomCount = newRooms.Count;
            for (int j = 0; j < 4; j++)
            {
                if (roomPageNum * 4 + j >= roomCount)
                {
                    roomTitles[j] = "Unavailable";
                }
                else
                {
                    roomTitles[j] = "Room #" + gameStatus.rooms[roomPageNum * 4 + j] + "    " + gameStatus.roomMemCount[roomPageNum * 4 + j] + "/5";
                }
            }
        }
	}

    void refreshRoomNumbers ()
    {
        Debug.Log("Refreshing...");
        StartCoroutine(request.RequestRooms());
       
    }
}
