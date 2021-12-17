//The Marker Script
//All markers must be a child of Marker Menager

using UnityEngine;
using System.Collections;

public class BS_Marker : MonoBehaviour 
{
	[HideInInspector]
	public Transform _target; //thing that was hit
	[HideInInspector]
	public Vector3 _tempPos; //Temporary position of the marker from the last frame
	[HideInInspector]
	public float _dist; //distance between temp and actual marker position
	[HideInInspector]
	public Vector3 _dir; //Direction of the above.
	[HideInInspector]
	public RaycastHit _hit; //What was hit in this frame?
	[Tooltip ("Choose which Layers should be affected by this marker's hit check.")]
	public LayerMask _layers;

	
	void Start()
	{
		_tempPos = transform.position;
		if(GetComponent<Renderer>() != null)
		{
			GetComponent<Renderer>().enabled = false;
		}
	}
	
	public Transform HitCheck() 
	{ 
		_target = null;
		_dir =  transform.position - _tempPos;				
		_dist = Vector3.Distance(transform.position, _tempPos);
		
		
		////////// DEBUG RAYS
		Debug.DrawRay(_tempPos, _dir, Color.white, 0.3f);
		//////////
		
		
		if (Physics.Raycast (_tempPos, _dir, out _hit, _dist, _layers)) {
			_target = _hit.collider.transform;
			_tempPos = transform.position;
			return _target;
		} 
		else 
		{
			_tempPos = transform.position;
			return null;
		}
		
		
		
	}

	
	
}