using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target; // El objetivo que la cámara seguirá
    public float velocidadCamara = 0.025f;
    public Vector3 desplazamiento;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Se ejecuta despues del update es decir despues del movimiento del jugador
    private void LateUpdate()
    {
        
        Vector3 posicionDeseada = target.position + desplazamiento;

        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        transform.position = posicionSuavizada;
    }
}
