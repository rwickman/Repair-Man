using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    public int attackPower = 1;
    public float healAmmount = 0.1f;

    public Score heroScore;

    private wasdCharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<wasdCharacterController>();
        heroScore = GetComponentInParent<Score>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster" && controller.anim.GetBool("attack1") && controller.lastAttack >= controller.attackRate)
        {
            controller.lastAttack = 0f;
            Health monsterHealth = other.GetComponentInParent<Health>();
            monsterHealth.Damage(attackPower);
            Monster curMonster = other.GetComponentInParent<Monster>();
            if (monsterHealth.healthPoints <= 0)
            {
                curMonster.agent.isStopped = true;
                // TODO: use coroutine to attack monster
                heroScore.UpdateScore(curMonster.monsterPoints);
                curMonster.manager.RemoveMonster(curMonster);
                Destroy(curMonster.gameObject);
            }
            else
            {
                if (curMonster.rend == null) return;
                if (monsterHealth.healthPoints == 2)
                {
                    MaterialPropertyBlock props = new MaterialPropertyBlock();
                    props.SetColor("_EColor", Color.yellow);
                    curMonster.rend.SetPropertyBlock(props);
                }
                else
                {
                    MaterialPropertyBlock props = new MaterialPropertyBlock();
                    props.SetColor("_EColor", Color.red);
                    curMonster.rend.SetPropertyBlock(props);
                }
            }
        }

        else if (other.tag == "PowerPill" && (Input.GetButton("Fire2") || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.R)))
        {
            PowerPill pill = other.GetComponent<PowerPill>();
            if (pill.health.healthPoints < pill.health.maxHealthPoints)
            {
                heroScore.UpdateScore(1);
                other.GetComponent<PowerPill>().Heal(healAmmount);
            }

        }
    }
}
