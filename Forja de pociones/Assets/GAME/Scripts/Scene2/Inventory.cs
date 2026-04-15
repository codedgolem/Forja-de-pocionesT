using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI cantidadTexto;
    public Image iconoImg;

    public void SetData(string nombre, int cantidad, Sprite icono)
    {
        if (nombreTexto != null) nombreTexto.text = nombre;
        if (cantidadTexto != null) cantidadTexto.text = cantidad.ToString();
        if (iconoImg != null && icono != null) iconoImg.sprite = icono;
    }
}