using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;

    [SerializeField] GameObject arrow;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;

    [SerializeField] AudioSource ballHitAudio;

    [SerializeField] TMP_Text shootCountText;

    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;

    [SerializeField] Transform cameraPivot;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camSensitivity;

    [SerializeField] float shootForce;

    Vector3 lastMousePosition;
    float ballDistance;
    bool isShooting;

    Vector3 forceDir;
    float forceFactor;

    Renderer[] arrowRends;
    Color[] arrowOriginalColors;

    int shootCount = 0;

    public int ShootCount { get => shootCount; }

    void Start()
    {
        ballDistance = Vector3.Distance(cam.transform.position, ball.Position) + 1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];

        for(int i = 0; i < arrowRends.Length; i++)
        {
            arrowOriginalColors[i] = arrowRends[i].material.color;
        }

        arrow.SetActive(false);
        shootCountText.text = shootCount.ToString();
    }

    void Update()
    {
        if (ball.IsMoving || ball.IsTeleporting || ball.IsEnteringHole)
            return;

        if (this.transform.position != ball.Position)
            this.transform.position = ball.Position;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, ballDistance, ballLayer))
            {
                isShooting = true;
                arrow.SetActive(true);
                aim.gameObject.SetActive(true);
                line.enabled = true;
            }
        }

        // Shooting mode
        if (Input.GetMouseButton(0) && isShooting == true)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, ballDistance * 2, rayLayer))
            {
                Debug.DrawLine(ball.Position, hit.point);

                var forceVector = ball.Position - hit.point;
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;

                //Debug.Log(forceMagnitude);

                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 3);
                forceFactor = forceMagnitude / 5;
            }

            this.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale = new Vector3(1 + 0.5f * forceFactor, 1 + 0.5f * forceFactor, 0.75f + 5 * forceFactor);

            for(int i = 0; i < arrowRends.Length; i++)
            {
                arrowRends[i].material.color = Color.Lerp(arrowOriginalColors[i], Color.red, forceFactor * 2.5f);
            }

            // aim
            var rect = aim.GetComponent<RectTransform>();            
            rect.anchoredPosition = Input.mousePosition;

            // line
            var ballScrPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[] { ballScrPos, Input.mousePosition });
        }

        // Camera mode
        if (Input.GetMouseButton(0) && isShooting == false)
        {
            var delta = Input.mousePosition - lastMousePosition;

            // rotate horizontal
            cameraPivot.transform.RotateAround(ball.Position, Vector3.up, delta.x * camSensitivity.x);

            // rotate vertical
            cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, -delta.y * camSensitivity.y);

            var angle = Vector3.SignedAngle(Vector3.up, cam.transform.up, cam.transform.right);

            if (angle < 3)
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 3 - angle);
            else if (angle > 75)
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 75 - angle);
        }

        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            ballHitAudio.Play(0);
            ball.AddForce(forceDir * shootForce * forceFactor);
            shootCount += 1;
            shootCountText.text = shootCount.ToString();
            forceFactor = 0;
            forceDir = Vector3.zero;
            isShooting = false;
            arrow.SetActive(false);
            aim.gameObject.SetActive(false);
            line.enabled = false;
        }

        lastMousePosition = Input.mousePosition;
    }
}