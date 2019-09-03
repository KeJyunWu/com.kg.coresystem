using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AVProPlayerControl : MonoBehaviour {

    [SerializeField]
    string m_folderName;

    [SerializeField]
    string m_fileName;

    AVProQuickTimeMovie m_avProQuickTimeMovie;
    AVProQuickTime m_avProQuickTime;

    [SerializeField]
    bool m_loop = false;

    [SerializeField]
    bool m_playOnStart = false;

    [SerializeField]
    RawImage m_TargetPanel;

    private DirectoryInfo m_directoryInfo;

    void Start()
    {
        if (m_directoryInfo == null)
        {
            m_directoryInfo = Directory.GetParent(Application.dataPath).Parent.CreateSubdirectory(m_folderName);
        }

        string path = m_directoryInfo.FullName + @"\" ;
        string _full_path = path + m_fileName;

        m_avProQuickTimeMovie = AVProManager.intance.GetAVProQuickTimeMovie(_full_path);
        m_avProQuickTimeMovie._folder = path;
        m_avProQuickTimeMovie._filename = m_fileName;
        m_avProQuickTimeMovie._loop = m_loop;
        LoadMovie();
        m_avProQuickTime = m_avProQuickTimeMovie.MovieInstance;

        if (m_TargetPanel == null)
            m_TargetPanel = GetComponent<RawImage>();

        if (m_playOnStart)
            Play();
    }

    void LoadMovie()
    {
        bool _loaded = false;
        _loaded = m_avProQuickTimeMovie.LoadMovie();
    }

    public void Play()
    {
        if (m_avProQuickTime != null)
            m_avProQuickTime.Play();
        else
        {
            m_avProQuickTime = m_avProQuickTimeMovie.MovieInstance;
            m_avProQuickTimeMovie.Play();
        }
    }

    public void PlayOneShot()
    {
        if (m_avProQuickTime != null)
        {
            m_avProQuickTime.Frame = 0;
            m_avProQuickTime.Play();
        }
        else
        {
            m_avProQuickTime = m_avProQuickTimeMovie.MovieInstance;
            m_avProQuickTime.Frame = 0;
            m_avProQuickTimeMovie.Play();
        }
    }

    public void Stop()
    {
        if (m_avProQuickTime != null)
            m_avProQuickTime.Pause();
        else
        {
            m_avProQuickTime = m_avProQuickTimeMovie.MovieInstance;
            m_avProQuickTimeMovie.Pause();
        }
    }

    private void Update()
    {
        if (m_TargetPanel != null)
            m_TargetPanel.texture = m_avProQuickTimeMovie.OutputTexture;
    }
}
