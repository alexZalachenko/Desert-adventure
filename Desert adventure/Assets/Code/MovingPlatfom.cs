using UnityEngine;
using System.Collections;

public class MovingPlatfom : MonoBehaviour
{

    [SerializeField]
    private float c_movementSpeed;

    [SerializeField]
    private Transform[] c_movingPointsObjects;
    private Vector3[] c_movingPoints;

    private int c_nextMovingPoint = 1;
    private bool c_movingForward = true;


    void Start()
    {
        c_movingPoints = new Vector3[c_movingPointsObjects.Length];
        for (int t_point = 0; t_point < c_movingPointsObjects.Length; t_point++)
            c_movingPoints[t_point] = c_movingPointsObjects[t_point].position;
    }

    void Update()
    {
        Vector3 t_movementDirection = c_movingPoints[c_nextMovingPoint] - transform.position;
        transform.position += t_movementDirection.normalized * c_movementSpeed * Time.deltaTime;

        if (t_movementDirection.sqrMagnitude < 0.1f)
        {
            if (c_movingForward)
            {
                if (c_nextMovingPoint == c_movingPoints.Length - 1)
                {
                    --c_nextMovingPoint;
                    c_movingForward = !c_movingForward;
                }
                else
                    ++c_nextMovingPoint;
            }
            else
            {
                if (c_nextMovingPoint == 0)
                {
                    ++c_nextMovingPoint;
                    c_movingForward = !c_movingForward;
                }
                else
                    --c_nextMovingPoint;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D p_collision)
    {

    }

    public void OnCollisionExit2D(Collision2D p_collision)
    {

    }
}