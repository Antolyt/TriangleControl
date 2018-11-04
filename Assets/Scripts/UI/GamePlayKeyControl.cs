using UnityEngine;
using UnityEngine.Events;

public class GamePlayKeyControl : MonoBehaviour {

	public UnityEvent actionOnCancel;
    public UnityEvent actionOnStart;
    public UnityEvent actionOnSubmit;

    void Update () {

        if (Input.GetButtonDown("Cancel"))
        {
            if (actionOnCancel != null) actionOnCancel.Invoke();
            return;
        }

        if (Input.GetButtonDown("Start"))
        {
            if (actionOnCancel != null) actionOnStart.Invoke();
            return;
        }

        if (Input.GetButton("Submit"))
        {
            if (actionOnCancel != null) actionOnSubmit.Invoke();
            return;
        }
    }
}