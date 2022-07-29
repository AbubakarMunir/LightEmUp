using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour
{


	public Space RotationSpace = Space.Self;

	/// The rotation speed. Positive means clockwise, negative means counter clockwise.
	public Vector3 RotationSpeed;


    /// <summary>
    /// Makes the object rotate on its center every frame.
    /// </summary>
    protected virtual void Update ()
	{

		transform.Rotate (RotationSpeed * Time.deltaTime, RotationSpace);
	}

}
