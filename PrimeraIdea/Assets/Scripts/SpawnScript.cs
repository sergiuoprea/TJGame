using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {


    public GameObject obstaculo;
    public GameObject powerup;

    private float tiempoTranscurrido;
	private float distanciaRecorrida;

    private float frecuenciaSpawn;
    private bool spawnPowerup;
    private float[] rails;

    private List<GameObject> obstacles;

	// Use this for initialization
	void Start () {

        obstacles = new List<GameObject>();
        tiempoTranscurrido = 0;
        frecuenciaSpawn = 25f;
        spawnPowerup = true;
        rails = new float[] {3, 1.5f, 0, -1.5f,-3};
	}
	
	// Update is called once per frame
	void Update () {

        
		if (!GameProgress.current.GameOver && GameProgress.current.StartFlag && Time.timeScale != 0)
        {

            tiempoTranscurrido += Time.deltaTime;
			distanciaRecorrida += GameProgress.current.Speed - GameProgress.current.ObstacleSpeed;

			if (distanciaRecorrida > frecuenciaSpawn && !GameProgress.current.GameOver)
            {
                GameObject temp;
                if (spawnPowerup)
                {
                    //temp = (GameObject)Instantiate(powerup);
                    //Vector3 pos = temp.transform.position;
                    //temp.transform.position = new Vector3((int)Random.Range(-3.75f, 3.75f), pos.y +5, 80);

                }
                else
                {
                    temp = ObjectPooler.current.GetPooledObstacle();
                    temp.SetActive(true);
                    // temp.transform.position = new Vector3(rails[(int)Random.Range(0, 4)], 0.5f, 80);
                    temp.transform.position = new Vector3(rails[2], 0.0f, 80);
                    obstacles.Add(temp);
                }

                tiempoTranscurrido -= frecuenciaSpawn;
                spawnPowerup = !spawnPowerup;
				distanciaRecorrida -= frecuenciaSpawn;
            }
        }
	}
}
