using UnityEngine;

public class SoldadoMelee : MonoBehaviour
{
    public GameObject zonaMelee;

    [Header("Punetazo (clic izquierdo)")]
    public float danoPunio = 20f;
    public float tiempoEntrePunios = 0.8f;
    public float inicioPunio = 0.25f;
    public float duracionPunio = 0.25f;

    [Header("Patada (clic derecho)")]
    public float danoPatada = 35f;
    public float tiempoEntrePatadas = 1.2f;
    public float inicioPatada = 0.6f;
    public float duracionPatada = 0.3f;

    Animator anim;
    VidaPlayer vida;
    ZonaMelee zona;
    float proximoGolpe;
    float danoActual;

    void Start()
    {
        anim = GetComponent<Animator>();
        vida = GetComponent<VidaPlayer>();
        if (zonaMelee != null)
        {
            zona = zonaMelee.GetComponent<ZonaMelee>();
            zonaMelee.SetActive(false);
        }
    }

    void Update()
    {
        if (vida == null || vida.EstaMuerto) return;
        if (!vida.GolpeDesbloqueado) return;
        if (Time.time < proximoGolpe) return;

        if (Input.GetMouseButtonDown(0))
        {
            proximoGolpe = Time.time + tiempoEntrePunios;
            danoActual = danoPunio;
            anim.SetTrigger("Use");
            Invoke(nameof(SonidoPunio), inicioPunio);
            Invoke(nameof(ActivarZona), inicioPunio);
            Invoke(nameof(DesactivarZona), inicioPunio + duracionPunio);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            proximoGolpe = Time.time + tiempoEntrePatadas;
            danoActual = danoPatada;
            anim.SetTrigger("Kick");
            Invoke(nameof(SonidoPatada), inicioPatada);
            Invoke(nameof(ActivarZona), inicioPatada);
            Invoke(nameof(DesactivarZona), inicioPatada + duracionPatada);
        }
    }

    // el sonido suena justo en el momento del impacto
    void SonidoPunio()
    {
        if (SonidosJuego.Instance != null) SonidosJuego.Instance.PlayPunio();
    }

    void SonidoPatada()
    {
        if (SonidosJuego.Instance != null) SonidosJuego.Instance.PlayPatada();
    }

    void ActivarZona()
    {
        if (zonaMelee == null) return;
        if (zona != null) zona.dano = danoActual;
        zonaMelee.SetActive(true);
    }

    void DesactivarZona()
    {
        if (zonaMelee != null) zonaMelee.SetActive(false);
    }
}