using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{

    public Transform cam;
    public List<Transform> positions;

    public float camSpeed;

    int currActive;
    Transform currActiveTransform;

    bool reposition;
    float repoTime;

    public static CameraSwitch Instance;
    private void Awake()
    {
        Instance = this;
        currActive = 0;
        currActiveTransform = positions[currActive];
        reposition = false;
        repoTime = 0f;
    }

    void Update()
    {
        if (reposition)
        {
            repoTime += Time.deltaTime * 0.1f * camSpeed;
            if (cam.position != currActiveTransform.position)
                cam.position = Vector3.Lerp(cam.position, currActiveTransform.position, repoTime);

            if (cam.rotation != currActiveTransform.rotation)
                cam.rotation = Quaternion.Lerp(cam.rotation, currActiveTransform.rotation, repoTime);

            if (cam.position == currActiveTransform.position && (cam.rotation == currActiveTransform.rotation || Vector3.Distance(cam.rotation.eulerAngles, currActiveTransform.rotation.eulerAngles) < 0.001f)) //OVO JE KRITICNO ZBOG RELATIVNE ROTACIJE
                reposition = false;
        }
    }

    public void ChangeCamera()
    {
        currActive++;
        currActive = currActive % positions.Count;

        currActiveTransform = positions[currActive];
        reposition = true;
        repoTime = 0f;
    }

    public void SetOgPosition()
    {
        currActive = 0;
        currActiveTransform = positions[currActive];
        cam.position = currActiveTransform.position;
        cam.rotation = currActiveTransform.rotation;
    }
}