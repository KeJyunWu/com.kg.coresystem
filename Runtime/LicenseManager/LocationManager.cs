using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System;

public class LocationManager : MonoBehaviour
{
    [Header("Debug")]
    // 這些變數是公開的，NotionManager 等一下會來這裡拿資料
    [ReadOnly]
    public string currentIP;
    [ReadOnly]
    public string currentCountry;
    [ReadOnly]
    public string currentCity;
    [ReadOnly]
    public string currentTimeZone;
    [ReadOnly]
    public double currentLat;
    [ReadOnly]
    public double currentLon;

    [Header("事件")]
    [Tooltip("當位置成功更新後觸發")]
    public UnityEvent onLocationUpdated;

    [Tooltip("發生錯誤時觸發 (參數為錯誤訊息)")]
    public UnityEvent<string> onError;

    // 定義 JSON 解析用的內部結構
    [Serializable]
    private class GeoData
    {
        public string status;
        public string country;
        public string city;
        public string timezone;
        public double lat;
        public double lon;
        public string query;
    }

    // 開放給外部手動呼叫 (例如按鈕重整)
    public void FetchLocation()
    {
        Debug.Log("[LocationManager] 開始位置請求...");
        StartCoroutine(GetLocationRoutine());
    }

    IEnumerator GetLocationRoutine()
    {
        Debug.Log("[LocationManager] 開始獲取位置...");

        using (UnityWebRequest request = UnityWebRequest.Get("http://ip-api.com/json/"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = $"請求失敗: {request.error}";
                Debug.LogError(errorMsg);
                onError?.Invoke(errorMsg);
                yield break;
            }

            try
            {
                // 解析資料
                GeoData data = JsonUtility.FromJson<GeoData>(request.downloadHandler.text);

                if (data.status == "success")
                {
                    // 填入 Public 參數
                    currentIP = data.query;
                    currentCountry = data.country;
                    currentCity = data.city;
                    currentTimeZone = data.timezone;
                    currentLat = data.lat;
                    currentLon = data.lon;

                    Debug.Log($"[LocationManager] 位置更新成功: {currentCity}");

                    // 通知大家：資料準備好了！
                    onLocationUpdated?.Invoke();
                }
                else
                {
                    string errorMsg = "API 回傳狀態非 success";
                    Debug.LogWarning(errorMsg);
                    onError?.Invoke(errorMsg);
                }
            }
            catch (Exception e)
            {
                string errorMsg = $"JSON 解析錯誤: {e.Message}";
                Debug.LogError(errorMsg);
                onError?.Invoke(errorMsg);
            }
        }
    }
}