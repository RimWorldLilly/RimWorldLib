using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200230A RID: 8970
	[StaticConstructorOnStartup]
	public static class WorldCameraManager
	{
		// Token: 0x170026B5 RID: 9909
		// (get) Token: 0x0600DD87 RID: 56711 RVA: 0x005280A1 File Offset: 0x005262A1
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		// Token: 0x170026B6 RID: 9910
		// (get) Token: 0x0600DD88 RID: 56712 RVA: 0x005280A8 File Offset: 0x005262A8
		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		// Token: 0x170026B7 RID: 9911
		// (get) Token: 0x0600DD89 RID: 56713 RVA: 0x005280AF File Offset: 0x005262AF
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		// Token: 0x0600DD8A RID: 56714 RVA: 0x005280B8 File Offset: 0x005262B8
		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		// Token: 0x0600DD8B RID: 56715 RVA: 0x00528168 File Offset: 0x00526368
		private static Camera CreateWorldCamera()
		{
			GameObject gameObject = new GameObject("WorldCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<WorldCameraDriver>();
			Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldLayerMask;
			component.clearFlags = CameraClearFlags.Depth;
			component.useOcclusionCulling = true;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 7500f;
			component.fieldOfView = 20f;
			component.depth = 1f;
			return component;
		}

		// Token: 0x0600DD8C RID: 56716 RVA: 0x00528200 File Offset: 0x00526400
		private static Camera CreateWorldSkyboxCamera(Camera parent)
		{
			GameObject gameObject = new GameObject("WorldSkyboxCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(true);
			Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.SetParent(parent.transform);
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldSkyboxLayerMask;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = WorldCameraManager.SkyColor;
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 7500f;
			component.fieldOfView = 60f;
			component.depth = 0f;
			return component;
		}

		// Token: 0x04008E42 RID: 36418
		private static Camera worldCameraInt;

		// Token: 0x04008E43 RID: 36419
		private static Camera worldSkyboxCameraInt;

		// Token: 0x04008E44 RID: 36420
		private static WorldCameraDriver worldCameraDriverInt;

		// Token: 0x04008E45 RID: 36421
		public static readonly string WorldLayerName = "World";

		// Token: 0x04008E46 RID: 36422
		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		// Token: 0x04008E47 RID: 36423
		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		// Token: 0x04008E48 RID: 36424
		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		// Token: 0x04008E49 RID: 36425
		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		// Token: 0x04008E4A RID: 36426
		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		// Token: 0x04008E4B RID: 36427
		public const int FarClipPlane = 7500;

		// Token: 0x04008E4C RID: 36428
		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.11764706f);
	}
}
