
using UnityEngine;
using System.Collections;

public class PlayerGrabV2 : MonoBehaviour {
	GameObject grabbedObject;
	float grabbedObjectSize;
    [SerializeField] public float throwForce = 10;

	GameObject GetMouseHoverObject(float range){
		//Check for Collider with Raycast
		Vector3 position = gameObject.transform.position;
		RaycastHit raycastHit;
		Vector3 target = position + Camera.main.transform.forward * range;
		if (Physics.Linecast(position, target, out raycastHit ))
			return raycastHit.collider.gameObject;
		return null;
	}

	void TryGrabbedObject(GameObject grabObject){

		//Check for Object if not null Grab it
		if (grabObject == null || !CanGrab(grabObject))
			return;
		
		grabbedObject = grabObject;
	}

	//Can grab condition (RigidBody)
	bool CanGrab(GameObject candidate){
		return candidate.GetComponent<Rigidbody> () != null; 

	}

	void DropObject(){

        //Check if hands are empty
        if (grabbedObject = null)
			return;
	}

	void ThrowObject(){

        //Release Direction and Velocity
        //Set multiplier larger for more force
        grabbedObject.GetComponent<Rigidbody>().AddForce(-Camera.main.transform.forward * throwForce);
        

        //Check if hands are empty
        if (grabbedObject = null)
			return;
	}	


	// UPDATE is called once per frame
	void Update () {
		//Assign Input for Grab
		if (Input.GetButtonDown("Use")) {
			if (grabbedObject == null)
				TryGrabbedObject (GetMouseHoverObject(5));
			else
				DropObject();
		}

		if (grabbedObject != null) {
			//Set position of grabbed Object after grabbing
			Vector3 newposition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize;
			grabbedObject.transform.position = newposition;
		}

		if(Input.GetMouseButtonDown(0) && grabbedObject != null) {
			ThrowObject();
		}

		if(Input.GetMouseButtonDown(1) && grabbedObject != null) {
			DropObject();
		}

		Debug.Log ("Mouse hover : " + GetMouseHoverObject (5));
		Debug.Log("Grabbed object : " + grabbedObject);

	
	}
}
