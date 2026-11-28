using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld.Planet
{
	// Token: 0x02002309 RID: 8969
	public class WorldCameraDriver : MonoBehaviour
	{
		// Token: 0x170026AC RID: 9900
		// (get) Token: 0x0600DD72 RID: 56690 RVA: 0x005270DE File Offset: 0x005252DE
		public static float MinAltitude
		{
			get
			{
				return 100f + (SteamDeck.IsSteamDeck ? 17f : 25f);
			}
		}

		// Token: 0x170026AD RID: 9901
		// (get) Token: 0x0600DD73 RID: 56691 RVA: 0x005270F9 File Offset: 0x005252F9
		public float TrueAltitude
		{
			get
			{
				return this.layerAltitudeOffset + this.altitude;
			}
		}

		// Token: 0x170026AE RID: 9902
		// (get) Token: 0x0600DD74 RID: 56692 RVA: 0x00527108 File Offset: 0x00525308
		private Camera MyCamera
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

		// Token: 0x170026AF RID: 9903
		// (get) Token: 0x0600DD75 RID: 56693 RVA: 0x0052712C File Offset: 0x0052532C
		public WorldCameraZoomRange CurrentZoom
		{
			get
			{
				float altitudePercent = this.AltitudePercent;
				if (altitudePercent < 0.025f)
				{
					return WorldCameraZoomRange.VeryClose;
				}
				if (altitudePercent < 0.042f)
				{
					return WorldCameraZoomRange.Close;
				}
				if (altitudePercent < 0.125f)
				{
					return WorldCameraZoomRange.Far;
				}
				return WorldCameraZoomRange.VeryFar;
			}
		}

		// Token: 0x170026B0 RID: 9904
		// (get) Token: 0x0600DD76 RID: 56694 RVA: 0x0052715F File Offset: 0x0052535F
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

		// Token: 0x170026B1 RID: 9905
		// (get) Token: 0x0600DD77 RID: 56695 RVA: 0x0052717A File Offset: 0x0052537A
		public Vector3 CameraPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170026B2 RID: 9906
		// (get) Token: 0x0600DD78 RID: 56696 RVA: 0x0052718C File Offset: 0x0052538C
		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(WorldCameraDriver.MinAltitude, 1100f, this.altitude);
			}
		}

		// Token: 0x170026B3 RID: 9907
		// (get) Token: 0x0600DD79 RID: 56697 RVA: 0x005271A3 File Offset: 0x005253A3
		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		// Token: 0x170026B4 RID: 9908
		// (get) Token: 0x0600DD7A RID: 56698 RVA: 0x005271BF File Offset: 0x005253BF
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldSelected;
			}
		}

		// Token: 0x0600DD7B RID: 56699 RVA: 0x005271D7 File Offset: 0x005253D7
		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		// Token: 0x0600DD7C RID: 56700 RVA: 0x005271E8 File Offset: 0x005253E8
		public void WorldCameraDriverOnGUI()
		{
			if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(2))
			{
				this.releasedLeftWhileHoldingMiddle = true;
			}
			else if (Event.current.rawType == EventType.mouseDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
			{
				this.releasedLeftWhileHoldingMiddle = false;
			}
			this.mouseCoveredByUI = false;
			if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
			{
				this.mouseCoveredByUI = true;
			}
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				this.ApplyPositionToGameObject();
				return;
			}
			if (Find.World == null)
			{
				return;
			}
			if (this.prevSelectedLayer != null && Find.WorldSelector.SelectedLayer != this.prevSelectedLayer)
			{
				this.altitudeDecay = 0f;
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				if (UnityGUIBugsFixer.MouseDrag(2) && (!SteamDeck.IsSteamDeck || !Find.WorldSelector.AnyCaravanSelected))
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					if (Event.current.type == EventType.mouseDrag)
					{
						Event.current.Use();
					}
					if (currentEventDelta != Vector2.zero)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.FrameInteraction);
						currentEventDelta.x *= -1f;
						this.desiredRotationRaw += currentEventDelta / GenWorldUI.CurUITileSize() * (0.273f * Prefs.MapDragSensitivity);
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.scrollWheel)
				{
					num -= Event.current.delta.y * 0.1f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				float num2 = this.desiredAltitude - num * this.config.zoomSpeed * this.TrueAltitude / 12f;
				if (Current.ProgramState == ProgramState.Playing && Prefs.ZoomSwitchWorldLayer)
				{
					if (num2 < WorldCameraDriver.MinAltitude)
					{
						PlanetLayer selected = PlanetLayer.Selected;
						PlanetLayer planetLayer = (selected != null) ? selected.zoomInToLayer : null;
						if (planetLayer != null)
						{
							float num3 = this.layerAltitudeOffset + num2 - planetLayer.ExtraCameraAltitude;
							num2 = (this.altitude = num3);
							this.altitudeDecay = planetLayer.ExtraCameraAltitude - this.layerAltitudeOffset;
							PlanetLayer.Selected = planetLayer;
						}
					}
					else if (num2 > 1100f)
					{
						PlanetLayer selected2 = PlanetLayer.Selected;
						PlanetLayer planetLayer2 = (selected2 != null) ? selected2.zoomOutToLayer : null;
						if (planetLayer2 != null)
						{
							float num4 = this.layerAltitudeOffset + num2 - planetLayer2.ExtraCameraAltitude;
							num2 = (this.altitude = num4);
							this.altitudeDecay = planetLayer2.ExtraCameraAltitude - this.layerAltitudeOffset;
							PlanetLayer.Selected = planetLayer2;
						}
					}
				}
				this.desiredAltitude = Mathf.Clamp(num2, WorldCameraDriver.MinAltitude, 1100f);
				this.desiredRotation = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredRotation.x = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredRotation.x = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredRotation.y = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredRotation.y = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				this.config.ConfigOnGUI();
				this.prevSelectedLayer = Find.WorldSelector.SelectedLayer;
			}
		}

		// Token: 0x0600DD7D RID: 56701 RVA: 0x00527578 File Offset: 0x00525778
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.World == null)
			{
				this.MyCamera.gameObject.SetActive(false);
				return;
			}
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				this.ApplyMapPositionToGameObject();
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.altitude - WorldCameraDriver.MinAltitude) / (1100f - WorldCameraDriver.MinAltitude) * 0.85f + 0.15f;
				this.rotationVelocity = new Vector2(vector.x, vector.y) * d;
			}
			if ((Input.GetMouseButtonUp(2) || (SteamDeck.IsSteamDeck && this.releasedLeftWhileHoldingMiddle)) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				this.rotationVelocity += CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 5f);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.sphereRotation *= Quaternion.AngleAxis(this.rotationVelocity.x * num * this.config.rotationSpeedScale, this.MyCamera.transform.up);
				this.sphereRotation *= Quaternion.AngleAxis(-this.rotationVelocity.y * num * this.config.rotationSpeedScale, this.MyCamera.transform.right);
				if (this.desiredRotationRaw != Vector2.zero)
				{
					this.sphereRotation *= Quaternion.AngleAxis(this.desiredRotationRaw.x, this.MyCamera.transform.up);
					this.sphereRotation *= Quaternion.AngleAxis(-this.desiredRotationRaw.y, this.MyCamera.transform.right);
				}
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredRotationRaw,
					time = Time.time
				});
			}
			this.desiredRotationRaw = Vector2.zero;
			int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num2; i++)
			{
				if (this.rotationVelocity != Vector2.zero)
				{
					this.rotationVelocity *= this.config.camRotationDecayFactor;
					if (this.rotationVelocity.magnitude < 0.05f)
					{
						this.rotationVelocity = Vector2.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num3 = Mathf.Lerp(this.altitude, this.desiredAltitude, 0.05f);
					this.desiredAltitude += (num3 - this.altitude) * this.config.zoomPreserveFactor;
					this.altitude = num3;
				}
				else
				{
					float num4 = (this.desiredAltitude - this.altitude) * 0.4f;
					this.desiredAltitude += this.config.zoomPreserveFactor * num4;
					this.altitude += num4;
				}
			}
			this.rotationAnimation_lerpFactor += Time.deltaTime * 8f;
			if (Find.PlaySettings.lockNorthUp)
			{
				this.RotateSoNorthIsUp(false);
				this.ClampXRotation(ref this.sphereRotation);
			}
			for (int j = 0; j < num2; j++)
			{
				this.config.ConfigFixedUpdate_60(ref this.rotationVelocity);
			}
			this.ApplyPositionToGameObject();
		}

		// Token: 0x0600DD7E RID: 56702 RVA: 0x00527914 File Offset: 0x00525B14
		public void ApplyMapPositionToGameObject()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			MapParent mapParent = currentMap.ParentHolder as MapParent;
			Vector3 vector;
			if (mapParent != null)
			{
				vector = mapParent.WorldCameraPosition;
			}
			else
			{
				vector = Find.WorldGrid.GetTileCenter(currentMap.Tile);
			}
			if (vector == Vector3.zero)
			{
				return;
			}
			Vector3 vector2 = -vector.normalized;
			vector += -vector2 * currentMap.Tile.Layer.BackgroundWorldCameraOffset;
			Transform transform = this.MyCamera.transform;
			Quaternion rotation = Quaternion.LookRotation(vector2, Vector3.up);
			transform.rotation = rotation;
			float num = currentMap.Tile.Layer.BackgroundWorldCameraParallaxDistancePer100Cells;
			if (num == 0f)
			{
				transform.position = vector;
				return;
			}
			Vector2 viewSpacePosition = Find.CameraDriver.ViewSpacePosition;
			IntVec3 size = Find.CurrentMap.Size;
			float num2 = 1f;
			float num3 = 1f;
			if (size.x > size.z)
			{
				num3 = (float)size.z / (float)size.x;
				num = num * (float)size.x / 100f;
			}
			else if (size.z > size.x)
			{
				num2 = (float)size.x / (float)size.z;
				num = num * (float)size.z / 100f;
			}
			Vector3 up = transform.up;
			Vector3 right = transform.right;
			Vector3 b = up * (viewSpacePosition.y * num * num3) - up * num / 2f * num3;
			Vector3 b2 = right * (viewSpacePosition.x * num * num2) - right * num / 2f * num2;
			transform.position = vector + b + b2 + currentMap.Tile.Layer.Origin;
		}

		// Token: 0x0600DD7F RID: 56703 RVA: 0x00527B20 File Offset: 0x00525D20
		private void ApplyPositionToGameObject()
		{
			Quaternion rotation;
			if (this.rotationAnimation_lerpFactor < 1f)
			{
				rotation = Quaternion.Lerp(this.rotationAnimation_prevSphereRotation, this.sphereRotation, this.rotationAnimation_lerpFactor);
			}
			else
			{
				rotation = this.sphereRotation;
			}
			if (Find.PlaySettings.lockNorthUp)
			{
				this.ClampXRotation(ref rotation);
			}
			float target = 0f;
			Vector3 target2 = Vector3.zero;
			if (PlanetLayer.Selected != null)
			{
				target = PlanetLayer.Selected.ExtraCameraAltitude;
				target2 = PlanetLayer.Selected.Origin;
			}
			float num = this.layerAltitudeOffset;
			this.layerAltitudeOffset = Mathf.SmoothDamp(this.layerAltitudeOffset, target, ref this.layerOffsetVel, 0.1f);
			this.layerOriginOffset = Vector3.SmoothDamp(this.layerOriginOffset, target2, ref this.layerOriginOffsetVel, 0.1f, float.PositiveInfinity, -1f);
			float num2 = this.layerAltitudeOffset - num;
			if (!Mathf.Approximately(this.altitudeDecay, 0f))
			{
				this.altitudeDecay -= num2;
			}
			else
			{
				this.altitudeDecay = 0f;
			}
			Transform transform = this.MyCamera.transform;
			transform.rotation = Quaternion.Inverse(rotation);
			Vector3 a = transform.rotation * Vector3.forward;
			transform.position = -a * (this.altitude + this.layerAltitudeOffset + this.altitudeDecay) + this.layerOriginOffset;
		}

		// Token: 0x0600DD80 RID: 56704 RVA: 0x00527C74 File Offset: 0x00525E74
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredRotation;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen || ResolutionUtility.BorderlessFullscreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Rect rect = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect2 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect3 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				WorldInspectPane inspectPane = Find.World.UI.inspectPane;
				if (Find.WindowStack.IsOpen<WorldInspectPane>() && inspectPane.RecentHeight > rect2.height)
				{
					rect2.yMin = (float)UI.screenHeight - inspectPane.RecentHeight;
				}
				if (!rect2.Contains(mousePositionOnUIInverted) && !rect3.Contains(mousePositionOnUIInverted) && !rect.Contains(mousePositionOnUIInverted))
				{
					Vector2 zero = Vector2.zero;
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						zero.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						zero.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						zero.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							zero.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += zero;
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

		// Token: 0x0600DD81 RID: 56705 RVA: 0x00527ECE File Offset: 0x005260CE
		public void ResetAltitude()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.altitude = 160f;
			}
			else
			{
				this.altitude = 550f;
			}
			this.desiredAltitude = this.altitude;
		}

		// Token: 0x0600DD82 RID: 56706 RVA: 0x00527EFC File Offset: 0x005260FC
		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			if (newLookAt != Vector3.zero)
			{
				this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
			}
		}

		// Token: 0x0600DD83 RID: 56707 RVA: 0x00527F48 File Offset: 0x00526148
		public void JumpTo(PlanetTile tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
			PlanetLayer.Selected = tile.Layer;
		}

		// Token: 0x0600DD84 RID: 56708 RVA: 0x00527F68 File Offset: 0x00526168
		public void RotateSoNorthIsUp(bool interpolate = true)
		{
			if (interpolate)
			{
				this.rotationAnimation_prevSphereRotation = this.sphereRotation;
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(Quaternion.Inverse(this.sphereRotation) * Vector3.forward));
			if (interpolate)
			{
				this.rotationAnimation_lerpFactor = 0f;
			}
		}

		// Token: 0x0600DD85 RID: 56709 RVA: 0x00527FB8 File Offset: 0x005261B8
		private void ClampXRotation(ref Quaternion invRot)
		{
			Vector3 eulerAngles = Quaternion.Inverse(invRot).eulerAngles;
			float altitudePercent = this.AltitudePercent;
			float num = Mathf.Lerp(88.6f, 78f, altitudePercent);
			bool flag = false;
			if (eulerAngles.x <= 90f)
			{
				if (eulerAngles.x > num)
				{
					eulerAngles.x = num;
					flag = true;
				}
			}
			else if (eulerAngles.x < 360f - num)
			{
				eulerAngles.x = 360f - num;
				flag = true;
			}
			if (flag)
			{
				invRot = Quaternion.Inverse(Quaternion.Euler(eulerAngles));
			}
		}

		// Token: 0x04008E1C RID: 36380
		public WorldCameraConfig config = new WorldCameraConfig_Normal();

		// Token: 0x04008E1D RID: 36381
		public Quaternion sphereRotation = Quaternion.identity;

		// Token: 0x04008E1E RID: 36382
		private Vector2 rotationVelocity;

		// Token: 0x04008E1F RID: 36383
		private Vector2 desiredRotation;

		// Token: 0x04008E20 RID: 36384
		private Vector2 desiredRotationRaw;

		// Token: 0x04008E21 RID: 36385
		private float desiredAltitude;

		// Token: 0x04008E22 RID: 36386
		public float altitude;

		// Token: 0x04008E23 RID: 36387
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x04008E24 RID: 36388
		private bool releasedLeftWhileHoldingMiddle;

		// Token: 0x04008E25 RID: 36389
		private float layerAltitudeOffset;

		// Token: 0x04008E26 RID: 36390
		private float layerOffsetVel;

		// Token: 0x04008E27 RID: 36391
		private Vector3 layerOriginOffset;

		// Token: 0x04008E28 RID: 36392
		private Vector3 layerOriginOffsetVel;

		// Token: 0x04008E29 RID: 36393
		private float altitudeDecay;

		// Token: 0x04008E2A RID: 36394
		private float altitudeDecayVel;

		// Token: 0x04008E2B RID: 36395
		private PlanetLayer prevSelectedLayer;

		// Token: 0x04008E2C RID: 36396
		private Camera cachedCamera;

		// Token: 0x04008E2D RID: 36397
		private bool mouseCoveredByUI;

		// Token: 0x04008E2E RID: 36398
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04008E2F RID: 36399
		private float fixedTimeStepBuffer;

		// Token: 0x04008E30 RID: 36400
		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		// Token: 0x04008E31 RID: 36401
		private float rotationAnimation_lerpFactor = 1f;

		// Token: 0x04008E32 RID: 36402
		private const float SphereRadius = 100f;

		// Token: 0x04008E33 RID: 36403
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04008E34 RID: 36404
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x04008E35 RID: 36405
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x04008E36 RID: 36406
		private const float MaxXRotationAtMinAltitude = 88.6f;

		// Token: 0x04008E37 RID: 36407
		private const float MaxXRotationAtMaxAltitude = 78f;

		// Token: 0x04008E38 RID: 36408
		private const float TileSizeToRotationSpeed = 0.273f;

		// Token: 0x04008E39 RID: 36409
		private const float VelocityFromMouseDragInitialFactor = 5f;

		// Token: 0x04008E3A RID: 36410
		private const float StartingAltitude_Playing = 160f;

		// Token: 0x04008E3B RID: 36411
		private const float StartingAltitude_Entry = 550f;

		// Token: 0x04008E3C RID: 36412
		private const float MaxAltitude = 1100f;

		// Token: 0x04008E3D RID: 36413
		private const float ZoomTightness = 0.4f;

		// Token: 0x04008E3E RID: 36414
		private const float ZoomScaleFromAltDenominator = 12f;

		// Token: 0x04008E3F RID: 36415
		private const float PageKeyZoomRate = 2f;

		// Token: 0x04008E40 RID: 36416
		private const float ScrollWheelZoomRate = 0.1f;

		// Token: 0x04008E41 RID: 36417
		private PlanetLayer lastSelectedLayer;
	}
}
