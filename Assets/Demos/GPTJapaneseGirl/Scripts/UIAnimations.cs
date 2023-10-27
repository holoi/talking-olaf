using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    [SerializeField]
    Animator CountdownAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // do not play at first:
        CountdownAnimator.enabled = false ;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCountDownAnimation()
    {
        if (!CountdownAnimator.isActiveAndEnabled) CountdownAnimator.enabled = true;

        CountdownAnimator.Rebind();
        CountdownAnimator.playbackTime = 0;
        CountdownAnimator.enabled = true;
        // animation to idel
        FindObjectOfType<NPCController>().GetComponent<Animator>().SetTrigger("idle1");
    }
}