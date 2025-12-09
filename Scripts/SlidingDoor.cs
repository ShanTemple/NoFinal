using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Setup")]
    public Transform door;          // The actual door object that moves
    public Transform openPosition;  // Where the door moves to when open
    public float openSpeed = 3f;

    [Header("Interaction")]
    public bool autoOpen = true;    // Automatically opens when player enters
    public bool useKey = false;     // If true, press key to toggle open/close
    public KeyCode interactKey = KeyCode.E;

    private Vector3 closedPosition;
    private bool playerInside = false;
    private bool isOpen = false;

    void Start()
    {
        if (door == null)
        {
            door = transform;
        }

        closedPosition = door.position;
    }

    void Update()
    {
        if (useKey && playerInside && Input.GetKeyDown(interactKey))
        {
            isOpen = !isOpen;
        }

        if (autoOpen)
        {
            isOpen = playerInside;
        }

        Vector3 target = isOpen ? openPosition.position : closedPosition;
        door.position = Vector3.Lerp(door.position, target, Time.deltaTime * openSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
