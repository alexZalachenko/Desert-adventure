using UnityEngine;
using System.Collections;

public class MovingPlatfom : MonoBehaviour {

    [SerializeField]
    private Vector2 c_movementDirection;

    private float c_leftBound;
    private float c_rightBound;
    private float c_topBound;
    private float c_bottomBound;

    [SerializeField]
    private bool c_useHorizontalBounds;

    void Start()
    {
        Transform p_bounds = transform.FindChild("Bounds");
        c_leftBound     = p_bounds.GetChild(0).transform.position.x;
        c_rightBound    = p_bounds.GetChild(1).transform.position.x;
        c_topBound      = p_bounds.GetChild(2).transform.position.y;
        c_bottomBound   = p_bounds.GetChild(3).transform.position.y;
    }
	
	void Update()
	{
        transform.position += new Vector3(c_movementDirection.x, c_movementDirection.y, 0) * Time.deltaTime;

        //if the platform has reached the bounds, set the movement to the oposite directon
        if(c_useHorizontalBounds)
        {
            if (transform.position.x > c_rightBound)
                c_movementDirection *= -1;
            else if (transform.position.x < c_leftBound)
                c_movementDirection *= -1;
        }
        else
        {
            if (transform.position.y > c_topBound)
                c_movementDirection *= -1;
            else if (transform.position.y < c_bottomBound)
                c_movementDirection *= -1;
        }
	}
}