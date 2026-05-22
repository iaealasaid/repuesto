using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinNivel : MonoBehaviour
{
    [Header("Configuración")]
    public int       indiceCapitulo;

    [Header("Cinemática")]
    public VideoClip videoCinematica;   
    public GameObject cinematicaPanel;  
    public RawImage   videoTextura;     
    private VideoPlayer    _videoPlayer;
    private RenderTexture  _renderTexture;
    private bool           _activado;

    void Start()
    {
        // Configura el VideoPlayer
        _videoPlayer = gameObject.AddComponent<VideoPlayer>();
        _videoPlayer.renderMode    = VideoRenderMode.RenderTexture;
        _videoPlayer.playOnAwake   = false;
        _renderTexture             = new RenderTexture(854, 480, 0);
        _videoPlayer.targetTexture = _renderTexture;
        videoTextura.texture       = _renderTexture;

        _videoPlayer.loopPointReached += VideoTerminado;

        cinematicaPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _activado) return;
        _activado = true;

        // Completa el capítulo
        GameManager.Instance.CompletarCapitulo(indiceCapitulo);

        // Congela el juego excepto el video
        Time.timeScale = 0f;

        // Reproduce la cinemática
        _videoPlayer.clip = videoCinematica;
        _videoPlayer.Play();
        cinematicaPanel.SetActive(true);
    }

    void VideoTerminado(VideoPlayer vp)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}