using UnityEngine;

public class ZonaMelee : MonoBehaviour
{
    public float dano = 20f;
    bool yaGolpeo;

    void OnEnable()
    {
        yaGolpeo = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (yaGolpeo) return;

        var vida = other.GetComponentInParent<SlenderVida>();
        if (vida == null) return;

        yaGolpeo = true;
        vida.RecibirDano(dano);
    }
}