using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();

        Messenger.AddListener(GameEvent.GAME_PAUSED, Pause);
        Messenger.AddListener(GameEvent.GAME_RESUMED, Resume);
    }

    void OnDestroy(){
        Messenger.RemoveListener(GameEvent.GAME_PAUSED, Pause);
        Messenger.RemoveListener(GameEvent.GAME_RESUMED, Resume);
    }

    
    void Pause(){
        Time.timeScale = 0;

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    void Resume(){
        Time.timeScale = 1;

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void OnResumeButton(){
        GameEvent.IsPaused = false;
    }

    public void OnQuitButton(){
        Application.Quit();
    }
}