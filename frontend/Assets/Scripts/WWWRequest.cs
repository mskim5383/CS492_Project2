using UnityEngine;
using System.Collections;
using LitJson;

namespace Assets.Scripts
{
    class WWWRequest
    {
        const string server_url = "http://127.0.0.1:8000";

        GameStatus status;
        public WWWRequest(GameStatus status)
        {
            this.status = status;
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
    }
}
