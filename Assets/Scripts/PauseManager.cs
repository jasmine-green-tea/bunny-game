using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public static PauseManager Instance { get; private set; }

    private List<IPausable> _pausableObjects = new List<IPausable>();

    [SerializeField] private bool _isGamePaused = false;
    [SerializeField] private float _globalTimeScale = 1f;

    public bool IsGamePaused => _isGamePaused;
    public float GlobalTimeScale => _globalTimeScale;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseAll();
            pauseMenu.SetActive(_isGamePaused);

        }
    }

    public bool RegisterPausable(IPausable pausable)
    {
        if (!_pausableObjects.Contains(pausable))
        {
            _pausableObjects.Add(pausable);
            return true;
        }
        return false;
    }

    public void UnregisterPausable(IPausable pausable)
    {
        _pausableObjects.Remove(pausable);
    }

    public void PauseAll()
    {
        _isGamePaused = true;
        foreach (var pausable in _pausableObjects)
        {
            Debug.Log(pausable.GetType());
            pausable.Pause();
        }
    }

    public void ResumeAll()
    {
        _isGamePaused = false;
        foreach (var pausable in _pausableObjects)
        {
            pausable.Resume();
        }
    }

    public void TogglePauseAll()
    {


        if (_isGamePaused)
            ResumeAll();
        else
            PauseAll();
    }

    public void SetGlobalTimeScale(float scale)
    {
        _globalTimeScale = Mathf.Max(0f, scale);
        Time.timeScale = _globalTimeScale;
    }
}
