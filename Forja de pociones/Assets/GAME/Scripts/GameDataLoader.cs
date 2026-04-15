using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Esta clase ayuda a leer el JSON
[System.Serializable]
public class ingredientesListWrapper
{
    public List<Ingrediente> ingredientes;
}

public class GameDataLoader : MonoBehaviour
{
    // Solo una Instance para evitar el error CS0117
    public static GameDataLoader Instance;

    private List<ingredientes> ingredientesList = new List<ingredientes>();
    public List<ingredientes> IngredientesList { get => ingredientesList; set => ingredientesList = value; }

    private List<Ingrediente> classIngrediente = new List<Ingrediente>();

    void Awake()
    {
        // Singleton limpio: solo uno puede existir
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadIngredientes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void loadIngredientes()
    {
        // Asegúrate de que el nombre del archivo sea el correcto (ingredientesData.json)
        string path = Path.Combine(Application.streamingAssetsPath, "ingredientesData.json");

        if (!File.Exists(path))
        {
            Debug.LogError("No se encontró el JSON en: " + path);
            return;
        }

        string jsonString = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<ingredientesListWrapper>(jsonString);

        if (wrapper != null && wrapper.ingredientes != null)
        {
            classIngrediente = wrapper.ingredientes;
            ingredientesList.Clear();

            foreach (var item in classIngrediente)
            {
                ingredientes newIng = ScriptableObject.CreateInstance<ingredientes>();
                newIng.nombre = item.nombre;
                newIng.valor = item.valor;
                newIng.iconId = item.iconoId;

                // Quita espacios para el Enum (ej: "Hongo Brillante" -> "HongoBrillante")
                string tipoTexto = item.nombre.Replace(" ", "");
                try
                {
                    newIng.ingrediente = (ingredientes.TipoIngrediente)System.Enum.Parse(typeof(ingredientes.TipoIngrediente), tipoTexto);
                    ingredientesList.Add(newIng);
                }
                catch
                {
                    Debug.LogWarning("No se pudo parsear el tipo: " + tipoTexto);
                }
            }
        }
    }
}