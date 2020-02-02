using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public Text energyText;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = "energy: 100";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
