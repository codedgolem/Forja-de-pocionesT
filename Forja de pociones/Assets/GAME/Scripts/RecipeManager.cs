using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
   
    public bool PuedePreparar(RecetaData receta)
    {
        
        Dictionary<IngredienteSO, int> inventario = GameManager.Instance.CollectedItems;

        if (receta.ingredientesRequeridos == null || receta.ingredientesRequeridos.Count == 0)
        {
            Debug.LogWarning($"La receta {receta.nombre} no tiene ingredientes definidos.");
            return false;
        }

        foreach (var requisito in receta.ingredientesRequeridos)
        {
            
            IngredienteSO dataIngrediente = GameDataLoader.instance.IngredientesList.Find(i => i.nombre == requisito.nombre);

            if (dataIngrediente != null)
            {
                
                if (inventario.TryGetValue(dataIngrediente, out int cantidadPoseida))
                {
                    if (cantidadPoseida < requisito.cantidad)
                    {
                        Debug.Log($"Faltan ingredientes para {receta.nombre}: {requisito.nombre} (Tienes: {cantidadPoseida} / Necesitas: {requisito.cantidad})");
                        return false;
                    }
                }
                else
                {
                    Debug.Log($"No tienes el ingrediente {requisito.nombre} en el inventario (0/{requisito.cantidad}).");
                    return false;
                }
            }
            else
            {
                Debug.LogError($"El ingrediente '{requisito.nombre}' requerido por la receta '{receta.nombre}' no existe en el GameDataLoader.");
                return false;
            }
        }

        Debug.Log($"¡Éxito! Tienes todos los ingredientes para preparar: {receta.nombre}");
        return true;
    }
}