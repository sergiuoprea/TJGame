using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterMove : MonoBehaviour
{

    public float speed;
    public float distance = 1.5f;

    private float pulsationTime = 0.1f;
    private float time;
    private bool canIPulse;
    private bool pulsed;

    private int carril;
    private Vector3 destination;
	private Vector3 destinationCamera;
    private bool moving = false;
    private int center;

    // Health
    private float currentHealth;
    private float health;
    private float damage;
    private bool hasBeenDamaged;

    public Slider healthSlider;
	public Text puntuation;

    // Use this for initialization
    void Start()
    {

        time = 0;
        canIPulse = true;
        pulsed = false;
        carril = center = 3;
        destination = transform.position;
		destinationCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position;

        center = carril;
        speed = 0.1f;

        // Health
        health = 100;
        currentHealth = health;
        damage = 40;
        healthSlider.value = currentHealth;
        hasBeenDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {


        /*
        if(hasBeenDamaged)
        {
            StartCoroutine(Damaged(1.5f,0.5f));
        }
         * */

		puntuation.text = ((int)Time.time).ToString() ;

        if (canIPulse)
        {
            if (Input.GetKeyDown(KeyCode.A) && carril != 1)
            {
                destination += Vector3.left * distance;
                StartCoroutine(MoveFromTo(transform.position, destination, 0.1f, true));
				destinationCamera += Vector3.left * (distance * 0.70f);
				StartCoroutine(MoveFromTo(GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position
					, destinationCamera, 0.1f, false));
				/*
				destinationCamera += Vector3.right * (distance / 2);
				StartCoroutine(MoveFromTo(GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position
					, destinationCamera, 0.2f, false));
					*/
				
                pulsed = true;
                carril--;
                time = 0;

            }

            if (Input.GetKeyDown(KeyCode.D) && carril != 5)
            {
                destination += Vector3.right * distance;
                StartCoroutine(MoveFromTo(transform.position, destination, 0.1f, true));
				destinationCamera += Vector3.right * (distance * 0.70f);
				StartCoroutine(MoveFromTo(GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position
					, destinationCamera, 0.1f, false));
				/*
				destinationCamera += Vector3.left * (distance / 2);
				StartCoroutine(MoveFromTo(GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position
					, destinationCamera, 0.2f, false));
					*/

                pulsed = true;
                carril++;
                time = 0;
            }
        }


        if (time < pulsationTime && pulsed)
            canIPulse = false;
        else
            canIPulse = true;

        if (pulsed)
            time += Time.deltaTime;
                
    }

    void OnTriggerEnter(Collider col)
    {
        if (!hasBeenDamaged && col.gameObject.tag == "Obstacles")
        {
            col.gameObject.SetActive(false);
            currentHealth -= damage;
            healthSlider.value = currentHealth;
            //hasBeenDamaged = true;
        }
    }

    IEnumerator Damaged(float duration, float s)
    {
        /*
        while (duration > s)
        {
            duration -= Time.deltaTime;
            GetComponentInChildren<MeshRenderer>().enabled = !GetComponentInChildren<MeshRenderer>().enabled;
            yield return new WaitForSeconds(s);
        }
         * */

        GetComponentInChildren<MeshRenderer>().enabled = !GetComponentInChildren<MeshRenderer>().enabled;
        yield return new WaitForSeconds(s);
        GetComponentInChildren<MeshRenderer>().enabled = !GetComponentInChildren<MeshRenderer>().enabled;
        yield return new WaitForSeconds(s);
        GetComponentInChildren<MeshRenderer>().enabled = !GetComponentInChildren<MeshRenderer>().enabled;
        yield return new WaitForSeconds(s);


        hasBeenDamaged = false;
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    IEnumerator MoveFromTo(Vector3 pA, Vector3 pB, float time, bool character)
    {

		if (!moving || !character)
        {
            moving = true;
            float t = 0;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;

				if (character)
					transform.position = Vector3.Lerp (pA, pB, t);
				else 
					GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position = Vector3.Lerp (pA, pB, t);


                yield return null;
            }

            moving = false;

        }
    }


}

