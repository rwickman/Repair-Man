using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Queue<EnergyPack> packs;
    public Queue<GameObject> bottleImages;
    public int maxInventorySize = 5;
    public GameObject bottleImagePrefab; 
    private GameObject BottlesUIParent;
    private void Start()
    {
        bottleImages = new Queue<GameObject>(maxInventorySize);
        packs = new Queue<EnergyPack>(maxInventorySize);
        BottlesUIParent = GameObject.Find("EnergyBottles");
    }

    public void PickUp(GameObject go)
    {
        if (packs.Count < maxInventorySize)
        {
            EnergyPack pack = go.GetComponent<EnergyPack>();
            pack.PickedUp();
            packs.Enqueue(pack);
            bottleImages.Enqueue(Instantiate(bottleImagePrefab, BottlesUIParent.transform));
        }
    }

    public float Consume()
    {
        if (packs.Count > 0)
        {
            Destroy(bottleImages.Dequeue());
            return packs.Dequeue().energyLevel;

        }
        else
        {
            return 0;
        }
        
    }
}
