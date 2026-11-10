using UnityEngine;
using UnityEngine.UI;

public class SlenderVida : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vida;
    public Slider barraVida;

    Transform cam;
    bool muerto;

    void Start()
    {
        vida = vidaMaxima;
        if (barraVida != null)
        {
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vida;
        }
        if (Camera.main != null) cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (barraVida != null && cam != null)
            barraVida.transform.parent.rotation = cam.rotation;
    }

    public void RecibirDano(float dano)
    {
        if (muerto) return;

        vida = Mathf.Max(0f, vida - dano);
        if (barraVida != null) barraVida.value = vida;

        if (vida <= 0f) Morir();
    }

    void Morir()
    {
        muerto = true;

        if (JuegoManager.Instance != null) JuegoManager.Instance.Victoria();

        var ia = GetComponent<IANavigation>();
        if (ia != null) { ia.Detener(); ia.enabled = false; }

        var agente = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agente != null) agente.enabled = false;

        var anim = GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        if (barraVida != null) barraVida.transform.parent.gameObject.SetActive(false);

        Destroy(gameObject, 2f);
    }
}
