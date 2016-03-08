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
    
    private Vector2 startPos = Vector2.zero;
    private bool couldBeSwipe = false;
    private float comfortZone;
    private float minSwipeDist = 50.0f;
    private float maxSwipeTime = 0.5f;
    private float startTime;

    private int direction;

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
               

        if (Input.touchCount > 0)
        {
            float swipeTime;
            float swipeDist;
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    couldBeSwipe = true;
                    startPos = touch.position;
                    startTime = Time.time;

                    break;
               
            case TouchPhase.Moved:

                    swipeTime = Time.time - startTime;
                    swipeDist = (touch.position - startPos).magnitude;

                    if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist))
                    {
                        // It's a swiiiiiiiiiiiipe!

                        if (Mathf.Sign(touch.position.x - startPos.x) >= 0)
                            direction = 1;
                        else
                            direction = -1;

                        couldBeSwipe = false;

                    }

                    break;

                    /*

            case TouchPhase.Stationary:
                couldBeSwipe = false;
                break;
                */

                case TouchPhase.Ended:
                    couldBeSwipe = true;
                    /*
                    swipeTime = Time.time - startTime;
                    swipeDist = (touch.position - startPos).magnitude;

                    if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist))
                    {
                        // It's a swiiiiiiiiiiiipe!

                        if (Mathf.Sign(touch.position.x - startPos.x) >= 0)
                            direction = 1;
                        else
                            direction = -1;
                        
                    }
                    */

                    break;

            }
        }
        
        if (canIPulse)
        {
            if ((Input.GetKeyDown(KeyCode.A) || direction == -1) && carril != 1)
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
                direction = 0;

            }

            if ((Input.GetKeyDown(KeyCode.D) || direction == 1) && carril != 5)
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
                direction = 0;
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

            if (currentHealth < 0)
                GameProgress.current.GameOver = true;

            healthSlider.value = currentHealth;
            hasBeenDamaged = true;
            StartCoroutine(Damaged(7, 0.15f));
        }
    }

    IEnumerator Damaged(float duration, float s)
    {
       

        for(int i = 0; i < duration; ++i)
        {
            GetComponentInChildren<MeshRenderer>().enabled = !GetComponentInChildren<MeshRenderer>().enabled;
            yield return new WaitForSeconds(s);
        }  
       
        GetComponentInChildren<MeshRenderer>().enabled = true;
        hasBeenDamaged = false;
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

