using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000FF RID: 255
	public class SubcameraDriver : MonoBehaviour
	{
		// Token: 0x060008F8 RID: 2296 RVA: 0x000338B8 File Offset: 0x00031AB8
		public void Init()
		{
			if (this.subcameras != null)
			{
				return;
			}
			if (!PlayDataLoader.Loaded)
			{
				return;
			}
			Camera camera = Find.Camera;
			this.subcameras = new Camera[DefDatabase<SubcameraDef>.DefCount];
			foreach (SubcameraDef subcameraDef in DefDatabase<SubcameraDef>.AllDefsListForReading)
			{
				Camera camera2 = new GameObject
				{
					name = subcameraDef.defName,
					transform = 
					{
						parent = base.transform,
						localPosition = Vector3.zero,
						localScale = Vector3.one,
						localRotation = Quaternion.identity
					}
				}.AddComponent<Camera>();
				camera2.orthographic = camera.orthographic;
				camera2.orthographicSize = camera.orthographicSize;
				camera2.cullingMask = (subcameraDef.layer.NullOrEmpty() ? 0 : LayerMask.GetMask(new string[]
				{
					subcameraDef.layer
				}));
				camera2.nearClipPlane = camera.nearClipPlane;
				camera2.farClipPlane = camera.farClipPlane;
				camera2.useOcclusionCulling = camera.useOcclusionCulling;
				camera2.allowHDR = camera.allowHDR;
				camera2.renderingPath = camera.renderingPath;
				camera2.enabled = subcameraDef.startEnabled;
				camera2.clearFlags = CameraClearFlags.Color;
				camera2.backgroundColor = subcameraDef.backgroundColor;
				camera2.depth = (float)subcameraDef.depth;
				this.subcameras[(int)subcameraDef.index] = camera2;
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00033A4C File Offset: 0x00031C4C
		public void UpdatePositions(Camera camera)
		{
			if (this.subcameras == null)
			{
				return;
			}
			for (int i = 0; i < this.subcameras.Length; i++)
			{
				SubcameraDef subcameraDef = DefDatabase<SubcameraDef>.AllDefsListForReading[i];
				if (!subcameraDef.doNotUpdate)
				{
					Camera camera2 = this.subcameras[i];
					camera2.orthographicSize = camera.orthographicSize;
					RenderTexture renderTexture = camera2.targetTexture;
					if (renderTexture != null && (renderTexture.width != Screen.width || renderTexture.height != Screen.height))
					{
						Object.Destroy(renderTexture);
						renderTexture = null;
					}
					if (renderTexture == null)
					{
						renderTexture = new RenderTexture(Screen.width, Screen.height, 0, subcameraDef.BestFormat);
					}
					if (!renderTexture.IsCreated())
					{
						renderTexture.Create();
					}
					camera2.targetTexture = renderTexture;
				}
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00033B0A File Offset: 0x00031D0A
		public Camera GetSubcamera(SubcameraDef def)
		{
			if (this.subcameras == null || def == null || this.subcameras.Length <= (int)def.index)
			{
				return null;
			}
			return this.subcameras[(int)def.index];
		}

		// Token: 0x04000429 RID: 1065
		private Camera[] subcameras;
	}
}
