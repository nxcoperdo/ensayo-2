using UnityEngine;
using UnityEngine.UI;

public class VidaPlayer : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaInicial = 50f;
    public float tasaDeDisminucion = 1.0f;
    public float danoFuego = 5.0f;
    public float cantidadDeVida = 25f;

    public Image barraDeVida;

    Animator anim;
    bool muerto;
    float vida;

    public bool EstaMuerto => muerto;
    public bool GolpeDesbloqueado { get; private set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        vida = Mathf.Clamp(vidaInicial, 0, vidaMaxima);
        ActualizarBarra();
    }

    void ActualizarBarra()
    {
        if (barraDeVida != null) barraDeVida.fillAmount = vida / vidaMaxima;
    }

    public void RecibirDano(float dano)
    {
        if (muerto) return;

        vida = Mathf.Clamp(vida - dano, 0, vidaMaxima);
        ActualizarBarra();

        // solo suena con golpes reales, no con el dano continuo pequeno
        if (SonidosJuego.Instance != null && dano > 0.5f) SonidosJuego.Instance.PlayDano();

        if (vida <= 0) Morir();
    }

    public void Curar(float cantidad)
    {
        if (muerto) return;

        vida = Mathf.Clamp(vida + cantidad, 0, vidaMaxima);
        ActualizarBarra();

        if (!GolpeDesbloqueado && vida >= vidaMaxima - 0.01f)
        {
            GolpeDesbloqueado = true;
            if (JuegoManager.Instance != null) JuegoManager.Instance.AlDesbloquear();
        }
    }

    void Morir()
    {
        muerto = true;

        if (anim != null)
        {
            anim.SetFloat("Y", 0f);
            anim.SetFloat("Speed", 0f);
            anim.SetBool("Sprint", false);
            anim.SetBool("Dead", true);
        }

        var control = GetComponent<SoldadoControl>();
        if (control != null) control.enabled = false;

        if (SonidosJuego.Instance != null) SonidosJuego.Instance.Curando(false);
        if (JuegoManager.Instance != null) JuegoManager.Instance.Derrota();
    }

    void OnTriggerStay(Collider other)
    {
        if (muerto) return;

        if (other.gameObject.name == "VFX" || other.gameObject.name == "VFX(1)")
        {
            RecibirDano(danoFuego * Time.deltaTime);
        }

        if (other.gameObject.name.StartsWith("VFX_GreenSmoke"))
        {
            Curar(cantidadDeVida * Time.deltaTime);
            if (SonidosJuego.Instance != null) SonidosJuego.Instance.Curando(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("VFX_GreenSmoke") && SonidosJuego.Instance != null)
            SonidosJuego.Instance.Curando(false);
    }

    void OnCollisionStay(Collision collision)
    {
        if (muerto) return;

        if (collision.gameObject.name == "Slender")
        {
            RecibirDano(tasaDeDisminucion * Time.deltaTime);
        }
    }
}