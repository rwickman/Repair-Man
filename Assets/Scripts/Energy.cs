using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public float maxEnergyLevel = 1f;

    private float curEnergyLevel = 1f;
    public float energyDecreseRate = 0.001f;
    public float energyIncreaseRate = 0.008f;
    private wasdCharacterController controller;
    private Text energyText;

    private void Start()
    {
        curEnergyLevel = maxEnergyLevel;
        controller = GetComponentInParent<wasdCharacterController>();
        energyText = GameObject.Find("EnergyUI").GetComponent<Text>();
    }

    private void Update()
    {
        if (controller.obj.velocity.magnitude <= 0)
        {
            curEnergyLevel = Mathf.Min(curEnergyLevel + energyIncreaseRate, maxEnergyLevel);
        }
        else
        {
            curEnergyLevel = Mathf.Max(curEnergyLevel - energyDecreseRate, 0f);
        }
        //print(curEnergyLevel / maxEnergyLevel);
        controller.speedModifier = curEnergyLevel / maxEnergyLevel;
        energyText.text = "energy: " + curEnergyLevel * 100;
    }

    public void AddEnergy(float energyPoints)
    { 
        curEnergyLevel = Mathf.Min(curEnergyLevel + energyPoints, maxEnergyLevel);
    }





}
