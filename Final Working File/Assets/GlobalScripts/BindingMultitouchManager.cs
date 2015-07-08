using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** More functionailty will be added to this if required
 * 
 * Objects require a collider to be touched (as with Unity's inbuilt click functions)
 * 
 * This script currently sends three method calls. 
 * They can be not implemented if it is not required.
 * The receiving method can choose to ignore the argument by having zero arguments.
 * 
 * // _vTouchHit is the position the touch raycast has hit the object in world space.
 * // _vTouchPosition is the origin of the touch raycast.
 * 
 * void OnTouchDown	( Vector3 _vTouchHit )
 * void OnTouchDrag	( Vector3 _vTouchPosition )
 * void OnTouchUp	( Vector3 _vTouchPosition )
 */
public class BindingMultitouchManager : MonoBehaviour
{
	public	float	m_fRaycastDistance = 100.0f;
	
	private	List<TouchBinding>	m_lBindedTouches = new List<TouchBinding>();
	
	void Update()
	{
		for( int n = 0; n < Input.touchCount; ++n )
		{
			Touch oTouch = Input.GetTouch(n);
			{
				// Even if we're not raycasting
				Ray oRay = Camera.main.ScreenPointToRay(oTouch.position);
				
				switch(oTouch.phase)
				{
				case TouchPhase.Began:
				{
					// Finds the touched object to bind the touch to
					RaycastHit oHit = new RaycastHit();

					if ( Physics.Raycast(oRay, out oHit, m_fRaycastDistance) )
					{
						// Bind the touch to the gameobject
						TouchBinding oBinding = new TouchBinding(oTouch.fingerId, oHit.collider.gameObject);
						
						// Calls OnTouchDown function if available
						oBinding.m_goTouched.SendMessage("OnTouchDown", oHit.point, SendMessageOptions.DontRequireReceiver);
						
						// Add the binded touch to the gameobject
						m_lBindedTouches.Add ( oBinding );
					}
					break;
				}
				case TouchPhase.Moved:
				{
					// Find our binded touch
					TouchBinding oBinded = m_lBindedTouches.Find(BindedTouch => BindedTouch.m_nFingerID == oTouch.fingerId);
					
					// Only proceed if we have an object binded
					if ( oBinded.m_goTouched )
					{
						// Calls OnTouchDrag function if available
						oBinded.m_goTouched.SendMessage("OnTouchDrag", oRay.origin, SendMessageOptions.DontRequireReceiver);
					}
					break;
				}
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
				{
					// Find our binded touch
					TouchBinding oBinded = m_lBindedTouches.Find(BindedTouch => BindedTouch.m_nFingerID == oTouch.fingerId);
					
					// Only proceed if we have an object binded
					if ( oBinded.m_goTouched )
					{
						// Calls OnTouchUp function if available when finger is lifted or systems stops touch tracking
						oBinded.m_goTouched.SendMessage("OnTouchUp", oRay.origin, SendMessageOptions.DontRequireReceiver);
					}
					// Unbind our touch !
					// Can't use remove at because it might not go in sequence
					m_lBindedTouches.RemoveAll( BindedTouch => BindedTouch.m_nFingerID == oTouch.fingerId );
					break;
				}
				case TouchPhase.Stationary: // Do we need this?
				default:
					break;
				}
			}
		}
	}
	
	private struct TouchBinding
	{
		public	int			m_nFingerID;
		public	GameObject	m_goTouched;
		
		public TouchBinding(int _nFingerID, GameObject _goTouched)
		{
			m_nFingerID = _nFingerID;
			m_goTouched = _goTouched;
		}
	}
}
