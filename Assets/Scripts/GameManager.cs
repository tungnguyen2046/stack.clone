using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] ScoreText scoreText;
    [SerializeField] GameObject gameOver;
     GameObject mainCamera;

    CubeSpawner[] spawners;
    int spawnerIndex;
    CubeSpawner currentSpawner;

    public float moveSpeed {get; set;}

    public static event Action OnCubeSpawn = delegate{};

    private void Awake() 
    {
        moveSpeed = 1f;
        spawners = FindObjectsOfType<CubeSpawner>();
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(MovingCube.currentCube != null) MovingCube.currentCube.Stop();

            spawnerIndex = spawnerIndex == 0 ? 1: 0;
            currentSpawner = spawners[spawnerIndex];

            currentSpawner.SpawnCube();
            IncrementSpeed();
            OnCubeSpawn();
        }
    }

    public void IncrementSpeed()
    {
        if(scoreText.IsDivisible())
        {
            moveSpeed += 0.5f;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + new Vector3(0, 1, 0), Time.deltaTime);
        }  
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
