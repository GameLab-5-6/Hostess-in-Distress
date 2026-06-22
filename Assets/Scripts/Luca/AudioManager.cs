using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer mixer;
    
    public const string MASTER_VOLUME_KEY = "MasterVolume";
    public const string MUSIC_VOLUME_KEY = "MusicVolume";
    public const string SFX_VOLUME_KEY = "SFXVolume";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void LoadVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.5f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f);
        
        mixer.SetFloat(MASTER_VOLUME_KEY, Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(sfxVolume) * 20);
    }

    public void PlaySFX2D(AudioClip clip)
    {
        if (clip == null) return;
        
        AudioSource source = AudioObjectPooler.Instance.GetPooledObject().GetComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 0f;
        source.gameObject.SetActive(true);
        source.Play();
        
        StartCoroutine(DeactivateAfterPlaying(source.gameObject, clip.length));
    }
    
    public void PlaySFX3D(AudioClip clip)
    {
        if (clip == null) return;
        
        AudioSource source = AudioObjectPooler.Instance.GetPooledObject().GetComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 1f;
        source.gameObject.SetActive(true);
        source.Play();
        
        StartCoroutine(DeactivateAfterPlaying(source.gameObject, clip.length));
    }
    
    

    IEnumerator DeactivateAfterPlaying(GameObject obj, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        obj.SetActive(false);
    }
}