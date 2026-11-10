using UnityEngine;

public class ObjetoVictoria : MonoBehaviour
{
    public bool soloConCombateDesbloqueado = false; // true = solo cuenta despues de curarte

    bool recogido;

    void OnTriggerEnter(Collider other)
    {
        if (recogido) return;

        var vida = other.GetComponentInParent<VidaPlayer>();
        if (vida == null || vida.EstaMuerto) return;

        if (soloConCombateDesbloqueado && !vida.GolpeDesbloqueado) return;

        recogido = true;

        // desaparece el chip
        gameObject.SetActive(false);

        // sale el dialogo de ganaste
        if (JuegoManager.Instance != null) JuegoManager.Instance.Victoria();
    }
}