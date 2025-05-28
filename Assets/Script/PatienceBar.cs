using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [SerializeField]
    Image bar;

    [SerializeField]
    GameObject fingerObject;

    [SerializeField]
    float startSpeed;

    [SerializeField]
    float acceleration;

    [SerializeField]
    float fingerStart;

    [SerializeField]
    float fingerEnd;

    [SerializeField]
    float patience;
    [SerializeField] float speed;
    bool isDraining;

    DialogeSystem DialogeSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDraining = false;
        patience = 1.0f;
        speed = startSpeed;

        DialogeSystem = FindObjectOfType<DialogeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Patience
        if(patience <= 0)
        {

            DialogeSystem.Lose();

            bar.transform.localScale = new Vector3(patience, 1, 1);
            bar.color = new Color(1.0f - patience, patience, 0);
            fingerObject.transform.position = new Vector3(-math.lerp(fingerEnd, fingerStart, patience) / (2 * fingerObject.transform.lossyScale.x), fingerObject.transform.position.y, fingerObject.transform.position.z);

            return;


        }

        if (isDraining == true)
        {
            patience -= speed * Time.deltaTime;
            patience = math.clamp(patience, 0.0f, 1.0f);
        }

        // UI
        bar.transform.localScale = new Vector3(patience, 1, 1);
        bar.color = new Color(1.0f - patience, patience, 0);
        fingerObject.transform.position = new Vector3(-math.lerp(fingerEnd, fingerStart, patience) / (2 * fingerObject.transform.lossyScale.x), fingerObject.transform.position.y, fingerObject.transform.position.z);
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
        patience -= newValue;
        patience = math.clamp(patience, 0.0f, 1.0f);
    }

    public void EnableDraining()
    {
        isDraining = true;
    }

    public void DisableDraining()
    {
        isDraining = false;
    }
}

