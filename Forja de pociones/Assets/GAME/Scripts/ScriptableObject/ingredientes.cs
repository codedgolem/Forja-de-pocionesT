using UnityEngine;

[CreateAssetMenu(fileName = "NuevoIngrediente", menuName = "Scriptable Objects/Ingrediente")]
public class ingredientes : ScriptableObject
{
    // Categorías basadas en tu lista
    public enum TipoIngrediente
    {
        HongoBrillante,
        HojadeSombra,
        Calabaza,
        Uva,
        Hojadebruja,
        Semillasdegigante,
        Frutadeojos,
        Especial,
    }

    [Header("Configuración del JSON")]
    public string nombre; // "Hongo Brillante", "Uva", etc.
    public int valor;
    public string iconId;

    [Header("Clasificación para el Juego")]
    // Esta es la variable que el GameDataLoader está buscando (Error CS1061)
    public TipoIngrediente ingrediente;
}