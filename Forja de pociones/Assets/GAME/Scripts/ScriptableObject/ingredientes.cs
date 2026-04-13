using UnityEngine;

[CreateAssetMenu(fileName = "ingredientes", menuName = "Scriptable Objects/ingredientes")]
public class ingredientes : ScriptableObject
{
    public string nombre;
    public int valor;
    public string iconId;
    public TipoIngrediente ingrediente;


    public enum TipoIngrediente
    {
        HongoBrillante,
        HojadeSombra,
        Calabaza,
        Uva,
        Hojadebruja,
        Semillasdegigante,
        Frutadeojos
    }

}
