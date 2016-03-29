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

    public GameObject pooledObstacle;
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

        int rand;
        List<int> created = new List<int>();

        for(int i = 0; i < roadPooledAmount; ++i)
        {
            rand = Random.Range(0, roadPooledAmount - 1);

            while(created.Contains(rand))
            {
                rand = Random.Range(0, roadPooledAmount - 1);
            }

            GameObject obj = Instantiate(roads[rand]);
            obj.SetActive(false);
            pooledRoads.Add(obj);            
        }

        for(int i = 0; i < obstaclePooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObstacle);
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

        for (int i = 0; i < pooledRoads.Count; ++i)
        {
            if (!pooledRoads[i].activeInHierarchy)
            {
                return pooledRoads[i];
            }
        }

        if(willGrowRoad)
        {
            int rand = Random.Range(0, 2);
            GameObject obj = (GameObject)Instantiate(roads[rand]);
            pooledRoads.Add(obj);
            return obj;
        }

         return null;        
    }

    public GameObject GetPooledObstacle()
    {

        for (int i = 0; i < pooledObstacles.Count; ++i)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }
        }

        if (willGrowObstacle)
        {
            GameObject obj = (GameObject)Instantiate(pooledObstacle);
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
	

