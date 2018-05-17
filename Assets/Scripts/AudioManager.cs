using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance { get { return instance; } }

    private Dictionary<string, string> audioPathDict;      // 存放音频文件路径
    private AudioSource musicAudioSource;
    private List<AudioSource> unusedSoundAudioSourceList;   // 存放可以使用的音频组件
    private List<AudioSource> usedSoundAudioSourceList;     // 存放正在使用的音频组件
    private Dictionary<string, AudioClip> audioClipDict;       // 缓存音频文件
    private float musicVolume = 1;
    private float soundVolume = 1;
    private string musicVolumePrefs = "MusicVolume";
    private string soundVolumePrefs = "SoundVolume";
    private int poolCount = 3;         // 对象池数量

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
        audioPathDict = new Dictionary<string, string>()       // 这里设置音频文件路径。需要修改。 TODO
        {
            { "SpeedUp", "Sounds/SpeedUp" },
            { "SpeedDown", "Sounds/SpeedDown" },
            { "BGM", "Sounds/BGM"},
        };

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        unusedSoundAudioSourceList = new List<AudioSource>();
        usedSoundAudioSourceList = new List<AudioSource>();
        audioClipDict = new Dictionary<string, AudioClip>();
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    public void PlayMusic(string name, bool loop = true)
    {
        musicAudioSource.clip = GetAudioClip(name);
        musicAudioSource.clip.LoadAudioData();
        musicAudioSource.loop = loop;
        musicAudioSource.volume = 0.5f;
        musicAudioSource.Play();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(name);
            audioSource.clip.LoadAudioData();
            audioSource.Play();

            StartCoroutine(WaitPlayEnd(audioSource));
        }
        else
        {
            AddAudioSource();

            AudioSource audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(name);
            audioSource.clip.LoadAudioData();
            audioSource.volume = soundVolume;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(WaitPlayEnd(audioSource));
        }            
    }

    /// <summary>
    /// 当播放音效结束后，将其移至未使用集合
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    IEnumerator WaitPlayEnd(AudioSource audioSource)
    {
        yield return new WaitUntil(() => { return !audioSource.isPlaying; });
        UsedToUnused(audioSource);
    }



    /// <summary>
    /// 获取音频文件，获取后会缓存一份
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(string name)
    {
        if (!audioClipDict.ContainsKey(name))
        {
            if (!audioPathDict.ContainsKey(name)) return null;

            AudioClip ac = Resources.Load(audioPathDict[name]) as AudioClip;
            audioClipDict.Add(name, ac);
        }
        return audioClipDict[name];
    }

    /// <summary>
    /// 添加音频组件
    /// </summary>
    /// <returns></returns>
    private AudioSource AddAudioSource()
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            return UnusedToUsed();
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            unusedSoundAudioSourceList.Add(audioSource);
            return audioSource;
        }
    }

    /// <summary>
    /// 将未使用的音频组件移至已使用集合里
    /// </summary>
    /// <returns></returns>
    private AudioSource UnusedToUsed()
    {
        AudioSource audioSource = unusedSoundAudioSourceList[0];
        unusedSoundAudioSourceList.RemoveAt(0);
        usedSoundAudioSourceList.Add(audioSource);
        return audioSource;
    }

    /// <summary>
    /// 将使用完的音频组件移至未使用集合里
    /// </summary>
    /// <param name="audioSource"></param>
    private void UsedToUnused(AudioSource audioSource)
    {
        usedSoundAudioSourceList.Remove(audioSource);
        if (unusedSoundAudioSourceList.Count >= poolCount)
        {
            Destroy(audioSource);
        }
        else
        {
            unusedSoundAudioSourceList.Add(audioSource);
        }
    }



    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;
    }

    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        for (int i = 0; i < unusedSoundAudioSourceList.Count; i++)
        {
            unusedSoundAudioSourceList[i].volume = volume;
        }
        for (int i = 0; i < usedSoundAudioSourceList.Count; i++)
        {
            usedSoundAudioSourceList[i].volume = volume;
        }
    }
}