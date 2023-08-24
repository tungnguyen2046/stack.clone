using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] MovingCube cubePrefab;
    [SerializeField] MoveDirection moveDirection;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if(MovingCube.lastCube != null && MovingCube.lastCube.gameObject != GameObject.Find("Start"))
        {
            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.lastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.lastCube.transform.position.z;

            cube.transform.position = new Vector3(x, MovingCube.lastCube.transform.position.y + cubePrefab.transform.localScale.y, z);
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }
}

public enum MoveDirection
{
    X,
    Z
}
