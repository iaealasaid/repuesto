using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausaPanel;
    public string   escenaMenu = "Menu";

    private bool _pausado = false;

    // ── Input Action callback ────────────────────────────────────────────────────
    // Este método se conecta al evento Pause del Input Actions
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_pausado)
            Reanudar();
        else
            Pausar();
    }

    // ── Pausar ───────────────────────────────────────────────────────────────────

    public void Pausar()
    {
        _pausado = true;
        pausaPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // ── Reanudar ─────────────────────────────────────────────────────────────────

    public void Reanudar()
    {
        _pausado = false;
        pausaPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // ── Salir al menú ────────────────────────────────────────────────────────────

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}
