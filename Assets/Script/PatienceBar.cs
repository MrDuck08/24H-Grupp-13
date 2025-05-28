using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [SerializeField]
    Image bar;

    [SerializeField]
    float startSpeed;

    [SerializeField]
    float acceleration;

    float patience;
    float speed;
    bool isDraining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDraining = false;
        patience = 1.0f;
        speed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Patience
        if (isDraining == true)
        {
            patience -= speed * Time.deltaTime;
            patience = math.clamp(patience, 0.0f, 1.0f);
            speed += acceleration;
        }

        // UI
        bar.transform.localScale = new Vector3(patience, 1, 1);
        bar.color = new Color(1.0f - patience, patience, 0);
    }

    /// <summary>
    /// Add patience to the president.
    /// Patience corresponds to a value between 0.0 and 1.0 
    /// </summary>
    /// <param name="newValue">the value to add</param>
    public void AddAmount(float newValue)
    {
        patience += newValue;
        patience = math.clamp(patience, 0.0f, 1.0f);
    }

    /// <summary>
    /// Subtract patience from the president.
    /// Patience corresponds to a value between 0.0 and 1.0 
    /// </summary>
    /// <param name="newValue">the value to subtract</param>
    public void SubtractAmount(float newValue)
    {
        patience += newValue;
        patience = math.clamp(patience, 0.0f, 1.0f);
    }

    public void EnableDraining()
    {
        isDraining = true;
    }

    public void DisableDraining()
    {
        isDraining = true;
    }
}
