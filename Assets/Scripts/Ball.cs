using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] AudioSource outBoundaryAudio;
    
    public Vector3 Position => rb.position;
    public bool IsMoving => rb.velocity != Vector3.zero;

    private bool isEnteringHole = false;
    public bool IsEnteringHole { get => isEnteringHole; set => isEnteringHole = value; }

    [SerializeField] Vector3 lastPosition;

    bool isTeleporting;
    public bool IsTeleporting => isTeleporting;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        lastPosition = this.transform.position;
    }

    internal void AddForce(Vector3 force)
    {
        rb.isKinematic = false;
        lastPosition = this.transform.position;
        rb.AddForce(force, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero && rb.velocity.magnitude < 0.5f)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Out")
        {
            StopAllCoroutines();
            StartCoroutine(DelayedTeleport());
        }
    }

    IEnumerator DelayedTeleport()
    {
        outBoundaryAudio.Play(0);
        isTeleporting = true;
        yield return new WaitForSeconds(0);
        this.transform.position = lastPosition;
        isTeleporting = false;
    }
}
