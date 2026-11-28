using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001617 RID: 5655
	[StaticConstructorOnStartup]
	public class GravshipCapturer
	{
		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x06008CE2 RID: 36066 RVA: 0x00333129 File Offset: 0x00331329
		// (set) Token: 0x06008CE3 RID: 36067 RVA: 0x00333131 File Offset: 0x00331331
		public bool IsCaptureComplete { get; private set; }

		// Token: 0x06008CE4 RID: 36068 RVA: 0x0033313C File Offset: 0x0033133C
		public void BeginGravshipRender(Building_GravEngine engine, Action<Capture> onComplete)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			this.onComplete = onComplete;
			this.captureInProgress = new Capture(engine);
			GravshipCapturer.IsGravshipRenderInProgress = true;
			this.IsCaptureComplete = false;
			Find.ScreenshotModeHandler.Active = true;
			CellRect cellRect = this.captureInProgress.captureBounds = CellRect.FromCellList(engine.ValidSubstructure).ExpandedBy(1);
			GravshipCapturer.GravshipCaptureBounds = cellRect;
			Camera camera = Find.Camera;
			this.captureCam = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.GravshipMask);
			this.captureCam.cullingMask = ((camera.cullingMask & ~GravshipCapturer.GravshipCaptureLayerMaskExclude) | GravshipCapturer.GravshipCaptureLayerMaskInclude);
			this.captureCam.Fit(cellRect, 15f);
			this.captureInProgress.drawSize = cellRect.Size.ToVector3().WithY(1f);
			this.captureInProgress.captureCenter = cellRect.CenterVector3;
			this.captureInProgress.maxCameraSize = Find.CameraDriver.config.sizeRange.max;
			this.captureInProgress.minCameraSize = Mathf.Max(Mathf.Max(Find.CameraDriver.config.sizeRange.min, this.captureInProgress.maxCameraSize / 2f), this.captureCam.orthographicSize + 10f);
			int screenshotWidth = Mathf.RoundToInt((float)Screen.height * this.captureCam.aspect);
			int screenshotHeight = Screen.height;
			RenderTexture screenshot = RenderTexture.GetTemporary(screenshotWidth, screenshotHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear, 1);
			this.captureCam.targetTexture = screenshot;
			this.DrawChromaKey();
			OnPostRenderHook.HookOnce(this.captureCam, delegate
			{
				this.captureCam.enabled = false;
				this.captureCam.targetTexture = null;
				if (DebugViewSettings.saveGravshipRenders)
				{
					screenshot.SaveAsPNG("Logs/gravship-screenshot.png");
				}
				RenderTexture temporary = RenderTexture.GetTemporary(screenshotWidth, screenshotHeight, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
				if (DebugViewSettings.disableGravshipRenderShader)
				{
					Graphics.Blit(screenshot, temporary);
				}
				else
				{
					Graphics.Blit(screenshot, temporary, GravshipCapturer.MatGravshipBlit);
				}
				this.captureInProgress.capture = (SavedTexture2D)temporary.CreateTexture2D(TextureFormat.ARGB32, true);
				this.captureInProgress.capture.Texture.filterMode = FilterMode.Bilinear;
				if (DebugViewSettings.saveGravshipRenders)
				{
					this.captureInProgress.capture.Texture.SaveAsPNG("Logs/gravship-render.png");
				}
				RenderTexture.active = null;
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(screenshot);
				GravshipCapturer.IsGravshipRenderInProgress = false;
				this.framesToWait = 1;
			});
			this.captureCam.enabled = true;
		}

		// Token: 0x06008CE5 RID: 36069 RVA: 0x00333320 File Offset: 0x00331520
		public void BeginTerrainRender(CellRect landingBounds, Action<Capture> onComplete)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			this.onComplete = onComplete;
			this.captureInProgress = new Capture(null);
			GravshipCapturer.IsGravshipRenderInProgress = true;
			this.IsCaptureComplete = false;
			Find.ScreenshotModeHandler.Active = true;
			Camera camera = Find.Camera;
			this.captureCam = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.GravshipMask);
			this.captureCam.cullingMask = camera.cullingMask;
			this.captureCam.Fit(landingBounds, 15f);
			this.captureInProgress.captureBounds = landingBounds;
			this.captureInProgress.drawSize = landingBounds.Size.ToVector3().WithY(1f);
			this.captureInProgress.captureCenter = landingBounds.CenterVector3;
			this.captureInProgress.maxCameraSize = Find.CameraDriver.config.sizeRange.max;
			this.captureInProgress.minCameraSize = Mathf.Max(Mathf.Max(Find.CameraDriver.config.sizeRange.min, this.captureInProgress.maxCameraSize / 2f), this.captureCam.orthographicSize + 10f);
			int width = Mathf.RoundToInt((float)Screen.height * this.captureCam.aspect);
			int height = Screen.height;
			RenderTexture screenshot = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
			this.captureCam.targetTexture = screenshot;
			OnPostRenderHook.HookOnce(this.captureCam, delegate
			{
				this.captureCam.enabled = false;
				this.captureCam.targetTexture = null;
				Texture2D texture2D = screenshot.CreateTexture2D(TextureFormat.ARGB32, true);
				texture2D.filterMode = FilterMode.Bilinear;
				if (DebugViewSettings.saveGravshipRenders)
				{
					texture2D.SaveAsPNG("Logs/terrain-screenshot.png");
				}
				this.captureInProgress.capture = (SavedTexture2D)texture2D;
				RenderTexture.active = null;
				RenderTexture.ReleaseTemporary(screenshot);
				GravshipCapturer.IsGravshipRenderInProgress = false;
				this.framesToWait = 1;
			});
			this.captureCam.enabled = true;
		}

		// Token: 0x06008CE6 RID: 36070 RVA: 0x003334C0 File Offset: 0x003316C0
		public static void CaptureWorldSkybox(Action<Texture2D> callback)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			Find.WorldCameraDriver.ApplyMapPositionToGameObject();
			GameObject worldCameraCopyGO = new GameObject("TerrainCurtainWorldCamera");
			GameObject worldSkyboxCameraCopyGO = new GameObject("TerrainCurtainWorldSkyboxCamera");
			Camera worldCameraCopy = worldCameraCopyGO.AddComponent<Camera>();
			Camera worldSkyboxCameraCopy = worldSkyboxCameraCopyGO.AddComponent<Camera>();
			worldCameraCopy.CopyFrom(WorldCameraManager.WorldCamera);
			worldSkyboxCameraCopy.CopyFrom(WorldCameraManager.WorldSkyboxCamera);
			RenderTexture screenshot = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
			worldCameraCopy.targetTexture = screenshot;
			worldSkyboxCameraCopy.targetTexture = screenshot;
			Action <>9__1;
			OnPostRenderHook.HookOnce(worldCameraCopy, delegate
			{
				Camera worldCameraCopy = worldCameraCopy;
				Action postRender;
				if ((postRender = <>9__1) == null)
				{
					postRender = (<>9__1 = delegate()
					{
						worldCameraCopy.targetTexture = null;
						worldSkyboxCameraCopy.targetTexture = null;
						Texture2D texture2D = screenshot.CreateTexture2D(TextureFormat.ARGB32, false);
						if (DebugViewSettings.saveGravshipRenders)
						{
							texture2D.SaveAsPNG("Logs/world-skybox-screenshot.png");
						}
						Object.Destroy(worldCameraCopyGO);
						Object.Destroy(worldSkyboxCameraCopyGO);
						RenderTexture.ReleaseTemporary(screenshot);
						Action<Texture2D> callback2 = callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(texture2D);
					});
				}
				OnPostRenderHook.HookOnce(worldCameraCopy, postRender);
			});
			worldCameraCopyGO.SetActive(true);
			worldSkyboxCameraCopyGO.SetActive(true);
			worldCameraCopy.enabled = true;
			worldSkyboxCameraCopy.enabled = true;
		}

		// Token: 0x06008CE7 RID: 36071 RVA: 0x003335D8 File Offset: 0x003317D8
		public void Update()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (GravshipCapturer.IsGravshipRenderInProgress)
			{
				this.DrawChromaKey();
			}
			if (DebugViewSettings.drawGravshipMask)
			{
				this.DrawChromaKey(0, null);
			}
			if (this.framesToWait > -1)
			{
				if (this.framesToWait == 0)
				{
					this.IsCaptureComplete = true;
					Action<Capture> action = this.onComplete;
					if (action != null)
					{
						action(this.captureInProgress);
					}
					this.captureInProgress = null;
				}
				this.framesToWait--;
				return;
			}
		}

		// Token: 0x06008CE8 RID: 36072 RVA: 0x0033364E File Offset: 0x0033184E
		private void DrawChromaKey()
		{
			this.DrawChromaKey(SubcameraDefOf.GravshipMask.LayerId, this.captureCam);
		}

		// Token: 0x06008CE9 RID: 36073 RVA: 0x00333666 File Offset: 0x00331866
		private void DrawChromaKey(int layer, Camera camera)
		{
			SkyOverlay.DrawScreenOverlay(GravshipCapturer.MatGravshipOpaqueBase, -1f, layer, camera);
			SkyOverlay.DrawScreenOverlay(GravshipCapturer.MatGravshipChromaKey, AltitudeLayer.MetaOverlays.AltitudeFor() + 0.03658537f, layer, camera);
		}

		// Token: 0x0400565A RID: 22106
		private static readonly int GravshipCaptureLayerMaskExclude = LayerMask.GetMask(new string[]
		{
			"UI",
			"GravshipExclude"
		});

		// Token: 0x0400565B RID: 22107
		private static readonly int GravshipCaptureLayerMaskInclude = LayerMask.GetMask(new string[]
		{
			"GravshipMask"
		});

		// Token: 0x0400565C RID: 22108
		private static readonly Material MatGravshipBlit = MatLoader.LoadMat("Map/Gravship/GravshipBlit", -1);

		// Token: 0x0400565D RID: 22109
		private static readonly Material MatGravshipOpaqueBase = MatLoader.LoadMatDirect("Map/Gravship/GravshipOpaqueBase");

		// Token: 0x0400565E RID: 22110
		private static readonly Material MatGravshipChromaKey = MatLoader.LoadMatDirect("Map/Gravship/GravshipChromaKey");

		// Token: 0x0400565F RID: 22111
		private const float CAMERA_MAX_ZOOM_PADDING = 10f;

		// Token: 0x04005660 RID: 22112
		public static bool IsGravshipRenderInProgress = false;

		// Token: 0x04005661 RID: 22113
		public static CellRect GravshipCaptureBounds;

		// Token: 0x04005663 RID: 22115
		private Capture captureInProgress;

		// Token: 0x04005664 RID: 22116
		private Action<Capture> onComplete;

		// Token: 0x04005665 RID: 22117
		private int framesToWait = -1;

		// Token: 0x04005666 RID: 22118
		private Camera captureCam;
	}
}
