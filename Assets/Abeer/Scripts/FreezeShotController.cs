using UnityEngine;

public class FreezeShotController : MonoBehaviour
{
    [SerializeField] IceBall iceBallPrefab;
    [SerializeField] Transform pivot;
    [SerializeField] Transform firePoint;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - pivot.position;
        direction.Normalize();

        // the angle we want the enemy to turn:
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // to turn the enemy toward the player at a graceful speed:
        pivot.rotation = targetRotation;

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(iceBallPrefab, firePoint.position, Quaternion.identity).moveDir = direction;
           
        }
    }
}
