using UnityEngine;
using UnityEngine.InputSystem;

public class Goalkeeper : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform body;
    public float moveSpeed = 5f;
    public float handSpeed = 10f;

    public InputActionAsset inputActionAsset;

    private InputAction leftHandPositionAction;
    private InputAction rightHandPositionAction;
    private InputAction headPositionAction;
    private int shotsTaken = 0;
    private const int totalShotsPerRound = 10;

    private void Start()
    {
        // Initialize the Input Actions for tracking positions
        var inputActions = inputActionAsset.FindActionMap("XRI LeftHand");
        leftHandPositionAction = inputActions.FindAction("Position");

        inputActions = inputActionAsset.FindActionMap("XRI RightHand");
        rightHandPositionAction = inputActions.FindAction("Position");

        inputActions = inputActionAsset.FindActionMap("XRI HMD");
        headPositionAction = inputActions.FindAction("Position");

        // Enable the input actions
        leftHandPositionAction.Enable();
        rightHandPositionAction.Enable();
        headPositionAction.Enable();
    }

    private void Update()
    {
        // Update hand and body positions based on VR input
        UpdateHandPositions();
        UpdateBodyPosition();

        // For testing purposes: simulate a shot being taken when pressing the space key
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SimulateShot();
        }
    }

    private void UpdateHandPositions()
    {
        if (leftHandPositionAction != null)
        {
            leftHand.position = leftHandPositionAction.ReadValue<Vector3>();
        }

        if (rightHandPositionAction != null)
        {
            rightHand.position = rightHandPositionAction.ReadValue<Vector3>();
        }
    }

    private void UpdateBodyPosition()
    {
        if (headPositionAction != null)
        {
            body.position = headPositionAction.ReadValue<Vector3>();
        }
    }

    public void RecordShot(string goalPosition, int score, float reflectTime, float errorDistance)
    {
        FindObjectOfType<GameController>().RecordRoundData(goalPosition, score, reflectTime, errorDistance);
        shotsTaken++;

        if (shotsTaken >= totalShotsPerRound)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        shotsTaken = 0; // Reset shots for the next round
        // Notify GameController to handle end of the round
        FindObjectOfType<GameController>().EndRound();
    }

    // Example method to simulate a shot being taken
    public void SimulateShot()
    {
        // These values should be dynamically set based on game logic
        string goalPosition = GetRandomGoalPosition(); // Example, should be set based on actual shot logic
        int score = Random.Range(0, 2); // Random score of 0 or 1 for simplicity
        float reflectTime = Random.Range(0.5f, 1.5f); // Example reflect time
        float errorDistance = Random.Range(0f, 1f); // Example error distance

        RecordShot(goalPosition, score, reflectTime, errorDistance);
    }

    // Example method to get a random goal position for simulation
    private string GetRandomGoalPosition()
    {
        string[] positions = { "Top Right", "Top Left", "Top Middle", "Bottom Right", "Bottom Left", "Bottom Middle" };
        return positions[Random.Range(0, positions.Length)];
    }
}
