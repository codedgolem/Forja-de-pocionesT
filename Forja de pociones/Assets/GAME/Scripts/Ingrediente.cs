using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemS
{
    public string nombre;
    public int cantidad;
} // <--- Faltaba esta llave

[System.Serializable]
public class IngredienteJSON
{
    public string nombre;
    public int valor;
    public string iconoId;
}

[System.Serializable]
public class RecetaIngrediente
{
    public string nombre;
    public int cantidad;
}

[System.Serializable]
public class RecetaData
{
    public string nombre;
    public string dificultad;
    public string descripcion;
    public List<RecetaIngrediente> ingredientesRequeridos;
}
