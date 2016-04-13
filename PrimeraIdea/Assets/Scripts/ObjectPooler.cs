using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler current;

    public List<GameObject> roads;
    //public GameObject pooledRoad;
    private int roadPooledAmount;
    public bool willGrowRoad = true;

    public List<GameObject> obstacles;
    public int obstaclePooledAmount;
    public bool willGrowObstacle = true;

	/*
	public GameObject pooledCloud;
	public int cloudPooledAmount;
	public bool willGrowCloud = true;
	*/

    List<GameObject> pooledRoads;
    List<GameObject> pooledObstacles;
	//List<GameObject> pooledClouds;


    void Awake()
    {
        current = this;
    }

    void Start()
    {

        pooledRoads = new List<GameObject>();
        pooledObstacles = new List<GameObject>();
        roadPooledAmount = roads.Count;

        int rand = 0;
        List<int> created = new List<int>();

        for(int i = 0; i < roadPooledAmount; ++i)
        {
			
            rand = Random.Range(0, roadPooledAmount);

            while(created.Contains(rand))
            {
                rand = Random.Range(0, roadPooledAmount);
            }
            

            created.Add(rand);
            GameObject obj = Instantiate(roads[i]);
            obj.SetActive(false);
            pooledRoads.Add(obj);            
        }

        created.Clear();

        for(int i = 0; i < obstaclePooledAmount; i++)
        {
			
			rand = Random.Range(0, obstacles.Count);

            while (created.Contains(rand))
            {
                rand = Random.Range(0, obstacles.Count);
            }


            GameObject obj = Instantiate(obstacles[rand]);
            obj.SetActive(false);
            pooledObstacles.Add(obj);
        }


		/*
		for(int i = 0; i < cloudPooledAmount; i++)
		{
			GameObject obj = Instantiate(pooledCloud);
			obj.SetActive(false);
			pooledClouds.Add(obj);
		}
		*/

    }

    public GameObject GetPooledRoad()
    {

        List<int> done = new List<int>();
        int rand;

        for (int i = 0; i < pooledRoads.Count; ++i)
        {
            
            do
            {
				rand = Random.Range(0, roadPooledAmount);
            }
            while (done.Contains(rand));

            done.Add(rand);

            if (!pooledRoads[rand].activeInHierarchy)
            {
                return pooledRoads[rand];
            }

        }      



        if(willGrowRoad)
        {
            rand = Random.Range(0, roadPooledAmount);
            GameObject obj = (GameObject)Instantiate(roads[rand]);
            pooledRoads.Add(obj);
            return obj;
        }

         return null;        
    }

    public GameObject GetPooledObstacle()
    {
        int rand;
        for (int i = 0; i < pooledObstacles.Count; ++i)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }
        }

        if (willGrowObstacle)
        {
            rand = Random.Range(0, roadPooledAmount);
            GameObject obj = (GameObject)Instantiate(obstacles[rand]);
            pooledObstacles.Add(obj);
            return obj;
        }

        return null;
    }

	/*
	public GameObject GetPooledCloud()
	{

		for (int i = 0; i < pooledClouds.Count; ++i)
		{
			if (!pooledClouds[i].activeInHierarchy)
			{
				return pooledClouds[i];
			}
		}

		if (willGrowCloud)
		{
			GameObject obj = (GameObject)Instantiate(pooledCloud);
			pooledClouds.Add(obj);
			return obj;
		}

		return null;
	}
	*/
}
	

