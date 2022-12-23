using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    public Canvas inventory;

    [HideInInspector] public int speed;

    Player player;

    private Camera mainCamera;

    Rigidbody rb;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Start()
    {   
        inventory.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Aim();
        if (Input.GetKeyDown(KeyCode.F))
            player.fPressed = true;
        else
            player.fPressed = false;
        if (Input.GetKeyDown(KeyCode.I) && inventory.gameObject.activeSelf == true)
            inventory.gameObject.SetActive(false);
        else if (Input.GetKeyDown(KeyCode.I) && inventory.gameObject.activeSelf == false)
            inventory.gameObject.SetActive(true);
    }

    public void setSpeed(int newSpeed) => speed = newSpeed;

    private void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, -0.25f, zDirection);

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        rb.velocity = moveDirection * speed * Time.deltaTime * 75;
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
}