using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlacementScript : MonoBehaviour
{
    public Color greenColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);
    public Color redColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
    // public float opacity = 0.5f;

    public GameObject tower;
    public Tilemap map;
    public TileBase ground;

    private GameObject instance;
    private Vector3 mousePosition;
    private Vector3 pos;

    private PlayerInput PlayerControls;
    private InputAction shoot;

    private void OnEnable()
    {
        PlayerControls = new PlayerInput();
        shoot = PlayerControls.Player.Attack;
        shoot.Enable();
        shoot.performed += Fire;
    }
    
    private void OnDisable()
    {
        shoot.Disable();
    }
    
    private void Fire(InputAction.CallbackContext context)
    {
        // Place();
        Debug.Log("placed");
    }

    // Start is called before the first frame update
    void Start()
    {
        pos = GetPosition();
        instance = Instantiate(tower, new Vector3(pos.x, pos.y, 0), transform.rotation);
        CheckIfPlacable();
    }

    // Update is called once per frame
    void Update()
    {
        pos = GetPosition();
        instance.transform.position = new Vector3(pos.x, pos.y, 0);
        CheckIfPlacable();
    }

    private Vector3 GetPosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return map.GetCellCenterWorld(map.WorldToCell(mousePosition));
    }

    private bool CheckIfPlacable()
    {
        SpriteRenderer towersColor = instance.GetComponent<SpriteRenderer>();
        Vector3Int point = map.WorldToCell(pos);
        point.z = 0;
        if (map.GetTile(point) == ground)
        {
            towersColor.color = greenColor;
            return true;
        }
        towersColor.color = redColor;
        return false;
    }

    public void DeleteTower()
    {
        Destroy(instance);
    }
}
