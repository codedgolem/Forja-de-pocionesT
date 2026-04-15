using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public ingredientes objScript; 
    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.actualizarData(objScript);
            if (clip != null) AudioSource.PlayClipAtPoint(clip, transform.position);
            Destroy(gameObject);
        }
    }
}