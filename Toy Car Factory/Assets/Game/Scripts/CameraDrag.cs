using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{

    public float lowerZBound;
    public float upperZBound;

    float lastX;
    float deltaX;

    public float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        lastX = 0f;
        deltaX = 0f;
    }

    // Update is called once per frame

    Vector3 position;

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
                lastX = Input.mousePosition.x;
            else if (Input.GetMouseButton(0))
            {
                deltaX = Input.mousePosition.x - lastX;
                lastX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButtonUp(0))
                deltaX = 0f;

            float movementIntensity = Mathf.Clamp(deltaX * speed * Time.deltaTime, -1, 1);
            Vector3 preliminaryPos = transform.position - transform.forward * movementIntensity;
            if (preliminaryPos.z > lowerZBound && preliminaryPos.z < upperZBound)
                transform.position = preliminaryPos;
        }
    }
}
