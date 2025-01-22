using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [SerializeField] private GameObject waveMusicSourceObj;
    [SerializeField] private AudioClip[] waveMusicClip;
    private int trakPlaying = 0;
    private GameObject[] musicPlayer;

    [SerializeField] private GameObject canvas;

    private bool canvasActive = false;

    private GameLoader _loader;
    public enum SFXSound
    {
        //Other
        Power_Newhand,
        UI_BarFill,
        UI_Click,
        UI_Negative,
        UI_Positive,
        //Spell
        Power_Blackhole_Cast,
        Power_Bomb_Fly,
        Power_Bomb_Impact,
        Power_EoD,
        Power_Fireball_Cast,
        Power_Fireball_Impact,
        Power_Freeze_Cast,
        Power_Freezetime,
        Power_Lightning_Impact,
        Power_Nuke_Detonate,
        //Tower
        Power_Earthquake,
        Tower_Arrow_Hit,
        Tower_Arrow_Shoot,
        Tower_Attraction_Hit,
        Tower_Attraction_Shot,
        Tower_Buff_Debuff,
        Tower_Buff_Powerup,
        Tower_Cannon_Impact,
        Tower_Cannon_Shoot,
        Tower_Destroy,
        Tower_Earthquake_Impact,
        Tower_Electricity_Hit,
        Tower_Electricity_Shot,
        Tower_Frost_Freeze,
        Tower_Frost_Melt,
        Tower_Inferno,
        Tower_Kebin_Lottery,
        Tower_Mortar_Explode,
        Tower_Mortar_Impact,
        Tower_Mortar_Shot,
        Tower_Organ_Wound,
        Tower_OrganShot_Var1,
        Tower_OrganShot_Var2,
        Tower_OrganShot_Var3,
        Tower_Place,
        Tower_Poison_Pool,
        Tower_Poison_Shot,
        Tower_Wave_Crash,
        Tower_Wave_Pickup,
        Tower_Wave_Shot,
        Tower_Mystery_Box
    }
    public enum musicSound
    {
        DeckedOut_Wave1To3,
        DeckedOut_Wave4To6,
        DeckedOut_Wave7To9,
        DeckedOut_Wave10To12,
        DeckedOut_Wave13AndOn,
        MainMenu_DeckedOut
    }
    private Dictionary<SFXSound, AudioClip> SFXSoundAudioClipDictionary;
    private Dictionary<musicSound, AudioClip> musicSoundAudioClipDictionary;

    protected override void Awake()
    {
        base.Awake();

        canvas.SetActive(canvasActive);

        DontDestroyOnLoad(gameObject);

        musicPlayer = new GameObject[waveMusicClip.Length];

        for (int i = 0; i < musicPlayer.Length; i++)
        {
            GameObject temp = Instantiate(waveMusicSourceObj, gameObject.transform.position , Quaternion.identity);
            temp.transform.SetParent(gameObject.transform);
            temp.name = i.ToString();
            musicPlayer[i] = temp;
            musicPlayer[i].GetComponent<AudioSource>().clip = waveMusicClip[i];
            musicPlayer[i].SetActive(false);
        }

        SFXSoundAudioClipDictionary = new Dictionary<SFXSound, AudioClip>();
        foreach (SFXSound sound in System.Enum.GetValues(typeof(SFXSound)))
        {
            SFXSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }

        musicSoundAudioClipDictionary = new Dictionary<musicSound, AudioClip>();
        foreach (musicSound sound in System.Enum.GetValues(typeof(musicSound)))
        {
            musicSoundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume") || PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            setMasterVolume();
            setMusicVolume();
            setSFXVolume();
        }
    }

    public void setMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            setMasterVolume();
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            setMusicVolume();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            setSFXVolume();
        }
    }

    public void playMusicClip(musicSound sound)
    {
        musicSource.clip = musicSoundAudioClipDictionary[sound];
        musicSource.Play();
    }
    public void stopMusicClip()
    {
        musicSource.Stop();
    }

    public void playSFXClip(SFXSound sound)
    {
        SFXSource.PlayOneShot(SFXSoundAudioClipDictionary[sound]);
    }
    public void playSFXClip(SFXSound sound, AudioSource source)
    {
        source.clip = SFXSoundAudioClipDictionary[sound];
        source.Play();
    }
    public AudioClip SetSFXClip(SFXSound sound)
    {
        return SFXSoundAudioClipDictionary[sound];
    }
    public void SetSFXClip(SFXSound sound, AudioSource source)
    {
        source.clip = SFXSoundAudioClipDictionary[sound];
    }

    public void StartWaveMucic()
    {
        stopMusicClip();
        foreach (GameObject source in musicPlayer)
        {
            source.SetActive(true);
            source.GetComponent<AudioSource>().Play();
            source.SetActive(false);
        }
        musicPlayer[0].SetActive(true);
    }
    public void PlayNextMuiscTrak()
    {
        trakPlaying++;
        if (trakPlaying < musicPlayer.Length && trakPlaying >= 0)
        {
            foreach (GameObject source in musicPlayer)
            {
                source.SetActive(false);
            }
            musicPlayer[trakPlaying].SetActive(true);
        }
    }
    public void StopWaveMusic()
    {
        foreach (GameObject source in musicPlayer)
        {
            source.SetActive(true);
            source.GetComponent<AudioSource>().Stop();
            source.SetActive(false);
        }
        musicSource.Play();
    }

    public void StopAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];

        foreach (var audioS in allAudioSources)
        {
            if (!audioS.loop)
            {
                audioS.Stop();
            }
        }
    }
    public void LoadTest()
    {
        SceneManager.LoadScene("Test");
    }
    public void OpenMenu()
    {
        canvas.SetActive(true);
    }
    public void CloseMenu()
    {
        canvas.SetActive(false);
    }
}
