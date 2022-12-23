using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTrackLogic : MonoBehaviour
{

    public float trackSpeed;
    public Material trackMat;
    float matTrackSpeed;

    [SerializeField]
    List<Transform> onTrackCars;
    List<Transform> toBeRemovedCars;

    public PhysicMaterial slipperyMat;

    public Transform mainTrack;

    void Start()
    {
        onTrackCars = new List<Transform>();
        toBeRemovedCars = new List<Transform>();
        matTrackSpeed = trackSpeed * 0.475f;
        trackMat.SetFloat("_TextureSpeed", matTrackSpeed);
    }

    void Update()
    {
        foreach (Transform car in onTrackCars)
            if (car != null)
                car.transform.position += transform.forward * trackSpeed * Time.deltaTime;

        foreach (Transform car in toBeRemovedCars)
            if (car != null)
                RemoveCarFromList(car);

        toBeRemovedCars.Clear();
    }

    public void AddCarToTrack(Transform car) => onTrackCars.Add(car);

    public void RemoveCarFromList(Transform car)
    {
        if (car != null)
            onTrackCars.Remove(car);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!onTrackCars.Contains(other.transform))
            onTrackCars.Add(other.transform);
    }
}
