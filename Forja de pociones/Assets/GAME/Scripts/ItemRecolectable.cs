using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{

    public ingredientes objScript;
    public AudioClip clip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.actualizarData(objScript);
                AudioSource.PlayClipAtPoint(clip, transform.position);
                Destroy(gameObject);
            }
    }
}
