using UnityEngine;

public class SlenderControl : MonoBehaviour
{
    public float velocidadCaminar = 1.5f;
    public float velocidadCorrer = 4f;
    public float velocidadGiro = 10f;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, z);

        bool correr = Input.GetKey(KeyCode.LeftShift);
        float vel = correr ? velocidadCorrer : velocidadCaminar;
        bool moviendo = dir.magnitude > 0.1f;

        if (moviendo)
        {
            // gira hacia donde te mueves y avanza hacia adelante
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, velocidadGiro * Time.deltaTime);
            transform.Translate(Vector3.forward * vel * Time.deltaTime);
        }

        anim.SetFloat("Speed", moviendo ? vel : 0f, 0.1f, Time.deltaTime);

        if (Input.GetMouseButtonDown(0)) anim.SetTrigger("Attack");
        if (Input.GetKeyDown(KeyCode.E)) anim.SetTrigger("Scream");
    }
}