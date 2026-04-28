using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public AudioSource bgmSource { get; private set; }
    public AudioSource sfxSource { get; private set; }
    [Header("배경음악 세팅")]
    [SerializeField] List<AudioFile> bgmClip;
    Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();

    [Header("효과음 세팅")]
    [SerializeField] List<AudioFile> sfxClip;
    Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    float lastTime = -999f;
    int repeat = 0;
    List<float> sfxTime = new List<float>();
    Queue<String> sfxQueue = new Queue<String>();


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
        // 반복재생사운드
        if(sfxTime.Count > 0 && sfxQueue.Count > 0) SfxRepeat();
    }

    void BgmPlay(Scene scene, LoadSceneMode mode)
    {

        if (bgmSource == null || sfxSource == null) return;

        string currentScene = SceneController.currentScene;
        //string currentScene = SceneManager.GetActiveScene().name;        

        if (bgmDictionary.TryGetValue(currentScene, out AudioClip audio))
        {
            bgmSource.clip = audio;
        }
        else
        {
            Debug.Log($"[SoundManager] 해당 Scene에 bgmClip이 없습니다 현재 씬 : {currentScene}");
            return;
        }
        Debug.Log(currentScene + "BGM 재생");
        bgmSource.Play();
    }
    public void BgmChange(String bgmName)
    {

        if (bgmSource == null || sfxSource == null) return;

        if (bgmDictionary.TryGetValue(bgmName, out AudioClip audio))
        {
            bgmSource.Stop();
            bgmSource.clip = audio;
        }
        else
        {
            Debug.Log($"[SoundManager] 해당 Scene에 bgmClip이 없습니다 현재 씬 : {bgmName}");
            return;
        }

        bgmSource.Play();
    }

    public void BgmStop() => bgmSource.Stop();
    public void SfxStop() => sfxSource.Stop();

    public void SetBgmSource (float volume) => bgmSource.volume = volume;


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
    public void SfxRepeatSet(string audioName, int count, float timing)
    {
        sfxTime.Clear();
        sfxQueue.Clear();
        repeat = 0;
        lastTime = -999f;

        for (int i = 0; i < count; i++) 
        {
            sfxTime.Add(timing * i);
            sfxQueue.Enqueue(audioName);
        }

        lastTime = Time.time;
    }
    void SfxRepeat()
    {
        if (Time.time > lastTime + sfxTime[repeat])
        {
            string sfx = sfxQueue.Dequeue();
            SfxPlay(sfx);
            repeat++;            
        }    
    }
}
