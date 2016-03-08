using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour {

    private float speed = -0.2F;
	// Update is called once per frame
	void Update () {

        if(!GameProgress.current.GameOver)
            transform.Translate(0, 0, speed - GameProgress.current.Speed);

        if (transform.position.z < -10f)
            gameObject.SetActive(false);
	}
}
