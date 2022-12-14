using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPackagingLogic : MonoBehaviour
{

    public GameObject boxPrefab;

    public MainTrackLogic mainTrackScript;
    public MiniTrackLogic miniTrackLogic;

    public Animator handAnimator;
    public Animator boxScaleAnimator;

    Transform car;

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Car")
        {
            car = other.transform;
            handAnimator.SetTrigger("box");
            boxScaleAnimator.SetTrigger("scaleIn");
        }
    }

    public void Pack()
    {
        if (mainTrackScript != null)
            mainTrackScript.RemoveCarFromList(car);
        else if (miniTrackLogic != null)
            miniTrackLogic.RemoveCarFromList(car);

        Rigidbody rb = car.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        car.GetComponent<Collider>().enabled = false;

        //Transform boxInstance = boxPool.Dequeue();
        Transform boxInstance = ObjectPooler.Instance.SpawnFromPool("Box").transform;
        //boxInstance.localScale = boxPrefab.transform.localScale;
        //boxInstance.gameObject.SetActive(true);

        //boxInstance.position = transform.position;
        boxInstance.position = new Vector3(car.position.x, transform.position.y, car.position.z);
        car.position = boxInstance.GetChild(0).position;
        car.rotation = boxInstance.GetChild(0).rotation;
        car.parent = boxInstance;
        boxInstance.gameObject.SetActive(true);

        boxInstance.GetComponent<BoxAttributes>().SetPrice(car.GetComponent<CarAttributes>().GetPrice());
        boxScaleAnimator.SetTrigger("scaleOut");
    }
}
