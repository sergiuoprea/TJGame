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
	private bool jump;


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
		jump = false;
        carril = center = 3;
        destination = transform.position;
		destinationCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position;

        //center = carril;
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

                        float h = Mathf.Abs(touch.position.x - startPos.x);
                        float v = Mathf.Abs(touch.position.y - startPos.y);

                        float hS = Mathf.Sign (touch.position.x - startPos.x);
						float vS = Mathf.Sign (touch.position.y - startPos.y);                        

						if (hS >= 0 && h > v)
							direction = 1;
						else if (hS <= 0 && h > v)
							direction = -1;
						else if (vS >= 0 && v > h)
							direction = 2;
						
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
        
        if (canIPulse && !GameProgress.current.GameOver)
        {
			if ((Input.GetKeyDown(KeyCode.A) || direction == -1) && carril != 1 && (!moving || jump))
            {
                destination += Vector3.left * distance;
				//StopCoroutine ("MoveFromTo");
                StartCoroutine(MoveFromTo(transform.position, destination, 0.1f));
				destinationCamera += Vector3.left * (distance * 0.70f);
				StartCoroutine(MoveCameraFromTo(GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position
					, destinationCamera, 0.1f));
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

			if ((Input.GetKeyDown(KeyCode.D) || direction == 1) && carril != 5 && (!moving || jump))
            {
                destination += Vector3.right * distance;
				//StopCoroutine ("MoveFromTo");
                StartCoroutine(MoveFromTo(transform.position, destination, 0.1f));
				destinationCamera += Vector3.right * (distance * 0.70f);
				StartCoroutine(MoveCameraFromTo(GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position
					, destinationCamera, 0.1f));
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

			if ((Input.GetKeyDown(KeyCode.W) || direction == 2) && !jump && !moving)
            {
                destination += Vector3.up * distance;
				StartCoroutine(MoveFromTo(transform.position, destination, 0.3f));

				GetComponent<Animator> ().SetTrigger ("Jumping");

				jump = true;
				direction = 0;
               	//pulsed = true;
                //time = 0;
                //direction = 0;
            }

			/*
			if ((Input.GetKeyDown(KeyCode.S)) && jump)
			{
				destination = new Vector3(transform.position.x, 0, -3);
				StartCoroutine(MoveFromTo(transform.position, destination, 0.1f, true));

				//GetComponent<Animator> ().SetTrigger ("Jumping");

				jump = false;
				moving = true;
				//pulsed = true;
				//time = 0;
				//direction = 0;
			}
			*/
        }


		if (jump && !moving && transform.position.y != 0f) 
		{
			destination += Vector3.down * distance;
			StartCoroutine(MoveFromTo(transform.position, destination, 0.3f));



		}



        if (time < pulsationTime && pulsed)
            canIPulse = false;
        else
            canIPulse = true;

        if (pulsed)
            time += Time.deltaTime;

        direction = 0;
                
    }

    void OnTriggerEnter(Collider col)
    {
        if (!hasBeenDamaged && (col.gameObject.tag == "Obstacles" || col.gameObject.tag == "Car" || col.gameObject.tag == "Peaton"))
        {
            col.gameObject.SetActive(false);
            currentHealth -= damage;

			if (currentHealth < 0) {
				GameProgress.current.GameOver = true;
				GetComponent<Animator> ().SetTrigger ("Dead");
			}
            else
            {
                healthSlider.value = currentHealth;
                hasBeenDamaged = true;
                StartCoroutine(Damaged(7, 0.15f));
            }
        }
    }

    IEnumerator Damaged(float duration, float s)
    {
       

        for(int i = 0; i < duration; ++i)
        {
            GetComponentInChildren<SkinnedMeshRenderer> ().enabled = !GetComponentInChildren<SkinnedMeshRenderer>().enabled;
            yield return new WaitForSeconds(s);
        }  
       
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        hasBeenDamaged = false;
    }

    IEnumerator MoveFromTo(Vector3 pA, Vector3 pB, float time)
    {
		if (!moving)
        {
            moving = true;
            float t = 0;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;

				transform.position = Vector3.Lerp (transform.position, destination, t);			

                yield return null;
            }

            moving = false;
			//Debug.Log ("OUT");

			if (transform.position.y == 0.0f && jump) {
				jump = false;
				Debug.Log ("FALSO");
			}

        }
    }

	IEnumerator MoveCameraFromTo(Vector3 pA, Vector3 pB, float time)
	{
		if (moving)
		{
			float t = 0;

			while (t < 1.0f)
			{
				t += Time.deltaTime / time;

				GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position = Vector3.Lerp (pA, pB, t);

				yield return null;
			}

		}
	}

}

