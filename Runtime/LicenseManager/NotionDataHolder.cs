using System;
using UnityEngine;
using UnityEngine.Events;

public class NotionDataHolder : MonoBehaviour
{
    [Header("! Important !")]
    public string m_artworkName;
    public string m_porjectUrl;
    public string m_databaseId;

    [Header("Debug")]
    public UnityEvent<string> OnResultMessage = new UnityEvent<string>();

    [Header("Common")]
    public NotionService m_notionService;
    public LicenseManager m_licenseManager;
    public LocationManager m_locationManager;

    public void SendData()
    {
        VisitorData data = new VisitorData
        {
            m_notiondataid = m_databaseId,
            m_artworkName = m_artworkName,
            m_projectURL = m_porjectUrl,
            m_ip = m_locationManager.currentIP,
            m_country = m_locationManager.currentCountry,
            m_city = m_locationManager.currentCity,
            m_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            m_timeZone = m_locationManager.currentTimeZone,
            m_computerName = SystemInfo.deviceName,
            m_computerFigerprint = SystemInfo.deviceUniqueIdentifier,
            m_LincenseStatus = m_licenseManager.statusMessage
        };

        m_notionService.UploadVisitorLog(data, OnSendDataComplete);
    }

    void OnSendDataComplete(bool success)
    {
        if (success)
        {
            OnResultMessage.Invoke("Data | Success");
            Debug.Log("<color=green>[NotionDataHolder] Data sent successfully!</color>");
        }
        else
        {
            OnResultMessage.Invoke("Data | Failure");
            Debug.LogError("[NotionDataHolder] Failed to send data.");
        }
    }
}
