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


	private TouchControl touchControl;
	private SwipeAction direction;

	private float currentSpeed;    

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

		touchControl = GetComponent<TouchControl> ();

		currentSpeed = GameProgress.current.Speed;


    }

    // Update is called once per frame
    void Update()
    {

		direction = touchControl.GetSwipeAction ();

		if (GameProgress.current.StartFlag && !GameProgress.current.Running) {
			GetComponent<Animator> ().SetTrigger ("Running");
		}

        
		if (canIPulse && !GameProgress.current.GameOver && GameProgress.current.EnableMovement)
        {
			if ((Input.GetKeyDown(KeyCode.A) || direction == SwipeAction.LeftSwipe) && carril != 1 && (!moving || jump))
            {
                destination += Vector3.left * distance;

				if (carril == 2) 
				{
					//destination.y = 0.2f;
				}

				if (carril == 5)
				{
					destination.y = 0.0f;
				}

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
				direction = SwipeAction.Nothing;

            }

			if ((Input.GetKeyDown(KeyCode.D) || direction == SwipeAction.RightSwipe) && carril != 5 && (!moving || jump))
            {
                destination += Vector3.right * distance;

				if (carril == 4) 
				{
					//destination.y = 0.2f;
				}

				if (carril == 1)
				{
					destination.y = 0.0f;
				}

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
				direction = SwipeAction.Nothing;
            }

			if ((Input.GetKeyDown(KeyCode.W) || direction == SwipeAction.UpSwipe) && !jump && !moving)
            {
				destination += Vector3.up * (distance);// + 0.2f);
				StartCoroutine(MoveFromTo(transform.position, destination, 0.3f));

				GetComponent<Animator> ().SetTrigger ("Jumping");

				jump = true;
				direction = SwipeAction.Nothing;
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


		if (jump && !moving) 
		{
			if((transform.position.y != 0.0f))// && (carril == 2 || carril == 3 || carril == 4)) || (transform.position.y != 0.2f && carril == 5) || (transform.position.y != 0.2f && carril == 1))
			{			
				destination += Vector3.down * (distance); //+ 0.2f);
				StartCoroutine(MoveFromTo(transform.position, destination, 0.3f));
			}
		}



        if (time < pulsationTime && pulsed)
            canIPulse = false;
        else
            canIPulse = true;

        if (pulsed)
            time += Time.deltaTime;


		direction = SwipeAction.Nothing;
                
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

		if(!hasBeenDamaged && col.gameObject.tag == "Powerup")
		{
			col.gameObject.SetActive (false);

			GameProgress.current.batterySlider.GetComponent<Slider> ().value += 10;
		}
    }

    IEnumerator Damaged(float duration, float s)
    {
		SkinnedMeshRenderer[] renderer = GetComponentsInChildren<SkinnedMeshRenderer> ();


        for(int i = 0; i < duration; ++i)
        {
           // GetComponentInChildren<SkinnedMeshRenderer> ().enabled = !GetComponentInChildren<SkinnedMeshRenderer>().enabled;

			foreach (var r in renderer) {
				r.enabled = !r.enabled;
			}

            yield return new WaitForSeconds(s);
        }  
       
		foreach (var r in renderer) {
			r.enabled = true;
		}
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

			if ((transform.position.y == 0.0f /* || (transform.position.y <= 0.2f && carril == 1) ||  (transform.position.y <= 0.2f && carril == 5))*/ && jump) )
			{
				jump = false;
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

