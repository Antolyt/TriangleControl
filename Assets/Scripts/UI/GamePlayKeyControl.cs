using UnityEngine;
using UnityEngine.Events;

public class GamePlayKeyControl : MonoBehaviour {

	public UnityEvent actionOnCancel;

    void Update () {

        if (Input.GetButtonDown("Cancel"))
        {
            if (actionOnCancel != null) actionOnCancel.Invoke();
            return;
        }
    }
}
