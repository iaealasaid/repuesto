using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CamaraUI : MonoBehaviour
{

    public Button[]     botonesRecuerdos;   
    public GameObject[] candados;           

    [Header("Videos — uno por recuerdo en el mismo orden")]
    public VideoClip[]  videos;

    [Header("Reproductor")]
    public GameObject   reproductorPanel;
    public RawImage     videoTextura;
    public Button       btnCerrar;

    private VideoPlayer     _videoPlayer;
    private RenderTexture   _renderTexture;

    void Start()
    {
        
        _videoPlayer = gameObject.AddComponent<VideoPlayer>();
        _videoPlayer.renderMode  = VideoRenderMode.RenderTexture;
        _videoPlayer.playOnAwake = false;
        _renderTexture           = new RenderTexture(854, 480, 0);
        _videoPlayer.targetTexture = _renderTexture;
        videoTextura.texture       = _renderTexture;

        reproductorPanel.SetActive(false);
        btnCerrar.onClick.AddListener(CerrarReproductor);

        CargarGaleria();
    }

    void CargarGaleria()
    {
        for (int i = 0; i < botonesRecuerdos.Length; i++)
        {
            bool desbloqueado = GameManager.Instance.RecuerdoDesbloqueado(i);
            int  indice       = i; // captura el índice para el lambda

            // Activa o desactiva el candado
            if (candados != null && i < candados.Length && candados[i] != null)
                candados[i].SetActive(!desbloqueado);

            // Configura el botón
            botonesRecuerdos[i].interactable = desbloqueado;
            botonesRecuerdos[i].onClick.RemoveAllListeners();
            botonesRecuerdos[i].onClick.AddListener(() => ReproducirRecuerdo(indice));
        }
    }

    public void ReproducirRecuerdo(int indice)
    {
        if (videos == null || indice >= videos.Length || videos[indice] == null)
        {
            Debug.LogWarning("No hay video para recuerdo " + (indice + 1));
            return;
        }

        // Marca como visto
        PlayerPrefs.SetInt("recuerdo_visto_" + indice, 1);
        PlayerPrefs.Save();

        _videoPlayer.clip = videos[indice];
        _videoPlayer.Play();
        reproductorPanel.SetActive(true);
    }

    public void CerrarReproductor()
    {
        _videoPlayer.Stop();
        reproductorPanel.SetActive(false);
    }
}
