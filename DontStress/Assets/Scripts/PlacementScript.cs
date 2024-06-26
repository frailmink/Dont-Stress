using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class PlacementScript : MonoBehaviour
{
    public static bool placed;

    public Color greenColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);
    public Color redColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
    private Color originalColor;
    // public float opacity = 0.5f;

    public GameObject tower;
    public Tilemap map;
    public TileBase ground;
    public TileBase taken;

    private GameObject instance;
    private Vector3 mousePosition;
    private Vector3 pos;

    private PlayerInput PlayerControls;
    private InputAction shoot;

    private TowerScript script;

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
        Place();
    }

    // Start is called before the first frame update
    void Start()
    {
        placed = false;
        pos = GetPosition();
        instance = Instantiate(tower, new Vector3(pos.x, pos.y, 0), transform.rotation);
        originalColor = instance.GetComponent<SpriteRenderer>().color;
        instance.GetComponent<BoxCollider2D>().enabled = false;
        script = instance.GetComponentInChildren<TowerScript>();
        script.DisableScript();
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
        if (instance == null)
        {
            return false;
        }

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

    private void Place()
    {
        if (instance == null)
        {
            return;
        }

        bool placable = CheckIfPlacable();
        if (placable)
        {
            Vector3Int point = map.WorldToCell(pos);
            point.z = 0;
            map.SetTile(point, taken);
            instance.GetComponent<SpriteRenderer>().color = originalColor;
            instance.GetComponent<BoxCollider2D>().enabled = true;
            script.EnableScript();
            placed = true;
            GlobalVariables.SetBuildingMode(false);
            Destroy(this.gameObject);
        }
    }

    public void DeleteTower()
    {
        Destroy(instance);
    }
}
