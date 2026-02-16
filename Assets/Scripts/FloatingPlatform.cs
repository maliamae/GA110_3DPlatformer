using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float moveDistance;
    public float lowestDistanceMult = 2f;
    public float moveSpeed;

    private Vector3 initialPos;

    [SerializeField]
    private bool isUnderPlayer = false;
    [SerializeField]
    private bool reseting = false;

    private float maxHeight;
    private float minHeight;
    private float oscilateT;
    private float resetT = 0.0f;

    private void Start()
    {
        initialPos = this.transform.position;
        maxHeight = initialPos.y;
        minHeight = initialPos.y - moveDistance;
        oscilateT = Random.Range(0.0f, 1.0f);
    }
    private void Update()
    {
        if (!isUnderPlayer && !reseting)
        {
            OscilateHeight(maxHeight, minHeight);
        }
        else if (!isUnderPlayer && reseting)
        {
            ResetPos(initialPos.y - moveDistance*lowestDistanceMult);
        }
        else
        {
            transform.position -= Vector3.up * (moveSpeed * 0.15f) * Time.deltaTime;
        }

        if (transform.position.y <= (initialPos.y - moveDistance*lowestDistanceMult))
        {
            reseting = true;
            //resetT = 0.0f;
            isUnderPlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player on barrel");

            isUnderPlayer = true;
        }
    }

    private void OscilateHeight(float max, float min)
    {
        transform.position = new Vector3(initialPos.x, Mathf.Lerp(max, min, oscilateT), initialPos.z);

        oscilateT += 0.1f * moveSpeed * Time.deltaTime;

        if (oscilateT > 1.0f)
        {
            float temp = maxHeight;
            maxHeight = minHeight;
            minHeight = temp;
            oscilateT = 0.0f;
        }

        resetT = 0.0f;
    }

    private void ResetPos(float currentY)
    {

        if (transform.position.y < initialPos.y)
        {
            //transform.position += Vector3.up * (moveSpeed * 0.1f) * Time.deltaTime;
            transform.position = new Vector3(initialPos.x, Mathf.Lerp(currentY, maxHeight, resetT), initialPos.z);

            resetT += 0.1f * moveSpeed * Time.deltaTime;

            oscilateT = 0.0f;

            if (resetT > 1.0f)
            {
                reseting = false;
            }
        }
    }
}
