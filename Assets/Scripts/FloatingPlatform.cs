using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float moveDistance; //distance platform will travel when the player is not on it
    public float lowestDistanceMult = 2f; //multiplied by moveDistance to get the lowest possible point the platform will sink (when player lands on it)
    public float moveSpeed; //speed at which the platform rises and falls

    private Vector3 initialPos; //used to keep track of the platform's initial position (maximum height)

    [SerializeField]
    private bool isUnderPlayer = false; //checks if the player has landed on it
    [SerializeField]
    private bool reseting = false; //checks if the platform has returned to it's initial position after sinking under player

    private float maxHeight; //highest point of platform (y axis)
    private float minHeight; //lowest point of platform (y axis)
    private float oscilateT; //timer for oscilation Lerp (not under player)
    private float resetT = 0.0f; //timer for reseting Lerp (after under player)

    private void Start()
    {
        initialPos = this.transform.position; //stores object's initial position
        maxHeight = initialPos.y; //gets highest point (initial y value)
        minHeight = initialPos.y - moveDistance; //gets lowest point
        oscilateT = Random.Range(0.0f, 1.0f); //gets a random float from 0-1 to begin oscilation from (gives random "floating" motion for each platform so they are not moving uniformly)
    }
    private void Update()
    {
        if (!isUnderPlayer && !reseting) //if the player is not on it and it is not reseting from the player being on it...
        {
            OscilateHeight(maxHeight, minHeight); //osciliates mimicking an object floating on water, accepts the current max and min y values
        }
        else if (!isUnderPlayer && reseting) //is the platform is reseting from the player being on it...
        {
            ResetPos(initialPos.y - moveDistance*lowestDistanceMult); //begins reseting the platform's position back to it's initial height starting from it's current one (the lowest possible height aka moveDistance * lowestDistanceMult)
        }
        else
        {
            transform.position -= Vector3.up * (moveSpeed * 0.15f) * Time.deltaTime; //if the player is on the platform, the platform sinks
        }

        if (transform.position.y <= (initialPos.y - moveDistance*lowestDistanceMult)) //if the player is at or below the lowest possible point (meaning the player has landed on it)
        {
            reseting = true; //begins reseting
            //resetT = 0.0f;
            isUnderPlayer = false; //indicates it is no longer under the player, and begins reseting
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isUnderPlayer = true; //if the player enters this platform's trigger, isUnderPlayer is set to true and the platform begins sinking
        }
    }

    private void OscilateHeight(float max, float min) //oscilates platform's y position between two heights
    {
        transform.position = new Vector3(initialPos.x, Mathf.Lerp(max, min, oscilateT), initialPos.z); //linearly interpolates between the two heights given at a rate tied to oscilateT

        oscilateT += 0.1f * moveSpeed * Time.deltaTime;

        if (oscilateT > 1.0f) //is oscilateT reaches a value of 1.0, it is at the target height
        {
            //max and min height values must be swapped to continue movement between the two positions in the opposite direction now
            float temp = maxHeight;
            maxHeight = minHeight;
            minHeight = temp;
            oscilateT = 0.0f; //timer is reset
        }

        resetT = 0.0f; //ensures the reset timer starts from it's initial point when next activated
    }

    private void ResetPos(float currentY) //returns platform to it's initial position before continuing oscilation
    {

        if (transform.position.y < initialPos.y)
        {
            transform.position = new Vector3(initialPos.x, Mathf.Lerp(currentY, maxHeight, resetT), initialPos.z); //linearly interpolates between it's current position and whatever value is currently being stored in maxHeight (not initial position so that it ensures it smoothly lines up with next oscilation loop)

            resetT += 0.1f * moveSpeed * Time.deltaTime;

            oscilateT = 0.0f; //resets oscilateT timer to zero so it is guaranteed to start from the maxHeight position when oscilation loop resumes

            if (resetT > 1.0f) //once the timer reaches 1.0 it is at its target height and can begin oscilation again
            {
                reseting = false;
            }
        }
    }
}
