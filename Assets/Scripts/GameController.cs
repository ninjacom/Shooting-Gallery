using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Text

public class GameController : MonoBehaviour {

    public static GameController _instance;
    private float timeLeft;
    public Text timeText;
    private int score;
    public Text scoreText;
    public Text highScoreText;

    [HideInInspector] // Hides var below from inspector
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

    void Awake()
    {
        _instance = this;
        timeLeft = 50;
        timeText.text = timeLeft.ToString();
    }
	// Use this for initialization
	void Start () {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", timeLeft,
            "to", 0,
            "time", timeLeft,
            "onupdatetarget", gameObject,
            "onupdate", "tweenUpdate",
            "oncomplete", "GameComplete"
            ));
        StartCoroutine("SpawnTargets");

        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore").ToString();
        score = 0;
	}

    void SpawnTarget()
    {
        //Get a random target
        int index = Random.Range(0, targets.Count);
        TargetBehaviour target = targets[index];

        //Show it
        target.ShowTarget();
    }
	
    IEnumerator SpawnTargets()
    {
        yield return new WaitForSeconds(1.0f);

        //Continue forever
        while (true)
        {
            int numOfTargets = Random.Range(1, 4);

            for(int i=0; i<numOfTargets; i++)
            {
                SpawnTarget();
            }

            yield return new WaitForSeconds(Random.Range(0.5f *numOfTargets, 2.5f));
        }
    }

    void GameComplete()
    {
        StopCoroutine("SpawnTargets");
        timeText.color = Color.black;
        timeText.text = "GAME OVER";
    }

    void tweenUpdate(float newValue)
    {
        timeLeft = newValue;
        if (timeLeft > 10)
        {
            timeText.text = timeLeft.ToString("#");
        }
        else
        {
            timeText.color = Color.red;
            timeText.text = timeLeft.ToString("#.0");
        }
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();

        if (score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", score);
            highScoreText.text = "High Score: " + score.ToString();
        }
    }
}
