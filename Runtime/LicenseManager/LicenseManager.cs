using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class LicenseManager : MonoBehaviour
{
    [Header("Keygen Settings")]
    // 🔴 Fill in your Account ID here
    public string accountId = "YOUR_ACCOUNT_ID_HERE";

    // 🔴 Fill in your License Key here
    public string licenseKey = "YOUR_LICENSE_KEY_HERE";

    [Header("Status (Read Only)")]
    [ReadOnly]
    public bool isAuthenticated = false;
    [ReadOnly]
    public string statusMessage = "Waiting for validation...";

    public UnityEvent<string> OnMessageEvent = new UnityEvent<string>();
    public UnityEvent OnValidEvent = new UnityEvent();
    public UnityEvent OnInvalidEvent = new UnityEvent();    

    void Start()
    {
        // Start validation automatically on launch
        StartCoroutine(ValidateLicenseFlow());
    }

    // --- Core Flow: Validate -> (Activate if needed) ---
    // --- 主要流程：驗證 -> 處理代碼 -> (需要時)自動激活 ---
    IEnumerator ValidateLicenseFlow()
    {
        statusMessage = "Validating license key...";
        Debug.Log($"[{nameof(LicenseManager)}] {statusMessage}");

        // 1. 取得裝置指紋
        string fingerprint = SystemInfo.deviceUniqueIdentifier;

        // 2. 準備驗證 API
        string validateUrl = $"https://api.keygen.sh/v1/accounts/{accountId}/licenses/actions/validate-key";

        // 建構 JSON payload
        string jsonBody = "{" +
                          "\"meta\": {" +
                              "\"key\": \"" + licenseKey + "\"," +
                              "\"scope\": { \"fingerprint\": \"" + fingerprint + "\" }" +
                          "}" +
                          "}";

        using (UnityWebRequest req = CreateRequest(validateUrl, "POST", jsonBody))
        {
            yield return req.SendWebRequest();

            // --- 🔴 嚴格網路檢查區塊 (Strict Network Check) ---
            if (req.result != UnityWebRequest.Result.Success)
            {
                // A. 根本沒連上網路 (斷網)
                if (req.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError("FATAL: No Internet Connection.");
                    statusMessage = "Network Error: Please check your internet connection.";
                }
                // B. 有連網，但伺服器回傳錯誤 (如 500, 404)
                else
                {
                    Debug.LogError($"Protocol Error: {req.error}");
                    statusMessage = "Server Error: Unable to verify license.";
                }

                // ⛔ 直接中斷，不允許進入
                OnLicenseInvalid();
                yield break;
            }
            // ------------------------------------------------

            // 3. 解析回應
            string jsonResponse = req.downloadHandler.text;
            Debug.Log($"[Keygen API] Response: {jsonResponse}");

            ValidationResponse response = JsonUtility.FromJson<ValidationResponse>(jsonResponse);

            // 4. 根據回傳代碼進行處理 (Switch Case Logic)
            string code = response.meta.code != null ? response.meta.code.ToUpper() : "UNKNOWN";

            switch (code)
            {
                // --- 🟢 成功 (Success) ---
                case "VALID":
                    Debug.Log("License is valid and bound.");
                    OnLicenseValid();
                    break;

                // --- 🟡 需要激活 (Needs Activation) ---
                // 包含：單數未綁定、複數未綁定、浮動席次未佔用、指紋不符(嘗試換機)
                case "NO_MACHINE":
                case "NO_MACHINES":
                case "NO_MACHINE_USES":
                case "FINGERPRINT_SCOPE_MISMATCH":

                    Debug.LogWarning($"License valid but machine not bound ({code}). Attempting auto-activation...");
                    statusMessage = "Binding this device...";

                    // 檢查是否有 License ID 可以用來激活
                    if (response.data != null && !string.IsNullOrEmpty(response.data.id))
                    {
                        yield return StartCoroutine(ActivateMachine(response.data.id, fingerprint));
                    }
                    else
                    {
                        Debug.LogError("Critical Error: License ID not found in response.");
                        statusMessage = "Error: Could not retrieve license ID.";
                        OnLicenseInvalid();
                    }
                    break;

                // --- 🔴 明確失敗 (Hard Failures) ---
                case "EXPIRED":
                    statusMessage = "License has expired. Access denied.";
                    Debug.LogError("License validation failed: EXPIRED");
                    OnLicenseInvalid();
                    break;

                case "SUSPENDED":
                    statusMessage = "License has been suspended. Contact support.";
                    Debug.LogError("License validation failed: SUSPENDED");
                    OnLicenseInvalid();
                    break;

                case "NOT_FOUND":
                    statusMessage = "License key not found. Please check your key.";
                    Debug.LogError("License validation failed: NOT_FOUND");
                    OnLicenseInvalid();
                    break;

                case "MACHINE_LIMIT_EXCEEDED":
                    statusMessage = "Machine limit reached. Cannot activate new device.";
                    Debug.LogError("Validation failed: Machine limit reached.");
                    OnLicenseInvalid();
                    break;

                // --- 未知代碼 ---
                default:
                    statusMessage = $"Validation failed: {code}";
                    Debug.LogError($"Unknown validation code: {code} / Detail: {response.meta.detail}");
                    OnLicenseInvalid();
                    break;
            }
        }
    }

    // --- Activation Flow ---
    // --- 激活流程 (Activation Flow) ---
    IEnumerator ActivateMachine(string licenseId, string fingerprint)
    {
        string machinesUrl = $"https://api.keygen.sh/v1/accounts/{accountId}/machines";
        string machineName = SystemInfo.deviceName; // 使用電腦名稱方便後台管理

        string jsonBody = "{" +
                          "\"data\": {" +
                              "\"type\": \"machines\"," +
                              "\"attributes\": {" +
                                  "\"fingerprint\": \"" + fingerprint + "\"," +
                                  "\"platform\": \"Windows\"," +
                                  "\"name\": \"" + machineName + "\"" +
                              "}," +
                              "\"relationships\": {" +
                                  "\"license\": {" +
                                      "\"data\": { \"type\": \"licenses\", \"id\": \"" + licenseId + "\" }" +
                                  "}" +
                              "}" +
                          "}" +
                          "}";

        using (UnityWebRequest req = CreateRequest(machinesUrl, "POST", jsonBody))
        {
            // 激活需要帶上 Authorization Header
            req.SetRequestHeader("Authorization", "License " + licenseKey);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Activation Failed: {req.error} \nResponse: {req.downloadHandler.text}");
                statusMessage = "Activation failed. Machine limit likely reached or Network Error.";
                OnLicenseInvalid();
            }
            else
            {
                Debug.Log("<color=green>Device bound successfully!</color>");
                OnLicenseValid();
            }
        }
    }

    // --- Success Handler ---
    void OnLicenseValid()
    {
        isAuthenticated = true;
        statusMessage = "Validation successful! Access granted.";
        OnValidEvent.Invoke();
        Debug.Log("<color=green>License valid. Loading content...</color>");
    }

    void OnLicenseInvalid()
    {
        isAuthenticated = false;
        Debug.LogError($"<color=red> >>> OnLicenseInvalid: {statusMessage} <<< </color>");
    }

    // --- Helper: Create Request ---
    UnityWebRequest CreateRequest(string url, string method, string bodyJson)
    {
        var request = new UnityWebRequest(url, method);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/vnd.api+json");
        request.SetRequestHeader("Accept", "application/vnd.api+json");

        return request;
    }

    void HandleError(UnityWebRequest req)
    {
        Debug.LogError($"API Error: {req.error} \nResponse: {req.downloadHandler.text}");
        statusMessage = "Network or Server Error";
    }

    void Update()
    {
        // Broadcast status message updates
        OnMessageEvent.Invoke(statusMessage);
    }

    // --- JSON Data Structures ---
    [System.Serializable]
    public class ValidationResponse
    {
        public MetaData meta;
        public DataObject data;
    }

    [System.Serializable]
    public class MetaData
    {
        public bool valid;
        public string code;
        public string detail;
    }

    [System.Serializable]
    public class DataObject
    {
        public string id;
        public string type;
    }
}