using UnityEngine;

public interface IPausable
{
    bool IsPaused { get;}
    float TimeScale { get; set; }

    void Pause();
    void Resume();
    void TogglePause();
}


public class PausableObject : MonoBehaviour, IPausable
{
    [SerializeField] private bool _isPaused = false;
    [SerializeField] private float _timeScale = 1f;

    public bool IsPaused => _isPaused;
    public float TimeScale
    {
        get => _timeScale;
        set 
        { 
            _timeScale = Mathf.Max(0f, value);
            OnTimeScaleChanged();
        }
    }
    protected virtual void OnPaused() { }
    protected virtual void OnResumed() { }
    protected virtual void OnTimeScaleChanged() { }

    public void Pause()
    {
        if (_isPaused) return;
        _isPaused = true;
        OnPaused();
    }

    public void Resume()
    {
        if (!_isPaused) return;
        _isPaused = false;
        OnResumed();
    }

    public void TogglePause()
    {
        if (_isPaused)
            Resume();
        else
            Pause();
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        if (_isPaused) return;

        float deltaTime = Time.deltaTime * _timeScale;
        UpdatePausable(deltaTime);
    }

    protected virtual void UpdatePausable(float deltaTime) { }
}
