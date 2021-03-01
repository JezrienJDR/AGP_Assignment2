using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    public Transform Door;
    public Transform Crank;

    public Lock mLock;

    public int level;

    private bool Picked;

    // Start is called before the first frame update
    void Start()
    {
        mLock = FindObjectOfType<Lock>();
        //UnLock();
        Picked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnLock()
    {
        Picked = true;
        FindObjectOfType<PlayerController>().UnDim();
        FindObjectOfType<PlayerController>().PickingLock = false;
        mLock.DestroyLock();
        StartCoroutine("CrankTurn");
    }



    IEnumerator CrankTurn()
    {
        for(int i = 0; i < 72; i++)
        {
            Crank.Rotate(0, 5, 0);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.0f);

        StartCoroutine("DoorOpen");
    }

    IEnumerator DoorOpen()
    {
        for (int i = 0; i < 120; i++)
        {
            Door.Rotate(0, 1, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }


    public Lock EngageLock()
    {
        if (!Picked)
        {
            mLock.BuildLock(level, this);

        }
        return mLock;
    }
}
