using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [SerializeField]
    Image bar;

    [SerializeField]
    float speed;

    float patience;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        patience = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Patience
        patience -= speed * Time.deltaTime;
        patience = math.clamp(patience, 0.0f, 1.0f);

        // UI
        bar.transform.localScale = new Vector3(patience, 1, 1);
        bar.color = new Color(1.0f - patience, patience, 0);
    }

    /// <summary>
    /// Add patience to the president.
    /// Patience corresponds to a value between 0.0 and 1.0 
    /// </summary>
    /// <param name="newValue">the value to add</param>
    public void addAmount(float newValue)
    {
        patience += newValue;
    }

    /// <summary>
    /// Subtract patience from the president.
    /// Patience corresponds to a value between 0.0 and 1.0 
    /// </summary>
    /// <param name="newValue">the value to subtract</param>
    public void subtractAmount(float newValue)
    {
        patience += newValue;
    }
}
