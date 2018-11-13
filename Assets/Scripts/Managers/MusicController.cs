using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    static private bool play = true;
    public static bool _enabled = true;
    static public AudioSource src;
    static public AudioListener listener;

	public static MusicController _instance;
	public static MusicController Get
	{
		get
		{
			if (_instance == null)
			{
				throw new System.Exception("${nameof(Game)}.Get called too early");
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			if (_instance != this)
			{
				DestroyImmediate(this.gameObject);
			}
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
			_instance.Init();
		}
	}

    void Init()
    {
        src = GetComponent<AudioSource>();
        listener = GetComponent<AudioListener>();
    }

    public static void PlayTrack(AudioClip track)
    {
        src.clip = track;

        MusicOn(true);
    }


    public static void MusicOn(bool on)
    {
        play = on;
        if (on)
        {
            src.Play();
        }
        else
        {
            src.Stop();
        }
    }
    public static void MusicStop()
    {
        src.Stop();
    }
    public static void MusicPlay()
    {
        src.Play();
    }
    
    public static bool IsMusicOn()
    {
        return listener.enabled;
    }

    public static AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
                             // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }
}
