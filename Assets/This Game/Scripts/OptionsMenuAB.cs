using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionsMenuAB : MonoBehaviour
{
    [SerializeField] private AudioMixer mixerR;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Text musicVolTextT;
    [SerializeField] private Text sfxVolTextT;
    [SerializeField] private Toggle lowW;
    [SerializeField] private Toggle medD;
    [SerializeField] private Toggle highH;
    [SerializeField] private bool hideOnStart = false;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        int q = PlayerPrefs.GetInt("Quality", 3);
        QualitySettings.SetQualityLevel(q);

        switch (q)
        {
            case 0:
                lowW.isOn = true;
                break;
            case 3:
                medD.isOn = true;
                break;
            case 5:
                highH.isOn = true;
                break;
            default:
                medD.isOn = true;
                break;
        }

        UpdateAudioVolumeE();
        if (hideOnStart)
        {
            this.GetComponent<CanvasGroup>().alpha = 1;
            this.gameObject.SetActive(false);
        }
    }

    private void UpdateAudioVolumeE()
    {
        mixerR.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
        mixerR.SetFloat("SfxVol", Mathf.Log10(sfxSlider.value) * 20);

        musicVolTextT.text = (int)(musicSlider.value * 100) + " %";
        sfxVolTextT.text = (int)(sfxSlider.value * 100) + " %";
    }

    public void SetMusicLevelL()
    {
        
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
        UpdateAudioVolumeE();
        
    }

    public void SetSfxLevelL()
    {
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.Save();
        UpdateAudioVolumeE();
    }

    public void SetQualityY(int q)
    {
        PlayerPrefs.SetInt("Quality", q);
        QualitySettings.SetQualityLevel(q);
        if (BGMAB.instanceE != null)
            BGMAB.instanceE.BtnClickSfxX();
        PlayerPrefs.Save();
    }
}