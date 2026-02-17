using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //built to test moving platform mechanics, not implemented in level blockout

    public float moveSpeed; 
    private Vector3 initialPos;
    public bool isMax = false;

    private void Start()
    {
        initialPos = transform.position;
    }
    private void Update()
    {
        if (isMax)
        {
            transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        if (transform.position.x <= initialPos.x)
        {
            isMax = false;
        }
        else if (transform.position.x >= initialPos.x + 5f)
        {
            isMax = true;
        }

    }
}
