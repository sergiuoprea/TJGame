using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        transform.Translate(0, 0, -0.2f);

        if (transform.position.z < -10f)
            gameObject.SetActive(false);
	}
}
