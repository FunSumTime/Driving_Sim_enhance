using UnityEngine;

public class NewCars : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject car;
    private Vector3 start_position;
    void Start()
    {
        start_position = car.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        car.transform.position += new Vector3(0, 0, -0.1f);
        if (car.transform.position.z  < -20)
        {
            car.transform.position = start_position;
        }
    }
}
