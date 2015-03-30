using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameUtilities;

public class RoamingCamera : BaseCamera {
	# region Public Variables

	[SerializeField]
	private Transform parentT;
	[SerializeField]
	private float speed = 100f;
	[SerializeField]
	private float rotDegreesPerSecond = 120f;
	[SerializeField]
	private float rotSmooth = 0.25f;

	# endregion Public Variables

	# region Private Variables

	private int waypointIndx;
	private Transform curWayPoint;
	private List<Transform> waypoints;

	# endregion Private Variables

	private void Start() {
		waypointIndx = 0;
		waypoints = new List<Transform>();
		GameUtility.GetChildrenRecursively(parentT, ref waypoints);
		curWayPoint = waypoints[waypointIndx];
	}

	private void LateUpdate() {
		float distanceToTarget = Vector3.Distance(transform.position, waypoints[waypointIndx].position);

		if (distanceToTarget > 0f) {
			Vector3 lookAtTarget = (curWayPoint.position - transform.position);
			lookAtTarget = Vector3.Lerp(transform.forward, curWayPoint.forward, rotDegreesPerSecond * Time.deltaTime);

			Quaternion lookAt = Quaternion.LookRotation(lookAtTarget);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotSmooth * Time.deltaTime);

			SmoothDampPosition(transform.position, curWayPoint.position);
		}
		else {
			waypointIndx++;

			if (waypointIndx > waypoints.Count - 1)
				waypointIndx = 0;

			curWayPoint = waypoints[waypointIndx];
		}
	}

	private void SmoothDampPosition(Vector3 from, Vector3 to) {
		//transform.position = Vector3.SmoothDamp(from, to, ref camVelocity, camDampTime);
		transform.position = Vector3.MoveTowards(from, to, speed * Time.deltaTime);
	}
}