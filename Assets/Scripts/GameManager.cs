using UnityEngine;
using PathCreation;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const float SNOW_DIST_Z = 22.5f;

    private float score;
    
    public PathCreator[] paths;

    public Text pointText;

    public Text scoreText;

    public Button restart;

    private List<Transform> snows = new List<Transform>();

    private Transform player;

    public Transform snow;

    public Transform[] obstacles;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        restart.gameObject.SetActive(false);
        AddSnow(3);
    }

    private void Update()
    {
        if (snows.Count > 2) {
            RepositionSnow();  
        }  
    }

    private void RepositionSnow()
    {
        Transform lastSnow = snows[snows.Count - 1];
        if (player.transform.position.z > snows[1].position.z) {
            Transform firstSnow = snows[0];
            firstSnow.gameObject.SetActive(false);
            snows.Remove(firstSnow);
            firstSnow.transform.position = new Vector3(lastSnow.transform.position.x, lastSnow.transform.position.y, lastSnow.transform.position.z + SNOW_DIST_Z);
            firstSnow.gameObject.SetActive(true);
            snows.Add(firstSnow);
            PlaceObstaclesRandomly(snows[snows.Count - 1]);
        }
    }

    public void PlayerDead(GameObject skier)
    {
        if (skier && skier.tag == "Player") {
            restart.gameObject.SetActive(true);
        } else {
            
        }
    }

    public void PlayerFinished(GameObject player)
    {
        restart.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPoint(float value = 10, bool withAnim = true)
    {
        pointText.text = "+" + value;
        if (withAnim) {
            pointText.gameObject.SetActive(true);
        }
        StartCoroutine(Inactivate(pointText.gameObject, 1, value));
    }

    public PathCreator GetPath(string pathName)
    {
        foreach( PathCreator path in paths) {
            if (path.name == pathName) {
                return path;
            }
        }

        return null;
    }

    public IEnumerator Inactivate(GameObject go, float time, float value) {
        yield return new WaitForSeconds(time);
        go.SetActive(false);
        score += value;
        scoreText.text = " Score " + (int)score;
    }

    private void AddSnow(int count)
    {
        Transform prototype = snow;

        for (int i = 1; i <= count; i++) {
            Transform newSnow = Instantiate(prototype);
            newSnow.transform.position = new Vector3(prototype.transform.position.x, prototype.transform.position.y, prototype.transform.position.z + SNOW_DIST_Z);
            snows.Add(newSnow);
            prototype = newSnow;
        }
    }

    private void PlaceObstaclesRandomly(Transform snow)
    {
        Shuffle(obstacles);
        float[] posibleXes = {0, 1.6f, 3.2f, -1.6f, -3.2f};
        float[] posibleZes = new float[23];
        float startZ = snow.transform.position.z - SNOW_DIST_Z;
        float endZ = snow.transform.position.z;

        int index = 0;

        for (float i = startZ; i < endZ; i += 1.5f)
        {
            posibleZes[index] = i;
            index++;
        }

        foreach (Transform obstacle in obstacles) {
            Transform newObstacle = Instantiate(obstacle);
            newObstacle.transform.position = new Vector3(posibleXes[Random.Range(0, posibleXes.Length)], newObstacle.transform.position.y, posibleZes[Random.Range(0, posibleZes.Length)]);
            newObstacle.gameObject.SetActive(true);
            Destroy(newObstacle.gameObject, 10f);
        }
    }

    private void Shuffle(Transform[] obstacles)
    {
        for (int i = 0; i < obstacles.Length; i++) {
            int rnd = Random.Range(0, obstacles.Length);
            Transform tempT = obstacles[rnd];
            obstacles[rnd] = obstacles[i];
            obstacles[i] = tempT;
        }
    }
}