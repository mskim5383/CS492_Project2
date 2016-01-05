using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class WWWRequest
    {
        const string server_url = "http://bit.sparcs.org:5383";

        GameStatus status;
        public WWWRequest()
        {
            this.status = GameStatus.getInstance();
        }

        public IEnumerator RequestUser()
        {
            WWW www = new WWW(server_url);
            yield return www;
            // www.error 값이 없다면 성공이다.
            if (string.IsNullOrEmpty(www.error))
            {
                JsonData response = JsonMapper.ToObject(www.text);
                status.id = response["id"].ToString();
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        public IEnumerator RequestRooms()
        {
            if (!string.IsNullOrEmpty(status.id))
            {
                WWW www = new WWW(server_url + "/" + status.id+"/");
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    JsonData jResponse = JsonMapper.ToObject(www.text);
                    JsonData jWaitingRooms = jResponse["waiting_rooms"];
                    ArrayList aWaitingRooms = new ArrayList(); /* new dynamic list */
                    ArrayList aWaitMemberCount = new ArrayList();
                    int tmp;
                    for (int i = 0; i < jWaitingRooms.Count; i++)
                    {
                        if (jWaitingRooms[i]["status"].Equals(0))
                        {
                            aWaitingRooms.Add(jWaitingRooms[i]["id"]);
                            tmp = 0;
                            for (int p = 0; p < 5; p++)
                            {
                                if (jWaitingRooms[i]["players"]["player"+p]["id"] == null)
                                {
                                }
                                else
                                {
                                    tmp += 1;
                                }
                            }
                            aWaitMemberCount.Add(tmp);
                        }
                        else
                        {
                        }
                    }
                    this.status.rooms = aWaitingRooms; /* is lock necessary? is it ok to memory? */
                    this.status.roomMemCounts = aWaitMemberCount;
                }
            }
            else
            {
                Debug.Log("status id is null");
            }

        }
        public IEnumerator createRoom()
        {
            WWW www = new WWW(server_url + "/" + status.id + "/create_room/");
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                JsonData response = JsonMapper.ToObject(www.text);
                status.room_id = response["id"].ToString();
                SceneManager.LoadScene("Scenes/Main");
            }
            else
            {
                Debug.Log(www.error);
            }
        }
        public IEnumerator participateRoom()
        {
            WWW www = new WWW(server_url + "/" + status.id + "/" + status.room_id+"/");
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                JsonData response = JsonMapper.ToObject(www.text);
                SceneManager.LoadScene("Scenes/Main");

            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}
