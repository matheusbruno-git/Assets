using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool attack1;
		public bool attack2;
		public bool crouch;
		public bool dodge;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}
		public void OnAttack1(InputValue value)
		{
			Attack1Input(value.isPressed);
		}
		public void OnAttack2(InputValue value)
		{
			Attack2Input(value.isPressed);
		}
		public void OnCrouch(InputValue value)
		{
			CrouchInput(value.isPressed);
		}
		public void OnDodge(InputValue value)
		{
			DodgeInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}
		public void Attack1Input(bool newAttack1State)
		{
			attack1 = newAttack1State;
		}
		public void Attack2Input(bool newAttack2State)
		{
			attack2 = newAttack2State;
		}
		public void CrouchInput(bool newCrouchState)
		{
			crouch = newCrouchState;
		}
		public void DodgeInput(bool newDodgeState)
		{
			dodge = newDodgeState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}