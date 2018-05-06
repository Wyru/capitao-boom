using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foe : MonoBehaviour {
	public Character playerStatus;
	public GameObject munition;

	public int maxLife;
	public int life;
	public int MoveSpeed;

	public bool isAttacking;
	public bool isReceivingDamage;

	public int speed;
	public int groundIndex = 0;
	public int enemyType; // 0 - Melee | 1 - Ranged

	private Rigidbody2D RB2d;
	private BoxCollider2D BC2d;
	private SpriteRenderer ownRenderer;
	public float verticalUpdateDistance = 0.5f;

	private float attackDistance = 3.0f;
	public float time;

    public AudioSource audioSource;
    private Animator animator;

    public bool dead = false;

    public GameObject pizza;

    // Use this for initialization
    void Start() {
		this.munition = Resources.Load ("prefabs/Munition", typeof (GameObject)) as GameObject;
		this.RB2d = this.GetComponent<Rigidbody2D>();
		this.BC2d = this.GetComponent<BoxCollider2D>();
		this.ownRenderer = this.GetComponent<SpriteRenderer>();
		this.playerStatus = GameObject.FindWithTag ("Player").GetComponent<Character>();
		this.life = maxLife;
        this.audioSource = this.GetComponent<AudioSource>();
        this.animator = this.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update() {
		if (this.life > 0) {
			BehaviorController ();
		} else {
			this.Die ();
		}
	}

	IEnumerator Delay ()
	{
		isAttacking = true;
		yield return new WaitForSeconds(.6f);
		isAttacking = false;
	}

	IEnumerator Delay2 ()
	{
		isAttacking = true;
		yield return new WaitForSeconds(1.5f);
		isAttacking = false;
	}


	void BehaviorController () {
		double horizDist = Mathf.Abs (this.transform.position.x - playerStatus.transform.position.x);
		double verticalDist = Mathf.Abs (this.transform.position.y - playerStatus.transform.position.y);

		if (enemyType == 0) {
			if (horizDist > 3) {
				Approach ();
			} else {
				ChasePlayer ();
			}
		} else {
			if (horizDist > 5) {
				Approach ();
				Align ();
			} else if (horizDist < (4.0)) {
				Retreat ();
			} else {
				FacePlayer ();
				Fire ();
			}
		}

	}

	void FacePlayer () {
		if (this.transform.position.x > playerStatus.transform.position.x) {
			this.Mirror(4);
		} else {
			this.Mirror(6);
		}
	}
		

	void Approach () {
		if (this.transform.position.x > playerStatus.transform.position.x) {
			this.Mirror (6);
			this.MoveLeft ();
		} else {
			this.Mirror (4);
			this.MoveRight ();
		}
	}

	void Align () {
		double verticalDist = Mathf.Abs (this.transform.position.y - playerStatus.transform.position.y);
		if (verticalDist >= 0.2) 
		{
			if (this.transform.position.y > playerStatus.transform.position.y) {
				this.MoveDown();
			} else {
				this.MoveUp();
			}
		}
	}

	void Retreat () {
		if (this.transform.position.x > playerStatus.transform.position.x) {
			this.Mirror (4);
			this.MoveRight ();	
		} else {
			this.Mirror (6);
			this.MoveLeft ();
		}
	}

	void Fire () {
		if (!isAttacking) {
			AttackRanged ();
			StartCoroutine ("Delay2");
		}
	}

	void ChasePlayer () {
		double horizDist = Mathf.Abs (this.transform.position.x - playerStatus.transform.position.x);
		double verticalDist = Mathf.Abs (this.transform.position.y - playerStatus.transform.position.y);

        if (!isAttacking) {
			// Check if it's in distance to the player
			if (horizDist <= attackDistance && verticalDist <= 0.2) 
			{
                
				Attack ();
				StartCoroutine ("Delay");
			} 
			else if (horizDist <= attackDistance && verticalDist >= 0.2) 
			{


                if (this.transform.position.y > playerStatus.transform.position.y) 
				{
					this.MoveDown();
				} 
				else 
				{
					this.MoveUp();
				}
				
			}
			else 
			{

                int op = (int) Random.Range (0, 100);
				op %= 4;
				if (op == 0 || (horizDist <= (attackDistance + 2))) 
				{
					if (verticalDist >= 0.2) 
					{
						if (this.transform.position.y > playerStatus.transform.position.y) {
							this.MoveDown();
						} else {
							this.MoveUp();
						}
					}
				} 
				else 
				{ 
					if (this.transform.position.x > playerStatus.transform.position.x) 
					{
						this.Mirror(6);
						this.MoveLeft();
					} 
					else 
					{ 
						this.Mirror(4);
						this.MoveRight();
					}
				}
			}
		}
	}

	public void Mirror(int dir) {
		if (dir == 6) this.transform.rotation = new Quaternion(0, 0, 0, 0);
		if (dir == 4) this.transform.rotation = new Quaternion(0, 180, 0, 0);
	}

	public void MoveLeft() {
		this.Mirror(4);
		this.Move();
	}

	public void MoveRight() {
		this.Mirror(6);
		this.Move();
	}

	public void MoveDown() {
		this.transform.Translate (new Vector2 (0, -0.6f) * Time.deltaTime * this.speed);
	}

	public void MoveUp() {
		this.transform.Translate (new Vector2 (0, 0.6f) * Time.deltaTime * this.speed);
	}
		

	public void Move() {
		this.animator.SetBool("walk", true);
		this.transform.Translate(new Vector2(1, 0) * Time.deltaTime * this.speed);
	}

	public void StopWalking() {
		// this.animator.SetBool("walking", false);
	}
		
	public void Attack () {
        this.animator.SetTrigger("attack");
        playerStatus.takeDamage (1);
	}

	public void AttackRanged () {
		Rigidbody2D foeProjectile = munition.GetComponent<Rigidbody2D> ();
		Rigidbody2D clone = Instantiate (foeProjectile, this.transform.position, Quaternion.identity) as Rigidbody2D;
		if (this.transform.position.x < playerStatus.transform.position.x) {
			clone.velocity = transform.TransformDirection (Vector2.left * 4);
		} else {
			clone.velocity = transform.TransformDirection (Vector2.right * 4);
		}
	}

	public void Damage(int damage) {
		if (!isReceivingDamage && this.life > 0) {
			this.life -= damage;
			isReceivingDamage = true;
			StartCoroutine("Flash");
		}
		if (this.life <= 0)
			this.Die ();
	}

	private void Die () {

        if (!dead) {
            dead = true;
            this.animator.SetTrigger("death");
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.GetComponent<BoxCollider2D>());
            Destroy(this.GetComponent<Foe>());
            if (Random.value > .6) {
                Instantiate(pizza, this.transform.position, Quaternion.identity);
            }
        }
    }



    /// <summary>
    /// Flashes upon receiving damage
    /// </summary>
    IEnumerator Flash ()
	{
		bool toggle = true;
		this.BC2d.enabled = false;
		for (int i = 0; i < 10; ++i) {
			yield return new WaitForSeconds(.1f);
			if (toggle) {
				this.ownRenderer.enabled = false;
				toggle = false;
			} else {
				this.ownRenderer.enabled = true;
				toggle = true;
			}
		}
		this.BC2d.enabled = true;
		isReceivingDamage = false;
	}
}

