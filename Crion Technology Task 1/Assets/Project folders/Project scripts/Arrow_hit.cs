using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Arrow_hit : MonoBehaviour
{
    public float speed = 10f;
    public Transform Tip;
    private Rigidbody rb;
    public bool isFlying = false;
    private Vector3 lastPosition;

    public float rayDistance = 50f;
    private Vector3 aimPoint;

    public LineRenderer lineRenderer;

    public GameObject hitPointMarker;
    private RectTransform hitPointMarkerRect;

    public Transform bowTransform;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }

        if (hitPointMarker != null)
        {
            hitPointMarker.SetActive(false);
            hitPointMarkerRect = hitPointMarker.GetComponent<RectTransform>();
        }
    }

    void FixedUpdate()
    {
        if (!isFlying) return;

        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        RaycastHit hit;
        if (Physics.Raycast(Tip.position, rb.velocity.normalized, out hit, rb.velocity.magnitude * Time.fixedDeltaTime))
        {
            HandleCollision(hit);
        }

        lastPosition = Tip.position;
    }

    public void Fire(Vector3 direction, float pullValue)
    {
        isFlying = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(direction * (pullValue * speed), ForceMode.Impulse);
        lastPosition = Tip.position;
    }

    private void HandleCollision(RaycastHit hit)
    {
        if ( hit.collider.CompareTag("Bow") || hit.collider.CompareTag("Hand")) return;

        isFlying = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = hit.point;
        transform.rotation = Quaternion.LookRotation(-hit.normal);
        transform.parent = hit.transform;
    }

    void Update()
    {
        if (Tip != null && lineRenderer != null)
        {
            lineRenderer.SetPosition(0, Tip.position);
            RaycastHit hit;
            if (Physics.Raycast(Tip.position, Tip.forward, out hit, rayDistance))
            {
                aimPoint = hit.point;
                lineRenderer.SetPosition(1, aimPoint);

                if (hitPointMarker != null)
                {
                    hitPointMarker.SetActive(true);
                    Vector2 screenPoint = Camera.main.WorldToScreenPoint(aimPoint);
                    hitPointMarkerRect.position = screenPoint;
                }
            }
            else
            {
                aimPoint = Tip.position + Tip.forward * rayDistance;
                lineRenderer.SetPosition(1, aimPoint);

                if (hitPointMarker != null)
                {
                    hitPointMarker.SetActive(false);
                }
            }
        }
    }

    public void StickToObject(Collider other)
    {

        if (other.gameObject.CompareTag("Bow") || other.gameObject.CompareTag("Hand")) return;
            isFlying = false;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.position = Tip.position;
            transform.parent = other.transform;

            float distanceToHitObject = Vector3.Distance(transform.position, other.transform.position);
            Debug.Log("Distance from arrow to object it hit: " + distanceToHitObject);
            Debug.Log(other.gameObject.name);
            FindObjectOfType<Arrow_physics>().Distance.text = distanceToHitObject.ToString();
            FindObjectOfType<Arrow_physics>().objectHit.text = other.gameObject.name.ToString();
            Destroy(this);
    }

}
