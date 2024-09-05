using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Item
    {
        public GameObject gameObject;
        public float weight;
    }

    public List<Item> items = new List<Item>();
    float totalWeight;

    private void Awake()
    {
        totalWeight = 0;
        foreach (var item in items)
        {
            totalWeight += item.weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float pick = Random.value * totalWeight;
        int selectedIndex = 0;
        float cumulativeWeight = items[0].weight;

        while(pick > cumulativeWeight && selectedIndex < items.Count - 1)
        {
            selectedIndex++;
            cumulativeWeight += items[selectedIndex].weight;
        }

        GameObject selectedItem = Instantiate(items[selectedIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
        var parentGameObject = this.transform.parent.gameObject;
        
        selectedItem.transform.parent = parentGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
