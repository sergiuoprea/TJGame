using UnityEngine;
using System.Collections;

public class ScenarioTransition : MonoBehaviour {

    
    void OnEnable()
    {
        Invoke("Destroy", 2f);
    }
	
    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    /*
	void Update () {

        if(transform.position.z <= -10)
        {
            Destroy(this.gameObject);
        }
        
        transform.Translate(0.1f,0,0);     
           
	}
    */
}
