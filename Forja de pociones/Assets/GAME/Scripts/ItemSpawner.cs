using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    //private List<ingredientes> ingredientesList = GameDataLoader.instance.IngredientesList; 
    public List<ItemS> ItemS;
    public List<Transform> puntosAleatorios;
    public GameObject itemPrefab;


    private List<int> usedIndex = new List<int>();
    private int index = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem()
    {
        foreach (var item in ItemS)
        {
            for (int i=0; i < item.cantidad; i++)
            {
                SpawnItemEnPunto(item);
            } 
        }
    }


    public void SpawnItemEnPunto(ItemS item)
    {
        //Debug.Log(item);
        //Debug.Log("Index lenght: "+usedIndex.Count);
        //if (usedIndex.Count > 0)
        //{
        //    for (int i = 0; i < usedIndex.Count; i++)
        //    {
        //        if (usedIndex[i] == index)
        //        {
        //            index = Random.Range(0, ItemS.Count);
        //            i = -1; // Reiniciar el bucle para verificar el nuevo índice
        //                    //Debug.Log("Index cambiado desde el if: " + index);
        //        }
        //    }
        //}
        //else
        //{
        //    index = Random.Range(0, ItemS.Count);
        //    //Debug.Log("Index desde el else: " + index);
        //}

        index = Random.Range(0, puntosAleatorios.Count);

        while (usedIndex.Contains(index))
        {
            index = Random.Range(0, puntosAleatorios.Count);
        }


        // Debug.Log("Index final: " + index);


        GameObject newObj = Instantiate(itemPrefab, puntosAleatorios[index].position, Quaternion.identity);
        usedIndex.Add(index); // Agregar el índice utilizado a la lista de índices usados

        foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
        {
            if (ingrediente.nombre == item.nombre)
            {
                newObj.GetComponent<ItemRecolectable>().objScript = ingrediente;
                newObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("IngredientesIcon/" + ingrediente.iconId);

                //Debug.Log("Ruta: " + "IngredientesIcon/" + ingrediente.iconId);
                break;
            }
        }

    }
}
