using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct AudioFile
{
    public string name;
    public AudioClip clip;    
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    AudioSource bgmSource;
    AudioSource sfxSource;
    [Header("배경음악 세팅")]
    [SerializeField] List<AudioFile> bgmClip;
    Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();

    [Header("효과음 세팅")]
    [SerializeField] List<AudioFile> sfxClip;
    Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    [Header("볼륨")]
    [SerializeField] Slider volumSlider;


    private void Awake()
    {

        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject); 
            return;            
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
        sfxSource = GetComponent<AudioSource>();
        if (bgmSource == null) 
        {
            Debug.Log("[SoundManager] bgmSource 설정 안되어있음");
            return;
        }
        if (bgmClip.Count == 0) 
        {
            Debug.Log("[SoundManager] AudioClip 설정 안되어있음");
            return;
        }
        if (volumSlider == null) 
        {
            Debug.Log("[SoundManager] volumSlider 참조안됨");
            return;
        }

        foreach (AudioFile sound in bgmClip)
        {
            bgmDictionary.Add(sound.name, sound.clip);
        }
        foreach (AudioFile sound in sfxClip)
        {
            sfxDictionary.Add(sound.name, sound.clip);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += BgmPlay;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= BgmPlay;
    }
    private void Update()
    {
        float value = volumSlider.value;
        bgmSource.volume = value;
        sfxSource.volume = value;
    }

    void BgmPlay(Scene scene, LoadSceneMode mode)
    {

        if (volumSlider == null || bgmSource == null || sfxSource == null) return;

        //string currentScene = SceneManager.GetActiveScene().name;
        string currentScene = SceneController.currentScene;


        if (bgmDictionary.TryGetValue(currentScene, out AudioClip audio))
        {
            bgmSource.clip = audio;
        }
        else
        {
            Debug.Log($"[SoundManager] 해당 Scene에 bgmClip이 없습니다 현재 씬 : {currentScene}");
            return;
        }

        bgmSource.Play();
    }

    public void BgmStop() => bgmSource.Stop();
    public void SfxStop() => sfxSource.Stop();




    public void SfxPlay(string audioName)
    {
        if (audioName.Trim() == "none") return;

        if (sfxDictionary.TryGetValue(audioName, out AudioClip audio))
        {
            sfxSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("[SoundManager] 해당하는 SFX가 없습니다.");
            return;
        }
    }
}
