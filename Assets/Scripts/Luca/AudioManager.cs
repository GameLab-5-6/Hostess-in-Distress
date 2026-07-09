using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer mixer;

    public const string MASTER_VOLUME_KEY = "MasterVolume";
    public const string SFX_VOLUME_KEY = "SFXVolume";
    public const string BG_VOLUME_KEY = "BgVolume";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        VolumeSettings.OnPrefsUpdated += UpdateVolumeMixer;
    }

    private void OnDisable()
    {
        VolumeSettings.OnPrefsUpdated -= UpdateVolumeMixer;
    }

    private void Start()
    {
        LoadVolumes();
    }

    public void LoadVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f);
        float musicVolume = PlayerPrefs.GetFloat(BG_VOLUME_KEY, 0.5f);
        
        mixer.SetFloat(MASTER_VOLUME_KEY, Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat(BG_VOLUME_KEY, Mathf.Log10(musicVolume) * 20);
    }

    public void UpdateVolumeMixer(string key, float volume)
    {
        mixer.SetFloat(key, Mathf.Log10(volume) * 20);
    }

    public void PlaySFX2D(AudioClip clip, float volume)
    {
        if (clip == null) return;
        
        AudioSource source = AudioObjectPooler.Instance.GetPooledObject().GetComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 0f;
        source.volume = volume;
        source.gameObject.SetActive(true);
        source.Play();
        
        StartCoroutine(DeactivateAfterPlaying(source.gameObject, clip.length));
    }
    
    public void PlaySFX3D(AudioClip clip, float volume, Vector3 position)
    {
        if (clip == null) return;

        AudioSource source = AudioObjectPooler.Instance.GetPooledObject().GetComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = 1f;
        source.volume = volume;
        source.gameObject.SetActive(true);
        source.gameObject.transform.position = position;
        source.Play();
        
        StartCoroutine(DeactivateAfterPlaying(source.gameObject, clip.length));
    }

    IEnumerator DeactivateAfterPlaying(GameObject obj, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        obj.SetActive(false);
    }
}