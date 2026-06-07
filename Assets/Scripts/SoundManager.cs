using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource btnSource;
    public Sprite soundOnImage;
    public Sprite soundOffImage;
    public Button musicButton;
    public Button sfxButton;
    private bool isMusicOn = true;
    private bool isSFxOn = true;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    private void Awake()
    {
        musicSource.volume = musicVolSlider.value;
        btnSource.volume = sfxVolSlider.value;
        if(musicSource.volume > 0) isMusicOn = false;
        if(btnSource.volume > 0) isSFxOn = false;
        if(soundOnImage!= null)
        {
            MusicOnOff();
            SFxOnOff();
        }
    }

    public void setMusicVolume(float volume)
    {
        // object data edit
        musicSource.enabled = true;
        musicSource.volume = volume;
        musicButton.GetComponent<Image>().sprite = soundOffImage;

        // prefab data edit
        AudioSource prefAudioSource = GameManagerScript.instance.readyObjPref.transform.Find("Audio Source").GetComponent<AudioSource>();
        prefAudioSource.enabled = true;
        prefAudioSource.volume = volume;

        Image prefMusicOffButton = GameManagerScript.instance.readyObjPref.transform.Find("WindowUI/SettingWindow/Panel/MusicOffButton").GetComponent<Image>();
        prefMusicOffButton.sprite = soundOffImage;
    }

    public void setButtonVolume(float volume)
    {
        // object data edit
        btnSource.enabled = true;
        btnSource.volume = volume;
        sfxButton.GetComponent<Image>().sprite = soundOffImage;

        // prefab data edit
        AudioSource prefAudioSource = GameManagerScript.instance.readyObjPref.transform.Find("SFx").GetComponent<AudioSource>();
        prefAudioSource.enabled = true;
        prefAudioSource.volume = volume;

        Image prefSFxOffButton = GameManagerScript.instance.readyObjPref.transform.Find("WindowUI/SettingWindow/Panel/SFxOffButton").GetComponent<Image>();
        prefSFxOffButton.sprite = soundOffImage;
    }

    public void onSFx()
    {
        btnSource.Play();
    }

    public void MusicOnOff()
    {
        if(isMusicOn)
        {
            musicSource.enabled = false;
            musicButton.GetComponent<Image>().sprite = soundOnImage;
            isMusicOn = false;

            // prefab data edit
            AudioSource prefAudioSource = GameManagerScript.instance.readyObjPref.transform.Find("Audio Source").GetComponent<AudioSource>();
            prefAudioSource.enabled = false;

            Image prefMusicOffButton = GameManagerScript.instance.readyObjPref.transform.Find("WindowUI/SettingWindow/Panel/MusicOffButton").GetComponent<Image>();
            prefMusicOffButton.sprite = soundOnImage;

            SoundManager prefSoundManager = GameManagerScript.instance.readyObjPref.transform.Find("SoundManager").GetComponent<SoundManager>();
            prefSoundManager.isMusicOn = false;
        }
        else
        {
            musicSource.enabled = true;
            musicButton.GetComponent<Image>().sprite = soundOffImage;
            isMusicOn = true;

            // prefab data edit
            AudioSource prefAudioSource = GameManagerScript.instance.readyObjPref.transform.Find("Audio Source").GetComponent<AudioSource>();
            prefAudioSource.enabled = true;

            Image prefMusicOffButton = GameManagerScript.instance.readyObjPref.transform.Find("WindowUI/SettingWindow/Panel/MusicOffButton").GetComponent<Image>();
            prefMusicOffButton.sprite = soundOffImage;

            SoundManager prefSoundManager = GameManagerScript.instance.readyObjPref.transform.Find("SoundManager").GetComponent<SoundManager>();
            prefSoundManager.isMusicOn = true;
        }
    }

    public void SFxOnOff()
    {
        if(isSFxOn)
        {
            btnSource.enabled = false;
            sfxButton.GetComponent<Image>().sprite = soundOnImage;
            isSFxOn = false;

            // prefab data edit
            AudioSource prefAudioSource = GameManagerScript.instance.readyObjPref.transform.Find("SFx").GetComponent<AudioSource>();
            prefAudioSource.enabled = false;

            Image prefSFxOffButton = GameManagerScript.instance.readyObjPref.transform.Find("WindowUI/SettingWindow/Panel/SFxOffButton").GetComponent<Image>();
            prefSFxOffButton.sprite = soundOnImage;

            SoundManager prefSoundManager = GameManagerScript.instance.readyObjPref.transform.Find("SoundManager").GetComponent<SoundManager>();
            prefSoundManager.isMusicOn = false;
        }
        else
        {
            btnSource.enabled = true;
            sfxButton.GetComponent<Image>().sprite = soundOffImage;
            isSFxOn = true;

            // prefab data edit
            AudioSource prefAudioSource = GameManagerScript.instance.readyObjPref.transform.Find("SFx").GetComponent<AudioSource>();
            prefAudioSource.enabled = true;

            Image prefSFxOffButton = GameManagerScript.instance.readyObjPref.transform.Find("WindowUI/SettingWindow/Panel/SFxOffButton").GetComponent<Image>();
            prefSFxOffButton.sprite = soundOffImage;

            SoundManager prefSoundManager = GameManagerScript.instance.readyObjPref.transform.Find("SoundManager").GetComponent<SoundManager>();
            prefSoundManager.isMusicOn = true;
        }        
    }
}
