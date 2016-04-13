using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameProgress : MonoBehaviour {

    public static GameProgress current;

    private int puntuation;
    private float totalPuntuation;
    public Text puntuationText;
    public Button restartButton;

	public GameObject startPanel;
    public GameObject endPanel;
	public GameObject pausePanel;

	public GameObject puntuationPanel;
	public GameObject healthSlider;
	public GameObject batterySlider;

    public bool StartFlag { get; set; }
    public bool GameOver { get; set; }
    public float Speed { get; set; }
	public float ObstacleSpeed{ get; set; }
	public bool EnableMovement { get; set; }

	public bool Running { get; set; }
	public float Battery { get; set; }

	private Vector3 initPos;
	private Vector3 finalPos;

	private Quaternion initRot;
	private Quaternion finalRot;

	private float time;

	private TouchControl control;
	private SwipeAction action;

	private bool restart;

	private AudioClip clickPhoto;

	public Text finalPuntuation;

    void Awake()
    {
        StartFlag = false;
        current = this;
        Speed = 0.15f;
		ObstacleSpeed = -0.1f;
		EnableMovement = false;

		GameObject.Find ("BackGround").SetActive (true);
		control = GameObject.Find ("Player").GetComponent<TouchControl>();
	}

	void Start () {

        GameOver = false;
		Running = false;

		//	-0.25	1.05 -2.15
		//	25 		163 	0

		GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position = new Vector3 (-0.25f, 1.05f, -2.15f);
		GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.rotation = Quaternion.Euler (25, 163, 0);

		initPos = GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position;
		finalPos = new Vector3 (0.0f, 3.3f, -6.05f);

		initRot = GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.rotation;
		finalRot = Quaternion.Euler (25, 0, 0);

		time = 0;

		restart = false;

		batterySlider.GetComponent<Slider> ().value = 100;


	}
	
	void Update () {


		if (StartFlag && !Running) 
		{

			startPanel.SetActive (false);

			puntuationPanel.SetActive (true);
			healthSlider.SetActive (true);
			batterySlider.SetActive (true);

			StartCoroutine(MoveStartCameraFromTo (initPos, finalPos, initRot, finalRot, 1.0f));

			Running = true;
		}


		if (!GameOver && StartFlag)
        {
            totalPuntuation += Speed + Speed;
            puntuation = (int)totalPuntuation;
            puntuationText.text = puntuation.ToString();
			//puntuationText.text = Speed.ToString();

			time += Time.deltaTime;

			batterySlider.GetComponent<Slider> ().value -= (1.0f / Speed) * 0.01f;
			Battery = batterySlider.GetComponent<Slider> ().value;

			if (Battery <= 0)
				GameOver = true;

			//SIMPLE DIFFICULTY INCREASE
			if (time > 10)
			{
				Speed += 0.02f;
				time = 0;

				//AUMENTAR VELOCIDAD ANIMACION
			}


			action = control.GetSwipeAction ();

			if (action == SwipeAction.DoubleTap && Running) 
			{
				pausePanel.SetActive (true);
				Time.timeScale = 0;
			}


			if (restart) 
			{
				pausePanel.SetActive (false);
				Time.timeScale = 1;

				restart = false;

			}

        }

		if (GameOver) 
		{
			finalPuntuation.text = puntuation.ToString ();
			endPanel.SetActive (true);
		}
				
	}

	public void StartButton()
	{
		StartFlag = true;
	}

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(0);
    }

	public void RestartButton()
	{
		restart = true;
	}



	IEnumerator MoveStartCameraFromTo(Vector3 pA, Vector3 pB, Quaternion rA, Quaternion rB, float time)
	{

		float t = 0;

		while (t < 1.0f)
		{
			t += Time.deltaTime / time;

			GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.position = Vector3.Lerp (pA, pB, t);
			GameObject.Find ("Main Camera").GetComponent<Camera> ().transform.rotation = Quaternion.Lerp (rA, rB, t);
			yield return null;
		}

		EnableMovement = true;
		GameObject.Find ("BackGround").SetActive (false);

	}
}
