using UnityEngine;
using UnityEngine.AI;

public class IANavigation : MonoBehaviour
{
    public Transform objetivo;
    public float velocidad = 4.5f;
    public float distanciaAtaque = 1.8f;
    public float tiempoEntreAtaques = 1.5f;

    [Header("Zona de golpe")]
    public GameObject zonaGolpe;
    public float inicioGolpe = 0.5f;
    public float duracionGolpe = 0.4f;

    NavMeshAgent agente;
    Animator anim;
    VidaPlayer vidaObjetivo;
    float proximoAtaque;
    bool muerto;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agente.speed = velocidad;
        agente.stoppingDistance = distanciaAtaque - 0.3f;

        anim.applyRootMotion = false;

        if (objetivo != null) vidaObjetivo = objetivo.GetComponent<VidaPlayer>();
        if (zonaGolpe != null) zonaGolpe.SetActive(false);
    }

    void Update()
    {
        if (muerto || objetivo == null || !agente.isOnNavMesh) return;

        // si el soldado murio, deja de perseguir y atacar
        if (vidaObjetivo != null && vidaObjetivo.EstaMuerto)
        {
            agente.isStopped = true;
            anim.SetFloat("Speed", 0f);
            return;
        }

        float distancia = Vector3.Distance(transform.position, objetivo.position);

        if (distancia <= distanciaAtaque)
        {
            agente.isStopped = true;
            anim.SetFloat("Speed", 0f);

            if (Time.time >= proximoAtaque)
            {
                proximoAtaque = Time.time + tiempoEntreAtaques;
                anim.SetTrigger("Attack");
                Invoke(nameof(SonidoAtaque), inicioGolpe);
                Invoke(nameof(ActivarZona), inicioGolpe);
            }
        }
        else
        {
            agente.isStopped = false;
            agente.SetDestination(objetivo.position);
            anim.SetFloat("Speed", 4f);
        }
    }

    void SonidoAtaque()
    {
        if (muerto) return;
        if (SonidosJuego.Instance != null) SonidosJuego.Instance.PlaySlenderAtaque();
    }

    void ActivarZona()
    {
        if (muerto || zonaGolpe == null) return;
        zonaGolpe.SetActive(true);
        Invoke(nameof(DesactivarZona), duracionGolpe);
    }

    void DesactivarZona()
    {
        if (zonaGolpe != null) zonaGolpe.SetActive(false);
    }

    public void Detener()
    {
        muerto = true;
        CancelInvoke();
        DesactivarZona();
        if (agente != null && agente.isOnNavMesh) agente.isStopped = true;
        if (anim != null) anim.SetFloat("Speed", 0f);
    }
}