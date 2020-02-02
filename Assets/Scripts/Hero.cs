using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    private Inventory inventory;
    private Energy energy;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        energy = GetComponent<Energy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            energy.AddEnergy(inventory.Consume());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnergyPack")
        {
            inventory.PickUp(other.gameObject);
        }
    }
}
