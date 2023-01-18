using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct RopeJoint// : System.IComparable
{
    public Vector3 position;
    public Vector3 vel;
    [HideInInspector]
    public int index;
    [HideInInspector]
    public float distance;
    //public bool locked;
    //public string name;

    /*
    public int CompareTo(object obj)
    {
        if (obj is RopeJoint joint)
        {
            return index.CompareTo(joint.index);
        }
        else
        {
            Debug.LogError($"Can only compare RopeJoint to RopeJoint (not {obj.GetType()})");
            return 1;
        }
    }
    */
}

public class Rope : MonoBehaviour
{
    //[SerializeField]
    //private float thickness;
    //[SerializeField]
    //private float length;
    //[SerializeField]
    //private float endMass;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float drag;
    //[SerializeField]
    //private GameObject lineEnd;
    //[SerializeField]
    //private Vector3 generateDirection = Vector3.down;
    [SerializeField]
    private List<RopeJoint> ropeJoints;
    // list of joints used as a medium between arrays and for adding unsorted joints and for making non-order related changes
    //private int r;

    private void Start()
    {
        //ropeJoints = new List<RopeJoint>();
        //lineEnd = transform.GetChild(0).gameObject;
        Generate();
    }
    void FixedUpdate()
    {
        //if (r == 1)
        //{
        UpdateRope();
        //} 
    }

    private void Generate()
    {
        for (int i = 0; i < ropeJoints.Count; i++)
        {
            RopeJoint tmp = ropeJoints[i];
            tmp.index = i;
            if (i != 0)
                tmp.distance = tmp.position.Distance(ropeJoints[i - 1].position);
            ropeJoints[i] = tmp;
        }

        /*
        if (Physics.SphereCast(transform.position, thickness,generateDirection,out RaycastHit hitInfo,length))
        {
            lineEnd.transform.position = hitInfo.point;
        }
        else
        {
            lineEnd.transform.position = transform.position + generateDirection.normalized * length;
        }
        lineEnd.transform.GetComponent<SphereCollider>().radius = thickness;
        SortJoints();
        AddJoint(0, transform.position + generateDirection.normalized * length, Vector3.zero, true, "base");
        r = 1;
        */
    }

    private void UpdateRope()
    {
        //RopeJoint[] tempP = new RopeJoint[ropeJoints.Count];

        for (int i = 0; i < ropeJoints.Count; i++)
        {
            RopeJoint joint = ropeJoints[i];

            if (i == 0) continue; // First joint is locked

            //joint.vel = joint.vel * (1 - Time.deltaTime * drag);
            //tempP[i].vel = Vector3.MoveTowards(tempP[i].vel,Vector3.zero,drag * Mathf.Clamp( Mathf.Log10(tempP[i].vel.magnitude) ,float.Epsilon,float.PositiveInfinity));
            //print(joint.vel.magnitude);
            //if (joint.vel.magnitude <= 1.85f) joint.vel = Vector3.zero;

            joint.position += joint.vel * Time.deltaTime;

            joint.vel += Vector3.down * gravity * Time.deltaTime;

            if (i != 0) // just in case
            {
                if (Vector3.Distance(joint.position, ropeJoints[i - 1].position) > joint.distance)
                {
                    joint.position = (joint.position - ropeJoints[i - 1].position).normalized * joint.distance + ropeJoints[i - 1].position;
                    //tempP[i].vel = Vector3.MoveTowards((tempP[i].position - ropeJoints[i].position).normalized * tempP[i].vel.magnitude, Vector3.zero, drag);
                    joint.vel = (joint.position - ropeJoints[i].position).normalized * joint.vel.magnitude;
                }
            }
            /*if (tempP[i].vel.x < 0.05f) tempP[i].vel.x = 0;
            if (tempP[i].vel.y < 0.05f) tempP[i].vel.y = 0;
            if (tempP[i].vel.z < 0.05f) tempP[i].vel.z = 0;*/

            //lineEnd.transform.position = joint.position;

            ropeJoints[i] = joint;
        }
    }

    private void OnDrawGizmos()
    {
        if (ropeJoints == null) return;

        for (int i = 0; i < ropeJoints.Count; i++)
        {
            RopeJoint joint = ropeJoints[i];

            //if (joint.locked)
            if (i == 0)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(joint.position, 0.1f);
            Gizmos.DrawLine(joint.position, joint.position + joint.vel);

            if (i != 0)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(joint.position, ropeJoints[i - 1].position);
            }
        }
        /*
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position + generateDirection.normalized * length);
        if(r >= 1)
        {
            foreach (RopeJoint item in ropeJoints)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(item.position, item.position + item.vel);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(item.position, 0.3f);
            }
            Gizmos.DrawLine(transform.position, ropeJoints[0].position);
        }
        */
    }

    private void SortJoints()
    {
        ropeJoints.Sort();
    }

    private void AddJoint(int DistanceToBottom, Vector3 Pos, Vector3 Vel, bool Basee = false , string Name = "")
    {
        RopeJoint[] tempA = new RopeJoint[ropeJoints.Count + 1];

        // add the joint
        tempA[ropeJoints.Count].position = Pos;
        tempA[ropeJoints.Count].vel = Vel;
        //tempA[ropeJoints.Count].name = Name;
        tempA[ropeJoints.Count].index = DistanceToBottom;

        if (!Basee)
        {
            // this updates other joints so dont run on first add
            for (int i = 0; i < ropeJoints.Count; i++)
            {
                tempA[i] = ropeJoints[i];
                if (tempA[i].index >= DistanceToBottom)
                {
                    tempA[i].index++;
                }
            }
        }
        else
        {
            // if its the rope end then set distance for gm
            //tempA[ropeJoints.Count].distance = length;
        }
        ropeJoints.Clear();
        for (int i = 0; i < tempA.Length; i++)
        {
            ropeJoints.Add(new RopeJoint() { position = tempA[i].position, distance = tempA[i].distance, index = tempA[i].index, vel = tempA[i].vel, }); //name = tempA[i].name });
        }

        SortJoints();
    }
}
