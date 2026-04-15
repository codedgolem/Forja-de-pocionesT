using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
   
    public bool PuedePreparar(RecetaData receta)
    {
        
        Dictionary<ingredientes, int> inventario = GameManager.Instance.CollectedItems;

        foreach (var requisito in receta.ingredientesRequeridos)
        {
           
            ingredientes dataIngrediente = GameDataLoader.instance.IngredientesList.Find(i => i.nombre == requisito.nombre);

            if (dataIngrediente != null)
            {
                
                if (inventario.TryGetValue(dataIngrediente, out int cantidadPoseida))
                {
                    if (cantidadPoseida < requisito.cantidad)
                    {
                        Debug.Log($"Faltan ingredientes para {receta.nombre}: {requisito.nombre} ({cantidadPoseida}/{requisito.cantidad})");
                        return false;
                    }
                }
                else
                {
                    Debug.Log($"No tienes el ingrediente {requisito.nombre} en el inventario.");
                    return false;
                }
            }
            else
            {
                Debug.LogWarning($"El ingrediente {requisito.nombre} de la receta no existe en la base de datos.");
                return false;
            }
        }

        Debug.Log($"¡Tienes todos los ingredientes para: {receta.nombre}!");
        return true;
    }
}