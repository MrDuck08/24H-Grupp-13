using UnityEngine;

public class DeathCanvasManager : MonoBehaviour
{
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ControllCanvasOfAndOn(bool _bool)
    {
        transform.GetChild(0).gameObject.SetActive(_bool);
    }
}
