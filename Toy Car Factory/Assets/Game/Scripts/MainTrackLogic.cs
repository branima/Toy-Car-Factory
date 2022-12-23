using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTrackLogic : MonoBehaviour
{

    public float trackSpeed;
    public Material trackMat;
    float matTrackSpeed;

    [SerializeField]
    List<Transform> onTrackCars;

    Vector3 toTheMiddleMovement;

    void Start()
    {
        onTrackCars = new List<Transform>();
        matTrackSpeed = trackSpeed * 0.42f;
        trackMat.SetFloat("_TextureSpeed", matTrackSpeed);
    }

    void Update()
    {
        foreach (Transform car in onTrackCars)
        {
            if (car != null)
            {
                toTheMiddleMovement = -car.right * car.position.x;
                car.transform.position += (transform.forward + toTheMiddleMovement) * trackSpeed * Time.deltaTime;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //OTKACI mini trackZ
        other.transform.GetComponent<Collider>().material = null;
        if (!onTrackCars.Contains(other.transform))
            onTrackCars.Add(other.transform);

        other.transform.GetComponent<CarAttributes>().GetMiniTrackScript().RemoveCarFromList(other.transform);
    }

    public void RemoveCarFromList(Transform car)
    {
        if (car != null && onTrackCars.Contains(car))
            onTrackCars.Remove(car);
    }

    void OnCollisionExit(Collision other) => RemoveCarFromList(other.transform);

    public void UpgradeTrackSpeed(float speedModif)
    {
        trackSpeed *= speedModif;
        matTrackSpeed *= speedModif;
        trackMat.SetFloat("_TextureSpeed", matTrackSpeed);
    }
}
