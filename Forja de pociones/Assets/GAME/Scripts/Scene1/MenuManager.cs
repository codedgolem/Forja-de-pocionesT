
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Texto entre comillas y parentesis")]
    public GameObject panelInstrucciones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
        }
    }

   public void BotonJugar()
    {
        GameManager.Instance.LoadScene("Escena2");
    }

    public void BotonInstrucciones()
    {
        if(panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(true);
        }
    }

    public void BotonCerrarIntrucciones()
    {
        if (panelInstrucciones != null)
        {
            panelInstrucciones.SetActive(false);
        }
    }

    public void BotonSalir()
    {
        GameManager.Instance.ExitGame();
    }
}
