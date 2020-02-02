using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPill : MonoBehaviour
{
    public WorldManager manager;
    public Cell cell;
    public float blowUpTime = 1f;
    public bool isDead;
    public Material[] materials = new Material[3];
    public Health health;
    Renderer rend;
    private Collider col;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        col = GetComponent<Collider>();
        rend = GetComponentInChildren<Renderer>();
    }

    public bool Damage(float attackPoints)
    {
        health.Damage(attackPoints);
        if (health.healthPoints <= 0)
        {
            manager.RemovePowerPill(this);
            // Destory this pill
            // Tell the manager that this is destroyed
            col.enabled = false;
            isDead = true;
            StartCoroutine(BlowUp());
            return true;
        }
        return false;
    }

    public bool Heal(float healPoints)
    {
        health.Damage(-healPoints);
        return false;
    }

    private IEnumerator BlowUp()
    {
        yield return new WaitForSeconds(blowUpTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(health.healthPoints >= 20 && rend.sharedMaterial != materials[0])
        {
            rend.sharedMaterial = materials[0];
        }else if (health.healthPoints >= 10 && health.healthPoints < 20 && rend.sharedMaterial != materials[1])
        {
            rend.sharedMaterial = materials[1];
        }else if (health.healthPoints >= 1 && health.healthPoints < 10 && rend.sharedMaterial != materials[2])
        {
            rend.sharedMaterial = materials[2];
        }

    }

}
