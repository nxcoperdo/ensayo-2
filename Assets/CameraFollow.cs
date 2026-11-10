using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    public Transform target; // Arrastra aquí al soldado

    [Header("Ajustes de Cámara y Zoom")]
    public Vector3 offset = new Vector3(0f, 2.5f, -4f);
    public float rotationSpeed = 5f;
    public float smoothSpeed = 10f;

    [Header("Límites de Zoom")]
    public float zoomSpeed = 2f;
    public float minZoom = -2f;
    public float maxZoom = -8f;
    float currentDistance;

    [Header("Colisión")]
    public LayerMask capasObstaculo = ~0;
    public float radioCamara = 0.3f;
    public float distanciaMinima = 0.5f;

    float currentX = 0f;
    float currentY = 15f;
    float minY = -10f;
    float maxY = 60f;

    void Start()
    {
        currentDistance = offset.z;
        BloquearCursor(true);
    }

    void Update()
    {
        // ESC libera el cursor
        if (Input.GetKeyDown(KeyCode.Escape)) BloquearCursor(false);

        // Clic izquierdo dentro del juego vuelve a bloquearlo
        if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButtonDown(0))
        {
            BloquearCursor(true);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Rotar la cámara con el mouse (solo si el cursor está bloqueado)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, minY, maxY);
        }

        // 2. Zoom con la rueda
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentDistance += scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, maxZoom, minZoom);
        }

        // 3. Posición deseada detrás del objetivo
        Vector3 pivote = target.position + Vector3.up * 1.5f;
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Vector3 direccion = rotation * new Vector3(offset.x, offset.y - 1.5f, currentDistance);
        float distancia = direccion.magnitude;
        Vector3 dirNorm = direccion.normalized;

        // 4. Si hay una pared entre el personaje y la cámara, acerca la cámara
        RaycastHit[] hits = Physics.SphereCastAll(pivote, radioCamara, dirNorm, distancia,
            capasObstaculo, QueryTriggerInteraction.Ignore);

        float masCercano = distancia;
        foreach (var h in hits)
        {
            // ignora al propio jugador y sus hijos
            if (h.collider.transform == target || h.collider.transform.IsChildOf(target)) continue;
            if (h.distance < masCercano) masCercano = h.distance;
        }
        distancia = Mathf.Max(masCercano, distanciaMinima);

        Vector3 desiredPosition = pivote + dirNorm * distancia;

        // 5. Movimiento suave y mirar al personaje
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(pivote);
    }

    void BloquearCursor(bool bloquear)
    {
        Cursor.lockState = bloquear ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !bloquear;
    }
}