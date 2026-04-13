using UnityEngine;

public class Reload : MonoBehaviour
{
    public GameObject player; // Referencia al jugador
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.transform.position = new Vector2(0, 0); // Ajusta las coordenadas según tu escena
            //Debug.Log("Player entered the reload area, loading Escena2...");
        }
    }
}
