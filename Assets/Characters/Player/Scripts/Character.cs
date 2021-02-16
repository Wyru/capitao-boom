using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    public int maxLife;
    public int life;

    public int maxBoomPower;
    public int boomPower;

    public int speed;
    public GameLoop gameLoop;
    public int groundIndex = 0;

    private Rigidbody2D RB2d;
    private BoxCollider2D BC2d;
    private Animator animator;
	private SpriteRenderer ownRenderer;

    public float verticalUpdateDistance = 0.5f;

    public float minAttackDistance;
    public float attackIncrement;
    public float attackDistance;
    public float maxAttackDistance;

    public float timeToBombExplode;

    public GameObject bombPrefab;
    public GameObject superBombPrefab;
	public int maxBombs = 5;
	public int bombsLeft;



    public bool charging = false;
    public bool verticalMoving;
	public bool isTakingDamage;



    //sounds
    public AudioClip damage;
    public AudioClip death;

    public AudioClip thrownSound;
    public AudioClip trownBomps1;
    public AudioClip trownBomps2;

    public AudioClip randomQuote;

    public AudioClip ultimateBomb;

    public AudioSource audioSource;



    // Use this for initialization
    void Start() {

        this.RB2d = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.BC2d = this.GetComponent<BoxCollider2D>();
		this.ownRenderer = this.GetComponent<SpriteRenderer> ();

        this.transform.position = new Vector2(this.transform.position.x, gameLoop.groundLayers[this.groundIndex].position.y);

        this.life = maxLife;
        this.bombsLeft = this.maxBombs;

		this.isTakingDamage = false;
        this.audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() {

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
        if (groundIndex > 0) {
            groundIndex--;
            StopCoroutine("VerticalMove");
            StartCoroutine("VerticalMove",2);
        } 
    }

    public void MoveUp() {
        if (groundIndex < (gameLoop.groundLayers.Length - 1)) {
            groundIndex++;
            StopCoroutine("VerticalMove");
            StartCoroutine("VerticalMove",8);
        }
    }

    IEnumerator VerticalMove(int dir) {
        verticalMoving = true;
        this.animator.SetBool("walking", true);

        float distance = Mathf.Abs(this.transform.position.y - gameLoop.groundLayers[this.groundIndex].position.y);

        for(float i = 0; i < distance; i+= verticalUpdateDistance) {
            if (dir == 2) this.transform.Translate(new Vector2(0, -verticalUpdateDistance)); 
            if (dir == 8) this.transform.Translate(new Vector2(0, verticalUpdateDistance));
            yield return new WaitForSeconds(.001f);
        }
        verticalMoving = false;
    }

    public void Move() {
        this.animator.SetBool("walking", true);
        this.transform.Translate(new Vector2(1, 0) * Time.deltaTime * this.speed);
    }

    public void StopWalking() {
        this.animator.SetBool("walking", false);
    }


    public void BeginChargeAttack() {
        this.charging = true;
        StartCoroutine("ChargingAttack");
    }
    public void BeginChargeUltimateAttack() {
        if (boomPower >= maxBombs) {
            this.charging = true;
            StartCoroutine("ChargingAttack");
        }
        else
            Debug.Log("sem energia");
        
    }

    IEnumerator ChargingAttack() {
        attackDistance = minAttackDistance;
        while (true) {
            if (attackDistance > maxAttackDistance) {
                attackDistance = minAttackDistance;
            }
            else {
                attackDistance += attackIncrement;
            }
            yield return new WaitForSeconds(.1f);
        }
    }


    public void Attack() {
        this.charging = false;

        StopCoroutine("ChargingAttack");

        PlayThrowBombSound();

        if (bombsLeft > 0) {
			this.animator.SetTrigger("attack");
			Rigidbody2D projectile = bombPrefab.GetComponent<Rigidbody2D> ();
			Rigidbody2D clone;
			bombsLeft--;
			clone = Instantiate (projectile, this.transform.position, Quaternion.identity) as Rigidbody2D;
			clone.GetComponent<BombBehavior> ().minY = this.transform.position.y - 1;
			clone.GetComponent<BombBehavior> ().playerStatus = this;
			clone.velocity = transform.TransformDirection ((Vector2.right + (2 * Vector2.up)) * attackDistance);
		}
    }

    public void Super() {
        if (boomPower >= maxBombs) {
            PlaySuperSound();
            this.charging = false;
            StopCoroutine("ChargingAttack");
            this.animator.SetTrigger("super");
            Rigidbody2D projectile = superBombPrefab.GetComponent<Rigidbody2D>();
            Rigidbody2D clone;
            bombsLeft--;
            clone = Instantiate(projectile, this.transform.position, Quaternion.identity) as Rigidbody2D;
            clone.GetComponent<UltimateBombBehavior>().minY = this.transform.position.y - 1;
            clone.GetComponent<UltimateBombBehavior>().playerStatus = this;
            clone.velocity = transform.TransformDirection((Vector2.right + (2 * Vector2.up)) * attackDistance);
            boomPower = 0;


        }


    }

	public void takeDamage (int damage) {
        this.animator.SetTrigger("damage");
        this.Damage (damage);
	}

    private void Damage(int damage) {
		if (isTakingDamage == false && this.life > 0) {
			isTakingDamage = true;
			this.life -= damage;
			StartCoroutine("Flash");
		} 
		if (this.life <= 0)
			this.Die ();
    }

	private void Die () {
        this.animator.SetBool("death", true);
        this.PlayDeathSound();
        Destroy(this);
	}

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
		isTakingDamage = false;
	}


    public void PlayHitSound() {
        audioSource.clip = damage;
        this.audioSource.Play();
    }

    public void PlayThrowBombSound() {
        audioSource.clip = thrownSound;
        this.audioSource.Play();

        if (Random.value <.4) {
            if (Random.value > .2) 
                audioSource.clip = trownBomps1;
            else
                audioSource.clip = trownBomps2;

            this.audioSource.Play();
        }
    }

    public void PlayDeathSound() {
        audioSource.clip = death;
        this.audioSource.Play();
    }

    public void PlaySuperSound() {
        audioSource.clip = ultimateBomb;
        this.audioSource.Play();
    }

}