using UnityEngine;
using FORGE3D;

public class TurretRotation : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] TurretTrackingType TrackingType;

    [SerializeField] GameObject Mount;
    [SerializeField] GameObject Swivel;

    [SerializeField] float HeadingTrackingSpeed = 2f;
    [SerializeField] float ElevationTrackingSpeed = 2f;
    [SerializeField] Vector2 HeadingLimit;
    [SerializeField] Vector2 ElevationLimit;

    public enum TurretTrackingType
    {
        Step,
        Smooth,
    }

    #endregion

    #region Variables

    private Vector3 defaultDir;
    private Quaternion defaultRot;

    private Transform headTransform;
    private Transform barrelTransform;

    [HideInInspector] public Vector3 headingVector;

    private float curHeadingAngle;
    private float curElevationAngle;
    private bool fullAccess;

    GameObject target;

    #endregion


    private void Awake()
    {
        headTransform = Swivel.GetComponent<Transform>();
        barrelTransform = Mount.GetComponent<Transform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultDir = Swivel.transform.forward;
        defaultRot = Quaternion.FromToRotation(transform.forward, defaultDir);

        if (HeadingLimit.y - HeadingLimit.x >= 359.9f)
            fullAccess = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(target !=  null)
            Rotate();
    }

    public void SetTarget(GameObject newTarget)
    {
        if (newTarget != null)
        {
            target = newTarget;
        }
        else
        {
            target = null;
        }
    }

    void Rotate()
    {
        if (TrackingType == TurretTrackingType.Step)
        {
            if (barrelTransform != null)
            {
                /////// Heading
                headingVector =
                    Vector3.Normalize(F3DMath.ProjectVectorOnPlane(headTransform.up,
                        target.transform.position - headTransform.position));
                float headingAngle =
                    F3DMath.SignedVectorAngle(headTransform.forward, headingVector, headTransform.up);
                float turretDefaultToTargetAngle = F3DMath.SignedVectorAngle(defaultRot * headTransform.forward,
                    headingVector, headTransform.up);
                float turretHeading = F3DMath.SignedVectorAngle(defaultRot * headTransform.forward,
                    headTransform.forward, headTransform.up);

                float headingStep = HeadingTrackingSpeed * Time.deltaTime;

                // Heading step and correction
                // Full rotation
                if (HeadingLimit.x <= -180f && HeadingLimit.y >= 180f)
                    headingStep *= Mathf.Sign(headingAngle);
                else // Limited rotation
                    headingStep *= Mathf.Sign(turretDefaultToTargetAngle - turretHeading);

                // Hard stop on reach no overshooting
                if (Mathf.Abs(headingStep) > Mathf.Abs(headingAngle))
                    headingStep = headingAngle;

                // Heading limits
                if (curHeadingAngle + headingStep > HeadingLimit.x &&
                    curHeadingAngle + headingStep < HeadingLimit.y ||
                    HeadingLimit.x <= -180f && HeadingLimit.y >= 180f || fullAccess)
                {
                    curHeadingAngle += headingStep;
                    headTransform.rotation = headTransform.rotation * Quaternion.Euler(0f, headingStep, 0f);
                }

                /////// Elevation
                Vector3 elevationVector =
                    Vector3.Normalize(F3DMath.ProjectVectorOnPlane(headTransform.right,
                        target.transform.position - barrelTransform.position));
                float elevationAngle =
                    F3DMath.SignedVectorAngle(barrelTransform.forward, elevationVector, headTransform.right);

                // Elevation step and correction
                float elevationStep = Mathf.Sign(elevationAngle) * ElevationTrackingSpeed * Time.deltaTime;
                if (Mathf.Abs(elevationStep) > Mathf.Abs(elevationAngle))
                    elevationStep = elevationAngle;

                // Elevation limits
                if (curElevationAngle + elevationStep < ElevationLimit.y &&
                    curElevationAngle + elevationStep > ElevationLimit.x)
                {
                    curElevationAngle += elevationStep;
                    barrelTransform.rotation = barrelTransform.rotation * Quaternion.Euler(elevationStep, 0f, 0f);
                }
            }
        }
        else if (TrackingType == TurretTrackingType.Smooth)
        {
            Transform barrelX = barrelTransform;
            Transform barrelY = Swivel.transform;

            //finding position for turning just for X axis (down-up)
            Vector3 targetX = target.transform.position - barrelX.transform.position;
            Quaternion targetRotationX = Quaternion.LookRotation(targetX, headTransform.up);

            barrelX.transform.rotation = Quaternion.Slerp(barrelX.transform.rotation, targetRotationX,
                HeadingTrackingSpeed * Time.deltaTime);
            barrelX.transform.localEulerAngles = new Vector3(barrelX.transform.localEulerAngles.x, 0f, 0f);

            //checking for turning up too much
            if (barrelX.transform.localEulerAngles.x >= 180f &&
                barrelX.transform.localEulerAngles.x < (360f - ElevationLimit.y))
            {
                barrelX.transform.localEulerAngles = new Vector3(360f - ElevationLimit.y, 0f, 0f);
            }

            //down
            else if (barrelX.transform.localEulerAngles.x < 180f &&
                     barrelX.transform.localEulerAngles.x > -ElevationLimit.x)
            {
                barrelX.transform.localEulerAngles = new Vector3(-ElevationLimit.x, 0f, 0f);
            }

            //finding position for turning just for Y axis
            Vector3 targetY = target.transform.position;
            targetY.y = barrelY.position.y;

            Quaternion targetRotationY = Quaternion.LookRotation(targetY - barrelY.position, barrelY.transform.up);

            barrelY.transform.rotation = Quaternion.Slerp(barrelY.transform.rotation, targetRotationY,
                ElevationTrackingSpeed * Time.deltaTime);
            barrelY.transform.localEulerAngles = new Vector3(0f, barrelY.transform.localEulerAngles.y, 0f);

            if (!fullAccess)
            {
                //checking for turning left
                if (barrelY.transform.localEulerAngles.y >= 180f &&
                    barrelY.transform.localEulerAngles.y < (360f - HeadingLimit.y))
                {
                    barrelY.transform.localEulerAngles = new Vector3(0f, 360f - HeadingLimit.y, 0f);
                }

                //right
                else if (barrelY.transform.localEulerAngles.y < 180f &&
                         barrelY.transform.localEulerAngles.y > -HeadingLimit.x)
                {
                    barrelY.transform.localEulerAngles = new Vector3(0f, -HeadingLimit.x, 0f);
                }
            }
        }
    }
}
