using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Dictionary<IngredienteSO, int> collectedItems = new Dictionary<IngredienteSO, int>();
    public Dictionary<IngredienteSO, int> CollectedItems { get => collectedItems; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void actualizarData(IngredienteSO data)
    {
        if (collectedItems.TryGetValue(data, out int count))
            collectedItems[data] = count + data.valor;
        else
            collectedItems[data] = data.valor;

        Debug.Log($"Inventario: {data.nombre} total = {collectedItems[data]}");
    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
}