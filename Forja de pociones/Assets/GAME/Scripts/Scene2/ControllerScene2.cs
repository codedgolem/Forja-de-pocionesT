using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScene2 : MonoBehaviour
{
    public ItemSpawner spawner;

    [Header("Totales")]
    public TextMeshProUGUI totalHongo;
    public TextMeshProUGUI totalHojaSombra;
    public TextMeshProUGUI totalCalabaza;
    public TextMeshProUGUI totalUva;
    public TextMeshProUGUI totalHojaBruja;
    public TextMeshProUGUI totalSemillas;
    public TextMeshProUGUI totalOjos;

    [Header("Conseguidos")]
    public TextMeshProUGUI conseguidoHongo;
    public TextMeshProUGUI conseguidoHojaSombra;
    public TextMeshProUGUI conseguidoCalabaza;
    public TextMeshProUGUI conseguidoUva;
    public TextMeshProUGUI conseguidoHojaBruja;
    public TextMeshProUGUI conseguidoSemillas;
    public TextMeshProUGUI conseguidoOjos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        amountTotal();
    }

    // Update is called once per frame
    void Update()
    {
        amountConseguido();
    }


    public void amountTotal()
    {
        foreach(var item in spawner.ItemS)
        {
            
            switch (item.nombre)
            {
                case "Hongo Brillante":
                    
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalHongo.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                      
                    }   
                    break;
                case "Hoja de Sombra":
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalHojaSombra.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                    
                    }
                    break;
                case "Calabaza":
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalCalabaza.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                       
                    }
                   
                    break;
                case "Uva":
                    
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalUva.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                       
                        
                    }
                    
                    break;
                case "Hoja de bruja":
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalHojaBruja.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                        
                    }
                    
                    break;
                case "Semillas de gigante":
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalSemillas.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                      
                    }
                   
                    break;
                case "Fruta de ojos":
                    foreach (var ingrediente in GameDataLoader.instance.IngredientesList)
                    {
                        if (ingrediente.nombre.Equals(item.nombre))
                        {
                            totalOjos.text = (item.cantidad * ingrediente.valor).ToString();
                            break;
                        }
                      
                    }
                  
                    break;
            }
        }

    }

    public void amountConseguido()
    {
        foreach (var item in spawner.ItemS)
        {
            switch (item.nombre)
            {
                case "Hongo Brillante":
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Hongo Brillante")), out int cantidadHongo);
                    conseguidoHongo.text = cantidadHongo.ToString();
                    break;
                case "Hoja de Sombra":
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Hoja de Sombra")), out int cantidadHojaSombra);
                    conseguidoHojaSombra.text = cantidadHojaSombra.ToString();
                    break;
                case "Calabaza":
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Calabaza")), out int cantidadCalabaza);
                    conseguidoCalabaza.text = cantidadCalabaza.ToString();
                    break;
                case "Uva":
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Uva")), out int cantidadUva);
                    conseguidoUva.text = cantidadUva.ToString();
                    break;
                case "Hoja de bruja":   
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Hoja de bruja")), out int cantidadHojaBruja);
                    conseguidoHojaBruja.text = cantidadHojaBruja.ToString();
                    break;
                case "Semillas de gigante":
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Semillas de gigante")), out int cantidadSemillas);
                    conseguidoSemillas.text = cantidadSemillas.ToString(); 
                    break;
                case "Fruta de ojos":           
                    GameManager.Instance.CollectedItems.TryGetValue(GameDataLoader.instance.IngredientesList.Find(i => i.nombre.Equals("Fruta de ojos")), out int cantidadOjos);
                    conseguidoOjos.text = cantidadOjos.ToString();     
                    break;
            }
        }
    }
}
