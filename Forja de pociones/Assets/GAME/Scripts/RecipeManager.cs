using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class RecipeManager : MonoBehaviour
{
    [Header("Configuración del Archivo")]
    public string nombreArchivoJson = "ingredientesData.json"; 

    [Header("Referencias UI")]
    public TextMeshProUGUI textoReceta;
    public TextMeshProUGUI textoInventario;
    public TextMeshProUGUI textoCaldero;
    public TMP_Dropdown menuDesplegable;

    [Header ("Panel Errores")]
    public GameObject panelErrores;
    public TextMeshProUGUI textoMensajeError;

    [Header("Ventana de Victoria")]
    public GameObject panelDeAlerta;
    public GameObject panelNext;

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
            
            foreach (var key in new List<ingredientes>(GameManager.Instance.CollectedItems.Keys)) // Envolvemos key en una lista para evitar errores al modificar los valores
            {
                
                if (key != null && key.nombre == item.Key) //Se verifica que key no sea nulo antes de intentar leer su nombre
                {
                    GameManager.Instance.CollectedItems[key] += item.Value;
                }
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

        // Mal ingrediente
        foreach (var item in ingredientesCaldero)
        {
            bool esDeReceta = false;
            foreach (var req in receta.ingredientesRequeridos)
            {
                if (item.Key == req.nombre) esDeReceta = true;
            }
            if (!esDeReceta)
            {
                MostrarError("Ingrediente incorrecto: " + item.Key);
                return;
            }
        }

        //Ingredientes faltantes o incompletos
        foreach (var req in receta.ingredientesRequeridos)
        {
            int enOlla = ingredientesCaldero.ContainsKey(req.nombre) ? ingredientesCaldero[req.nombre] : 0;

            if (enOlla == 0)
            {
                MostrarError("Falta ingrediente: " + req.nombre);
                return;
            }
            if (enOlla < req.cantidad)
            {
                MostrarError("Cantidad incompleta de: " + req.nombre);
                return;
            }
            if (enOlla > req.cantidad)
            {
                MostrarError("Demasiada cantidad de: " + req.nombre);
                return;
            }
        }

        // ÉXITO
        if (panelErrores != null) panelErrores.SetActive(false);
        ingredientesCaldero.Clear();
        indiceRecetaActual++;

        if (indiceRecetaActual >= misDatos.recetas.Count)
        {
            if (panelDeAlerta != null) panelDeAlerta.SetActive(true);
        }
        else
        {
            if (panelNext != null) panelNext.SetActive(true);
            ActualizarMenuDesplegable();
            ActualizarInterfaz();
        }
    }

    private void MostrarError(string mensaje)
    {
        if (panelErrores != null && textoMensajeError != null)
        {
            textoMensajeError.text = mensaje;
            panelErrores.SetActive(true);
        }
    }

    public void CerrarPanelError()
    {
        if (panelErrores != null) panelErrores.SetActive(false);
    }

    public void CerrarPanelNext()
    {
        if (panelNext != null) panelNext.SetActive(false);
    }

    public void BotonInicio() { GameManager.Instance.LoadScene("Escena1"); }
}