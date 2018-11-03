using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CancelTimer : MonoBehaviour {

    public string input;
    private float[] timeOfCancelPress;    // = time, if not pressed
    public float cancelTime;
    public bool dependencyActive = false;
    public GameObject edge;
    public Image arrow;
    public Image filling;

    public UnityEvent action;

    private void Start()
    {
        timeOfCancelPress = new float[4];
    }

    private void Update()
    {
        if (!dependencyActive)
        {
            // Go backwards
            if (Input.GetButton(input))
            {
                if(!edge.activeSelf)
                    edge.SetActive(true);
                filling.rectTransform.sizeDelta = new Vector2(-arrow.rectTransform.sizeDelta.x + (Time.time - timeOfCancelPress[0]) / cancelTime * arrow.rectTransform.sizeDelta.x, 0);
                
                if (timeOfCancelPress[0] + cancelTime <= Time.time)
                {
                    if(action != null)
                        action.Invoke();
                }
            }
            else
            {
                if (edge.activeSelf)
                    edge.SetActive(false);
                filling.rectTransform.sizeDelta = new Vector2(-arrow.rectTransform.sizeDelta.x, 0);
                timeOfCancelPress[0] = Time.time;
            }
        }
    }

    public void DoAction(int controller)
    {
        float oldestTime = float.MaxValue;

        foreach(float f in timeOfCancelPress)
        {
            if(oldestTime > f)
            {
                oldestTime = f;
            }
        }

        if (dependencyActive)
        {
            if (Input.GetButton(input))
            {
                if (!edge.activeSelf)
                    edge.SetActive(true);
                filling.rectTransform.sizeDelta = new Vector2(-arrow.rectTransform.sizeDelta.x + (Time.time - oldestTime) / cancelTime * arrow.rectTransform.sizeDelta.x, 0);
            }
            else
            {
                if (edge.activeSelf)
                    edge.SetActive(false);
                filling.rectTransform.sizeDelta = new Vector2(-arrow.rectTransform.sizeDelta.x, 0);
            }

            // Go backwards
            if (Input.GetButton(input + controller))
            {
                if (timeOfCancelPress[controller] + cancelTime <= Time.time)
                {
                    if (action != null)
                        action.Invoke();
                }
            }
            else
            {
                timeOfCancelPress[controller] = Time.time;
            }
        }
    }
}
