using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedBooster : MonoBehaviour
{
	PlayerController playerControl;
	float angle;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			angle = Vector3.Angle (other.transform.forward, transform.forward);
			Debug.Log (angle);
			playerControl = other.gameObject.GetComponent<PlayerController> ();

			if (angle < 10)
			{
				playerControl.Boost ();

			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		try
		{
			playerControl.DeBoost ();

		} catch
		{
			
		}


	}
}
