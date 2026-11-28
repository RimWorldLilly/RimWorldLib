using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020014F2 RID: 5362
	public static class PawnCacheCameraManager
	{
		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06008382 RID: 33666 RVA: 0x002F3A01 File Offset: 0x002F1C01
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.pawnCacheCameraInt;
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06008383 RID: 33667 RVA: 0x002F3A08 File Offset: 0x002F1C08
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.pawnCacheRendererInt;
			}
		}

		// Token: 0x06008385 RID: 33669 RVA: 0x002F3A2C File Offset: 0x002F1C2C
		private static Camera CreatePawnCacheCamera()
		{
			GameObject gameObject = new GameObject("PortraitCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<PawnCacheRenderer>();
			Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.position = new Vector3(0f, 10f, 0f);
			component.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			component.orthographic = true;
			component.cullingMask = 0;
			component.orthographicSize = 1f;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = new Color(0f, 0f, 0f, 0f);
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 5f;
			component.farClipPlane = 12f;
			return component;
		}

		// Token: 0x04005139 RID: 20793
		private static Camera pawnCacheCameraInt = PawnCacheCameraManager.CreatePawnCacheCamera();

		// Token: 0x0400513A RID: 20794
		private static PawnCacheRenderer pawnCacheRendererInt = PawnCacheCameraManager.pawnCacheCameraInt.GetComponent<PawnCacheRenderer>();
	}
}
