using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public WorldManager manager;
    public MazeGenerator maze;
    public float pillStoppingDist = 0.1f;
    public float attackRate = 3f;
    public float lastAttack = 0f;
    public float lastPosition = 0f;

    public int attackPower = 1;
    public int monsterPoints = 5;


    //public int randWalkDenom = 2;
    private PowerPill foundPill;
    private GameObject foundPillGO;
    public NavMeshAgent agent;
    private Vector3 destPosition;
    public Animator anim;
    public Renderer rend;
    

    private bool setInitDest;
    private bool foundPowerPill;
    private bool isAttackingPill;

    // Start is called before the first frame update
    void Awake()
    {
        lastAttack = attackRate;
        agent = GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            props.SetColor("_EColor", Color.black);
            rend.SetPropertyBlock(props);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastAttack < attackRate)
        {
            lastAttack += Time.deltaTime;
        }

        if (!setInitDest)
        {
            if (!maze.buildNavMesh)
            {
                UpdateDestination();
                setInitDest = true;
            }
        }
        else if (foundPowerPill && foundPill == null)
        {
            foundPowerPill = false;
            foundPillGO = null;
            UpdateDestination();
        }
        else if (foundPowerPill)
        {
            //print("Distance: " + Vector3.Distance(agent.transform.position, foundPill.transform.position));
            if (!isAttackingPill && Vector3.Distance(agent.transform.position, foundPill.transform.position) < pillStoppingDist)
            {
                agent.isStopped = true;
                isAttackingPill = true;
                Vector3 dirToPill = (foundPillGO.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(dirToPill);
                anim.SetFloat("speed", 0f);
                anim.SetBool("attack1", true);
            }
            else if (isAttackingPill && lastAttack >= attackRate)
            {
                Vector3 dirToPill = (foundPillGO.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(dirToPill);
                lastAttack = 0;
                bool destroyedPill;
                if (foundPill == null || foundPill.isDead)
                {
                    destroyedPill = true;
                }
                else
                {
                    destroyedPill = foundPill.Damage(attackPower);
                }
                if (destroyedPill)
                {
                    isAttackingPill = false;
                    foundPowerPill = false;
                    UpdateDestination();
                    agent.isStopped = false;
                    anim.SetBool("attack1", false);
                    anim.SetFloat("speed", 1f);
                }
                lastAttack = 0;
            }
        }

        else if (!isAttackingPill && Vector3.Distance(agent.transform.position, destPosition) < 1f)
        {
            UpdateDestination();
        }
        else if (!isAttackingPill && Vector3.Distance(agent.transform.position, destPosition) >= 1f && agent.velocity.magnitude < 0.03f)
        {
            UpdateDestination();
        }
    }


    void UpdateDestination()
    {
        GetRandomDistancePoint();
        agent.SetDestination(destPosition);
        anim.SetFloat("speed", 1f);
    }

    Vector3 GetRandomDistancePoint()
    {
        int randH = Random.Range(0, maze.mazeHeight);
        int randW = Random.Range(0, maze.mazeWidth);
        destPosition = maze.mazeCells[randW, randH].transform.position;
        return destPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerPill" && other.gameObject != foundPillGO && !foundPowerPill)
        {
            foundPowerPill = true;
            destPosition = other.transform.position;
            agent.SetDestination(destPosition);
            foundPill = other.GetComponent<PowerPill>();
            foundPillGO = other.gameObject;
        }
    }
}
