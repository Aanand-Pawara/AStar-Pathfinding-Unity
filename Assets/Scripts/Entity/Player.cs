using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : BaseAI
{
  
    private Coroutine followPath;

    

    private void Start()
    {
        UIManager.Instance.UpdatePosition(transform);
        OnDestinationReached.AddListener(UIManager.Instance.OnDestinationReached);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = UIManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                if (tileInfo != null)
                {
                    Vector3 startPos = transform.position;
                    Vector3 targetPos = tileInfo.Node.worldPosition;

                    Debug.Log("Clicked tile position: " + targetPos);


                    MakeCurrentNodeObstacle(false);
                    currentPath = Generator.Instance.mPathfinding.FindPath(startPos, targetPos);

                    if (currentPath != null)
                    {
                        Debug.Log("Path found from " + startPos + " to " + targetPos);
                        if (followPath != null) StopCoroutine(followPath);
                        followPath = StartCoroutine(FollowPath());
                    }
                    else
                    {
                        Debug.LogWarning("No path found from " + startPos + " to " + targetPos);
                    }
                    bool hasCurrentPath = currentPath != null;
                    UIManager.Instance.SetTileInfo(tileInfo, hasCurrentPath);
                }
            }
        }
    }

    
}
