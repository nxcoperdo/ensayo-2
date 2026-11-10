using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JuegoManager : MonoBehaviour
{
    public static JuegoManager Instance;

    [Header("Referencias")]
    public GameObject slender;
    public TMP_Text textoObjetivo;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("Textos")]
    public string textoInicial = "Busca el humo verde para curarte";
    public string textoObjetivoFinal = "Mata al intruso o recupera el chip";

    bool terminado;

    void Awake()
    {
        Instance = this;

        if (slender != null) slender.SetActive(false);
        if (textoObjetivo != null) textoObjetivo.text = textoInicial;
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }

    public void AlDesbloquear()
    {
        if (slender != null) slender.SetActive(true);
        if (textoObjetivo != null) textoObjetivo.text = textoObjetivoFinal;

        if (SonidosJuego.Instance != null)
        {
            SonidosJuego.Instance.PlaySlenderAparece();
            SonidosJuego.Instance.IniciarMusica();
        }
    }

    public void Victoria()
    {
        if (terminado) return;
        terminado = true;

        if (slender != null)
        {
            var ia = slender.GetComponent<IANavigation>();
            if (ia != null) { ia.Detener(); ia.enabled = false; }
        }

        if (textoObjetivo != null) textoObjetivo.gameObject.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(true);
        if (SonidosJuego.Instance != null) SonidosJuego.Instance.PlayVictoria();
    }

    public void Derrota()
    {
        if (terminado) return;
        terminado = true;

        if (textoObjetivo != null) textoObjetivo.gameObject.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(true);
        if (SonidosJuego.Instance != null) SonidosJuego.Instance.PlayDerrota();
    }

    void Update()
    {
        if (terminado && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}