using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ƒx.UnityUtils.Editor;

[ExecuteInEditMode]
public class GameObjectTracker : MonoBehaviour
{
    [System.Serializable]
    public class Constrains
    {
        public bool noTranslationX = false;
        public bool noTranslationY = false;
        public bool noTranslationZ = false;
        [Space(10)]
        public bool noRotationX = false;
        public bool noRotationY = false;
        public bool noRotationZ = false;
    }

    public GameObject trackedObject;

    [ReadOnly, SerializeField] Vector3 thisPosition;
    [ReadOnly, SerializeField] Quaternion thisRotation;

    [ReadOnly, SerializeField] Vector3 thatPosition;
    [ReadOnly, SerializeField] Quaternion thatRotation;

    [ReadOnly, SerializeField] Vector3 diffPosition;
    [ReadOnly, SerializeField] Quaternion diffRotation;
    public bool absolutePosition = true;
    public bool absoluteRotation = true;
    public Constrains constrains;

    void Start()
    {
        SaveState();
    }

/*
    void OnDrawGizmos()
    {
        if(trackedObject != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, trackedObject.transform.position);
            Gizmos.DrawSphere(transform.position, 0.05f);
            Gizmos.DrawSphere(trackedObject.transform.position, 0.025f);

            // Quaternion aQuaternion = Quaternion.identity * Quaternion.Inverse(trackedObject.transform.rotation);
            // Quaternion bQuaternion = Quaternion.identity * Quaternion.Inverse(this.transform.rotation);
            // Quaternion deltaQuaternion = bQuaternion * Quaternion.Inverse(aQuaternion);


            // Quaternion diffRotation = transform.rotation * Quaternion.Inverse(trackedObject.transform.rotation);
            Quaternion orientation = diffRotation * trackedObject.transform.rotation;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, orientation, transform.localScale);
            Gizmos.DrawFrustum(new Vector3(), 10f, Vector3.Distance(transform.position, trackedObject.transform.position), 0, 1.0f);
        }
    }
*/

    void Update()
    {
        if(trackedObject != null){
            if (absolutePosition)
            {
                UpdatePosition(trackedObject.transform.position);
            }
            else
            {
                UpdatePosition(trackedObject.transform.position + diffPosition);
            }


            if (absoluteRotation)
            {
                UpdateRotation(trackedObject.transform.rotation);
            }
            else
            {
                UpdateRotation(diffRotation * trackedObject.transform.rotation);
            }
        } else Debug.Log("[GameObjectTracker]: no object reference!");
    }

    void UpdatePosition(Vector3 pIn)
    {
        Vector3 pNew = transform.position;
        if(!constrains.noTranslationX) pNew.x = pIn.x;
        if(!constrains.noTranslationY) pNew.y = pIn.y;
        if(!constrains.noTranslationZ) pNew.z = pIn.z;
        transform.position = pNew;
    }

    void UpdateRotation(Quaternion qIn)
    {
        // transform.rotation = diffRotation * trackedObject.transform.rotation;
        Vector3 eulerNew = trackedObject.transform.rotation.eulerAngles;
        if(constrains.noRotationX) eulerNew.x = 0;
        if(constrains.noRotationY) eulerNew.y = 0;
        if(constrains.noRotationZ) eulerNew.z = 0;
        Quaternion qNew = Quaternion.Euler(eulerNew);
        transform.rotation = diffRotation * qNew;
        
    }

    [ContextMenu("Save Current State")]
    void SaveState()
    {
        // https://forum.unity.com/threads/get-the-difference-between-two-quaternions-and-add-it-to-another-quaternion.513187/
        thisRotation = this.transform.rotation;
        thisPosition = this.transform.position;

        thatPosition = trackedObject.transform.position;
        thatRotation = trackedObject.transform.rotation;

        diffPosition = thisPosition - thatPosition;
        diffRotation = thisRotation * Quaternion.Inverse(thatRotation);
        Debug.Log("[GameObjectTracker]: saved current state.");
    }
}
