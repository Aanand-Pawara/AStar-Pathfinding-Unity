using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseAI : MonoBehaviour
{
    [SerializeField] protected List<Vector3> currentPath;
    [SerializeField] protected bool isMoving;


    public UnityEvent OnDestinationReached;
    public UnityEvent OnNewDestinationSelected;

    protected IEnumerator FollowPath()
    {
        isMoving = true;
        OnNewDestinationSelected?.Invoke();
        while (currentPath != null && currentPath.Count > 0)
        {
            Vector3 waypoint = currentPath[0];
            Vector3 targetPosition = new Vector3(waypoint.x, waypoint.y + 1.5f, waypoint.z);

            bool pathBlocked = false;

            for (int i = 1; i < Mathf.Min(5, currentPath.Count); i++)
            {
                if (Generator.Instance.mGrid.GetNode(currentPath[i]).IsObstacle)
                {
                    pathBlocked = true;
                    break;
                }
            }

            if (pathBlocked)
            {
                MakeCurrentNodeObstacle(false);
                currentPath = Generator.Instance.mPathfinding.FindPath(transform.position, currentPath[currentPath.Count - 1]);
                if (currentPath == null)
                {
                    Debug.Log($"{gameObject.name} No new path found.");
                    isMoving = false;
                    break;
                }

                Debug.Log($"{gameObject.name} Finding a new path because the current path has an obstacle");
                continue;
            }

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Generator.Instance.DELTA_TIME * 5);
                yield return null;
            }

            transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, waypoint.y + 0.5f), transform.position.z);
            if (currentPath != null && currentPath.Count > 0)
            {
                currentPath.RemoveAt(0);
            }

           
        }

        MakeCurrentNodeObstacle(true);
        isMoving = (currentPath != null && currentPath.Count > 0);
        ClearPath();
        OnDestinationReached?.Invoke();
    }


    protected void MakeCurrentNodeObstacle(bool value)
    {
        Generator.Instance.mGrid.GetNode(transform.position).SetObstacle(value);
    }

    protected void ClearPath()
    {
        if (currentPath != null)
        {
            currentPath.Clear();
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    protected virtual void OnDrawGizmos()
    {
        if (currentPath != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
            }

            Gizmos.color = Color.red;
            foreach (Vector3 waypoint in currentPath)
            {
                Gizmos.DrawSphere(waypoint, 0.2f);
            }
        }
    }
}
