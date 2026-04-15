using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Configuración")]
    public List<ItemS> ItemS;
    public List<Transform> puntosAleatorios;
    public GameObject itemPrefab;

    private List<int> usedIndex = new List<int>();

    void Start()
    {
        
        Invoke("SpawnItem", 0.1f);
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
        do
        {
            index = Random.Range(0, puntosAleatorios.Count);
        } while (usedIndex.Contains(index));

        usedIndex.Add(index);

        GameObject newObj = Instantiate(itemPrefab, puntosAleatorios[index].position, Quaternion.identity);
        ItemRecolectable recolectable = newObj.GetComponent<ItemRecolectable>();

        if (recolectable != null)
        {
            IngredienteSO data = GameDataLoader.instance.IngredientesList.Find(ing => ing.nombre == config.nombre);
            if (data != null)
            {
                recolectable.objScript = data;
                Sprite spriteIcono = Resources.Load<Sprite>("IngredientesIcon/" + data.iconId);
                if (spriteIcono != null) newObj.GetComponent<SpriteRenderer>().sprite = spriteIcono;
            }
        }
    }
}