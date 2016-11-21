using UnityEngine;
using System.Collections;

public class CameraMovementManager : MonoBehaviour {

    #region Variables
    [SerializeField]
    private float c_maxAwayX;
    [SerializeField]
    private float c_maxAwayY;
    [SerializeField]
    private Transform c_playerTransform;
    [SerializeField]
    private float c_movementSpeed;
    [SerializeField]
    private bool c_useMovementSmooth;

    [SerializeField]
    private ScrollingParallax c_scrollingParallaxScript;

    [SerializeField]
    private Transform c_leftBoundX;
    [SerializeField]
    private Transform c_rightBoundX;
    [SerializeField]
    private Transform c_bottomBoundY;
    [SerializeField]
    private Transform c_topBoundY;

    private float c_mapMinimumX;
    private float c_mapMaximumX;
    private float c_mapMinimumY;
    private float c_mapMaximumY;

    private float c_cameraWidth;
    private float c_cameraHeight;
    #endregion

    void Start()
    {
        c_cameraWidth = gameObject.GetComponent<Camera>().aspect * gameObject.GetComponent<Camera>().orthographicSize;
        c_cameraHeight = gameObject.GetComponent<Camera>().orthographicSize;
        c_mapMinimumX = c_leftBoundX.position.x;
        c_mapMaximumX = c_rightBoundX.position.x;
        c_mapMinimumY = c_bottomBoundY.position.y;
        c_mapMaximumY = c_topBoundY.position.y;
    }

    void Update()
    {
        Vector2 t_movementDirection = c_playerTransform.position - transform.position;
        if (Mathf.Abs(t_movementDirection.x) < c_maxAwayX)
            t_movementDirection.x = 0;
        if (Mathf.Abs(t_movementDirection.y) < c_maxAwayY)
            t_movementDirection.y = 0;

        if (t_movementDirection == Vector2.zero)
            return;
        CheckMapBounds(ref t_movementDirection);
        if (t_movementDirection == Vector2.zero)
            return;
        MoveCamera(t_movementDirection);
    }

    void MoveCamera(Vector2 p_movementDirection)
    {
        Vector2 t_movementDirection;

        if (c_useMovementSmooth)
            t_movementDirection = p_movementDirection * c_movementSpeed * p_movementDirection.sqrMagnitude * Time.deltaTime;
        else
            t_movementDirection = p_movementDirection * c_movementSpeed * Time.deltaTime;

        transform.Translate(t_movementDirection);
        c_scrollingParallaxScript.Move(t_movementDirection.x);
    }

    //check if the camera can move or it is already at the bound of the map
    void CheckMapBounds(ref Vector2 p_movementDirection)
    {
        //trying to move to the right
        if      (p_movementDirection.x > 0 && transform.position.x + c_cameraWidth >= c_mapMaximumX)
                    p_movementDirection.x = 0;
        //trying to move to the left
        else if (p_movementDirection.x < 0 && transform.position.x - c_cameraWidth <= c_mapMinimumX)
                    p_movementDirection.x = 0;
        //trying to move to the top
        if      (p_movementDirection.y > 0 && transform.position.y + c_cameraHeight >= c_mapMaximumY)
                    p_movementDirection.y = 0;
        //trying to move to the bottom
        else if (p_movementDirection.y < 0 && transform.position.y - c_cameraHeight <= c_mapMinimumY)
                    p_movementDirection.y = 0;
    }
}