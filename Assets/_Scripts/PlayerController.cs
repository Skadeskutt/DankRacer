using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	Rigidbody playerRigid;

	public LayerMask collisionMask;
	public GameObject playerModel;

	//turning varaibles
	public float handeling = 10;
	Vector3 rotation;

	//speed
	float maxSpeed;
	public float boostmaxSpeed;
	public float normalMaxSpeed;
	public float speed;

	//aceleration
	float aceleration;
	public float normalAceleration;
	public float boostAceleration;
	public float brakeAceleration;
	public float deAceleration;

	//collision
	float angle;
	Vector3 collisionNormal;

	//boost
	public float boostFuel;
	public float boostConsumption;
	public float maxBoostFuel;

	//Animation
	public Animator playerAnim;

	void Start ()
	{
		//playerAnim = GetComponentInChildren<Animator> ();
		playerRigid = GetComponent<Rigidbody> ();
		maxSpeed = normalMaxSpeed;

		//boostFuel = maxBoostFuel;
	}

	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		
		if (Input.GetAxis ("Vertical") == 1 && Input.GetAxis ("Break") != 1)
		{
			

			if (speed < maxSpeed)
				speed += aceleration;
			
			if (speed > maxSpeed)
				speed -= deAceleration;
			
		}
		//break decreases the player's speed over time 
		else if (Input.GetAxis ("Break") > 0)
		{
			playerAnim.SetFloat ("TiltY", (Input.GetAxis ("Vertical") * -1));
			speed -= brakeAceleration;
			if (speed <= 0)
			{
				speed = 0;
			}

		} else if (Input.GetAxis ("Vertical") < 1 && Input.GetAxis ("Break") != 1)
		{
			speed -= deAceleration;
			if (speed < 0)
				speed = 0;
		}

		//give the player a boost
		if (Input.GetAxis ("Boost") > 0)
		{
			if (boostFuel > 0)
			{
				//this prevents the player from geting infinit aceleration if he is standing still, (no 0-maxSpeed in an instance)
				if (speed < maxSpeed)
				{
					aceleration = boostAceleration;
				
					boostFuel -= boostConsumption;
					if (boostFuel <= 0)
						boostFuel = 0;
					
				} 
				//however if the player is going maxspeed he will be boosted to the boostMaxSpeed
				else
				{
					maxSpeed = boostmaxSpeed;
					speed = maxSpeed;
					boostFuel -= boostConsumption;
					if (boostFuel <= 0)
						boostFuel = 0;
				}

			} 
			//make sure the boost wears off even tho the player is holding shift
			else
			{
				maxSpeed = normalMaxSpeed;
				aceleration = normalAceleration;
			}


		} else
		{
			maxSpeed = normalMaxSpeed;
			aceleration = normalAceleration;
		}

		//move the player, and rotate him based on the players input 
		if (speed > 0)
		{
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			float turn = Input.GetAxis ("Horizontal") * handeling;

			rotation.y = turn;
			Quaternion deltaRotation = Quaternion.Euler (rotation * Time.deltaTime);

			playerRigid.MoveRotation (playerRigid.rotation * deltaRotation);

		}

		PreCollisionPreCheck ();
	}

	//checking the angle of possible upcomming collisions
	void PreCollisionPreCheck ()
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, collisionMask))
		{
			if (hit.collider != null)
			{
				collisionNormal = hit.normal;

				angle = Vector3.Angle ((hit.point - transform.position), collisionNormal);
			}
			if (hit.collider == null)
			{
				angle = 0;
			}
		}
	}

	//executes action based on the collision angle
	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.layer == 8)
		{
			Debug.Log ("speed on inpact: " + speed);
			Debug.Log ("Angle on inpact: " + angle);

			if (angle > 45 && speed > 40)
			{
				playerRigid.velocity = Vector3.zero;
				speed = 0;
				Debug.Log ("death");
				Crash ();

			} else
			{
				playerRigid.velocity = Vector3.zero;
				Debug.Log ("survive");
			}
		}
	}

	void Crash ()
	{
		//detach Camera
		Camera.main.transform.parent = null;

		//instansiate explotion

		//destroy player model
		//Destroy (playerModel);
		Destroy (this.gameObject);
	}

	public void Boost ()
	{
		maxSpeed = boostmaxSpeed;
		speed = maxSpeed;
	}

	public void DeBoost ()
	{
		maxSpeed = normalMaxSpeed;

	}
}
