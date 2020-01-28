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
        private bool isCollider, isTrigger;
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
                Vector3 center = transform.InverseTransformPoint(headPos);
                center.y = 0.6f;
                
                capsuleCollider.center = center;
            }
            if (isTrigger)
            {
                headPos.y = 0;
                transform.position = headPos;
            }

        }
	}
}
