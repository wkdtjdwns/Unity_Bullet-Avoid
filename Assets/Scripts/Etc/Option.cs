using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [Header("Option Info")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;

    private void Awake()
    {
        muteToggle.isOn = false;

        bgmSlider.onValueChanged.AddListener(ChangeBgmVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSfxVolume);
        muteToggle.onValueChanged.AddListener(SetMute);
    }

    private void ChangeBgmVolume(float value)
    {
        SoundManager.Instance.bgmVolume = value;
    }

    private void ChangeSfxVolume(float value)
    {
        SoundManager.Instance.sfxVolume = value;
    }

    private void SetMute(bool isMute)
    {
        SoundManager.Instance.isMute = isMute;
    }
}