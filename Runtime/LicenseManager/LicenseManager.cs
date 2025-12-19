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
    public bool isAuthenticated = false;
    public string statusMessage = "Waiting for validation...";

    public UnityEvent<string> OnMessageEvent = new UnityEvent<string>();    

    void Start()
    {
        // Start validation automatically on launch
        StartCoroutine(ValidateLicenseFlow());
    }

    // --- Core Flow: Validate -> (Activate if needed) ---
    IEnumerator ValidateLicenseFlow()
    {
        statusMessage = "Validating license key...";
        Debug.Log($"[{nameof(LicenseManager)}] {statusMessage}");

        // 1. Get Device Fingerprint
        string fingerprint = SystemInfo.deviceUniqueIdentifier;

        // 2. Prepare Validation API
        string validateUrl = $"https://api.keygen.sh/v1/accounts/{accountId}/licenses/actions/validate-key";

        // Construct JSON payload
        string jsonBody = "{" +
                          "\"meta\": {" +
                              "\"key\": \"" + licenseKey + "\"," +
                              "\"scope\": { \"fingerprint\": \"" + fingerprint + "\" }" +
                          "}" +
                          "}";

        using (UnityWebRequest req = CreateRequest(validateUrl, "POST", jsonBody))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                HandleError(req);
                yield break;
            }

            // 3. Parse Response
            string jsonResponse = req.downloadHandler.text;
            Debug.Log($"[Keygen API] Response: {jsonResponse}");

            ValidationResponse response = JsonUtility.FromJson<ValidationResponse>(jsonResponse);

            if (response.meta.valid)
            {
                // A. Valid & Bound
                OnLicenseValid();
            }
            else
            {
                // B. Valid but not bound (NO_MACHINE_USES or NO_MACHINE)
                // "NO_MACHINE" occurs when policy is strict and no machine is attached yet.
                // "NO_MACHINE_USES" occurs when policy is floating but no seat is taken.
                if (response.meta.code == "NO_MACHINE_USES" || response.meta.code == "NO_MACHINE" || response.meta.code == "NO_MACHINES")
                {
                    Debug.LogWarning("License valid but machine not bound. Attempting auto-activation...");
                    statusMessage = "Binding this device...";

                    // Get License ID for activation
                    string licenseId = response.data.id;

                    // Execute Activation
                    yield return StartCoroutine(ActivateMachine(licenseId, fingerprint));
                }
                else
                {
                    // C. Other Errors (Expired, Suspended, etc.)
                    statusMessage = $"Validation failed: {response.meta.detail} ({response.meta.code})";
                    Debug.LogError(statusMessage);
                }
            }
        }
    }

    // --- Activation Flow ---
    IEnumerator ActivateMachine(string licenseId, string fingerprint)
    {
        string machinesUrl = $"https://api.keygen.sh/v1/accounts/{accountId}/machines";

        string machineName = SystemInfo.deviceName;

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
            // Authorization Header required for activation
            req.SetRequestHeader("Authorization", "License " + licenseKey);

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("<color=green>Device bound successfully!</color>");
                OnLicenseValid();
            }
            else
            {
                HandleError(req);
                statusMessage = "Binding failed: Machine limit reached or permission denied.";
            }
        }
    }

    // --- Success Handler ---
    void OnLicenseValid()
    {
        isAuthenticated = true;
        statusMessage = "Validation successful! Access granted.";
        Debug.Log("<color=green>License valid. Loading content...</color>");

        // TODO: Load your scene here
        // SceneManager.LoadScene("ExhibitionMain");
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