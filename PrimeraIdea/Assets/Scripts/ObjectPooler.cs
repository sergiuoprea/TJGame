using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler current;

    public GameObject pooledRoad;
    public int roadPooledAmount;
    public bool willGrowRoad = true;

    public GameObject pooledObstacle;
    public int obstaclePooledAmount;
    public bool willGrowObstacle = true;

    List<GameObject> pooledRoads;
    List<GameObject> pooledObstacles;


    void Awake()
    {
        current = this;
    }

    void Start()
    {

        pooledRoads = new List<GameObject>();
        pooledObstacles = new List<GameObject>();

        for(int i = 0; i < roadPooledAmount; ++i)
        {
            GameObject obj = Instantiate(pooledRoad);
            obj.SetActive(false);
            pooledRoads.Add(obj);            
        }

        for(int i = 0; i < obstaclePooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObstacle);
            obj.SetActive(false);
            pooledObstacles.Add(obj);
        }

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
            GameObject obj = (GameObject)Instantiate(pooledRoad);
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

        if (willGrowRoad)
        {
            GameObject obj = (GameObject)Instantiate(pooledObstacle);
            pooledObstacles.Add(obj);
            return obj;
        }

        return null;
    }
}
	

