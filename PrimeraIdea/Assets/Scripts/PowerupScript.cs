using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour {

    private float speed = -0.1F;
	// Update is called once per frame
	void Update () {

        if(!GameProgress.current.GameOver)
            transform.Translate(0, 0, speed - GameProgress.current.Speed);

        if (transform.position.z < -30f)
            gameObject.SetActive(false);
	}
}
