using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class TimelineManager : MonoBehaviour
{
    [SerializeField] private PlayableAsset _startScene;
    [SerializeField] private PlayableAsset _arrestScene;
    [SerializeField] private PlayableAsset _deathScene;
    private PlayableDirector _director;
    

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            PlayStartScene();
        if(SceneManager.GetActiveScene().buildIndex == 2)
            PlayArrestScene();
        if(SceneManager.GetActiveScene().buildIndex == 5)
            PlayDeathScene();

    }
    

    private void Update()
    {
        if (_director.state != PlayState.Playing && SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameStateHandler.Instance.State = GameState.START;
            SceneManager.LoadScene(1);
        }

        if (_director.state != PlayState.Playing && (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 5))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void PlayStartScene()
    {
        Debug.Log("start game scene");
        _director.playableAsset = _startScene;
        _director.Play();
    }

    private void PlayArrestScene()
    {
        Debug.Log("arrest scene");
        _director.playableAsset = _arrestScene;
        _director.Play();
    }

    private void PlayDeathScene()
    {
        Debug.Log("deathScene");
        _director.playableAsset = _deathScene;
        _director.Play();
    }
}
