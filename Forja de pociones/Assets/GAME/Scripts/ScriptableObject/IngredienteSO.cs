using UnityEngine;

[CreateAssetMenu(fileName = "NuevoIngrediente", menuName = "Scriptable Objects/IngredienteSO")]
public class IngredienteSO : ScriptableObject
{
    public string nombre;
    public int valor;
    public string iconId;
}