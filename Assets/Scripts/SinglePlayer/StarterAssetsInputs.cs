using UnityEngine;

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
    {
        public bool canInput;
        [Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
        

        private void Update()
        {
            if (canInput)
            {
                Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                MoveInput(moveDirection);

                Vector2 lookDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * -1);
                LookInput(lookDirection);

                JumpInput(Input.GetButtonDown("Jump"));

                SprintInput(Input.GetKey(KeyCode.LeftShift));
            }

            else
            {
                MoveInput(Vector2.zero);
                LookInput(Vector2.zero);
                JumpInput(false);
                SprintInput(false);
            }
        }

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

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        public void SetCanInput(bool canInput)
        {
            this.canInput = canInput;
        }

    }

}