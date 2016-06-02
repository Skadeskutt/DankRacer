using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	float timer;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.parent == null)
		{
			timer += Time.deltaTime;

			if (timer <= 2)
			{
				transform.Translate (Vector3.back * 0.3f, Space.Self);
			}
		}
	}
}
