using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private Dictionary<ingredientes, int> collectedItems = new Dictionary<ingredientes, int>();

    public Dictionary<ingredientes, int> CollectedItems { get => collectedItems; set => collectedItems = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void actualizarData(ingredientes data)
    {
        if(collectedItems.TryGetValue(data, out int currentCount))
        {
            collectedItems[data] = currentCount + data.valor;
            Debug.Log($"Updated {data.nombre} count to {collectedItems[data]}");
        }
        else
        {
            collectedItems[data] = data.valor;
            Debug.Log($"Updated {data.nombre} count to {collectedItems[data]}");
        }

        //foreach (var item in collectedItems)
        //{
        //    Debug.Log($"Item: {item.Key.nombre}, Count: {item.Value}");
        //}
    }


}
