using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScenarioGeneration : MonoBehaviour {

    public GameObject road;

    public Shader curvedWorld;
    public GameObject building;
    public float numberRoads;

    private List<GameObject> currentRoads = new List<GameObject>();
    private List<GameObject> nonVisibleRoads = new List<GameObject>();
    private int currentRoadId = 0;
    private Transform CenterPoints;

    private float speed;

    private bool newRoad = false;
    private object obj;



    void Start () {
        Application.targetFrameRate = 60;
        Camera.main.RenderWithShader(curvedWorld, "RenderType");
        numberRoads = 3;

        CreateNewRoad();
        speed = GameProgress.current.Speed;
    }


    void CreateNewRoad()
    {
        if(currentRoads.Count != 0)
        {
            GameObject tempRoad = ObjectPooler.current.GetPooledRoad();

            if (tempRoad == null) return;

            tempRoad.transform.position = new Vector3(0, 0, currentRoads[currentRoads.Count - 1].transform.position.z + 50);
            tempRoad.SetActive(true);
            currentRoads.Add(tempRoad);

        }
        else
        {

            float initialPosition = 0;
            GameObject obj;
            for (int i = 0; i < 3; ++i)
            {
                if(currentRoads.Count != 0)
                {
                    initialPosition = currentRoads[currentRoads.Count - 1].transform.position.z + 50;
                }

                obj = ObjectPooler.current.GetPooledRoad();

                if (obj == null) return;

                obj.transform.position = new Vector3(0, 0, initialPosition);
                obj.SetActive(true);
                currentRoads.Add(obj);

            }
        }
    }
	
	void Update () {

        // Vector3 endPoint = transform.position + speed * Vector3.back;
        //transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime);

        //clouds.transform.position += Vector3.right * 0.01f;

        if (!GameProgress.current.GameOver)
        {
            foreach (var r in currentRoads)
            {
                if (r.transform.position.z <= -35)
                {
                    nonVisibleRoads.Add(r);
                    newRoad = true;
                }
                else
                    r.transform.position += speed * Vector3.back;
            }

            if (newRoad)
            {
                newRoad = false;
                CreateNewRoad();
                currentRoadId++;
            }

            foreach (var r in nonVisibleRoads)
            {
                currentRoadId--;
                currentRoads.Remove(r);
                r.SetActive(false);

            }
            nonVisibleRoads.Clear();
        }
    }
	
}
