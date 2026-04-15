using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

// --- CLASES DE DATOS (Fuera de la clase principal para evitar errores) ---
[System.Serializable]
public class RequisitoJSON
{
    public string ingrediente;
    public int cantidad;
}

[System.Serializable]
public class RecetaJSON
{
    public string nombre;
    public List<RequisitoJSON> ingredientesRequeridos;
    public string efecto;
}

[System.Serializable]
public class ContenedorRecetas
{
    public List<RecetaJSON> recetas;
}

public class RecipeManager : MonoBehaviour
{
    [Header("Configuración del Archivo")]
    public string nombreArchivoJson = "recetas.json";

    [Header("Referencias UI")]
    public TextMeshProUGUI textoReceta;
    public TextMeshProUGUI textoInventario;
    public TextMeshProUGUI textoCaldero;
    public TMP_Dropdown menuDesplegable;

    [Header("Ventana de Victoria")]
    public GameObject panelDeAlerta;
    public TextMeshProUGUI textoDeAlerta;

    private ContenedorRecetas misDatos;
    private int indiceRecetaActual = 0;
    private Dictionary<string, int> ingredientesCaldero = new Dictionary<string, int>();

    void Start()
    {
        if (panelDeAlerta != null) panelDeAlerta.SetActive(false);

        CargarJson();

        // Si cargó algo, inicializamos la UI
        if (misDatos != null && misDatos.recetas != null && misDatos.recetas.Count > 0)
        {
            ActualizarMenuDesplegable();
            ActualizarInterfaz();
        }
    }

    void CargarJson()
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivoJson);

        if (File.Exists(ruta))
        {
            string contenido = File.ReadAllText(ruta);

            // --- MENSAJE DE DEPURACIÓN CRÍTICO ---
            // Esto nos dirá qué está leyendo Unity realmente
            Debug.Log("Contenido bruto del archivo: " + contenido);

            misDatos = JsonUtility.FromJson<ContenedorRecetas>(contenido);

            if (misDatos == null || misDatos.recetas == null || misDatos.recetas.Count == 0)
            {
                Debug.LogError("¡ERROR! El JSON se leyó pero la lista de recetas está VACÍA. Revisa que el nombre 'recetas' en el JSON coincida con el código.");
            }
            else
            {
                Debug.Log("<color=green>¡ÉXITO!</color> Se cargaron " + misDatos.recetas.Count + " recetas.");
            }
        }
        else
        {
            Debug.LogError("¡ARCHIVO NO ENCONTRADO! No existe nada en: " + ruta);
        }
    }

    public void ActualizarInterfaz()
    {
        if (misDatos == null || misDatos.recetas == null || indiceRecetaActual >= misDatos.recetas.Count) return;

        RecetaJSON receta = misDatos.recetas[indiceRecetaActual];
        string texto = "<color=yellow>POCIÓN ACTUAL:</color>\n" + receta.nombre + "\n\n";
        texto += "<b>Necesitas:</b>\n";
        foreach (var req in receta.ingredientesRequeridos)
        {
            texto += "- " + req.ingrediente + ": " + req.cantidad + "\n";
        }
        textoReceta.text = texto;

        // Inventario del GameManager
        textoInventario.text = "<b>Tu Morral:</b>\n";
        foreach (var item in GameManager.Instance.CollectedItems)
        {
            textoInventario.text += item.Key.nombre + ": x" + item.Value + "\n";
        }

        // Caldero
        textoCaldero.text = "<b>En el Caldero:</b>\n";
        foreach (var item in ingredientesCaldero)
        {
            textoCaldero.text += item.Key + ": x" + item.Value + "\n";
        }
    }

    public void ActualizarMenuDesplegable()
    {
        menuDesplegable.ClearOptions();
        List<string> nombres = new List<string>();

        foreach (var item in GameManager.Instance.CollectedItems)
        {
            if (item.Value > 0) nombres.Add(item.Key.nombre);
        }

        if (nombres.Count > 0) menuDesplegable.AddOptions(nombres);
        else menuDesplegable.AddOptions(new List<string> { "Morral Vacío" });
    }

    // --- LÓGICA DE BOTONES (Añadir, Borrar, Preparar) ---

    public void BotonAñadir()
    {
        if (menuDesplegable.options.Count == 0 || menuDesplegable.options[0].text == "Morral Vacío") return;

        string seleccionado = menuDesplegable.options[menuDesplegable.value].text;
        ingredientes data = null;

        foreach (var key in GameManager.Instance.CollectedItems.Keys)
        {
            if (key.nombre == seleccionado) data = key;
        }

        if (data != null && GameManager.Instance.CollectedItems[data] > 0)
        {
            GameManager.Instance.CollectedItems[data]--;
            if (ingredientesCaldero.ContainsKey(seleccionado)) ingredientesCaldero[seleccionado]++;
            else ingredientesCaldero.Add(seleccionado, 1);

            ActualizarMenuDesplegable();
            ActualizarInterfaz();
        }
    }

    public void BotonBorrar()
    {
        foreach (var item in ingredientesCaldero)
        {
            foreach (var key in GameManager.Instance.CollectedItems.Keys)
            {
                if (key.nombre == item.Key) GameManager.Instance.CollectedItems[key] += item.Value;
            }
        }
        ingredientesCaldero.Clear();
        ActualizarMenuDesplegable();
        ActualizarInterfaz();
    }

    public void BotonPreparar()
    {
        if (misDatos == null || indiceRecetaActual >= misDatos.recetas.Count) return;

        RecetaJSON receta = misDatos.recetas[indiceRecetaActual];

        // Validar que no haya ingredientes extra
        foreach (var item in ingredientesCaldero)
        {
            bool esDeReceta = false;
            foreach (var req in receta.ingredientesRequeridos)
                if (item.Key == req.ingrediente) esDeReceta = true;
            if (!esDeReceta) return;
        }

        // Validar cantidades exactas
        foreach (var req in receta.ingredientesRequeridos)
        {
            int enOlla = ingredientesCaldero.ContainsKey(req.ingrediente) ? ingredientesCaldero[req.ingrediente] : 0;
            if (enOlla != req.cantidad) return;
        }

        // ÉXITO
        ingredientesCaldero.Clear();
        indiceRecetaActual++;

        if (indiceRecetaActual >= misDatos.recetas.Count)
        {
            textoReceta.text = "¡CURSO COMPLETADO!";
            if (panelDeAlerta != null) panelDeAlerta.SetActive(true);
        }
        else
        {
            ActualizarMenuDesplegable();
            ActualizarInterfaz();
        }
    }

    public void BotonInicio() { GameManager.Instance.LoadScene("Escena 1"); }
}