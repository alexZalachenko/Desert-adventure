using UnityEngine;

public class ScrollingParallax : MonoBehaviour {

    #region Variables
    [SerializeField]
    private SpriteRenderer[] c_leftSprites;
    private SpriteRenderer[] c_rightSprites;

    [SerializeField]
    private float[] c_speeds;

    [SerializeField]
    private Transform c_camera;

    [SerializeField]
    private Rigidbody2D c_playerRigidbody;

    private float c_leftCameraBound;
    private float c_rightCameraBound;
    private float c_spritesSize;
    private float c_halfSpritesSize;
    private float c_halfCameraWidth;
    #endregion

    void Start()
	{
        c_halfSpritesSize = c_leftSprites[0].sprite.bounds.max.x;
        c_spritesSize = c_halfSpritesSize * 2;
        c_halfCameraWidth = c_camera.GetComponent<Camera>().aspect * c_camera.GetComponent<Camera>().orthographicSize;

        //clone all the background sprites, so when doing scrolling parallax the movement looks to be continous without gaps
        c_rightSprites = new SpriteRenderer[c_leftSprites.Length];
        for (int t_sprite = 0; t_sprite < c_leftSprites.Length; t_sprite++)
        {
            c_rightSprites[t_sprite] = (Instantiate(c_leftSprites[t_sprite].gameObject, transform) as GameObject).GetComponent<SpriteRenderer>();
            c_rightSprites[t_sprite].transform.position = new Vector2(c_leftSprites[t_sprite].transform.position.x + c_spritesSize,
                                                                      c_leftSprites[t_sprite].transform.position.y);
        }
    }

	public void Move(float p_scrollingSpeed)
	{

        c_leftCameraBound = c_camera.transform.position.x - c_halfCameraWidth;
        c_rightCameraBound = c_camera.transform.position.x + c_halfCameraWidth;

        for (int t_index = 0; t_index < c_leftSprites.Length; t_index++)
        {
            //move the sprites
            c_leftSprites[t_index].transform.position += Vector3.left * c_speeds[t_index] * p_scrollingSpeed;
            c_rightSprites[t_index].transform.position += Vector3.left * c_speeds[t_index] * p_scrollingSpeed;

            //scrolling to the right. Check min bound of the left sprite
            if (p_scrollingSpeed < 0)
            {
                if (c_leftSprites[t_index].bounds.min.x >= c_leftCameraBound)
                    MoveSpriteIntoBounds(c_rightSprites[t_index], c_leftSprites[t_index], -c_spritesSize, t_index);
            }
            //scrolling to the left. Check max bound of the right sprite
            else
            {
                if (c_rightSprites[t_index].bounds.max.x <= c_rightCameraBound)
                    MoveSpriteIntoBounds(c_leftSprites[t_index], c_rightSprites[t_index], c_spritesSize, t_index);
            }
        }
    }

    private void MoveSpriteIntoBounds(SpriteRenderer p_spriteToMove, 
                                      SpriteRenderer p_spriteInsideBounds, float p_offset, int p_index)
    {
        Vector3 t_newPosition = p_spriteInsideBounds.transform.position;
        t_newPosition.x += p_offset;
        p_spriteToMove.transform.position = t_newPosition;

        SwapSprites(p_index);
    }

    private void SwapSprites(int p_index)
    {
        SpriteRenderer t_leftSprite = c_leftSprites[p_index];
        c_leftSprites[p_index] = c_rightSprites[p_index];
        c_rightSprites[p_index] = t_leftSprite;
    }
}