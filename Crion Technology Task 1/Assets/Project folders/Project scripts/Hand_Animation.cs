using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand_Animation : MonoBehaviour
{
    [SerializeField] public InputActionProperty Trigger;
    [SerializeField] public InputActionProperty Grip;

    public Animator animator;
    void Start()
    {
        
    }
    void Update()
    {
        float triggerVal = Trigger.action.ReadValue<float>();
        float gripVal = Grip.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerVal);
        animator.SetFloat("Grip", gripVal);
    }
}
