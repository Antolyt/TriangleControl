using UnityEngine;

/// <summary>
/// Deactiveds gameObject after n seconds
/// </summary>
public class DeactiveAfterSeconds : MonoBehaviour {

    public float seconds;
    private float timeStemp;

    private void OnEnable()
    {
        timeStemp = Time.fixedTime;
    }

    void Update () {
		if(timeStemp + seconds <= Time.fixedTime)
        {
            this.gameObject.SetActive(false);
        }
	}
}
