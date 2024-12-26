using Unity.VisualScripting;
using UnityEngine;

public class Arrow_tip : MonoBehaviour
{
    private Arrow_hit arrowScript; 

    void Start()
    {
        arrowScript = GetComponentInParent<Arrow_hit>();
        if (arrowScript == null)
        {
            Debug.LogError("Arrow_hit script not found in parent!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (arrowScript != null)
        {
            arrowScript.StickToObject(other);
            Debug.Log("Arrow hit");
        }
    }
}
