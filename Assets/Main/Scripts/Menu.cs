using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject rpage1;
    public GameObject lpage1;
    public GameObject rpage2;
    public GameObject lpage2;

    public GameObject last;
    public GameObject next;

    private int actpage = 1;

    void Start()
    {
        ShowPage(1);
    }

    public void Nextpage()
    {
        if (actpage >= 2) return;
        actpage++;
        ShowPage(actpage);
    }

    public void Lastpage()
    {
        if (actpage <= 1) return;
        actpage--;
        ShowPage(actpage);
    }

    void ShowPage(int numero)
    {
        rpage1.SetActive(numero == 1);
        lpage1.SetActive(numero == 1);
        rpage2.SetActive(numero == 2);
        lpage2.SetActive(numero == 2);

        last.SetActive(numero > 1);
        next.SetActive(numero < 2);
    }
    
    public void Volver()
    {
        SceneManager.LoadScene("Desk");
    }
    public void C1()
    {
        SceneManager.LoadScene("Chapter1");
    }
    public void C2()
    {
        SceneManager.LoadScene("Chapter2");
    }
    public void C3()
    {
        SceneManager.LoadScene("Chapter3");
    }
    public void C4()
    {
        SceneManager.LoadScene("Chapter4");
    }
}
