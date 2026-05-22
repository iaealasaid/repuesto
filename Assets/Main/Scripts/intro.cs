using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    [Header("Referencias")]
    public VideoPlayer videoPlayer;
    public string      escenaDestino = "Desk"; 

    void Start()
    {
        // Cuando el video termina llama a VideoTerminado
        videoPlayer.loopPointReached += VideoTerminado;
    }

    void VideoTerminado(VideoPlayer vp)
    {
        // Marca que ya vio la cinemática
        PlayerPrefs.SetInt("cinematicaVista", 1);
        PlayerPrefs.Save();

        // Carga el juego
        SceneManager.LoadScene(escenaDestino);
    }
}