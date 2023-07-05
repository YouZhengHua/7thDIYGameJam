using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
	public class CameraController : ICameraController
	{
		private Camera _mainCamera;

		public CameraController()
		{
			_mainCamera = Camera.main;
		}

		public void MoveMainCameraTo(Vector2 target)
		{
			_mainCamera.transform.position = new Vector3(target.x, target.y, _mainCamera.transform.position.z);
		}
	}
}
