using UnityEngine;
using UnityEngine.Events;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace WinterboltGames.TouchInput.Scripts.Controls
{
	public sealed class Thumbstick : TouchControl
	{
		[SerializeField]
		private RectTransform thumb;

		[SerializeField]
		private float maxThumbDistance;

		private bool _isActive;

		public bool IsActive
		{
			get => _isActive;

			set
			{
				if (_isActive == value) return;

				_isActive = value;

				if (_isActive)
				{
					OnActivated?.Invoke();
				}
				else
				{
					OnDeactivated?.Invoke();
				}
			}
		}

		public UnityEvent OnActivated;
		public UnityEvent OnDeactivated;

		public Vector2 Input { get; private set; }

		protected override void Update()
		{
			base.Update();

			if (touchIndex == -1)
			{
				ResetThumbstick();
			}
			else
			{
				Touch touch = Touch.activeTouches[touchIndex];

				if (touch.phase == TouchPhase.Began)
				{
					IsActive = true;
				}
				else if (touch.phase is TouchPhase.Ended or TouchPhase.Canceled)
				{
					ResetThumbstick();
				}

				if (_isActive)
				{
					_ = RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, touch.screenPosition, null, out Vector2 position);

					position = Vector2.ClampMagnitude(position, maxThumbDistance);

					thumb.anchoredPosition = position;

					Input = new(position.x / maxThumbDistance, position.y / maxThumbDistance);
				}
				else
				{
					thumb.anchoredPosition = Vector2.zero;
				}
			}
		}

		private void ResetThumbstick()
		{
			touchId = -1;

			IsActive = false;

			thumb.anchoredPosition = Vector2.zero;

			Input = Vector2.zero;
		}
	}
}
