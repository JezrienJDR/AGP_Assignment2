using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Lock : MonoBehaviour
{

    private List<Transform> rings;
    private List<int> offsets;

    [Header("Lock Ring Segment Meshes")]
    public GameObject plain;
    public GameObject tooth;
    public GameObject hole;
    public GameObject both;

    private int ActiveRing = 0;
    Transform ActiveTransform;
    public float timeStep = 0.01f;
    public float camStep = 0.01f;
    private float ActiveScale;

    public Camera cam;
    private Vector3 camStartPosition;

    public Safe mSafe;

    private enum segment
    {
        PLAIN = 0,
        TOOTH = 1,
        HOLE = 2,
        BOTH = 3
    }

    void Start()
    {
        rings = new List<Transform>();
        camStartPosition = cam.transform.position;
        //BuildLock(4);
    }

    public void DestroyLock()
    {
        foreach(Transform t in rings)
        {
            foreach(Transform s in t)
            {
                Destroy(s.gameObject);
                Destroy(s);
            }
            Destroy(t.gameObject);
            Destroy(t);
        }

        cam.transform.position = camStartPosition;
       
    }

    public void BuildLock(int numRings, Safe s)
    {

        ActiveRing = 0;

        mSafe = s;

        rings = new List<Transform>();
        offsets = new List<int>();

        segment[,] segs = new segment[numRings, 8];

        // rings[0] is the outer ring. rings[size-1] is the innermost ring.

        for(int i = 0; i < 8; i++)
        {
            segs[0, i] = (segment)Random.Range(0, 2);
        }

        for(int i = 1; i < numRings; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(segs[i-1, j] == segment.TOOTH || segs[i-1,j] == segment.BOTH)
                {
                    segs[i, j] = (segment)Random.Range(2, 4);
                }
                else
                {
                    segs[i, j] = (segment)Random.Range(0, 2);
                }

            }
        }

        float ringScale = 1.0f;

        for (int i = 0; i < numRings; i++)
        {
            GameObject r = new GameObject();
            for (int j = 0; j < 8; j++)
            {
                GameObject temp;

                switch (segs[i, j])
                {
                    case segment.PLAIN:
                        temp = plain;
                        break;
                    case segment.TOOTH:
                        temp = tooth;
                        break;
                    case segment.HOLE:
                        temp = hole;
                        break;
                    case segment.BOTH:
                        temp = both;
                        break;
                    default:
                        temp = plain;
                        break;
                }

                GameObject n = Instantiate(temp, r.transform);
                n.transform.Rotate(new Vector3(0.0f, 0.0f, j * 45.0f));

            }

            r.transform.localScale = new Vector3(ringScale, ringScale, 0.2f);

            int offset = Random.Range(1, 7);

            r.transform.Rotate(0.0f, 0.0f, 45.0f * offset);

            Debug.Log(offset);
            
            offsets.Add(offset);


            ringScale *= 0.65f;

            rings.Add(r.transform);

        }



        
        ActiveTransform = rings[0];
    }

    IEnumerator RotateActiveCW()
    {
        for(int i = 0; i < 45; i++)
        {
            ActiveTransform.Rotate(0.0f, 0.0f, 1.0f);
            
            yield return new WaitForSeconds(timeStep);
        }
    }

    IEnumerator RotateActiveCCW()
    {
        for (int i = 0; i < 45; i++)
        {
            ActiveTransform.Rotate(0.0f, 0.0f, -1.0f);

            yield return new WaitForSeconds(timeStep);
        }
    }

    IEnumerator ShrinkActive()
    {
        ActiveScale = ActiveTransform.localScale.x;
        float TargetScale = ActiveScale * 0.78f;
        while(ActiveTransform.localScale.x > TargetScale)
        {
            float newScale = ActiveTransform.localScale.x * 0.99f;
            ActiveTransform.localScale = new Vector3(newScale, newScale, 0.2f);
            yield return new WaitForSeconds(timeStep);
        }

        rings[ActiveRing].parent = rings[ActiveRing + 1];

        ActiveRing++;

        ActiveTransform = rings[ActiveRing];

        if (rings.Count - 1 != ActiveRing)
        {
            StartCoroutine("ZoomIn");
        }
        else
        {
            StartCoroutine("VictorySpin");
        }
    }

    IEnumerator VictorySpin()
    {
        for(int i = 0; i < 360; i++)
        {
            ActiveTransform.Rotate(0.0f, 0.0f, -5.0f);
            yield return new WaitForSeconds(timeStep);
        }

        mSafe.UnLock();
    }

    IEnumerator FailShrink()
    {
        ActiveScale = ActiveTransform.localScale.x;
        float TargetScale = ActiveScale * 0.8f;
        float ReturnScale = ActiveScale;
        while (ActiveTransform.localScale.x > TargetScale)
        {
            float newScale = ActiveTransform.localScale.x * 0.99f;
            ActiveTransform.localScale = new Vector3(newScale, newScale, 0.2f);
            yield return new WaitForSeconds(timeStep);
        }
        while(ActiveTransform.localScale.x < ReturnScale)
        {
            float newScale = ActiveTransform.localScale.x * 1.01f;
            ActiveTransform.localScale = new Vector3(newScale, newScale, 0.2f);
            yield return new WaitForSeconds(timeStep);
        }

    }

    IEnumerator ZoomIn()
    {
        

        for(int i = 0; i < 50; i++)
        {
            cam.transform.Translate(0.0f, 0.0f, camStep);
            
            //foreach(Transform t in rings)
            //{
            //    t.localScale = new Vector3(t.localScale.x * 1.01f, t.localScale.y * 1.01f, 1.0f);
            //}
            
            yield return new WaitForSeconds(timeStep);
        }
    }

    public void SpinCW()
    {

        StartCoroutine("RotateActiveCW");

        offsets[ActiveRing]++;
        if(offsets[ActiveRing] == 8)
        {
            offsets[ActiveRing] = 0;
        }

        Debug.Log(offsets[ActiveRing]);
        //ActiveTransform.Rotate(0.0f, 0.0f, 1.0f);
    }

    public void SpinCCW()
    {

        StartCoroutine("RotateActiveCCW");

        offsets[ActiveRing]--;
        if (offsets[ActiveRing] == -1)
        {
            offsets[ActiveRing] = 7;
        }

        Debug.Log(offsets[ActiveRing]);
        //ActiveTransform.Rotate(0.0f, 0.0f, 1.0f);
    }

    public void Shrink()
    {
        //Debug.Log(ActiveTransform.localRotation.z);
        //if (ActiveTransform.localRotation.z - 0.0f < 0.1f)
        if(offsets[ActiveRing] == offsets[ActiveRing + 1])
        {
            StartCoroutine("ShrinkActive");
        }
        else
        {
            StartCoroutine("FailShrink");
        }
    }

    public void OnLookLeft(InputValue val)
    {
        SpinCCW();
    }
    public void OnLookRight(InputValue val)
    {
        SpinCW();
    }
    public void OnShrink(InputValue val)
    {
        Shrink();
    }

}
