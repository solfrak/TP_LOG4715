using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioSource m_SFXSource;
    [SerializeField] private AudioClip m_MusicClip;

    private void Awake()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        m_MusicSource.clip = m_MusicClip;
        m_MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        m_SFXSource.clip = clip;
        m_SFXSource.Play();
    }
}
