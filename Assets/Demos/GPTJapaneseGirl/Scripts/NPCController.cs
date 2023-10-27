using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCController : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    SkinnedMeshRenderer face_Blendshape;

    int blinking = 0;
    float blinkingValue = 0;
    float blinkingTimer = 0;
    float blinkingTimerTotal = 3.5f;

    // for dotween animations
    float durationDefault = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        DOTween.Init();
        DOTween.defaultEaseType = Ease.InSine;
    }

    // Update is called once per frame
    void Update()
    {
        if (face_Blendshape != null)
        {
            blinkingTimer += Time.deltaTime;
            if (blinking == 0 && (Random.value < 0.001f || blinkingTimer > blinkingTimerTotal))
            {
                blinkingTimer = 0;
                blinkingTimerTotal = Random.Range(1.1f, 5.01f);
                blinking = 1;
                blinkingValue = 0;
            }
            else if (blinking == 1)
            {
                blinkingValue += Time.deltaTime * 1000;
                if (blinkingValue > 100)
                {
                    blinking = 2;
                    face_Blendshape.SetBlendShapeWeight(35, 100);
                }
                else
                {
                    face_Blendshape.SetBlendShapeWeight(35, blinkingValue);
                }
            }
            else if (blinking == 2)
            {
                blinkingValue -= Time.deltaTime * 600;
                if (blinkingValue < 0)
                {
                    blinking = 0;
                    face_Blendshape.SetBlendShapeWeight(35, 0);
                }
                else
                {
                    face_Blendshape.SetBlendShapeWeight(35, blinkingValue);
                }
            }
        }

    }

    public void ShowAnimation(string animID)
    {
        if (face_Blendshape != null)
        {
            for (int i = 0; i < 60; i++)
            {
                if (i != 1)
                {
                    face_Blendshape.SetBlendShapeWeight(i, 0);
                }
            }
        }


        if (animID == "idle")
        {
            if (Random.value < 0.3f)
            {
                anim.SetTrigger("idle1");
            }
            else if (Random.value < 0.6f)
            {
                anim.SetTrigger("idle2");
            }
            else
            {
                anim.SetTrigger("idle");
            }
            if(Random.value < 0.5f)
            {
                float myValue = 0;
                if (face_Blendshape != null)
                    DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                    OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(9, myValue); });

                //face_Blendshape.SetBlendShapeWeight(9, 100);
            }
            else
            {
                float myValue = 0;
                if (face_Blendshape != null)
                    DOTween.To(() => myValue, x => myValue = x, 67, durationDefault).
                    OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(24, myValue); });

                //face_Blendshape.SetBlendShapeWeight(24, 67);
            }
        }
        else if (animID == "shy")
        {
            anim.SetTrigger("shy");
        }
        else if (animID == "confuse")
        {
            anim.SetTrigger("confuse");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(32, myValue); });
            //face_Blendshape.SetBlendShapeWeight(32, 100);
        }
        else if (animID == "joking")
        {
            anim.SetTrigger("joking");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(33, myValue); });
            //face_Blendshape.SetBlendShapeWeight(33, 100);
        }
        else if (animID == "worried")
        {
            anim.SetTrigger("worried");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(52, myValue); });
            //face_Blendshape.SetBlendShapeWeight(52, 100);
        }
        else if (animID == "surprise")
        {
            anim.SetTrigger("surprise");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(53, myValue); });
            //face_Blendshape.SetBlendShapeWeight(53, 100);
        }
        else if (animID == "focus")
        {
            anim.SetTrigger("focus");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(50, myValue); });
            //face_Blendshape.SetBlendShapeWeight(50, 100);
        }
        else if (animID == "angry")
        {
            anim.SetTrigger("angry");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(49, myValue); });
            //face_Blendshape.SetBlendShapeWeight(49, 100);
        }
        else if (animID == "cheers")
        {
            anim.SetTrigger("cheers");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(24, myValue); });
            //face_Blendshape.SetBlendShapeWeight(24, 100);
        }
        else if (animID == "nod")
        {
            anim.SetTrigger("nod");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(9, myValue); });
            //face_Blendshape.SetBlendShapeWeight(9, 100);
        }
        else if (animID == "waving_arm")
        {
            anim.SetTrigger("waving_arm");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(24, myValue); });
            //face_Blendshape.SetBlendShapeWeight(24, 100);
        }
        else if (animID == "proud")
        {
            anim.SetTrigger("proud");
            float myValue = 0;
            if (face_Blendshape != null)
                DOTween.To(() => myValue, x => myValue = x, 100, durationDefault).
                OnUpdate(() => { face_Blendshape.SetBlendShapeWeight(24, myValue); });
            //face_Blendshape.SetBlendShapeWeight(24, 100);
        }
    }
}
