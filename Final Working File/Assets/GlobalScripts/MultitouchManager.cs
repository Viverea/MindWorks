using UnityEngine;
using System.Collections;


/** More functionailty will be added to this if required
 * 
 * Objects require a collider to be touched (as with Unity's inbuilt click functions)
 * This does not bind the touch to the object. 
 * ie. The object will stop recieving method calls if the touch moves out of the object's collider.
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
public class MultitouchManager : MonoBehaviour
{
	public	float	m_fRaycastDistance = 100.0f;
	
	void Update()
	{
		for( int n = 0; n < Input.touchCount; ++n )
		{
			Touch oTouch = Input.GetTouch(n);
			
			Ray oRay = Camera.main.ScreenPointToRay(oTouch.position);
			RaycastHit oHit = new RaycastHit();
			if ( Physics.Raycast(oRay, out oHit, m_fRaycastDistance) )
			{
				switch(oTouch.phase)
				{
				case TouchPhase.Began:
				{
					// Calls OnTouchDown function if available
					oHit.collider.gameObject.SendMessage("OnTouchDown", SendMessageOptions.DontRequireReceiver);
					break;
				}
				case TouchPhase.Moved:
				{
					// Calls OnTouchDrag function if available
					oHit.collider.gameObject.SendMessage("OnTouchDrag", oRay.origin, SendMessageOptions.DontRequireReceiver);
					break;
				}
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
				{
					// Calls OnTouchUp function if available when finger is lifted or systems stops touch tracking
					oHit.collider.gameObject.SendMessage("OnTouchUp", SendMessageOptions.DontRequireReceiver);
					break;
				}
				case TouchPhase.Stationary: // Do we need this?
				default:
					break;
				}
			}
		}
	}
}
