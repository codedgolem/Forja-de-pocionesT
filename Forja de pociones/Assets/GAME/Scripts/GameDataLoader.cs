using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ingredientesListWrapper
{
    public List<IngredienteJSON> ingredientes;
    public List<RecetaData> recetas;
}

public class GameDataLoader : MonoBehaviour
{
    public static GameDataLoader instance;

    private List<IngredienteSO> ingredientesList = new List<IngredienteSO>();
    public List<IngredienteSO> IngredientesList { get => ingredientesList; }

    private List<RecetaData> recetasList = new List<RecetaData>();
    public List<RecetaData> RecetasList { get => recetasList; }

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        loadIngredientes();
    }

    public void loadIngredientes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ingredientesData.json");

        if (!File.Exists(path))
        {
            Debug.LogError("No se encontró el archivo JSON en: " + path);
            return;
        }

        string jsonString = File.ReadAllText(path);
        ingredientesListWrapper data = JsonUtility.FromJson<ingredientesListWrapper>(jsonString);

        recetasList = data.recetas;
        ingredientesList.Clear();

        foreach (var item in data.ingredientes)
        {
            IngredienteSO newSO = ScriptableObject.CreateInstance<IngredienteSO>();
            newSO.nombre = item.nombre;
            newSO.valor = item.valor;
            newSO.iconId = item.iconoId;
            ingredientesList.Add(newSO);
        }
        Debug.Log("Datos cargados correctamente.");
    }
}