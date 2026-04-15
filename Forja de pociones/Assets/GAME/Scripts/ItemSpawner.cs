using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Configuración")]
    public List<ItemS> ItemS; // Esta es la lista de estructuras que definimos en Ingrediente.cs
    public List<Transform> puntosAleatorios;
    public GameObject itemPrefab;

    private List<int> usedIndex = new List<int>();

    void Start()
    {
        // Un pequeño delay para asegurar que el GameDataLoader ya leyó el JSON
        Invoke("SpawnItem", 0.5f);
    }

    public void SpawnItem()
    {
        if (puntosAleatorios.Count == 0) return;

        foreach (var config in ItemS)
        {
            for (int i = 0; i < config.cantidad; i++)
            {
                SpawnItemEnPunto(config);
            }
        }
    }

    public void SpawnItemEnPunto(ItemS config)
    {
        if (usedIndex.Count >= puntosAleatorios.Count) return;

        int index;
        int intentos = 0; // Seguridad para evitar bucles infinitos

        do
        {
            index = Random.Range(0, puntosAleatorios.Count);
            intentos++;
        } while (usedIndex.Contains(index) && intentos < 100);

        if (usedIndex.Contains(index)) return; // Si no encontró punto libre, sale

        usedIndex.Add(index);

        GameObject newObj = Instantiate(itemPrefab, puntosAleatorios[index].position, Quaternion.identity);
        ItemRecolectable recolectable = newObj.GetComponent<ItemRecolectable>();

        if (recolectable != null)
        {
            // Usamos 'Instance' (con I mayúscula) para conectar con el script de Esteban
            if (GameDataLoader.Instance != null && GameDataLoader.Instance.IngredientesList != null)
            {
                ingredientes data = GameDataLoader.Instance.IngredientesList.Find(ing => ing.nombre == config.nombre);

                if (data != null)
                {
                    recolectable.objScript = data;

                    // --- SOLUCIÓN AL ERROR DE LA LÍNEA 55 ---
                    // Buscamos el componente SpriteRenderer con seguridad
                    SpriteRenderer sRenderer = newObj.GetComponent<SpriteRenderer>();

                    // Si no está en el padre, lo buscamos en los hijos (por si acaso)
                    if (sRenderer == null) sRenderer = newObj.GetComponentInChildren<SpriteRenderer>();

                    if (sRenderer != null)
                    {
                        // Cargamos el sprite usando el iconId que viene del JSON
                        Sprite spriteIcono = Resources.Load<Sprite>("IngredientesIcon/" + data.iconId);
                        if (spriteIcono != null)
                        {
                            sRenderer.sprite = spriteIcono;
                        }
                        else
                        {
                            Debug.LogWarning("No se encontró el sprite en Resources/IngredientesIcon/ para el ID: " + data.iconId);
                        }
                    }
                    else
                    {
                        // Si llegamos aquí, el Prefab que pusiste en el Inspector no tiene forma de mostrar imagen
                        Debug.LogError("¡ERROR! El prefab '" + itemPrefab.name + "' no tiene un SpriteRenderer.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró la data en el JSON para el ingrediente: " + config.nombre);
                }
            }
        }
    }
}