using UnityEngine;
using System.Collections;

public class HoverBikeMeshController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	/*void SetYPos ()
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
	}*/
}
