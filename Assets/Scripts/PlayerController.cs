using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{

    public int skill;

    public float walkSpeed;
    public float lookSpeed;

    private float rotation;

    private bool a;
    private bool s;
    private bool w;
    private bool d;


    private bool l;
    private bool r;

    public GameObject interactable;

    public Lock mLock;

    public Volume vol;
    public ColorAdjustments ca;

    public bool PickingLock;


    private void Awake()
    {
        vol.profile.TryGet(out ca);
        if(ca == null)
        {
            Debug.Log("NO COLOR ADJUSTMENTS");
        }

        PickingLock = false;
    }

    private void Update()
    {
        if (!PickingLock)
        {

            Vector3 move = Vector3.zero;

            if (w && !s)
            {
                move = transform.forward * walkSpeed * Time.deltaTime;
            }
            else if (s && !w)
            {
                move = -transform.forward * walkSpeed * Time.deltaTime;
            }
            if (a && !d)
            {
                rotation = -lookSpeed * Time.deltaTime;
            }
            else if (d && !a)
            {
                rotation = lookSpeed * Time.deltaTime;
            }
            else if (!a && !d) rotation = 0;


            transform.position += move;
            transform.Rotate(0, rotation, 0);
        }
    }

    public void OnW(InputValue val)
    {

        if (val.isPressed)
        {
            w = true;
        }
        else
        {
            w = false;
        }
    }

    public void OnA(InputValue val)
    {

        if (val.isPressed)
        {
            a = true;
        }
        else
        {
            a = false;
        }
    }

    public void OnS(InputValue val)
    {

        if (val.isPressed)
        {
            s = true;
        }
        else
        {
            s = false;
        }
    }

    public void OnD(InputValue val)
    {
 
        if (val.isPressed)
        {
            d = true;
        }
        else
        {
            d = false;
        }
    }

    public void OnLookLeft(InputValue val)
    {
        if (PickingLock)
        {
            mLock.SpinCCW();
        }
    }

    public void OnLookRight(InputValue val)
    {
        if (PickingLock)
        {
            mLock.SpinCW();
        }
    }

    public void OnExit(InputValue val)
    {
        Application.Quit();
    }

    public void Dim()
    {

        ca.postExposure.value = -5;
    }

    public void UnDim()
    {
        ca.postExposure.value = 0;
    }

    public void OnShrink(InputValue val)
    {
        if (PickingLock)
        {
            mLock.Shrink();
        }
        else
        {


            var b = interactable.GetComponent<skillButton>();

            if (b != null)
            {
                b.SetSkill();
            }

            var s = interactable.GetComponent<Safe>();

            if (s != null)
            {
                Debug.Log("SAFE");
                mLock = s.EngageLock();
                Dim();
                PickingLock = true;
            }
            else
            {
                Debug.Log("NO SAFE");
            }
        }
    }
}
