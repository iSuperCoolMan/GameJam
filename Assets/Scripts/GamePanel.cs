using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    private void OnEnable()
    {
        Time.timeScale = 0;

        AudioSource[] audios = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audio in audios)
            audio.Stop();

        _audio.Play();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
