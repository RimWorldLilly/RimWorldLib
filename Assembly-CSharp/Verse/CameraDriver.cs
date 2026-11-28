using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020000F3 RID: 243
	public class CameraDriver : MonoBehaviour
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0003192B File Offset: 0x0002FB2B
		private static float ScrollWheelZoomRate
		{
			get
			{
				if (!SteamDeck.IsSteamDeck)
				{
					return 0.35f;
				}
				return 0.55f;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0003193F File Offset: 0x0002FB3F
		public Camera MyCamera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00031961 File Offset: 0x0002FB61
		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				if (Screen.fullScreen || ResolutionUtility.BorderlessFullscreen)
				{
					return 6f;
				}
				return 20f;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0003197C File Offset: 0x0002FB7C
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x00031984 File Offset: 0x0002FB84
		public float RootSize
		{
			get
			{
				return this.rootSize;
			}
			private set
			{
				if (this.rootSize == value)
				{
					return;
				}
				if (Current.ProgramState != ProgramState.Playing || LongEventHandler.ShouldWaitForEvent || Find.Camera == null || !Prefs.ZoomToMouse)
				{
					this.rootSize = value;
					return;
				}
				this.ApplyPositionToGameObject();
				Vector3 a = UI.MouseMapPosition();
				this.rootSize = value;
				this.ApplyPositionToGameObject();
				this.rootPos += a - UI.MouseMapPosition();
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x000319FC File Offset: 0x0002FBFC
		public CameraZoomRange CurrentZoom
		{
			get
			{
				if (this.RootSize < this.config.sizeRange.min + 1f)
				{
					return CameraZoomRange.Closest;
				}
				if (this.RootSize < this.config.sizeRange.max * 0.23f)
				{
					return CameraZoomRange.Close;
				}
				if (this.RootSize < this.config.sizeRange.max * 0.7f)
				{
					return CameraZoomRange.Middle;
				}
				if (this.RootSize < this.config.sizeRange.max * 0.95f)
				{
					return CameraZoomRange.Far;
				}
				return CameraZoomRange.Furthest;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00031A8A File Offset: 0x0002FC8A
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00031A9C File Offset: 0x0002FC9C
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || WorldRendererUtility.WorldSelected || !Current.Game.PlayerHasControl;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x00031AC0 File Offset: 0x0002FCC0
		public IntVec3 MapPosition
		{
			get
			{
				IntVec3 result = this.CurrentRealPosition.ToIntVec3();
				result.y = 0;
				return result;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00031AE4 File Offset: 0x0002FCE4
		public Vector2 ViewSpacePosition
		{
			get
			{
				Vector3 currentRealPosition = this.CurrentRealPosition;
				float x = Mathf.InverseLerp(2f, (float)Find.CurrentMap.Size.x + -2f, currentRealPosition.x);
				float y = Mathf.InverseLerp(2f, (float)Find.CurrentMap.Size.z + -2f, currentRealPosition.z);
				return new Vector2(x, y);
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00031B4C File Offset: 0x0002FD4C
		public CellRect CurrentViewRect
		{
			get
			{
				if (Time.frameCount != CameraDriver.lastViewRectGetFrame)
				{
					CameraDriver.lastViewRect = default(CellRect);
					float num = (float)UI.screenWidth / (float)UI.screenHeight;
					Vector3 currentRealPosition = this.CurrentRealPosition;
					CameraDriver.lastViewRect.minX = Mathf.FloorToInt(currentRealPosition.x - this.RootSize * num - 1f);
					CameraDriver.lastViewRect.maxX = Mathf.CeilToInt(currentRealPosition.x + this.RootSize * num);
					CameraDriver.lastViewRect.minZ = Mathf.FloorToInt(currentRealPosition.z - this.RootSize - 1f);
					CameraDriver.lastViewRect.maxZ = Mathf.CeilToInt(currentRealPosition.z + this.RootSize);
					CameraDriver.lastViewRectGetFrame = Time.frameCount;
				}
				return CameraDriver.lastViewRect;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00031C18 File Offset: 0x0002FE18
		public static float HitchReduceFactor
		{
			get
			{
				float result = 1f;
				if (Time.deltaTime > 0.1f)
				{
					result = 0.1f / Time.deltaTime;
				}
				return result;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00031C44 File Offset: 0x0002FE44
		public float CellSizePixels
		{
			get
			{
				return (float)UI.screenHeight / (this.RootSize * 2f);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00031C59 File Offset: 0x0002FE59
		public float ZoomRootSize
		{
			get
			{
				return this.RootSize;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00031C61 File Offset: 0x0002FE61
		public Vector3 InverseFovScale
		{
			get
			{
				float num = 1f - (float)UI.screenHeight / (4f * this.RootSize * this.RootSize);
				return new Vector3(num, num, num);
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00031C8C File Offset: 0x0002FE8C
		public bool InViewOf(Thing thing)
		{
			CellRect cellRect = this.CurrentViewRect.ExpandedBy(1);
			cellRect.ClipInsideMap(thing.MapHeld);
			return cellRect.Overlaps(thing.OccupiedDrawRect());
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00031CC4 File Offset: 0x0002FEC4
		public void Awake()
		{
			this.ResetSize();
			this.reverbDummy = GameObject.Find("ReverbZoneDummy");
			this.ApplyPositionToGameObject();
			this.MyCamera.nearClipPlane = 0.5f;
			this.MyCamera.farClipPlane = 65.5f;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00031D02 File Offset: 0x0002FF02
		public void OnPreCull()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (WorldRendererUtility.DrawingMap)
			{
				Find.CurrentMap.weatherManager.DrawAllWeather();
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00031D2C File Offset: 0x0002FF2C
		public void CameraDriverOnGUI()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(2))
			{
				this.releasedLeftWhileHoldingMiddle = true;
			}
			else if (Event.current.rawType == EventType.mouseDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
			{
				this.releasedLeftWhileHoldingMiddle = false;
			}
			this.mouseCoveredByUI = (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null);
			if (!this.AnythingPreventsCameraMotion)
			{
				if (UnityGUIBugsFixer.MouseDrag(2) && (!SteamDeck.IsSteamDeck || !Find.Selector.AnyPawnSelected))
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					if (Event.current.type == EventType.mouseDrag)
					{
						Event.current.Use();
					}
					if (currentEventDelta != Vector2.zero)
					{
						currentEventDelta.x *= -1f;
						this.desiredDollyRaw += currentEventDelta / UI.CurUICellSize() * Prefs.MapDragSensitivity;
						this.panner.JumpOnNextUpdate();
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.scrollWheel)
				{
					num -= Event.current.delta.y * CameraDriver.ScrollWheelZoomRate;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.TinyInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				this.desiredSize -= num * this.config.zoomSpeed * this.RootSize / 35f;
				this.desiredSize = Mathf.Clamp(this.desiredSize, this.config.sizeRange.min, this.config.sizeRange.max);
				this.desiredDolly = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredDolly.x = -this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredDolly.x = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredDolly.y = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredDolly.y = -this.config.dollyRateKeys;
				}
				if (this.desiredDolly != Vector2.zero)
				{
					this.panner.JumpOnNextUpdate();
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00031FB0 File Offset: 0x000301B0
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				if (Current.SubcameraDriver != null)
				{
					Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				}
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.RootSize - this.config.sizeRange.min) / (this.config.sizeRange.max - this.config.sizeRange.min) * 0.7f + 0.3f;
				this.velocity = new Vector3(vector.x, 0f, vector.y) * d;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraDolly, KnowledgeAmount.FrameInteraction);
			}
			if ((Input.GetMouseButtonUp(2) || (SteamDeck.IsSteamDeck && this.releasedLeftWhileHoldingMiddle)) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				Vector2 extraVelocityFromReleasingDragButton = CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 0.75f);
				this.velocity += new Vector3(extraVelocityFromReleasingDragButton.x, 0f, extraVelocityFromReleasingDragButton.y);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.rootPos += this.velocity * (num * this.config.moveSpeedScale);
				this.rootPos += new Vector3(this.desiredDollyRaw.x, 0f, this.desiredDollyRaw.y);
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredDollyRaw,
					time = Time.time
				});
				this.rootPos.x = Mathf.Clamp(this.rootPos.x, 2f, (float)Find.CurrentMap.Size.x + -2f);
				this.rootPos.z = Mathf.Clamp(this.rootPos.z, 2f, (float)Find.CurrentMap.Size.z + -2f);
			}
			this.desiredDollyRaw = Vector2.zero;
			int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num2; i++)
			{
				if (this.velocity != Vector3.zero)
				{
					this.velocity *= this.config.camSpeedDecayFactor;
					if (this.velocity.magnitude < 0.1f)
					{
						this.velocity = Vector3.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num3 = Mathf.Lerp(this.RootSize, this.desiredSize, 0.05f);
					this.desiredSize += (num3 - this.RootSize) * this.config.zoomPreserveFactor;
					this.RootSize = num3;
				}
				else
				{
					float num4 = (this.desiredSize - this.RootSize) * 0.4f;
					this.desiredSize += this.config.zoomPreserveFactor * num4;
					this.RootSize += num4;
				}
				this.config.ConfigFixedUpdate_60(ref this.rootPos, ref this.velocity);
			}
			CameraPanner.Interpolant? interpolant = this.panner.Update();
			if (interpolant != null)
			{
				CameraPanner.Interpolant valueOrDefault = interpolant.GetValueOrDefault();
				this.rootPos = valueOrDefault.Position;
				this.RootSize = valueOrDefault.Size;
				this.desiredSize = this.RootSize;
			}
			this.shaker.Update();
			this.ApplyPositionToGameObject();
			Current.SubcameraDriver.UpdatePositions(this.MyCamera);
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				rememberedCameraPos.rootPos = this.rootPos;
				rememberedCameraPos.rootSize = this.RootSize;
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0003239C File Offset: 0x0003059C
		private void ApplyPositionToGameObject()
		{
			if (base.gameObject == null)
			{
				return;
			}
			this.rootPos.y = 15f + (this.RootSize - this.config.sizeRange.min) / (this.config.sizeRange.max - this.config.sizeRange.min) * 50f;
			this.MyCamera.orthographicSize = this.RootSize;
			this.MyCamera.transform.position = this.rootPos + this.shaker.ShakeOffset;
			if (this.reverbDummy != null)
			{
				Vector3 position = base.transform.position;
				position.y = 65f;
				this.reverbDummy.transform.position = position;
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00032478 File Offset: 0x00030678
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredDolly;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen || ResolutionUtility.BorderlessFullscreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 vector2 = mousePositionOnUI;
				vector2.y = (float)UI.screenHeight - vector2.y;
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect4 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
				if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && mainTabWindow_Inspect.RecentHeight > rect3.height)
				{
					rect3.yMin = (float)UI.screenHeight - mainTabWindow_Inspect.RecentHeight;
				}
				if (!rect.Contains(vector2) && !rect3.Contains(vector2) && !rect2.Contains(vector2) && !rect4.Contains(vector2))
				{
					Vector2 b = new Vector2(0f, 0f);
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						b.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						b.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						b.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							b.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += b;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0003271C File Offset: 0x0003091C
		public void Expose()
		{
			if (Scribe.EnterNode("cameraMap"))
			{
				try
				{
					Scribe_Values.Look<Vector3>(ref this.rootPos, "camRootPos", default(Vector3), false);
					Scribe_Values.Look<float>(ref this.desiredSize, "desiredSize", 0f, false);
					this.RootSize = this.desiredSize;
					this.shaker.Expose();
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00032794 File Offset: 0x00030994
		public void ResetSize()
		{
			this.desiredSize = 24f;
			this.RootSize = this.desiredSize;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000327AD File Offset: 0x000309AD
		public void JumpToCurrentMapLoc(IntVec3 cell)
		{
			this.JumpToCurrentMapLoc(cell.ToVector3Shifted());
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000327BC File Offset: 0x000309BC
		public void JumpToCurrentMapLoc(Vector3 loc)
		{
			this.rootPos = new Vector3(loc.x, this.rootPos.y, loc.z);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000327E0 File Offset: 0x000309E0
		public void PanToMapLoc(IntVec3 cell)
		{
			Vector3 vector = cell.ToVector3Shifted();
			float x = Vector3.Distance(vector, this.CurrentRealPosition);
			float duration = GenMath.LerpDoubleClamped(0f, 70f, 0f, 0.25f, x);
			this.panner.PanTo(new CameraPanner.Interpolant(this.rootPos, this.RootSize), new CameraPanner.Interpolant(new Vector3(vector.x, this.rootPos.y, vector.z), this.RootSize), duration, null);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00032864 File Offset: 0x00030A64
		public void PanToMapLocAndSize(Vector3 loc, float size, float duration = 0.25f, PanCompletionCallback onComplete = null)
		{
			float x = Vector3.Distance(loc, this.CurrentRealPosition);
			float duration2 = GenMath.LerpDoubleClamped(0f, 70f, 0f, duration, x);
			this.panner.PanTo(new CameraPanner.Interpolant(this.rootPos, this.RootSize), new CameraPanner.Interpolant(new Vector3(loc.x, this.rootPos.y, loc.z), size), duration2, onComplete);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000328D8 File Offset: 0x00030AD8
		public void SetRootPosAndSize(Vector3 rootPos, float rootSize)
		{
			this.rootPos = rootPos;
			this.rootSize = rootSize;
			this.desiredDolly = Vector2.zero;
			this.desiredDollyRaw = Vector2.zero;
			this.desiredSize = this.RootSize;
			this.dragTimeStamps.Clear();
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00032931 File Offset: 0x00030B31
		public void SetRootSize(float size)
		{
			this.rootSize = size;
			this.desiredSize = this.RootSize;
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00032958 File Offset: 0x00030B58
		public static Vector2 GetExtraVelocityFromReleasingDragButton(List<CameraDriver.DragTimeStamp> dragTimeStamps, float velocityFromMouseDragInitialFactor)
		{
			float num = 0f;
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < dragTimeStamps.Count; i++)
			{
				if (dragTimeStamps[i].time < Time.time - 0.05f)
				{
					num = 0.05f;
				}
				else
				{
					num = Mathf.Max(num, Time.time - dragTimeStamps[i].time);
					vector += dragTimeStamps[i].posDelta;
				}
			}
			if (vector != Vector2.zero && num > 0f)
			{
				return vector / num * velocityFromMouseDragInitialFactor;
			}
			return Vector2.zero;
		}

		// Token: 0x040003ED RID: 1005
		public CameraShaker shaker = new CameraShaker();

		// Token: 0x040003EE RID: 1006
		private CameraPanner panner;

		// Token: 0x040003EF RID: 1007
		private Camera cachedCamera;

		// Token: 0x040003F0 RID: 1008
		private GameObject reverbDummy;

		// Token: 0x040003F1 RID: 1009
		public CameraMapConfig config = new CameraMapConfig_Normal();

		// Token: 0x040003F2 RID: 1010
		private Vector3 velocity;

		// Token: 0x040003F3 RID: 1011
		private Vector3 rootPos;

		// Token: 0x040003F4 RID: 1012
		private float desiredSize;

		// Token: 0x040003F5 RID: 1013
		private Vector2 desiredDolly = Vector2.zero;

		// Token: 0x040003F6 RID: 1014
		private Vector2 desiredDollyRaw = Vector2.zero;

		// Token: 0x040003F7 RID: 1015
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x040003F8 RID: 1016
		private bool releasedLeftWhileHoldingMiddle;

		// Token: 0x040003F9 RID: 1017
		private bool mouseCoveredByUI;

		// Token: 0x040003FA RID: 1018
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x040003FB RID: 1019
		private float fixedTimeStepBuffer;

		// Token: 0x040003FC RID: 1020
		private static int lastViewRectGetFrame = -1;

		// Token: 0x040003FD RID: 1021
		private static CellRect lastViewRect;

		// Token: 0x040003FE RID: 1022
		public const float MaxDeltaTime = 0.1f;

		// Token: 0x040003FF RID: 1023
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04000400 RID: 1024
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x04000401 RID: 1025
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x04000402 RID: 1026
		private const float DragTimeStampExpireSeconds = 0.05f;

		// Token: 0x04000403 RID: 1027
		private const float VelocityFromMouseDragInitialFactor = 0.75f;

		// Token: 0x04000404 RID: 1028
		private const float MapEdgeClampMarginCells = -2f;

		// Token: 0x04000405 RID: 1029
		public const float StartingSize = 24f;

		// Token: 0x04000406 RID: 1030
		private const float ZoomTightness = 0.4f;

		// Token: 0x04000407 RID: 1031
		private const float ZoomScaleFromAltDenominator = 35f;

		// Token: 0x04000408 RID: 1032
		private const float PageKeyZoomRate = 4f;

		// Token: 0x04000409 RID: 1033
		public const float MinAltitude = 15f;

		// Token: 0x0400040A RID: 1034
		private const float NearClipPlane = 0.5f;

		// Token: 0x0400040B RID: 1035
		private const float MaxAltitude = 65f;

		// Token: 0x0400040C RID: 1036
		private const float ReverbDummyAltitude = 65f;

		// Token: 0x0400040D RID: 1037
		private float rootSize;

		// Token: 0x0400040E RID: 1038
		public const float FullDurationPanDistance = 70f;

		// Token: 0x0200250A RID: 9482
		public struct DragTimeStamp
		{
			// Token: 0x04009774 RID: 38772
			public Vector2 posDelta;

			// Token: 0x04009775 RID: 38773
			public float time;
		}
	}
}
