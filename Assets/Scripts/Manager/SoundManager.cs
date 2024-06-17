using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// this sound manager
    /// </summary>
    static SoundManager g_soundManager = null;

    [Header("Music")]
    /// <summary>
    /// background music
    /// </summary>
    [SerializeField]
    AudioSource m_backgroundMusicSound = null;

    [Header("Sound Effect")]
    /// <summary>
    /// player move audio clip
    /// </summary>
    [SerializeField]
    AudioSource m_moveSound = null;

    /// <summary>
    /// money get audio
    /// </summary>
    [SerializeField]
    AudioSource m_moneyGetSound = null;

    /// <summary>
    /// get audio
    /// </summary>
    [SerializeField]
    AudioSource m_ingredientGetSound = null;

    /// <summary>
    /// build audio
    /// </summary>
    [SerializeField]
    AudioSource m_buildSound = null;

    /// <summary>
    /// hit audio
    /// </summary>
    [SerializeField]
    AudioSource m_raftDamageSound = null;

    /// <summary>
    /// raft destroy audio
    /// </summary>
    [SerializeField]
    AudioSource m_raftDestroySound = null;

    private void Awake()
    {
        if (g_soundManager == null)
        {
            g_soundManager = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_backgroundMusicSound.Play();
    }

    /// <summary>
    /// instance
    /// </summary>
    public static SoundManager Instance
    {
        get { return g_soundManager; }
    }
    public AudioSource MoveSound
    {
        get { return m_moveSound; }
    }
    public AudioSource MoneyGetSound
    {
        get { return m_moneyGetSound; }
    }
    public AudioSource IngredientGetSound
    {
        get { return m_ingredientGetSound; }
    }
    public AudioSource BuildSound
    {
        get { return m_buildSound; }
    }
    public AudioSource RaftDamageSound
    {
        get { return m_raftDamageSound; }
    }
    public AudioSource RaftDestroyAudio
    {
        get { return m_raftDestroySound; }
    }
}
