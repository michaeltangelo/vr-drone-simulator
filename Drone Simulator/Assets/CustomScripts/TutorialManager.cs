using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	enum TutorialNode{Intro01, Intro02, PilotCtrlLeft, PilotCtrlRight, SwitchMode, OperatorCtrlRight, MissileFire }

	[SerializeField] private GameObject dialogBoxRoot;
	[SerializeField] private Text dialogBoxText;

//	Welcome to UAVR, a VR drone flight simulator! Today, you will be flying the MQ-9 Reaper. Press 'x' to get started.
//
//	2. The MQ-9 Reaper is an unmanned aerial vehicle (UAV) and is operated by two people at a time: the pilot, and the sensor operator. Press 'x' to continue.
//
//	3. If you look to your right, you'll notice an empty seat beside you. This is where the sensor operator sits, and it also means that you're the pilot! Press 'x' to continue.
//
//	4. Let's begin! Take a look at the control stick on the left, known as the "throttle". Try pushing the left joystick of your PS4 controller forwards and backwards and see what happens. Press 'x' to move on.
//	// user will push the left joystick and the throttle will move forward and backwards 
//
//	5. Great! The joystick on your right controls the orientation of your drone. See how your drone reacts by moving the right joystick of your PS4 controller. Press 'x' to continue.
//	// user will play with the right ps4 joystick and see the drone's flaps move accordingly
//
//	6. As the pilot, your controls move the drone directly. Let's see what happens as the sensor operator. Press 'o' to switch into sensor operator mode.
//	// user presses O
//
//	7. Now, try moving your right joystick. Notice how the camera swivels, but the drone itself does not move. Don't be upset though, as the sensor operator you can fire missiles! Press 'x' to continue.
//
//	8. Aim the camera to a target and hit the right trigger to fire a missile! Press 'x' to continue.
//
//	9. You can switch between pilot and sensor operator modes mid-flight by pressing 'o'. Try switching to pilot mode now. Press 'o' to switch to pilot mode.
//
//	10. You're all set! Push your left stick forward to activate the throttle, and control your drone with the right. Good luck!
//
	Dictionary<TutorialNode, string> dialogs = new Dictionary<TutorialNode, string>(){
		{TutorialNode.Intro01, "Welcome to UAVR, a VR drone flight simulator! Today, you will be flying the MQ-9 Reaper."},
		{TutorialNode.Intro02, "The MQ-9 Reaper is an unmanned aerial vehicle (UAV) and is operated by two people at a time: the pilot, and the sensor operator." },
		{TutorialNode.PilotCtrlLeft, "Let's begin! You are now the Pilot. Take a look at the control stick on the left, known as the \"throttle\". Try pushing the left joystick of your PS4 controller forwards and backwards and see what happens."},
		{TutorialNode.PilotCtrlRight, "Great! The joystick on your right controls the orientation of your drone. See how your drone reacts by moving the right joystick of your PS4 controller." },
		{TutorialNode.SwitchMode, "As the pilot, your controls move the drone directly. Let's see what happens as the sensor operator. Press 'o' to switch into sensor operator mode."},
		{TutorialNode.OperatorCtrlRight, "Now, try moving your right joystick. Notice how the camera swivels, but the drone itself does not move. Don't be upset though, as the sensor operator you can fire missiles!"},
		{TutorialNode.MissileFire, "Aim the camera to a target and hit the right trigger to fire a missile! Press 'x' to continue."}
	};

	TutorialNode curNode = TutorialNode.Intro01;

	public void Transition(){
		Debug.Log("Transitioning to next step");
		switch(curNode){
		case TutorialNode.Intro01:
//		case TutorialNode.Intro02:
			curNode = (TutorialNode)((int)curNode + 1);
			dialogBoxText.text = dialogs[curNode];
			break;
		}
	}
}
