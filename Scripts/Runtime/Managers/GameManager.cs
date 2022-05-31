using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public sealed class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private bool isPaused;
    [SerializeField] private CinemachineVirtualCamera camera;

    [SerializeField] private UnityEvent onStartEvent;
    [SerializeField] private UnityEvent onStartGameEvent;
    [SerializeField] private UnityEvent onRestartEvent;
    [SerializeField] private UnityEvent onPauseEvent;
    [SerializeField] private UnityEvent onUnPauseEvent;
    [SerializeField] private UnityEvent onQuitEvent;
    [SerializeField] private UnityEvent onGameOverEvent;
    

    private float _currentZoom = 5;
    private float _newZoom;
    public void Start()
    {
        isPaused = false;
        onStartEvent?.Invoke();
        camera.m_Lens.OrthographicSize = _currentZoom;
        AudioManager.Instance.PlayMusic("Intro", 0f, 10f);
        AudioManager.Instance.PlaySoundEffect("Supermarket");
    }

    private void Update()
    {
        if (Math.Abs(_currentZoom - _newZoom) > .01f)
            HandleCamera();
    }

    public void TogglePauseGame()
    {
        if (!isPaused)
            PauseGame();
        else
            UnPauseGame();
    }

    private void PauseGame()
    {
        isPaused = true;
        onPauseEvent?.Invoke();
    }

    private void UnPauseGame()
    {
        isPaused = false;
        onUnPauseEvent?.Invoke();
    }

    public void QuitGame()
    {
        onQuitEvent?.Invoke();
        Application.Quit();
    }

    public void ToggleTime()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void GameOver()
    {
        onGameOverEvent?.Invoke();
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayMusic("Game", 10, 10);
        onStartGameEvent?.Invoke();
    }

    public void RestartGame()
    {
        onRestartEvent?.Invoke();
        SceneManager.LoadScene("Game");
    }

    public void CameraZoom(float zoom) => _newZoom = zoom;

    private void HandleCamera()
    {
        var angle = Mathf.Abs(_newZoom - _currentZoom);
        camera.m_Lens.OrthographicSize = Mathf.MoveTowards(_currentZoom, _newZoom, angle / .5f * Time.deltaTime);
        _currentZoom = camera.m_Lens.OrthographicSize;
    }
}
