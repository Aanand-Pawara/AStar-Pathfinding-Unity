using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text m_positionText;
    [SerializeField] private TMP_Text m_tileInfoText;

    [SerializeField] private SpriteRenderer TilePin;

    public static UIManager Instance;

    public Camera MainCamera;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }



    }
    private void Start()
    {
        
        if(MainCamera == null) MainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_positionText != null && Target != null)
        {
            m_positionText.text = $"X : {Target.transform.position.x} Y : {Target.transform.position.y} Z : {Target.transform.position.z}";
        }
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                SetTileInfo(tileInfo);
            }
        }
    }

    private Transform Target;
    public void UpdatePosition(Transform target)
    {
        this.Target = target;
    }
    public void SetTileInfo(TileInfo tileInfo, bool Walkable)
    {
        if (m_tileInfoText != null && tileInfo != null)
        {
            TilePin.gameObject.SetActive(true);
            Vector3 pinPosition = tileInfo.Node.worldPosition + Vector3.up * 1f;
            TilePin.transform.position = pinPosition;
            Vector3 directionToCamera = MainCamera.transform.position - TilePin.transform.position;

            directionToCamera.y = 0;
            Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);


            TilePin.transform.rotation = rotationToCamera;


            if (Walkable)
            {
                TilePin.color = Color.green;
            }
            else
            {
                TilePin.color = Color.red;
            }
        }
    }
    public void SetTileInfo(TileInfo tileInfo)
    {
        if (m_tileInfoText != null && tileInfo != null)
        {
            m_tileInfoText.text = $"Type : {tileInfo.tileType} IsObstacle : {tileInfo.Node.IsObstacle}";
        }
    }
    public void OnDestinationReached()
    {
        TilePin.gameObject.SetActive(false);
    }
}
