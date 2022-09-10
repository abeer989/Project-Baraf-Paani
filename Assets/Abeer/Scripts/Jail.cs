using UnityEngine;

public class Jail : MonoBehaviour
{
    public static Jail instance;

    [SerializeField] float range;

    Transform player;
    bool runnerInRange;

    public bool RunnerInRange { get { return runnerInRange; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void OnEnable() => player = PlayerController.instance.transform;

    private void Update()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>().transform;

        else
        {
            if (Vector2.Distance(transform.position, player.transform.position) < range)
                runnerInRange = true;

            else
                runnerInRange = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
