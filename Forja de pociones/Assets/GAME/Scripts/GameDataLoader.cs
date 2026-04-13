using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ingredientesListWrapper
{
    public List<Ingrediente> ingredientes;
}


public class GameDataLoader : MonoBehaviour
{

    public static GameDataLoader instance;

    private List<ingredientes> ingredientesList = new List<ingredientes>();
    private List<Ingrediente> classIngrediente = new List<Ingrediente>();

    public List<ingredientes> IngredientesList { get => ingredientesList; set => ingredientesList = value; }
    public List<Ingrediente> ClassIngrediente { get => classIngrediente; set => classIngrediente = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        loadIngredientes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadIngredientes()
    {

        string path = Application.streamingAssetsPath + "/ingredientesData.json";

        string jsonString = File.ReadAllText(path);
        classIngrediente = JsonUtility.FromJson<ingredientesListWrapper>(jsonString).ingredientes;

        foreach (var ingrediente in classIngrediente)
        {
            ingredientes newIngrediente = ScriptableObject.CreateInstance<ingredientes>();
            newIngrediente.nombre = ingrediente.nombre;
            newIngrediente.valor = ingrediente.valor;
            newIngrediente.iconId = ingrediente.iconoId;

            string tipoTexto = ingrediente.nombre.Replace(" ", "");
            newIngrediente.ingrediente = (ingredientes.TipoIngrediente)System.Enum.Parse(typeof(ingredientes.TipoIngrediente), tipoTexto);
            ingredientesList.Add(newIngrediente);
        }

        //Debug.Log("Ingredientes cargados: " + ingredientesList.Count);
        //Debug.Log("Primer ingrediente: " + ingredientesList[0].nombre);
        //Debug.Log("Primer ingrediente enum: " + ingredientesList[0].ingrediente);
        //Debug.Log("Primer ingrediente valor: " + ingredientesList[0].valor);
        //Debug.Log("Primer ingrediente iconId: " + ingredientesList[0].iconId);
        //Debug.Log("Clase ingredeingte: " + classIngrediente.Count);


    }
}
