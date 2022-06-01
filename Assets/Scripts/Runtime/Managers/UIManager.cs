using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] public UnityEvent displayIntroUIEvent;
    [SerializeField] public UnityEvent displayTitleUIEvent;
    [SerializeField] public UnityEvent displayGameUIEvent;
    [SerializeField] public UnityEvent displayGameOverUIEvent;
    [SerializeField] public UnityEvent displayPauseUIEvent;
    [SerializeField] public UnityEvent hidePauseUIEvent;
    
    private static readonly int PlayIntro = Animator.StringToHash("PlayIntro");

    private Animator _animator;
    private bool _introPlaying;

    public void IntroIsPlaying() => _introPlaying = true;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_introPlaying && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            StartGame();
        }
    }

    public void DisplayIntroUI()
    {
        // Debug.Log("DisplayIntroUI");
        displayIntroUIEvent?.Invoke();
    }

    public void DisplayTitleUI()
    {
        // Debug.Log("DisplayTitleUI");
        AudioManager.Instance.PlayMusic(AudioManager.Instance.musicTracks[1]);
        _animator.Play("TitleSlideshow");
        displayTitleUIEvent?.Invoke();
        StartCoroutine(StartingSlideshow());
    }

    public void DisplayGameOverUI()
    {
        // Debug.Log("DisplayGameOverUI");
        displayGameOverUIEvent?.Invoke();
    }

    public void DisplayPauseUI()
    {
        // Debug.Log("DisplayPauseUI");
        displayPauseUIEvent?.Invoke();
    }

    public void HidePauseUI()
    {
        // Debug.Log("HidePauseUI");
        hidePauseUIEvent?.Invoke();
    }

    public void StartGame()
    {
        // Debug.Log("StartGame UI");
        _introPlaying = false;
        displayGameUIEvent?.Invoke();
    }

    private IEnumerator StartingSlideshow()
    {
        yield return new WaitForSeconds(3);
        _introPlaying = true;
    }
}
