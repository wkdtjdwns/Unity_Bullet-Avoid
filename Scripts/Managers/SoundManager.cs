using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Info")]
    public bool isMute;
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] audioClips;

    public float bgmVolume;
    public float sfxVolume;

    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;

    private void Awake()
    {
        Instance = this;

        bgmVolume = 0.5f;
        sfxVolume = 0.5f;

        bgmPlayer = GameObject.Find("Bgm Player").gameObject.GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("Sfx Player").gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isMute)
        {
            bgmPlayer.volume = 0;
            sfxPlayer.volume = 0;
        }

        else
        {
            ChangeBgmSound();
            ChangeSfxSound();
        }
    }

    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Shot": index = 0; break;
            case "Meteor": index = 1; break;
            case "Hit": index = 2; break;
            case "Die": index = 3; break;
            case "Dying": index = 4; break;
            case "Ranking": index = 5; break;
            case "Destroy": index = 6; break;
            case "Run": index = 7; break;
            case "Barrier": index = 8; break;
            case "Heal": index = 9; break;
            case "Wrong": index = 10; break;
            case "Sound Hear": index = 11; break;
            case "Button": index = 12; break;
        }

        sfxPlayer.clip = audioClips[index];
        sfxPlayer.PlayOneShot(sfxPlayer.clip);
    }

    public void PlayBgm(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Bgm 1": index = 0; break;
            case "Bgm 2": index = 1; break;
            case "Bgm 3": index = 2; break;
            case "Null": bgmPlayer.clip = null; return;
        }

        bgmPlayer.clip = bgmClips[index];
        bgmPlayer.Play();
    }

    private void ChangeBgmSound() { bgmPlayer.volume = bgmVolume * 0.25f; }
    private void ChangeSfxSound() { sfxPlayer.volume = sfxVolume * 0.25f; }
}