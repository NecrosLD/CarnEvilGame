using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiBall : MonoBehaviour
{
    private bool isPressed;

    private float releaseDelay;
    private float maxDragDistance = 2f;

    private Rigidbody2D rb;
    private SpringJoint2D sj;
    private Rigidbody2D slingRb;
    private LineRenderer lr;
    private TrailRenderer tr;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();
        slingRb = sj.connectedBody;
        lr = GetComponent<LineRenderer>();
        tr = GetComponent<TrailRenderer>();

        lr.enabled = false;
        tr.enabled = false;

        releaseDelay = 1 / (sj.frequency * 4);

    }

    // Update is called once per frame
    void Update() {
        if (isPressed)
        {
            DragBall();
        }
    }
    //this is the "power" bar for how far you can drag the ball
    private void DragBall() {
        SetlineRendererPositions();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousePosition, slingRb.position);


        //Above lets the player click and drag the ball
        //Below locks how far the mouse can drag the ball

        if(distance > maxDragDistance) {
            Vector2 direction = (mousePosition - slingRb.position).normalized;
            rb.position = slingRb.position + direction * maxDragDistance;
        } else {
            rb.position = mousePosition;
        }

        
    }

    //this is the trail
    private void SetlineRendererPositions() {
        Vector3[] positions = new Vector3[2];
        positions[0] = rb.position;
        positions[1] = slingRb.position;
        lr.SetPositions(positions);
    }

    private void OnMouseDown()  {
        isPressed = true;
        rb.isKinematic = true;
        lr.enabled = true;
        tr.enabled = false;
    }

    private void OnMouseUp() {
        isPressed = false;
        rb.isKinematic = false;
        StartCoroutine(Release());
        lr.enabled = false;
        tr.enabled = true;
    }
    
    private IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseDelay);
        sj.enabled = false;
    }

}
