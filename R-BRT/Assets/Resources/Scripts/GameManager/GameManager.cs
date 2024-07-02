using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static GameManager Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[GameManager] Instance already destroyed on application quit. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (GameManager)FindObjectOfType(typeof(GameManager));

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<GameManager>();
                        singleton.name = typeof(GameManager).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[GameManager] An instance of GameManager is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[GameManager] Using instance already created: " + _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this as GameManager;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_applicationIsQuitting) return;

        Debug.Log($"Scene loaded: {scene.name}");

        switch (scene.name)
        {
            case "GameScene":
                Debug.Log("First playthrough");
                // Additional logic for the GameScene if needed
                break;

            case "SamDies":
            case "VictorySamLives":
                Time.timeScale = 1;
                if (!_applicationIsQuitting)
                {
                    StartCoroutine(TransitionBackToStart());
                }
                break;

            default:
                Debug.LogWarning($"Scene '{scene.name}' not handled in OnSceneLoaded");
                break;
        }
    }

    private IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
        if (!_applicationIsQuitting)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}