using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject targetPrefab;
    private int targetsDisappearedLimit = 3;

    public double spawnMultiplier = 1;
    public float targetsDestroyed = 0;
    public float totalShots = 0;
    public float targetsDisappeared = 0;
    public string gameMode = "Incremental";
    private float timeSinceTargetSpawn = 0;
    private float totalTime = 0;
    private bool escAlreadyPressed = false;
    private GameObject[] targets;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void spawnTarget()
    {
        Instantiate(targetPrefab, new Vector3(Random.Range(-20, 20), Random.Range(1, 22), Random.Range(20, 40)), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
    }
    void restartGame()
    {
        GameObject.Find("GameOver").GetComponent<Text>().enabled = false;
        GameObject.Find("EndStats").GetComponent<Text>().enabled = false;
        targetsDestroyed = 0;
        targetsDisappeared = 0;
        timeSinceTargetSpawn = 0;
        spawnMultiplier = 1;
        totalShots = 0;
        totalTime = 0;
        // despawn active targets
        targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject currentTarget in targets)
        {
            Destroy(currentTarget);
        }
        if (gameMode == "Instant")
        {
            for (float i = 0; i < 3; i++)
            {
                spawnTarget();
            }
            GameObject.Find("EndStats").GetComponent<Text>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ESC twice to exit or LMB to cancel
        if (escAlreadyPressed && Input.GetMouseButtonDown(0))
        {
            totalShots -= 1;
            escAlreadyPressed = false;
            GameObject.Find("ExitScreen").GetComponent<Text>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escAlreadyPressed)
            {
                Application.Quit();
            }
            else
            {
                escAlreadyPressed = true;
                GameObject.Find("ExitScreen").GetComponent<Text>().enabled = true;
            }
        }
        // R to restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            restartGame();
        }
        // Q to switch game modes
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gameMode == "Incremental")
            {
                gameMode = "Instant";
                GameObject.Find("GameMode").GetComponent<Text>().text = "Mode: Instant\nQ to switch";
                GameObject.Find("EndStats").GetComponent<Text>().enabled = true;
            }
            else if (gameMode == "Instant")
            {
                gameMode = "Incremental";
                GameObject.Find("GameMode").GetComponent<Text>().text = "Mode: Incremental\nQ to switch";
                GameObject.Find("EndStats").GetComponent<Text>().enabled = false;
            }
            restartGame();
        }
        // Is it game over?
        if (targetsDisappeared > targetsDisappearedLimit)
        {
            GameObject.Find("GameOver").GetComponent<Text>().enabled = true;
            GameObject.Find("EndStats").GetComponent<Text>().enabled = true;
        }
        else
        {
            // Update UI
            if (float.IsNaN(targetsDestroyed / totalShots * 100))
                GameObject.Find("EndStats").GetComponent<Text>().text = "Precision: 0%";
            else
                GameObject.Find("EndStats").GetComponent<Text>().text = "Precision: " + (targetsDestroyed / totalShots * 100).ToString("F2") + "%";
            GameObject.Find("TargetsHit").GetComponent<Text>().text = "Targets hit: " + targetsDestroyed;
            GameObject.Find("TargetsMissed").GetComponent<Text>().text = "Missed: " + targetsDisappeared;
            totalTime += Time.deltaTime;
            GameObject.Find("TotalTime").GetComponent<Text>().text = "Time: " + totalTime.ToString("F2");
            // Create targets
            if (gameMode == "Incremental")
            {
                spawnMultiplier += 0.01 * Time.deltaTime; // increase the spawn rate by X every second
                timeSinceTargetSpawn += Time.deltaTime;
                if (timeSinceTargetSpawn * spawnMultiplier > 1)
                {
                    timeSinceTargetSpawn = 0;
                    spawnTarget();
                }
            }
            else if (gameMode == "Instant")
            {
            }
        }
    }
}
