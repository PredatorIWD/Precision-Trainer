using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelfDestroy : MonoBehaviour
{
    public float spawnAnimationTimeMultiplier = 1f;
    public float destroyAfterSeconds = 3f;
    public float timeSinceSpawn = 0f;

    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Shader Graphs/Disintegrate");
    }

    // Update is called once per frame
    void Update()
    {
        // PercentDisintegrated
        if (rend.material.GetFloat("Vector1_6F879DB1") > 0)
        {
            rend.material.SetFloat("Vector1_6F879DB1", rend.material.GetFloat("Vector1_6F879DB1") - Time.deltaTime * spawnAnimationTimeMultiplier);
        }
        // Self destroy after X seconds
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn > destroyAfterSeconds && GameObject.Find("GameManager").GetComponent<GameManager>().gameMode == "Incremental")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().targetsDisappeared += 1;
            Destroy(this.gameObject);
        }
    }
}