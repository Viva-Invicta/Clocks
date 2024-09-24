using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Clocks
{
    public class TimeRequestService : MonoBehaviour
    {
        public event Action TimeUpdated;

        [SerializeField]
        private string _timeRequestUrl = "https://yandex.com/time/sync.json";

        public DateTime LastRecievedTime { get; private set; }

        public void UpdateTime()
        {
            StartCoroutine(GetRequest(_timeRequestUrl));
        }

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                else
                {
                    TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
                    LastRecievedTime = DateTimeOffset.FromUnixTimeMilliseconds(timeData.time).LocalDateTime;

                    TimeUpdated?.Invoke();
                }
        }
    }

        [Serializable]
        private class TimeData
        {
            public long time;
        }
    }
}
