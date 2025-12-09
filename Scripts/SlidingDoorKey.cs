using UnityEngine;
using TMPro;

public class SlidingDoorKey : MonoBehaviour
{
    [Header("Door Setup")]
    public Transform door;            // The actual door mesh
    public Transform openPosition;    // Empty object where the door should slide to
    public float openSpeed = 3f;      // How fast the door moves

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI promptText;
    [TextArea]
    public string promptMessage = "Press E to open / close";

    private bool playerInRange = false;
    private bool isOpen = false;

    private Vector3 closedPosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        if (door != null)
        {
            // Remember where the door starts = closed position
            closedPosition = door.position;
            targetPosition = closedPosition;
        }

        if (promptText != null)
        {
            promptText.text = promptMessage;
            promptText.enabled = false;   // start hidden
        }
    }

    private void Update()
    {
        // Move the door smoothly toward the target (open or closed)
        if (door != null)
        {
            door.position = Vector3.MoveTowards(
                door.position,
                targetPosition,
                openSpeed * Time.deltaTime
            );
        }

        // Handle E key when player is inside trigger
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        if (isOpen)
        {
            // Go back to the saved closed position
            targetPosition = closedPosition;
        }
        else
        {
            // Go to the open position
            if (openPosition != null)
                targetPosition = openPosition.position;
        }

        isOpen = !isOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (promptText != null)
            promptText.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (promptText != null)
            promptText.enabled = false;
    }
}