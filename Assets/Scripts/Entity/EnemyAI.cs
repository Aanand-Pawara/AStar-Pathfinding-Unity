using UnityEngine;

public class EnemyAI : BaseAI
{
    public PlayerController Player;
    private float checkRadius = 4f;

    private Coroutine followPath;
    public float RecheckTime = 3f;

    private Vector3 oldKnownPosition;
    private Vector3 lastPosition;
    private Vector3 oldposition;
    public float positionChangeThreshold = 0.01f;
    private void Start()
    {
        Player.OnNewDestinationSelected.AddListener(UpdatePath);
        base.OnDestinationReached.AddListener(CheckPlayer);
        UpdatePath();
    }

    float t = 0f;
    public void Update()
    {

        t += Generator.Instance.DELTA_TIME;
        if (t >= RecheckTime && Vector3.Distance(transform.position, Player.transform.position) > checkRadius || oldKnownPosition != Player.transform.position)
        {
            t = 0f;
            if (!isMoving)
            {
                UpdatePath();
            }
            else if (isMoving && oldKnownPosition != Player.transform.position && (currentPath == null || currentPath.Count <= 0))
            {
                isMoving = false;
            }

        }
    }

    private void UpdatePath()
    {

        oldKnownPosition = Player.transform.position;
        lastPosition = transform.position;

        Debug.Log($"{gameObject.name} Trying to find a new path");
        Vector3 targetPosition = GetRandomPositionWithinRadius(Player.transform.position, checkRadius);



        int maxIterations = 20;
        int iteration = 0;

        do
        {
            MakeCurrentNodeObstacle(false);
            currentPath = Generator.Instance.mPathfinding.FindPath(transform.position, targetPosition);
            iteration++;
            if (currentPath != null) break;
        } while ((currentPath == null || currentPath.Count == 0 || Generator.Instance.mGrid.GetNode(currentPath[currentPath.Count - 1]).IsObstacle) && iteration < maxIterations);

        if (followPath != null) StopCoroutine(followPath);

        if (currentPath != null) followPath = StartCoroutine(FollowPath());


    }

    void CheckPlayer()
    {
        if(Vector3.Distance(transform.position, Player.transform.position) > checkRadius) { UpdatePath(); }
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Vector3.Distance(transform.position, Player.transform.position) > checkRadius ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        base.OnDrawGizmos();
    }

    public Vector3 GetRandomPositionWithinRadius(Vector3 center, float radius)
    {
        float u = Random.value;
        float v = Random.value;
        float theta = 2 * Mathf.PI * u;
        float phi = Mathf.Acos(2 * v - 1);

        float x = center.x + radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = center.y + radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = center.z + radius * Mathf.Cos(phi);

        return new Vector3(x, y, z);
    }
    private bool IsPositionChanging()
    {
        return Vector3.Distance(transform.position, lastPosition) >= positionChangeThreshold;
    }
}
