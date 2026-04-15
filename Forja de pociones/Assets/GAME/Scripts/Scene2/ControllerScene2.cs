using TMPro;
using UnityEngine;
using System.Collections.Generic;

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
        Invoke("GenerarInterfaz", 0.2f); // Esperar a que los datos carguen
    }

    void Update()
    {
        if (!goalCompleted)
        {
            ActualizarUI();
            VerificarObjetivos();
        }
    }

    void GenerarInterfaz()
    {
        foreach (Transform child in contenedorUI) Destroy(child.gameObject);

        foreach (var data in GameDataLoader.instance.IngredientesList)
        {
            GameObject fila = Instantiate(itemPrefab, contenedorUI);
            Inventory uiScript = fila.GetComponent<Inventory>();

            Sprite icono = Resources.Load<Sprite>("IngredientesIcon/" + data.iconId);
            uiScript.SetData(data.nombre, 0, icono);

            if (!textosCantidades.ContainsKey(data.nombre))
                textosCantidades.Add(data.nombre, uiScript.cantidadTexto);

            var config = spawner.ItemS.Find(x => x.nombre == data.nombre);
            if (config != null && !objetivosRecoleccion.ContainsKey(data.nombre))
                objetivosRecoleccion.Add(data.nombre, config.cantidad);
        }
    }

    void ActualizarUI()
    {
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

        bool todos = true;
        foreach (var obj in objetivosRecoleccion)
        {
            IngredienteSO info = GameDataLoader.instance.IngredientesList.Find(i => i.nombre == obj.Key);
            int actual = 0;
            if (info != null) GameManager.Instance.CollectedItems.TryGetValue(info, out actual);

            if (actual < obj.Value) { todos = false; break; }
        }

        if (todos)
        {
            goalCompleted = true;
            if (panelFin) panelFin.SetActive(true);
            if (portal) portal.SetActive(true);
        }
    }
}