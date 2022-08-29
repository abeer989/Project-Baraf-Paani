using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] BoxCollider2D cameraBoundsBox;

    private Transform player;
    private float halfHeight, halfWidth;

    void Start()
    {
        

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        
    }

    void Update()
    {
        // following the player if the player exists within the scene and clamping the cam movement, such that,
        // it never follows the player off of the map:
        if (player)
            transform.position = new Vector3(x: Mathf.Clamp(player.position.x, cameraBoundsBox.bounds.min.x + halfWidth, cameraBoundsBox.bounds.max.x - halfWidth),
                                             y: Mathf.Clamp(player.position.y, cameraBoundsBox.bounds.min.y + halfHeight, cameraBoundsBox.bounds.max.y - halfHeight),
                                             z: transform.position.z);

        
    }
}
