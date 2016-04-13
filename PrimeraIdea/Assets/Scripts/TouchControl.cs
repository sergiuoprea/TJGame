using UnityEngine;
using System.Collections;

public enum SwipeAction
{
	Nothing = -1,
	Tap = 0,
	DoubleTap = 1,
	LeftSwipe = 2,
	RightSwipe = 3, 
	UpSwipe = 4,
	DownSwipe = 5
}

public class TouchControl : MonoBehaviour {


	private Vector2 startPos = Vector2.zero;
	private bool couldBeSwipe = false;
	private float comfortZone;
	private float minSwipeDist = 50.0f;
	private float maxSwipeTime = 0.3f;
	private float startTime;

	private SwipeAction direction;

	// Use this for initialization
	void Start () {

		direction = SwipeAction.Nothing;
	
	}


	
	// Update is called once per frame
	void Update () {

		direction = SwipeAction.Nothing;


		if (Input.touchCount > 0) 
		{
			float swipeTime;
			float swipeDist;
			Touch touch = Input.touches [0];

			if (Input.touchCount == 1)
			{
				switch (touch.phase)
				{
				case TouchPhase.Began:
					couldBeSwipe = true;
					startPos = touch.position;
					startTime = Time.time;

					break;

				case TouchPhase.Moved:

					swipeTime = Time.time - startTime;
					swipeDist = (touch.position - startPos).magnitude;

					if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) {
						// It's a swiiiiiiiiiiiipe!

						float h = Mathf.Abs (touch.position.x - startPos.x);
						float v = Mathf.Abs (touch.position.y - startPos.y);

						float hS = Mathf.Sign (touch.position.x - startPos.x);
						float vS = Mathf.Sign (touch.position.y - startPos.y);                        

						if (hS >= 0 && h > v)
							direction = SwipeAction.RightSwipe;
						else if (hS <= 0 && h > v)
							direction = SwipeAction.LeftSwipe;
						else if (vS >= 0 && v > h)
							direction = SwipeAction.UpSwipe;

						couldBeSwipe = false;

					}

					break;

				case TouchPhase.Stationary:
					direction = SwipeAction.Nothing;
					break;

				case TouchPhase.Ended:
					couldBeSwipe = true;
						/*
		                    swipeTime = Time.time - startTime;
		                    swipeDist = (touch.position - startPos).magnitude;

		                    if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist))
		                    {
		                        // It's a swiiiiiiiiiiiipe!

		                        if (Mathf.Sign(touch.position.x - startPos.x) >= 0)
		                            direction = 1;
		                        else
		                            direction = -1;
		                        
		                    }
		                    */

					break;
				}
			} 
			else if (Input.touchCount == 2) 
			{
				direction = SwipeAction.DoubleTap;
			}
		} 


	}


	public SwipeAction GetSwipeAction()
	{
		return direction;
	}
}
