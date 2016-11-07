using UnityEngine;
using System.Collections;

public class Shiftbetween : MonoBehaviour { 
	public Highscoremanger h;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Shift(){

		if (h.shiftscore == false) {
			h.shiftscore = true;
		}
		else if (h.shiftscore == true) {
			h.shiftscore = false;
		}
	}

}
