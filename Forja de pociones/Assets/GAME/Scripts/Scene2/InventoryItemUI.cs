using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI cantidadTexto;
    public Image iconoImg;

    public void SetData(string nombre, int cantidad, Sprite icono)
    {
        nombreTexto.text = nombre;
        cantidadTexto.text = cantidad.ToString();
        if (icono != null) iconoImg.sprite = icono;
    }
}