using UnityEngine;
using System.Collections;
using LitJson;

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
                WWW www = new WWW(server_url + "/" + status.id);
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    JsonData jResponse = JsonMapper.ToObject(www.text);
                    Debug.Log("Response successful");
                    JsonData jWaitingRooms = jResponse["waiting_rooms"];
                    Debug.Log("waiting_rooms found " + jWaitingRooms.IsArray);
                    ArrayList aWaitingRooms = new ArrayList(); /* new dynamic list */
                    for (int i = 0; i < jWaitingRooms.Count; i++)
                    {
                        if (jWaitingRooms[i]["status"].Equals(0))
                            aWaitingRooms.Add(jWaitingRooms[i]["id"]);
                    }
                    this.status.rooms = aWaitingRooms; /* is lock necessary? is it ok to memory? */
                }
            }
            else
            {
                Debug.Log("status id is null");
            }

        }
    }
}
