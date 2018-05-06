﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {
    public Character player;
    public GameObject munition;
    public GameObject pivot;

    public int maxLife;
    public int currentLife;

    public int hoverSpeed;
    public int attackDistance;
    public int safeDistance;

    public bool isTakingDamage = false;
    public bool isAttacking = false;
    public bool isRunning = false;

    private float distanceToPlayer = 0f;

    private Rigidbody2D bossBody;
    private BoxCollider2D bossCollider;
    private SpriteRenderer bossRendererer;

    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
        munition = Resources.Load("prefabs/Munition", typeof(GameObject)) as GameObject;

        bossBody = this.GetComponent<Rigidbody2D>();
        bossCollider = this.GetComponent<BoxCollider2D>();
        bossRendererer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Mathf.Abs(this.transform.position.x - player.transform.position.x);
        if (isRunning)
        {
            if (distanceToPlayer > (safeDistance + 10))
                isRunning = false;
            else
                Run();
        }
        else
        {
            if ((distanceToPlayer < attackDistance) && (distanceToPlayer >= safeDistance))
            {
                Attack();
            }

            if (distanceToPlayer >= safeDistance || distanceToPlayer > attackDistance)
            {
                Pursue();
            }
            else
            {
                Run();
               
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void Pursue()
    {
        if (this.transform.position.x > player.transform.position.x)
            MoveLeft();
        else
            MoveRight();
    }

    void Run ()
    {
        isRunning = true;
        if (this.transform.position.x > player.transform.position.x)
            StrafeRight();
        else
            StrafeLeft();

        distanceToPlayer = Mathf.Abs(this.transform.position.x - player.transform.position.x);

        if (distanceToPlayer < attackDistance)
            Attack();
    }

    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine("Cooldown");
            Debug.Log("ATTACK!");

            Vector2 playerVector = player.GetComponent<Rigidbody2D>().transform.position;
            Vector2 bossVector = this.transform.position;
            Vector2 direction = playerVector - bossVector;
            GameObject newMunition;

            newMunition = Instantiate(munition, pivot.transform.position, Quaternion.identity);
            newMunition.GetComponent<Rigidbody2D>().velocity = direction;
            newMunition.transform.LookAt(player.transform);
        }
    }

    public void dealDamage (int dam)
    {
        this.Damage (dam);
    }

    void Damage (int dam)
    {
        if (!isTakingDamage)
        {
            this.currentLife -= dam;
            StartCoroutine("Flash");
            if (this.currentLife <= 0)
                Die();
        }
    }

    void Die ()
    {
        player.foesFell++;
        Destroy(this.gameObject);
    }

    public void MoveLeft()
    {
        Vector2 movement = new Vector2(-1, 0);
        this.Move(movement);
    }

    public void MoveRight()
    {
        Vector2 movement = new Vector2(1, 0);
        this.Move(movement);
    }

    public void StrafeRight ()
    {
        Vector2 movement = new Vector2(2, 0);
        this.Move(movement);
    }

    public void StrafeLeft()
    {
        Vector2 movement = new Vector2(-2, 0);
        this.Move(movement);
    }

    public void Move(Vector2 movement)
    {
        // this.animator.SetBool("walking", true);
        this.transform.Translate(movement * Time.deltaTime * this.hoverSpeed);
    }

    IEnumerator Flash()
    {
        bool toggle = true;
        this.bossCollider.enabled = false;
        for (int i = 0; i < 10; ++i)
        {
            yield return new WaitForSeconds(.1f);
            if (toggle)
            {
                this.bossRendererer.enabled = false;
                toggle = false;
            }
            else
            {
                this.bossRendererer.enabled = true;
                toggle = true;
            }
        }
        this.bossCollider.enabled = true;
        isTakingDamage = false;
    }
}