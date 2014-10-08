using UnityEngine;
using System.Collections;
using BellaProject;
using Bindings;

public class AirplaneController : MonoBehaviour
{
		//Public
		public float distanceTraveled;
		public GameObject landingPlatform;
		public float timeLeftUntilStart;
		public float timeLeftUntilToShop;
		public float _score, _scoreAdd, currentScore;

		//Private
		private bool isLanded;
		private bool isLanding;
		private bool isTakingOff;
		private bool _status;
		private float durationOfWeakBreath;
		private float timeLeftUntilTakeOff;

		//Const
		const float ReadyTimeDuration = 3f;
		const float NormalFlyUpForce = 4f;
		const float TakeOffForce = 3f;
		const float WeakBreathDuationThreshHold = 2f;
		const float TurbulanceRotation = 300f;
		const float TurbulanceUpForce = 3f;
		[SerializeField]
		Color
				tooHigh;
		[SerializeField]
		Color
				good;
		[SerializeField]
		Color
				tooLow;
		[SerializeField]
		private string
				bindingForCurrentValue;
		[SerializeField]
		private string
				bindingForMinValue;
		[SerializeField]
		private string
				bindingForMaxValue;
		[SerializeField]
		private string
				bindingForMinThresh;
		Property<float> propertyForCurrentValue;
		Property<float> propertyForMinValue;
		Property<float> propertyForMaxValue;
		Property<float> propertyForMinThresh;

	string skinname;

		//Init
		void Start ()
		{

				if (GameObject.Find ("Global") != null) {
					skinname = GameObject.Find ("Global").GetComponent<global> ().skinname;
					GetComponent<SpriteRenderer> ().sprite = Resources.Load (skinname, typeof(Sprite)) as Sprite;
				} 


				this.InitializeBindings ();
				this.InitializeTimedEvents ();
				
				this.distanceTraveled = 0;
				this.isLanding = false;
				this.isLanded = false;
				this.StartTakingOff ();
				this._score = (float)PlayerPrefs.GetInt ("Player Score");
		}

		private void InitializeBindings ()
		{
				this.propertyForCurrentValue = new Property<float> (0);
				this.propertyForCurrentValue.AddToBinding (bindingForCurrentValue, BindingDirection.BindingToProperty, AssignmentOnAdd.TakeBindingValue);
				this.propertyForMinValue = new Property<float> (0);
				this.propertyForMinValue.AddToBinding (bindingForMinValue, BindingDirection.BindingToProperty, AssignmentOnAdd.TakeBindingValue);
				this.propertyForMaxValue = new Property<float> (0);
				this.propertyForMaxValue.AddToBinding (bindingForMaxValue, BindingDirection.BindingToProperty, AssignmentOnAdd.TakeBindingValue);
				this.propertyForMinThresh = new Property<float> (0);
				this.propertyForMinThresh.AddToBinding (bindingForMinThresh, BindingDirection.BindingToProperty, AssignmentOnAdd.TakeBindingValue);
		}

		private void InitializeTimedEvents ()
		{
				timeLeftUntilToShop = ReadyTimeDuration;
				durationOfWeakBreath = 0;
				timeLeftUntilStart = ReadyTimeDuration;
				timeLeftUntilTakeOff = ReadyTimeDuration;
		}

		//Each Frame
		void Update ()
		{
				HandleKey ();
				this.HandleTimedEvents ();
				if (isLanded) {
						this.GoToShop ();
				} else if (isLanding) {
						this.Land ();

				} else if (isTakingOff) {
						this.TakeOff ();
				} else {
						this.HandleBreath ();
				}
				
		handleScore ();
		}
	
		private void HandleTimedEvents ()
		{
				timeLeftUntilStart -= Time.deltaTime;
		}

		private void handleScore() {
				_score += _scoreAdd;
		}

		private void HandleBreath ()
		{
				if (IsGoodBreath ()) {
						FlyNormally ();
						this.durationOfWeakBreath = 0;
				} else if (IsStrongBreath ()) {
						Turbulance ();
						this.durationOfWeakBreath = 0;
				} else if (IsWeakBreath ()) {
						Turbulance ();
				}
				distanceTraveled += 3f;
				transform.Translate (3f * Time.deltaTime, 0f, 0f);
		}

		private void HandleKey ()
		{
				if (Input.GetKey (KeyCode.Alpha4)) {
						Manager.messenger.Publish (this, BellaMessages.SetEnd);	
				}
		}


		//Checks
		private bool IsStillWeak ()
		{
				return this.durationOfWeakBreath > WeakBreathDuationThreshHold && 
						propertyForCurrentValue.value > propertyForMinThresh.value && 
						propertyForCurrentValue.value < propertyForMinValue.value;
		}

		private bool IsWeakBreath ()
		{
				if (propertyForCurrentValue.value > propertyForMinThresh.value 
						&& propertyForCurrentValue.value < propertyForMinValue.value) {
						this.durationOfWeakBreath += Time.deltaTime;
				} else {
						this.durationOfWeakBreath = 0;
				}
		
				if (IsStillWeak ()) {
						return true;
				} else {
						return false;
				}
		}
	
		private bool IsStrongBreath ()
		{
				return propertyForCurrentValue.value > propertyForMaxValue.value;
		}
	
		private bool IsGoodBreath ()
		{
				return propertyForCurrentValue.value >= propertyForMinValue.value && propertyForCurrentValue.value <= propertyForMaxValue.value;
		}

		public bool IsKinematic ()
		{
				return this.rigidbody2D.isKinematic;
		}

		public void EnableForces ()
		{
				this.rigidbody2D.isKinematic = false;
		}

		//Movement
		private void FlyNormally ()
		{	
				BackToNormal ();
				this.rigidbody2D.AddForce (Vector2.up * NormalFlyUpForce);
				_scoreAdd += 1f;
		}

		private void BackToNormal ()
		{
				transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.identity, 2f * Time.deltaTime);
		}

		private void Turbulance ()
		{
				transform.Rotate (0, 0, TurbulanceRotation * Time.deltaTime);
				transform.Translate (0, TurbulanceUpForce * Time.deltaTime, 0f);
				_scoreAdd -= 1f;
		}

		public void StartLanding ()
		{
				this.isLanding = true;
				this.rigidbody2D.gravityScale = 0f;
				this.rigidbody2D.velocity = new Vector2 (2f, -5f);
		                               
		}

		private void Land ()
		{
				transform.Translate (3f * Time.deltaTime, 0f, 0f);
		}

		void OnCollisionEnter2D (Collision2D collision)
		{
				if (collision.gameObject.tag == "LandingPlatform")
						this.isLanded = true;
		}

		public bool IsLanded ()
		{
				return this.isLanded;
		}

		private void StartTakingOff ()
		{
				this.isTakingOff = true;
		}
	
		private void TakeOff ()
		{
				if (this.timeLeftUntilTakeOff > 0) {
						transform.Translate (3f * Time.deltaTime, 0f, 0f);
						if (this.timeLeftUntilTakeOff <= 1.25f) {
								this.rigidbody2D.AddForce (Vector3.up * Mathf.Lerp (0f, TakeOffForce, ReadyTimeDuration));
						}
						this.timeLeftUntilTakeOff -= Time.deltaTime;
				} else {
						this.isTakingOff = false;
				}
		}

		private void GoToShop ()
		{
				this.timeLeftUntilToShop -= Time.deltaTime;
				if (this.timeLeftUntilToShop < 0) {
				Application.LoadLevel ("Shop");
						//Call application load level
				}

		}
	
		public void CreateLandingPlatform ()
		{
				GameObject platformObject = Instantiate (landingPlatform)as GameObject;
				var platformObjectPosition = new Vector3 (this.rigidbody2D.position.x, this.rigidbody2D.position.y - 10f, 0f);
		
				platformObject.transform.Translate (platformObjectPosition);
		}


	public float scoreAdd {
				get {
						_score += _scoreAdd;
						currentScore = _scoreAdd;
						_scoreAdd = 0f;
						return currentScore;
				}
		}

	public bool status {
				get { return _status; }
				set { _status = value; }
		}

	public float score {
		get { return _score;} 
	}



}
