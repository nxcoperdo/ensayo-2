using UnityEngine;

public class SlenderGolpe : MonoBehaviour
{
    public float dano = 10f;
    bool yaGolpeo; // un solo golpe por ataque

    // se llama cada vez que se activa la zona (inicio del ataque)
    void OnEnable()
    {
        yaGolpeo = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (yaGolpeo) return;

        var vida = other.GetComponentInParent<VidaPlayer>();
        if (vida == null || vida.EstaMuerto) return;

        yaGolpeo = true;
        vida.RecibirDano(dano);
    }
}