using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow_physics : MonoBehaviour
{
    public float speed = 10f;
    public Transform Tip;
    private bool isFlying = false;
    private Vector3 lastPosition;

    public GameObject arrowPrefab;
    public Transform notch;
    public XRGrabInteractable bow;
    private bool arrowNotched = false;
    private GameObject currentArrow = null;
    public TMP_Text Distance;
    public TMP_Text objectHit;
    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        Bow_aim_script.BowReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        Bow_aim_script.BowReleased -= NotchEmpty;
    }

    private void Update()
    {
        if (bow.isSelected && !arrowNotched)
        {
            arrowNotched = true;
            StartCoroutine(DelayedSpawn());
        }

        if (!bow.isSelected && currentArrow != null)
        {
            Destroy(currentArrow);
        }
    }

    private void FixedUpdate()
    {
        if (!isFlying || currentArrow == null) return;

        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            currentArrow.transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        if (Physics.Linecast(lastPosition, Tip.position))
        {
            StopArrow();
        }

        lastPosition = Tip.position;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        currentArrow = Instantiate(arrowPrefab, notch.position, notch.rotation, notch);
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void NotchEmpty(float value)
    {
        arrowNotched = false;
        currentArrow = null;
    }

    public void Fire(float pullValue)
    {
        if (currentArrow == null) return;

        isFlying = true;
        currentArrow.transform.parent = null;
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.forward * (pullValue * speed), ForceMode.Impulse);
        lastPosition = Tip.position;
    }

    private void StopArrow()
    {
        isFlying = false;
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}
