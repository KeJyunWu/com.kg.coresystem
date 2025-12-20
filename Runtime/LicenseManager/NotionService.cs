using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NotionService : MonoBehaviour
{
    public static NotionService Instance { get; private set; }

    private void Awake()
    {
        // 實作單例模式 (Singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切換場景時不銷毀
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 公開方法：接收資料並開始上傳
    /// </summary>
    /// <param name="data">要上傳的訪客資料</param>
    /// <param name="onComplete">上傳完成後的回呼 (成功/失敗)</param>
    public void UploadVisitorLog(VisitorData data, Action<bool> onComplete = null)
    {
        StartCoroutine(PostCoroutine(data, onComplete));
    }

    private IEnumerator PostCoroutine(VisitorData data, Action<bool> callback)
    {
        // 修正 1: 網址必須包含通訊協定 https://
        string proxyUrl = "https://unity-notion-proxy-artwork-activity.kejyunwu.workers.dev";

        // 1. 建構 JSON Payload
        string jsonPayload = BuildNotionJson(data);

        Debug.Log($"[NotionService] 準備上傳資料... \n網址: {proxyUrl}");

        using (UnityWebRequest request = new UnityWebRequest(proxyUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            // 發送請求並等待
            yield return request.SendWebRequest();

            // --- 修正 2: 這裡是你原本缺少的處理邏輯 ---

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 成功：印出 Cloudflare/Notion 回傳的訊息
                string responseText = request.downloadHandler.text;
                Debug.Log($"<color=green>[NotionService] 上傳成功！</color>\n回應: {responseText}");

                // 通知外部成功
                callback?.Invoke(true);
            }
            else
            {
                // 失敗：印出錯誤原因 (包含我們在 Cloudflare 寫的自定義錯誤訊息)
                string errorText = request.downloadHandler.text;
                Debug.LogError($"[NotionService] 上傳失敗！");
                Debug.LogError($"Unity 錯誤: {request.error}");
                Debug.LogError($"伺服器回應: {errorText}"); // 這一行最重要，它會顯示 Cloudflare 告訴你的錯誤

                // 通知外部失敗
                callback?.Invoke(false);
            }
        }
    }

    // 將資料封裝成 Notion 需要的 JSON 格式
    private string BuildNotionJson(VisitorData data)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("{");
        sb.Append($"\"parent\": {{ \"database_id\": \"{data.m_notiondataid}\" }},");
        sb.Append("\"properties\": {");

        sb.Append(CreateTitleProperty("Artwork", data.m_artworkName) + ",");
        sb.Append(CreateUrlProperty("Project URL", data.m_projectURL) + ",");
        sb.Append(CreateRichTextProperty("IP", data.m_ip) + ",");
        sb.Append(CreateRichTextProperty("Country", data.m_country) + ",");
        sb.Append(CreateRichTextProperty("City", data.m_city) + ",");
        sb.Append(CreateRichTextProperty("Time", data.m_time) + ",");
        sb.Append(CreateRichTextProperty("Computer Name", data.m_computerName) + ",");
        sb.Append(CreateRichTextProperty("Computer Fingerprint", data.m_computerFigerprint) + ",");
        sb.Append(CreateRichTextProperty("LincenseStatus", data.m_LincenseStatus));

        sb.Append("}"); // properties
        sb.Append("}"); // root

        return sb.ToString();
    }

    private string CreateUrlProperty(string key, string url)
    {
        return $"\"{key}\": {{ \"url\": \"{url}\" }}";
    }

    private string CreateTitleProperty(string name, string content)
    {
        return $"\"{name}\": {{ \"title\": [ {{ \"text\": {{ \"content\": \"{content}\" }} }} ] }}";
    }

    private string CreateRichTextProperty(string name, string content)
    {
        return $"\"{name}\": {{ \"rich_text\": [ {{ \"text\": {{ \"content\": \"{content}\" }} }} ] }}";
    }

    private string CreateNumberProperty(string name, double number)
    {
        return $"\"{name}\": {{ \"number\": {number.ToString(System.Globalization.CultureInfo.InvariantCulture)} }}";
    }
}