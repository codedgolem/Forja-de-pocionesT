using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class RecipeManager : MonoBehaviour
{
    [Header("Configuración del Archivo")]
    public string nombreArchivoJson = "DatosJuego.json"; // Asegúrate que este nombre sea igual al de tu amiga

    [Header("Referencias UI")]
    public TextMeshProUGUI textoReceta;
    public TextMeshProUGUI textoInventario;
    public TextMeshProUGUI textoCaldero;
    public TMP_Dropdown menuDesplegable;

    [Header("Ventana de Victoria")]
    public GameObject panelDeAlerta;

    // Usamos el nuevo contenedor que tiene Ingredientes y Recetas juntos
    private ContenedorUnificado misDatos;
    private int indiceRecetaActual = 0;
    private Dictionary<string, int> ingredientesCaldero = new Dictionary<string, int>();

    void Start()
    {
        if (panelDeAlerta != null) panelDeAlerta.SetActive(false);

        CargarJson();

        // Verificamos que misDatos no sea nulo y que tenga recetas
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
            // Ahora leemos usando ContenedorUnificado
            misDatos = JsonUtility.FromJson<ContenedorUnificado>(contenido);
        }
        else
        {
            Debug.LogError("¡ARCHIVO NO ENCONTRADO! en: " + ruta);
        }
    }

    public void ActualizarInterfaz()
    {
        if (misDatos == null || misDatos.recetas == null || indiceRecetaActual >= misDatos.recetas.Count) return;

        RecetaJSON receta = misDatos.recetas[indiceRecetaActual];

        string texto = "<b>POCIÓN ACTUAL:</b> \n" + receta.nombre + "\n\n";
        texto += "<b>Necesitas:</b>\n";

        foreach (var req in receta.ingredientesRequeridos)
        {
            
            texto += "- " + req.nombre + ": " + req.cantidad + "\n";
        }

        texto += "\n<i>Dificultad: " + receta.dificultad + "</i>";
        textoReceta.text = texto;

        // Mostrar lo que tienes en el inventario
        textoInventario.text = "<b>Tu Morral:</b>\n";
        foreach (var item in GameManager.Instance.CollectedItems)
        {
            textoInventario.text += item.Key.nombre + ": x" + item.Value + "\n";
        }

        // Mostrar lo que tiene el caldero
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
        else menuDesplegable.AddOptions(new List<string> { "Inventario Vacío" });
    }

    public void BotonAñadir()
    {
        if (menuDesplegable.options.Count == 0 || menuDesplegable.options[0].text == "Inventario Vacío") return;

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

        // 1. Validar que no haya ingredientes que NO pertenecen a la receta
        foreach (var item in ingredientesCaldero)
        {
            bool esDeReceta = false;
            foreach (var req in receta.ingredientesRequeridos)
            {
                if (item.Key == req.nombre) esDeReceta = true;
            }
            if (!esDeReceta) return; // Si hay algo extra, no se prepara
        }

        // 2. Validar que las cantidades sean EXACTAS
        foreach (var req in receta.ingredientesRequeridos)
        {
            int enOlla = ingredientesCaldero.ContainsKey(req.nombre) ? ingredientesCaldero[req.nombre] : 0;
            if (enOlla != req.cantidad) return; // Si falta o sobra, no se prepara
        }

        // ÉXITO: Limpiamos caldero y pasamos a la siguiente poción
        ingredientesCaldero.Clear();
        indiceRecetaActual++;

        if (indiceRecetaActual >= misDatos.recetas.Count)
        {
            textoReceta.text = "¡Victoria!";
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