using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube currentCube {get; private set;}
    public static MovingCube lastCube {get; private set;}
    public MoveDirection MoveDirection {get; set; }
    public Color color {get; set; }

    // [SerializeField] float moveSpeed = 1f;
    float moveSpeed;
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        moveSpeed = gameManager.moveSpeed;
    }

    void OnEnable() 
    {
        if(lastCube == null)
        {
            lastCube = GameObject.Find("Start").GetComponent<MovingCube>();
            lastCube.moveSpeed = 0;
        }
        currentCube = this;
        
        color = GetRandomColor();
        GetComponent<Renderer>().material.color = color;

        transform.localScale = new Vector3(lastCube.transform.localScale.x, transform.localScale.y, lastCube.transform.localScale.z);
    }

    void Update()
    {
        if(MoveDirection == MoveDirection.Z)
        transform.position += transform.forward * Time.deltaTime * moveSpeed;

        else transform.position += transform.right * Time.deltaTime * moveSpeed;
    } 

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }  

    public void Stop()
    {
        moveSpeed = 0;
        float hangover;

        if(MoveDirection == MoveDirection.Z) hangover = transform.position.z - lastCube.transform.position.z;          
        else hangover = transform.position.x - lastCube.transform.position.x;
        
        float direction = hangover > 0 ? 1f: -1f;
        float max = MoveDirection == MoveDirection.Z ? lastCube.transform.localScale.z: lastCube.transform.localScale.x;

        if(Mathf.Abs(hangover) > max)
        {
            lastCube = null;
            currentCube = null;
            Time.timeScale = 0;
            gameManager.GameOver();
        }

        if(MoveDirection == MoveDirection.Z) SplitCubeOnZ(hangover, direction);
        else SplitCubeOnX(hangover, direction);

        lastCube = currentCube;
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        if (lastCube != null)
        {
            float newXsize = lastCube.transform.localScale.x - Mathf.Abs(hangover);
            float fallingXBlockSize = transform.localScale.x - newXsize;
            float newXpos = lastCube.transform.position.x + hangover/2;

            transform.localScale = new Vector3(newXsize, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(newXpos, transform.position.y, transform.position.z);

            float cubeEgde = transform.position.x + newXsize/2 * direction;
            float fallingBlockXpos = cubeEgde + fallingXBlockSize/2 * direction;

            SpawnDropCube(fallingBlockXpos, fallingXBlockSize);
        }     
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        if (lastCube != null)
        {
            float newZsize = lastCube.transform.localScale.z - Mathf.Abs(hangover);
            float fallingZBlockSize = transform.localScale.z - newZsize;
            float newZpos = lastCube.transform.position.z + hangover/2;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZsize);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZpos);

            float cubeEgde = transform.position.z + newZsize/2 * direction;
            float fallingBlockZpos = cubeEgde + fallingZBlockSize/2 * direction;

            SpawnDropCube(fallingBlockZpos, fallingZBlockSize);
        }        
    }
    
    private void SpawnDropCube(float fallingBlockPos, float fallingBlockSize)
    {
        if(lastCube == GameObject.Find("Start").GetComponent<MovingCube>()) return;

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPos);
        }
        else
        {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPos, transform.position.y, transform.position.z);
        }
        
        cube.GetComponent<Renderer>().material.color = color;

        cube.AddComponent<Rigidbody>();
        Destroy(cube.gameObject, 1f);
    }
}