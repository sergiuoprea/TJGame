using UnityEngine;
using System.Collections;

public class CloudTransition : MonoBehaviour {

	// Use this for initialization
	public float cloudSpeed = 0.01f;
	
	// Update is called once per frame
	void Update () {

		transform.position += Vector3.right * cloudSpeed;
	}
}
