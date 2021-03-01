using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class skillButton : MonoBehaviour
{
    public int skill;

    public Transform button;

    private Vector3 buttonNormal = new Vector3(-0.3f, 0.6f, 0);

    public void SetSkill()
    {
        StartCoroutine("ButtonPress");
    }



    IEnumerator ButtonPress()
    {
        for(int i = 0; i < 20; i++)
        {
            button.position -= buttonNormal * 0.005f;
            yield return new WaitForSeconds(0.01f);
        }



        for (int i = 0; i < 20; i++)
        {
            button.position += buttonNormal * 0.005f;
            yield return new WaitForSeconds(0.01f);
        }

        FindObjectOfType<PlayerController>().skill = skill;
        FindObjectsOfType<TMP_Text>()[0].SetText(skill.ToString());

    }


}
