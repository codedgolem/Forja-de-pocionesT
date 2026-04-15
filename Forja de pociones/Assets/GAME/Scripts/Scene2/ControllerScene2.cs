using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ControllerScene2 : MonoBehaviour
{
    public ItemSpawner spawner;
    public GameObject panelFin;
    public GameObject portal;

    [Header("Interfaz")]
    public GameObject itemPrefab;
    public Transform contenedorUI;

    private Dictionary<string, TextMeshProUGUI> textosCantidades = new Dictionary<string, TextMeshProUGUI>();
    private Dictionary<string, int> objetivosRecoleccion = new Dictionary<string, int>();
    private bool goalCompleted = false;

    void Start()
    {
        if (panelFin) panelFin.SetActive(false);
        if (portal) portal.SetActive(false);

        // Esperamos un momento a que el JSON cargue antes de dibujar
        Invoke("GenerarInterfaz", 0.3f);
    }

    void Update()
    {
        // Solo actualizamos si el juego sigue en marcha y los datos existen
        if (!goalCompleted && GameDataLoader.Instance != null)
        {
            ActualizarUI();
            VerificarObjetivos();
        }
    }

    public void GenerarInterfaz()
    {
        // CORRECCIÓN: Usamos 'Instance' (I mayúscula) e 'IngredientesList'
        if (GameDataLoader.Instance == null || GameDataLoader.Instance.IngredientesList == null)
        {
            Debug.LogWarning("ControllerScene2: Los datos aún no están listos. Reintentando...");
            Invoke("GenerarInterfaz", 0.5f);
            return;
        }

        // Limpiar la UI antes de generar
        foreach (Transform child in contenedorUI) Destroy(child.gameObject);
        textosCantidades.Clear();

        // Recorremos la lista que Esteban cargó
        foreach (var data in GameDataLoader.Instance.IngredientesList)
        {
            GameObject fila = Instantiate(itemPrefab, contenedorUI);
            Inventory uiScript = fila.GetComponent<Inventory>();

            // Cargamos el icono usando el iconId del JSON
            Sprite icono = Resources.Load<Sprite>("IngredientesIcon/" + data.iconId);
            uiScript.SetData(data.nombre, 0, icono);

            // Guardamos la referencia del texto para actualizarlo en el Update
            if (!textosCantidades.ContainsKey(data.nombre))
                textosCantidades.Add(data.nombre, uiScript.cantidadTexto);

            // Buscamos cuánto hay que recoger de este item en el spawner
            var config = spawner.ItemS.Find(x => x.nombre == data.nombre);
            if (config != null && !objetivosRecoleccion.ContainsKey(data.nombre))
            {
                objetivosRecoleccion.Add(data.nombre, config.cantidad);
            }
        }
    }

    void ActualizarUI()
    {
        if (GameManager.Instance == null) return;

        foreach (var item in GameManager.Instance.CollectedItems)
        {
            if (textosCantidades.ContainsKey(item.Key.nombre))
            {
                int objetivo = objetivosRecoleccion.ContainsKey(item.Key.nombre) ? objetivosRecoleccion[item.Key.nombre] : 0;
                textosCantidades[item.Key.nombre].text = $"{item.Value} / {objetivo}";
            }
        }
    }

    public void VerificarObjetivos()
    {
        if (objetivosRecoleccion.Count == 0) return;

        bool todosCompletados = true;

        foreach (var obj in objetivosRecoleccion)
        {
            // Buscamos la info del ingrediente en la lista de Esteban
            ingredientes info = GameDataLoader.Instance.IngredientesList.Find(i => i.nombre == obj.Key);
            int cantidadActual = 0;

            if (info != null)
            {
                GameManager.Instance.CollectedItems.TryGetValue(info, out cantidadActual);
            }

            if (cantidadActual < obj.Value)
            {
                todosCompletados = false;
                break;
            }
        }

        if (todosCompletados)
        {
            goalCompleted = true;
            if (panelFin) panelFin.SetActive(true);
            if (portal) portal.SetActive(true);
            Debug.Log("¡Objetivos completados! Portal abierto.");
        }
    }
}