//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Collider dangling from the player's head
//
//=============================================================================

using UnityEngine;
using System.Collections;
using UnityEngine.Animations;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	//[RequireComponent( typeof( CapsuleCollider ) )]   // edit to main
	public class BodyCollider : MonoBehaviour
	{
		public Transform head;

        Vector3 headPos;

		private CapsuleCollider capsuleCollider;

        // edit to main
        [SerializeField]
        private bool isCollider;
        [SerializeField]
        float fromHeadDistance;

    //-------------------------------------------------
    void Awake()
        {
            if (GetComponent<CapsuleCollider>() != null)
            {
                capsuleCollider = GetComponent<CapsuleCollider>();
            }
        }


        //-------------------------------------------------
        void FixedUpdate()
        {
            // float distanceFromFloor = Vector3.Dot( head.localPosition, Vector3.up );
            // capsuleCollider.height = Mathf.Max( capsuleCollider.radius, distanceFromFloor );
            // transform.localPosition = head.localPosition - 0.5f * distanceFromFloor * Vector3.up;

            // edit to main script, the above one doesn't do great
            headPos = head.transform.position;

            if (isCollider)
            {
                var amount = headPos.y - fromHeadDistance;
                capsuleCollider.height = amount;
                Vector3 center = capsuleCollider.center;
                center.y = amount / 1.82f;
                capsuleCollider.center = center;
            }

            headPos.y = 0;
            transform.position = headPos;
        }
	}
}
