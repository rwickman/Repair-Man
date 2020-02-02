﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float healthPoints;
    public float maxHealthPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float attackPoints)
    {
        healthPoints -= attackPoints;
        if (healthPoints > maxHealthPoints)
        {
            healthPoints = maxHealthPoints;
        }
    }
}
