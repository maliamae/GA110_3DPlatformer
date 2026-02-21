using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int amount = 1; //base amount of rays counted when object is collected
    [SerializeField] CollectibleType type; //type of collectible

    private Vector3 initialPos;
    private float oscilateT = 0.0f;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float moveDistance;
    private float maxHeight;
    private float minHeight;

    public enum CollectibleType
    {
        Light //only collecting lightrays for now
    }

    private void Start()
    {
        switch(type)
        {
            case CollectibleType.Light:
                GetComponent<MeshRenderer>().material.color = Color.yellow; //changes material color of collectible in accordance with type to display difference from start
                break;
        }

        initialPos = transform.position;
        maxHeight = initialPos.y;
        minHeight = initialPos.y - moveDistance;
        oscilateT = Random.Range(0.0f, 1.0f);
    }

    private void Update()
    {
        OscilateHeight(maxHeight, minHeight);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //if the player hits the collectible, collectible amount is added to the curent amount and this object is destroyed
        {
            //CollectibleManager.Instance.Add(type, amount);
            CollectibleEventSystem.RaiseCollectibleCollected(type, amount); //calls function from even system script that triggers event that passes along values to manager and UI manager
            Destroy(gameObject);
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
    }
}
