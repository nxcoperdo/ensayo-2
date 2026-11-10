using UnityEngine;

public class SoldadoControl : MonoBehaviour
{
    public float velocidadGiro = 10f;
    public Transform camara;

    [Header("Correr")]
    public float multiplicadorCorrer = 1.6f;

    Animator anim;
    VidaPlayer vida;

    void Start()
    {
        anim = GetComponent<Animator>();
        vida = GetComponent<VidaPlayer>();
        anim.SetBool("OnGround", true);
        if (camara == null && Camera.main != null) camara = Camera.main.transform;
    }

    void Update()
    {
        if (vida != null && vida.EstaMuerto)
        {
            anim.speed = 1f;
            return;
        }

        // correr y rodar bloqueados hasta que la vida llegue al maximo por primera vez
        bool desbloqueado = vida != null && vida.GolpeDesbloqueado;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // direccion relativa a la camara, sin inclinacion vertical
        Vector3 adelante = camara.forward; adelante.y = 0; adelante.Normalize();
        Vector3 derecha = camara.right; derecha.y = 0; derecha.Normalize();
        Vector3 dir = adelante * v + derecha * h;

        bool moviendo = dir.sqrMagnitude > 0.01f;
        bool correr = desbloqueado && Input.GetKey(KeyCode.LeftShift);

        float avance = 0f;
        if (moviendo)
        {
            Quaternion rot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, velocidadGiro * Time.deltaTime);

            // solo avanza cuando ya esta mirando hacia donde va
            float angulo = Vector3.Angle(transform.forward, dir);
            avance = angulo < 45f ? 1f : 0f;
        }

        anim.SetBool("OnGround", true);
        anim.SetBool("Sprint", moviendo && correr);
        anim.speed = (moviendo && correr) ? multiplicadorCorrer : 1f;
        anim.SetFloat("X", 0f, 0.1f, Time.deltaTime);
        anim.SetFloat("Y", avance, 0.1f, Time.deltaTime);
        anim.SetFloat("Speed", avance, 0.1f, Time.deltaTime);

        if (desbloqueado && Input.GetKeyDown(KeyCode.Space)) anim.SetTrigger("Roll");
    }
}