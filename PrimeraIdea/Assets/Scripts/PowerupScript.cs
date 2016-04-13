using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour {


	void Update () 
	{

		if (!GameProgress.current.GameOver && GameProgress.current.StartFlag && Time.timeScale != 0)
        {
			transform.Translate(0, 0, GameProgress.current.ObstacleSpeed - GameProgress.current.Speed);

            if (transform.position.z < -30f)
                gameObject.SetActive(false);
        }
	}


}
