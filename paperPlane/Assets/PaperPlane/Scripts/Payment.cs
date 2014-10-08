using UnityEngine;
using System.Collections;
using BellaProject;

public class Payment : MonoBehaviour {

	private double _balance;
	private AirplaneController airplaneController;
	private LerpFollower follower;
	private float time;
	private float timer;
	private float timer2;
	private bool status;
	
	public GameObject landingPlatform;

	public const string GoodBreath = "GoodBreath";
	public const string WeakBreath = "WeakBreath";
	public const string StrongBreath = "StrongBreath";
	public const string ReadyForInput = "ReadyForInput";
	public const string BreakTimeStarted = "BreakTimeStarted";
	public const string BreakTimeMinReached = "BreakTimeMinReached";
	public const string SessionFinished = "SessionFinished";
	
	public const string ExhaleStart = "ExhaleStart";
	public const string ExhaleEnd = "ExhaleEnd";
	public const string GoodExhale = "GoodExhale";
	public const string BreathEnd = "BreathEnd";
	public const string SetEnd = "SetEnd";
	public const string GoodSet = "GoodSet";
	public const string SessionEnd = "SessionEnd";
	public const string StatusChanged = "StatusChanged";

	void Start ()
	{
		InitializeBalance ();
		InitializeGameObjects ();
		Manager.messenger.Subscribe (BellaMessages.GoodBreath, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.WeakBreath, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.StrongBreath, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.ReadyForInput, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.BreakTimeStarted, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.BreakTimeMinReached, OnMessage);	
		Manager.messenger.Subscribe (BellaMessages.SessionFinished, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.ExhaleStart, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.ExhaleEnd, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.GoodExhale, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.BreathEnd, OnMessage);	
		//Manager.messenger.Subscribe (BellaMessages.SetEnd, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.GoodSet, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.SessionEnd, OnMessage);
		Manager.messenger.Subscribe (BellaMessages.StatusChanged, OnMessage);	
		status = true;
	}

	void OnDestroy ()
	{
		Manager.messenger.Unsubscribe (BellaMessages.GoodBreath, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.WeakBreath, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.StrongBreath, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.ReadyForInput, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.BreakTimeStarted, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.BreakTimeMinReached, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.SessionFinished, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.ExhaleStart, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.ExhaleEnd, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.GoodExhale, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.BreathEnd, OnMessage);
		//Manager.messenger.Unsubscribe (BellaMessages.SetEnd, OnMessage);	
		Manager.messenger.Unsubscribe (BellaMessages.GoodSet, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.SessionEnd, OnMessage);
		Manager.messenger.Unsubscribe (BellaMessages.StatusChanged, OnMessage);
	}

	void Update () {
	}

	void InitializeBalance() {
		_balance = 100;
	}

	void InitializeGameObjects() {
		airplaneController = GameObject.Find("Plane").GetComponent<AirplaneController>();
		follower = GameObject.Find("Main Camera").GetComponent<LerpFollower>();
	}

	void OnMessage(Object sender, string msgID, float num1 = 0f, float num2 = 0f, float num3 = 0f, float num4 = 0f) {

		switch (msgID) {
		case BellaMessages.WeakBreath:
			_balance --;
			break;
		case BellaMessages.GoodBreath:
			_balance ++;
			break;
		case BellaMessages.StrongBreath:
			_balance --;
			break;
		case BellaMessages.ReadyForInput:
			if (airplaneController.IsKinematic()) {
				airplaneController.EnableForces();
			}
			break;
		case BellaMessages.BreakTimeStarted:
			break;
		case BellaMessages.BreakTimeMinReached:
			break;
		case BellaMessages.SessionFinished:
			break;
		case BellaMessages.ExhaleStart:
			status = true;
			break;
		case BellaMessages.ExhaleEnd:
			status = false;
			break;
		case BellaMessages.GoodExhale:
			break;
		case BellaMessages.BreathEnd:
			break;
		case BellaMessages.SetEnd:
			break;
		case BellaMessages.GoodSet:
			break;
		case BellaMessages.SessionEnd:
			break;
		case BellaMessages.StatusChanged:
			break;
		}
	}
}