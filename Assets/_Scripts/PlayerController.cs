using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	Rigidbody playerRigid;
	
	public float heightOverGround = 1.3f;


	//Hover variables
	float step;
	Vector3 initialPos;

	//turning varaibles
	public float handeling = 10;
	Vector3 rotation;

	//speed
	public float maxSpeed;
	public float maxBreakPower;

	float speed;
	float breakPower;

	void Start ()
	{
		initialPos = transform.position;
		playerRigid = GetComponent<Rigidbody> ();
		//offset = transform.position.y + transform.localScale.y;
	}

	void FixedUpdate ()
	{
		//SetYPos ();
		//GetInput ();

		breakPower = maxBreakPower * Input.GetAxis ("Break");
		speed = maxSpeed * Input.GetAxis ("Vertical");




		if (speed > 0)
		{
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			float turn = Input.GetAxis ("Horizontal") * handeling;

			rotation.y = turn;
			Quaternion deltaRotation = Quaternion.Euler (rotation * Time.deltaTime);

			playerRigid.MoveRotation (playerRigid.rotation * deltaRotation);

		}


	}

	void GetInput ()
	{


		if (Input.GetKey (KeyCode.W))
		{
			
		}
		if (Input.GetKey (KeyCode.A))
		{
			
		}
		if (Input.GetKey (KeyCode.S))
		{
			
		}
		if (Input.GetKey (KeyCode.D))
		{
		}
	}

	void SetYPos ()
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, Vector3.down, out hit))
		{
			if (hit.transform.tag == "Terrain")
			{
				if (hit.distance > 1.4)
				{
					playerRigid.isKinematic = false;
				} else if (speed == 0 && hit.distance < 1.4)
				{
					playerRigid.isKinematic = true;
					step += 0.1f;
					if (step > 9999)
						step = 1;
					
					transform.position = new Vector3 (transform.position.x, Mathf.Sin (step) * 0.06f + heightOverGround, transform.position.z);
				}
			}
		}	
	}
}
