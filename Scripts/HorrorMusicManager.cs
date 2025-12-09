using UnityEngine;

public class HorrorMusicManager : MonoBehaviour
{
    public static HorrorMusicManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource ambientSource;
    public AudioSource chaseSource;

    [Header("Volumes")]
    [Range(0f, 1f)] public float ambientMaxVolume = 0.5f;
    [Range(0f, 1f)] public float chaseMaxVolume = 0.8f;
    public float fadeSpeed = 1.5f;

    bool isChasing = false;

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (ambientSource != null)
        {
            ambientSource.loop = true;
            ambientSource.volume = ambientMaxVolume;
            if (!ambientSource.isPlaying) ambientSource.Play();
        }

        if (chaseSource != null)
        {
            chaseSource.loop = true;
            chaseSource.volume = 0f;
            if (!chaseSource.isPlaying) chaseSource.Play();
        }
    }

    void Update()
    {
        if (ambientSource == null || chaseSource == null) return;

        float targetAmbient = isChasing ? 0f : ambientMaxVolume;
        float targetChase   = isChasing ? chaseMaxVolume : 0f;

        ambientSource.volume = Mathf.MoveTowards(
            ambientSource.volume, targetAmbient, fadeSpeed * Time.deltaTime);
        chaseSource.volume = Mathf.MoveTowards(
            chaseSource.volume, targetChase, fadeSpeed * Time.deltaTime);
    }

    public void PlayChaseMusic()
    {
        isChasing = true;
    }

    public void StopChaseMusic()
    {
        isChasing = false;
    }
}