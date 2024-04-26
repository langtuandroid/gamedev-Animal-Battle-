using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BGMAB : MonoBehaviour
{
    public static BGMAB instanceE;
    [SerializeField] private AudioClip musicC;
    [SerializeField] private AudioClip victoryMusicC;
    [SerializeField] private AudioClip lostMusicC;
    [SerializeField] private AudioClip btnSoundD;
    [SerializeField] private AudioClip btnSound2D;


    [SerializeField] private AudioSource sfxPlayerR;
    [SerializeField] private AudioSource musicPlayer;

    private void Start()
    {
        musicPlayer = this.GetComponent<AudioSource>();
        if (instanceE)
        {
            Destroy(instanceE.gameObject);
            instanceE = null;
        }

        if (!instanceE)
        {
            instanceE = this;
            this.transform.position = GameObject.FindObjectOfType<Camera>().transform.position;
            musicPlayer.clip = musicC;
            musicPlayer.Play();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void BtnClickSfxX()
    {
        if (btnSoundD)
            sfxPlayerR.PlayOneShot(btnSoundD);
    }


    public void BtnClickSfx2X()
    {
        if (btnSound2D)
            sfxPlayerR.PlayOneShot(btnSound2D);
    }

    public void PlayVictoryMusicC()
    {
        if (victoryMusicC)
        {
            musicPlayer.clip = victoryMusicC;
            musicPlayer.Play();
        }
    }

    public void PlayGameOverMusic()
    {
        if (lostMusicC)
        {
            musicPlayer.clip = lostMusicC;
            musicPlayer.Play();
        }
    }
}