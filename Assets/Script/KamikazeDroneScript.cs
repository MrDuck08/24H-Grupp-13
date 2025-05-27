using UnityEngine;
// They will grow and get closer to the player faster as the game progress
// They will move slighly faster as the game progress but won't move way to fast
public class KamikazeDroneScript : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float horizontalMoveSpeed = 3f;
    [SerializeField] float verticalMoveSpeed = 3f;
    [SerializeField] float minChangeDirectionTime = 1f;
    [SerializeField] float maxChangeDirectionTime = 4f;

    [Header("Consecutive Direction Limits")]
    [SerializeField] int maxConsecutiveHorizontal = 3;
    [SerializeField] int maxConsecutiveVertical = 3;

    int horizontalDirection = 0;
    int verticalDirection = 0;

    float nextChangeDirectionTime;

    const float DRONE_MOVEABLE_WORLD_WIDTH = 15f;
    const float DRONE_MOVEABLE_WORLD_HEIGHT = 8f;

    float minXPos, maxXPos, minYPos, maxYPos;

    float droneHalfWidth;
    float droneHalfHeight;

    SpriteRenderer droneSprite;

    enum MovementAxis { None, Horizontal, Vertical }
    MovementAxis lastChosenAxis = MovementAxis.None;
    int consecutiveHorizontalCount = 0;
    int consecutiveVerticalCount = 0;

    void Start()
    {
        droneSprite = GetComponentInChildren<SpriteRenderer>();
        if (droneSprite == null)
        {
            Debug.LogError("KamikazeDroneScript requires a SpriteRenderer component on this GameObject!");
            return;
        }

        droneHalfWidth = droneSprite.bounds.extents.x;
        droneHalfHeight = droneSprite.bounds.extents.y;

        minXPos = -(DRONE_MOVEABLE_WORLD_WIDTH / 2) + droneHalfWidth;
        maxXPos = (DRONE_MOVEABLE_WORLD_WIDTH / 2) - droneHalfWidth;

        minYPos = -(DRONE_MOVEABLE_WORLD_HEIGHT / 2) + droneHalfHeight;
        maxYPos = (DRONE_MOVEABLE_WORLD_HEIGHT / 2) - droneHalfHeight;

        SetNextChangeDirectionTime();
        ChooseRandomDirections();

        Debug.Log($"Drone Moveable Bounds: X({minXPos}, {maxXPos}), Y({minYPos}, {maxYPos})");
    }

    void Update()
    {
        Movement();
        CheckForDirectionChange();
    }

    void Movement()
    {
        Vector2 targetPosition = transform.position;
        targetPosition.x += horizontalDirection * horizontalMoveSpeed * Time.deltaTime;
        targetPosition.y += verticalDirection * verticalMoveSpeed * Time.deltaTime;

        bool hitBoundary = false;

        if (targetPosition.x < minXPos)
        {
            targetPosition.x = minXPos;
            horizontalDirection *= -1;
            hitBoundary = true;
        }
        else if (targetPosition.x > maxXPos)
        {
            targetPosition.x = maxXPos;
            horizontalDirection *= -1;
            hitBoundary = true;
        }

        if (targetPosition.y < minYPos)
        {
            targetPosition.y = minYPos;
            verticalDirection *= -1;
            hitBoundary = true;
        }
        else if (targetPosition.y > maxYPos)
        {
            targetPosition.y = maxYPos;
            verticalDirection *= -1;
            hitBoundary = true;
        }

        transform.position = targetPosition;

        if (hitBoundary)
        {
            SetNextChangeDirectionTime();
        }
    }

    void CheckForDirectionChange()
    {
        if (Time.time >= nextChangeDirectionTime)
        {
            ChooseRandomDirections();
            SetNextChangeDirectionTime();
        }
    }

    void ChooseRandomDirections()
    {
        horizontalDirection = 0;
        verticalDirection = 0;

        bool canChooseHorizontal = true;
        bool canChooseVertical = true;

        if (lastChosenAxis == MovementAxis.Horizontal && consecutiveHorizontalCount >= maxConsecutiveHorizontal)
        {
            canChooseHorizontal = false;
            Debug.Log("Horizontal limit reached. Forcing Vertcial.");
        }
        else if (lastChosenAxis == MovementAxis.Vertical && consecutiveVerticalCount >= maxConsecutiveVertical)
        {
            canChooseVertical = false;
            Debug.Log("Vertical limit reached. Forcing Horizontal.");
        }

        MovementAxis chosenAxis;

        if (!canChooseHorizontal && !canChooseVertical)
        {
            Debug.LogWarning("Both consecutive movement limits reached simultaneously. Forcing opposite direction.");
            if (lastChosenAxis == MovementAxis.Horizontal) chosenAxis = MovementAxis.Vertical;
            else chosenAxis = MovementAxis.Horizontal;
        }
        else if (!canChooseHorizontal)
        {
            chosenAxis = MovementAxis.Vertical;
        }
        else if (!canChooseVertical)
        {
            chosenAxis = MovementAxis.Horizontal;
        }
        else
        {
            chosenAxis = (Random.Range(0, 2) == 0) ? MovementAxis.Horizontal : MovementAxis.Vertical;
        }

        if (chosenAxis == MovementAxis.Horizontal)
        {
            horizontalDirection = Random.Range(0, 2) * 2 - 1;
            lastChosenAxis = MovementAxis.Horizontal;
            consecutiveHorizontalCount++;
            consecutiveVerticalCount = 0;
            Debug.Log($"New Directions: Purely Horizontal = {horizontalDirection}. " +
                $"Consecutive H: {consecutiveHorizontalCount}/{maxConsecutiveHorizontal}");
        }
        else
        {
            verticalDirection = Random.Range(0, 2) * 2 - 1;
            lastChosenAxis = MovementAxis.Vertical;
            consecutiveVerticalCount++;
            consecutiveHorizontalCount = 0;
            Debug.Log($"New Directions: Purely Vertical = {verticalDirection}. " +
                $"Consecutive V: {consecutiveVerticalCount}/{maxConsecutiveVertical}");
        }
    }

    void SetNextChangeDirectionTime()
    {
        float randomInterval = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
        nextChangeDirectionTime = Time.time + randomInterval;

        Debug.Log($"Next direction change in: {randomInterval} seconds (at Time.time {nextChangeDirectionTime})");
    }
}
