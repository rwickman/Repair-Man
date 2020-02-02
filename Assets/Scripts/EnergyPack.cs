using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPack : MonoBehaviour
{
    public float energyLevel = 1;
    public WorldManager manager;
    public Cell cell;
    public void PickedUp()
    {
        manager.RemoveEnergyPack(this);
        Destroy(gameObject);
    }
}
