using System.Collections;
using UnityEngine;

public class KamikazeDroneScript : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float horizontalMoveSpeed = 3f;
    [SerializeField] float verticalMoveSpeed = 3f;
    [SerializeField] float minChangeDirectionTime = 1f;
    [SerializeField] float maxChangeDirectionTime = 4f;
    [SerializeField] AudioSource droneMovingAudioSource;
    [SerializeField] AudioClip movingDroneSound;

    [Header("Consecutive Direction Limits")]
    [SerializeField] int maxConsecutiveHorizontal = 3;
    [SerializeField] int maxConsecutiveVertical = 3;

    [Header("Scaling Effect")]
    [SerializeField] float startScale = 0.35f;
    [SerializeField] float endScale = 2f;
    [SerializeField] float scaleDuration = 30f;
    [SerializeField] float warningTime = 3f;

    [Header("Destruction Settings")]
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] float destroyDelay = 15f;
    [SerializeField] float DroneParticalEffectTime = 0.3f;
    [SerializeField] GameObject droneParticleEffect, electicEffect, explotionEffect;
    [SerializeField] float blinkTime = 0.2f;
    [SerializeField] float deathCanvasDelay = 2f;

    [Header("Animation Settings")]
    [SerializeField] Sprite droneSpriteFrame1;
    [SerializeField] Sprite droneSpriteFrame2;
    [SerializeField] float animationFrameDelay = 0.15f;

    int horizontalDirection = 0;
    int verticalDirection = 0;

    float nextChangeDirectionTime;

    const float DRONE_MOVEABLE_WORLD_WIDTH = 15f;
    const float DRONE_MOVEABLE_WORLD_HEIGHT = 8f;

    float minXPos, maxXPos, minYPos, maxYPos;

    float droneHalfWidth;
    float droneHalfHeight;

    float currentScaleTime = 0f;

    bool isDestroyed = false;
    bool isGoingToExplode = false;
    bool isBlinking = false;
    bool isAnimating = true;

    SpriteRenderer droneSprite;

    SoundManager soundManager;
    DeathCanvasManager deathCanvasManager;

    enum MovementAxis { None, Horizontal, Vertical }
    MovementAxis lastChosenAxis = MovementAxis.None;
    int consecutiveHorizontalCount = 0;
    int consecutiveVerticalCount = 0;

    void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();

        droneMovingAudioSource = GetComponent<AudioSource>();

        if (droneMovingAudioSource != null && movingDroneSound != null)
        {
            droneMovingAudioSource.clip = movingDroneSound;
            droneMovingAudioSource.loop = true;
        }

        droneSprite = GetComponentInChildren<SpriteRenderer>();
        if (droneSprite == null)
        {
            Debug.LogWarning("KamikazeDroneScript requires a SpriteRenderer component on this GameObject!");
            return;
        }

        if (droneSpriteFrame1 != null && droneSpriteFrame2 != null)
        {
            StartCoroutine(AnimateDroneSprite());
        }
        else
        {
            Debug.LogWarning("Drone animation sprites not assigned. Animation will not play.");
        }

        deathCanvasManager = FindAnyObjectByType<DeathCanvasManager>();

        droneParticleEffect.SetActive(false);
        electicEffect.SetActive(false);

        droneHalfWidth = droneSprite.bounds.extents.x;
        droneHalfHeight = droneSprite.bounds.extents.y;

        minXPos = -(DRONE_MOVEABLE_WORLD_WIDTH / 2) + droneHalfWidth;
        maxXPos = (DRONE_MOVEABLE_WORLD_WIDTH / 2) - droneHalfWidth;

        minYPos = -(DRONE_MOVEABLE_WORLD_HEIGHT / 2) + droneHalfHeight;
        maxYPos = (DRONE_MOVEABLE_WORLD_HEIGHT / 2) - droneHalfHeight;

        SetNextChangeDirectionTime();
        ChooseRandomDirections();

        transform.localScale = new Vector3(startScale, startScale, 1);

        //Debug.Log($"Drone Moveable Bounds: X({minXPos}, {maxXPos}), Y({minYPos}, {maxYPos})");
    }

    void Update()
    {
        if (!isDestroyed)
        {
            Movement();
            CheckForDirectionChange();
            HandleScaling();

            if (droneMovingAudioSource != null && !droneMovingAudioSource.isPlaying)
            {
                droneMovingAudioSource.Play();
            }
        }
        else
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (droneMovingAudioSource != null && droneMovingAudioSource.isPlaying)
            {
                droneMovingAudioSource.Stop();
            }
        }
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
            //Debug.Log("Horizontal limit reached. Forcing Vertcial.");
        }
        else if (lastChosenAxis == MovementAxis.Vertical && consecutiveVerticalCount >= maxConsecutiveVertical)
        {
            canChooseVertical = false;
            //Debug.Log("Vertical limit reached. Forcing Horizontal.");
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
            /*Debug.Log($"New Directions: Purely Horizontal = {horizontalDirection}. " +
                $"Consecutive H: {consecutiveHorizontalCount}/{maxConsecutiveHorizontal}");*/
        }
        else
        {
            verticalDirection = Random.Range(0, 2) * 2 - 1;
            lastChosenAxis = MovementAxis.Vertical;
            consecutiveVerticalCount++;
            consecutiveHorizontalCount = 0;
            /*Debug.Log($"New Directions: Purely Vertical = {verticalDirection}. " +
                $"Consecutive V: {consecutiveVerticalCount}/{maxConsecutiveVertical}");*/
        }
    }

    void SetNextChangeDirectionTime()
    {
        float randomInterval = Random.Range(minChangeDirectionTime, maxChangeDirectionTime);
        nextChangeDirectionTime = Time.time + randomInterval;

        //Debug.Log($"Next direction change in: {randomInterval} seconds (at Time.time {nextChangeDirectionTime})");
    }

    void HandleScaling()
    {
        currentScaleTime += Time.deltaTime;

        float t = Mathf.Clamp01(currentScaleTime / scaleDuration);

        float newScale = Mathf.Lerp(startScale, endScale, t);

        transform.localScale = new Vector3(newScale, newScale, 1);

        if (newScale >= endScale)
        {
            warningTime -= Time.deltaTime;

            if (!isBlinking)
            {
                isBlinking = true;
                soundManager.PlaySound(soundManager.countdownSound);
                StartCoroutine(BlinkingCountdown());
            }

            if (warningTime <= 0 && !isGoingToExplode)
            {
                isGoingToExplode = true;
                StartCoroutine(DroneExplode());
            }
        }
    }

    IEnumerator BlinkingCountdown()
    {
        while (!isGoingToExplode)
        {
            droneSprite.color = Color.red;

            yield return new WaitForSeconds(blinkTime);

            droneSprite.color = Color.white;

            yield return new WaitForSeconds(blinkTime);
        }
    }
    IEnumerator DroneExplode()
    {
        Debug.Log("THE DRONE HAS EXPLODED!!!!!");

        isDestroyed = true;

        soundManager.PlaySound(soundManager.explosionSound);

        Instantiate(explotionEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(deathCanvasDelay);

        deathCanvasManager.ControllCanvasOfAndOn(true);
        Time.timeScale = 0;
    }

    public void DroneDestroyed()
    {
        if (isDestroyed) return;

        isDestroyed = true;

        isAnimating = false;
        StopCoroutine(AnimateDroneSprite());

        if (droneParticleEffect != null)
        {
            StartCoroutine(DroneParticalEffectTimmer());
        }

        if (electicEffect != null)
        {
            electicEffect.SetActive(true);
        }

        soundManager.PlaySound(soundManager.droneDestructionSound);

        Destroy(gameObject, destroyDelay);
    }

    IEnumerator DroneParticalEffectTimmer()
    {
        droneParticleEffect.SetActive(true);
        yield return new WaitForSeconds(DroneParticalEffectTime);
        droneParticleEffect.SetActive(false);
    }

    IEnumerator AnimateDroneSprite()
    {
        while (isAnimating)
        {
            if (droneSprite != null && droneSpriteFrame1 != null)
            {
                droneSprite.sprite = droneSpriteFrame1;
            }

            yield return new WaitForSeconds(animationFrameDelay);

            if (droneSprite != null && droneSpriteFrame2 != null)
            {
                droneSprite.sprite = droneSpriteFrame2;
            }

            yield return new WaitForSeconds(animationFrameDelay);
        }
    }
}
