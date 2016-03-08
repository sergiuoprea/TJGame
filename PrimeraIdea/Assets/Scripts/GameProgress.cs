using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameProgress : MonoBehaviour {

    public static GameProgress current;

    private int puntuation;
    private float totalPuntuation;
    public Text puntuationText;

    public GameObject endPanel;

    public bool GameOver { get; set; }
    public float Speed { get; set; }

    void Awake()
    {
        current = this;
        Speed = 0.3f;
    }

	void Start () {

        GameOver = false;	
	}
	
	void Update () {


        if (!GameOver)
        {
            totalPuntuation += Speed + Speed;
            puntuation = (int)totalPuntuation;
            puntuationText.text = puntuation.ToString();
        }

        if (GameOver)
            endPanel.SetActive(true);

        //Aqui se definira el aumento de la dificultad


	
	}
}
