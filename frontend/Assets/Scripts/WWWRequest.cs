using UnityEngine;
using System.Text;
using Assets.Scripts.Card;
using System;
using UnityEngine.SceneManagement;
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
                WWW www = new WWW(server_url + "/" + status.id+"/");
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    JsonData jResponse = JsonMapper.ToObject(www.text);
                    Debug.Log("Response successful");
                    JsonData jWaitingRooms = jResponse["waiting_rooms"];
                    Debug.Log("waiting_rooms found " + jWaitingRooms.IsArray);
                    ArrayList aWaitingRooms = new ArrayList(); /* new dynamic list */
                    ArrayList aWaitMemberCount = new ArrayList();
                    int tmp;
                    for (int i = 0; i < jWaitingRooms.Count; i++)
                    {
                        if (jWaitingRooms[i]["status"].Equals(0))
                        {
                            Debug.Log("[STATUS] status is zero");
                            aWaitingRooms.Add(jWaitingRooms[i]["id"]);
                            tmp = 0;
                            for (int p = 0; p < 5; p++)
                            {
                                if (jWaitingRooms[i]["players"]["player"+p]["id"] == null)
                                {
                                    Debug.Log("player p not null :" + p);
                                }
                                else
                                {
                                    Debug.Log("player p null :" + p);
                                    tmp += 1;
                                }
                            }
                            aWaitMemberCount.Add(tmp);
                        }
                        else
                        {
                            Debug.Log("[STATUS] status is not zero");
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

        bool reqLock = false;
        public IEnumerator RequestGameStatus()
        {
            string url = server_url + "/" + status.id + "/" + status.room_id;
            if (reqLock) yield return url;
            else
            {
                reqLock = true;
                WWW www = new WWW(url);
                yield return www;
                status.jsonData = JsonMapper.ToObject(www.text);
                try
                {
                    status.nowStatus = (int)status.jsonData["game_status"]["status"];
                }
                catch (Exception)
                {
                    Debug.Log(www.text);
                }
                status.dirty = true;
                reqLock = false;
            }
        }

        public IEnumerator RequestContract(bool pass, string face = "", int number = 0)
        {
            string url = server_url + "/" + status.id + "/" + status.room_id + "/";
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            {
                if (pass)
                {
                    writer.WritePropertyName("action");
                    writer.Write("pass");
                }
                else
                {
                    writer.WritePropertyName("action");
                    writer.Write("contract");

                    writer.WritePropertyName("face");
                    writer.Write(face);

                    writer.WritePropertyName("number");
                    writer.Write(number);
                }
            }
            writer.WriteObjectEnd();

            Debug.Log(sb.ToString());
            WWW www = new WWW(url, Encoding.UTF8.GetBytes(sb.ToString()));
            yield return www;
            status.jsonData = JsonMapper.ToObject(www.text);
            status.nowStatus = (int)status.jsonData["game_status"]["status"];
            status.dirty = true;
        }

        public IEnumerator RequestFriend(int friend, Mark sMark, int sC, int select)
        {
            string url = server_url + "/" + status.id + "/" + status.room_id + "/";
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            {
                writer.WritePropertyName("action");
                writer.Write("friend");
                writer.WritePropertyName("friend");
                writer.Write(friend);
                if (friend == 2)
                {
                    writer.WritePropertyName("face");
                    writer.Write(sMark.ToString());
                    if (sMark != Mark.JK)
                    {
                        writer.WritePropertyName("value");
                        writer.Write(sC);
                    }
                }
                else if (friend == 3)
                {
                    writer.WritePropertyName("select");
                    writer.Write(select);
                }
            }
            writer.WriteObjectEnd();

            Debug.Log(sb.ToString());
            WWW www = new WWW(url, Encoding.UTF8.GetBytes(sb.ToString()));
            yield return www;
            status.jsonData = JsonMapper.ToObject(www.text);
            status.nowStatus = (int)status.jsonData["game_status"]["status"];
            status.dirty = true;
        }

        public IEnumerator RequestRemain(string cards)
        {
            string url = server_url + "/" + status.id + "/" + status.room_id + "/";
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            {
                writer.WritePropertyName("action");
                writer.Write("remain");
                writer.WritePropertyName("cards");
                writer.Write(cards);
            }
            writer.WriteObjectEnd();

            Debug.Log(sb.ToString());
            WWW www = new WWW(url, Encoding.UTF8.GetBytes(sb.ToString()));
            yield return www;
            status.jsonData = JsonMapper.ToObject(www.text);
            status.nowStatus = (int)status.jsonData["game_status"]["status"];
            status.dirty = true;
        }


        public IEnumerator RequestThrow(string card, string face=null)
        {
            string url = server_url + "/" + status.id + "/" + status.room_id + "/";
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            {
                writer.WritePropertyName("action");
                writer.Write("throw");
                writer.WritePropertyName("card");
                writer.Write(card);
            }
            writer.WriteObjectEnd();

            Debug.Log(sb.ToString());
            WWW www = new WWW(url, Encoding.UTF8.GetBytes(sb.ToString()));
            yield return www;
            status.jsonData = JsonMapper.ToObject(www.text);
            status.nowStatus = (int)status.jsonData["game_status"]["status"];
            status.dirty = true;
        }

        public IEnumerator RequestJC(string card)
        {
            string url = server_url + "/" + status.id + "/" + status.room_id + "/";
            StringBuilder sb = new StringBuilder();
            JsonWriter writer = new JsonWriter(sb);
            writer.WriteObjectStart();
            {
                writer.WritePropertyName("action");
                writer.Write("joker_call");
                writer.WritePropertyName("card");
                writer.Write(card);
            }
            writer.WriteObjectEnd();

            Debug.Log(sb.ToString());
            WWW www = new WWW(url, Encoding.UTF8.GetBytes(sb.ToString()));
            yield return www;
            status.jsonData = JsonMapper.ToObject(www.text);
            status.nowStatus = (int)status.jsonData["game_status"]["status"];
            status.dirty = true;
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
