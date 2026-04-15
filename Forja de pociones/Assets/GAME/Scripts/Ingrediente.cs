using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemS 
{
    public string nombre;
    public int cantidad;
}
 

[Serializable]
public class Ingrediente
{
    public string nombre;
    public int valor;
    public string iconoId;
}


[Serializable]
public class RequisitoJSON
{
    public string nombre;
    public int cantidad;
}


[Serializable]
public class RecetaJSON
{
    public string nombre;
    public string dificultad;
    public string descripcion;
    public List<RequisitoJSON> ingredientesRequeridos;
}


[Serializable]
public class ContenedorUnificado
{
    public List<Ingrediente> ingredientes;
    public List<RecetaJSON> recetas;
}