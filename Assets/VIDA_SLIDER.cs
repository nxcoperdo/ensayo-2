using UnityEngine;
using UnityEngine.UI;

public class DamageOnCollision : MonoBehaviour
{
    public Slider healthSlider;   // Slider de vida
    public float damagePerSecond = 10f; // Daño por segundo

    private bool isDead = false;

    private void OnCollisionStay(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("enemigo"))
        {
            if (healthSlider != null)
            {
                // Hace daño constante mientras sigue en contacto
                healthSlider.value -= damagePerSecond * Time.deltaTime;

                Debug.Log("Vida restante: " + healthSlider.value);

                if (healthSlider.value <= 0)
                {
                    healthSlider.value = 0;
                    isDead = true;
                    Debug.Log("¡El jugador ha sido derrotado!");
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogError("¡El Slider de vida no está asignado!");
            }
        }
    }
}