using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;

public class Room : MonoBehaviour {
    GameStatus gameStatus;
    WWWRequest request;
    ArrayList rooms; // list of room numbers (integer)
    int i = 0;

    ArrayList roomButtons;
    ArrayList roomLabels;

    public GameObject prefab;
    public RectTransform parentPanel;

    private int layerUI = 1;

    void OnGUI ()
    {
        GUI.Label(new Rect(10, 10, 200, 40), "Mighty");
        if (GUI.Button(new Rect(Screen.width-80, 10, 70, 40), "Refresh"))
        {
            Debug.Log("refresh with " + i + " times");
            i++;
            refreshRoomNumbers();
        }
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
        GameObject Button1 = (GameObject) Instantiate(prefab);
        Button1.transform.SetParent(parentPanel, false);
        Button1.transform.localScale = new Vector3(1, 1, 1);

        Button Button = Button1.GetComponent<Button>();
        Button.name = "Button1";
        Button.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        Button.onClick.AddListener(listener);
        roomButtons.Add(Button);
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
            //foreach ()
        }
	}

    void refreshRoomNumbers ()
    {
        Debug.Log("Refreshing...");
        StartCoroutine(request.RequestRooms());

    }
}
