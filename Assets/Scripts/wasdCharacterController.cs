using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasdCharacterController : MonoBehaviour
{

    private float hMove;
    private float vMove;
    private Vector3 movement;
    public Rigidbody obj;
    private float speed = 0f;

    public float speedWalk;
    public float speedAttack = 0f;
    public float speedIdle = 0f;
    public float speedModifier = 1f;
    public Animator anim;

    public float attackRate = 1.4f;
    [HideInInspector]
    public float lastAttack;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        obj = GetComponent<Rigidbody>();
        speedWalk = 15f;
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "Lumbering")
            {
                attackRate = ac.animationClips[i].length;
            }
        }

        lastAttack = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastAttack < attackRate)
        {
            lastAttack += Time.deltaTime;
        }
        //print(lastAttack);
        
        // NOTE: Use GetAxisRaw to allow for holding down button without joystick lag of GetAxis
        if (Input.GetAxisRaw("Horizontal") > 0) {
            //right
            transform.rotation = Quaternion.Euler(0, 90, 0);
            speed = speedWalk;
        }
        if (Input.GetAxisRaw("Horizontal") < 0 ) {
            //left
            transform.rotation = Quaternion.Euler(0, -90, 0);
            speed = speedWalk;
        }
        if (Input.GetAxisRaw("Vertical") > 0) {
            //up
            transform.rotation = Quaternion.Euler(0, 0, 0);
            speed = speedWalk;
        }
        if (Input.GetAxisRaw("Vertical") < 0) {
            //down
            transform.rotation = Quaternion.Euler(0, 180, 0);
            speed = speedWalk;
        }
        if ((Input.GetButton("Fire3") || Input.GetButton("Fire1")) && lastAttack >= attackRate) {
            anim.SetBool("attack1",true);
        } else {
            anim.SetBool("attack1", false);
        }

        obj.velocity = transform.forward * speed * speedModifier;
        anim.SetFloat("speed", speed);
        speed = speedIdle;
    }
}