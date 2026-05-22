using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalCapitulos = 10;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
static void CrearInstancia()
{
    if (Object.FindAnyObjectByType<GameManager>() == null)
    {
        GameObject gm = new GameObject("GameManager");
        gm.AddComponent<GameManager>();
    }
}
    
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ── Completar capítulo ────────────────────────────────────────────────────────

    public void CompletarCapitulo(int indice)
    {
        PlayerPrefs.SetInt("capitulo_" + indice, 1);
        PlayerPrefs.SetInt("recuerdo_" + indice, 1);
        PlayerPrefs.Save(); // escribe en disco inmediatamente
        Debug.Log("Capítulo " + (indice + 1) + " completado.");
    }

    // ── Verificaciones ────────────────────────────────────────────────────────────

    public bool CapituloCompletado(int indice)
    {
        return PlayerPrefs.GetInt("capitulo_" + indice, 0) == 1;
    }

    public bool RecuerdoDesbloqueado(int indice)
    {
        return PlayerPrefs.GetInt("recuerdo_" + indice, 0) == 1;
    }

    // ── Nueva partida ─────────────────────────────────────────────────────────────

    public void NuevaPartida()
    {
        for (int i = 0; i < totalCapitulos; i++)
        {
            PlayerPrefs.DeleteKey("capitulo_" + i);
            PlayerPrefs.DeleteKey("recuerdo_" + i);
        }
        PlayerPrefs.Save();
        Debug.Log("Partida reiniciada.");
    }

    // ── Cargar escena del capítulo ────────────────────────────────────────────────

    public void CargarCapitulo(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
