using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] clips;
    [SerializeField]private AudioClip bgmClip;

    private Dictionary<string, AudioClip> clipDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();

            PlayBGM();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 외부에서 clip 이름을 갖는 것보다는 SoundManager가 여러 메소드를 처리
    public void PlayClayBtnClick() => PlaySFX("Effect_Clay");
    public void PlayGravelBtnClick() => PlaySFX("Effect_Gravel");
    public void PlayUIBtnClick() => PlaySFX("Effect_UI");

    void Init()
    {
        clipDict = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in clips)
        {
            clipDict[clip.name] = clip;
        }
    }

    void PlaySFX(string clipName)
    {
        if (clipDict.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(clipDict[clipName]);
        }
        else
        {
            Debug.Log($"Sound: {clipName} not found!");
        }
    }

    //배경음악은 반복재생
    void PlayBGM()
    {
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();

    }
}
