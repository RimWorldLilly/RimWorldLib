using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001618 RID: 5656
	[StaticConstructorOnStartup]
	public class GravshipRenderer
	{
		// Token: 0x06008CEC RID: 36076 RVA: 0x0033371D File Offset: 0x0033191D
		public GravshipRenderer()
		{
			GravshipRenderer.manualTicker = new EventQueue(0.016666668f);
		}

		// Token: 0x06008CED RID: 36077 RVA: 0x0033374C File Offset: 0x0033194C
		public void Init(Map map)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			this.map = map;
			this.exhaustFleckSystem = new FleckSystemThrown(map.flecks);
			this.flareBlock = new MaterialPropertyBlock();
			this.distortionBlock = new MaterialPropertyBlock();
			this.thrusterFlameBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06008CEE RID: 36078 RVA: 0x003337A0 File Offset: 0x003319A0
		public void BeginCutscene(Gravship gravship, Vector3 takeoffOrLandingCenter, Vector3 takeoffOrLandingEnginePos, Rot4 landingRotation)
		{
			this.gravshipOffset = Vector3.zero;
			this.gravship = gravship;
			this.takeoffOrLandingPosition = takeoffOrLandingCenter;
			this.takeoffOrLandingEnginePos = takeoffOrLandingEnginePos;
			this.landingRotation = landingRotation;
			this.exhaustFleckSystem.RemoveAllFlecks((IFleck _) => true);
			this.exhaustTimers.Clear();
			foreach (Thing thing in gravship.Thrusters)
			{
				CompGravshipThruster compGravshipThruster;
				if (thing.TryGetComp(out compGravshipThruster) && compGravshipThruster.Props.exhaustSettings != null && compGravshipThruster.Props.exhaustSettings.enabled && compGravshipThruster.Props.exhaustSettings.ExhaustFleckDef != null)
				{
					this.exhaustFleckSystem.handledDefs.AddUnique(compGravshipThruster.Props.exhaustSettings.ExhaustFleckDef);
					this.exhaustTimers.Add(thing, new EventQueue(1f / compGravshipThruster.Props.exhaustSettings.emissionsPerSecond));
				}
			}
		}

		// Token: 0x06008CEF RID: 36079 RVA: 0x003338D4 File Offset: 0x00331AD4
		public void BeginUpdate()
		{
			this.exhaustFleckSystem.parent = Find.CurrentMap.flecks;
			GravshipRenderer.manualTicker.Push(Time.deltaTime);
			while (GravshipRenderer.manualTicker.Pop())
			{
				this.exhaustFleckSystem.Tick();
			}
			this.exhaustFleckSystem.Update(Time.deltaTime);
		}

		// Token: 0x06008CF0 RID: 36080 RVA: 0x00333930 File Offset: 0x00331B30
		public void UpdateTakeoff(float progress)
		{
			float num = Mathf.Pow(progress, 3.5f);
			Vector3 vector = new Vector3(0f, 0f, Find.Camera.orthographicSize * 2.5f * num);
			Vector3 normalized = this.gravship.launchDirection.ToVector3().normalized;
			if (Mathf.Approximately(normalized.z, -1f))
			{
				vector = -vector;
			}
			float num2 = (Mathf.Approximately(normalized.x, 1f) || Mathf.Approximately(normalized.z, -1f)) ? -1f : 1f;
			float y = Mathf.SmoothStep(0f, num2 * 20f, Mathf.InverseLerp(0.2f, 1f, progress + 0.02f * Mathf.Sin(23.561945f * progress)));
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			this.gravshipOffset += quaternion * normalized * (25f * Time.deltaTime * Mathf.InverseLerp(0.2f, 1f, progress));
			this.DrawGravshipGroundEffects(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + this.gravshipOffset, quaternion, progress, num, true, default(Rot4));
			this.DrawGravship(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + this.gravshipOffset + vector, this.takeoffOrLandingEnginePos + this.gravshipOffset + vector, quaternion, progress, num, true, default(Rot4), false);
		}

		// Token: 0x06008CF1 RID: 36081 RVA: 0x00333AF4 File Offset: 0x00331CF4
		public void UpdateLanding(float progress, bool isPollutedLanding)
		{
			progress = progress.RemapClamped(0f, 0.95f, 0f, 1f);
			float num = Mathf.Pow(1f - progress, 5f);
			Vector3 b;
			Vector3 b2;
			if (this.landingRotation == Rot4.North || this.landingRotation == Rot4.South)
			{
				b = new Vector3(0f, 0f, 100f * num);
				b2 = this.landingRotation.AsQuat * -this.gravship.launchDirection.ToVector3().normalized * 200f * num;
			}
			else
			{
				b = new Vector3(0f, 0f, 200f * num);
				b2 = this.landingRotation.AsQuat * -this.gravship.launchDirection.ToVector3().normalized * 100f * Mathf.Pow(1f - progress, 9f);
			}
			Quaternion identity = Quaternion.identity;
			this.DrawGravshipGroundEffects(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + b2, identity, progress, num, false, this.landingRotation);
			this.DrawGravship(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + b + b2, this.takeoffOrLandingEnginePos + b + b2, identity, progress, num, false, this.landingRotation, isPollutedLanding);
		}

		// Token: 0x06008CF2 RID: 36082 RVA: 0x00333C9E File Offset: 0x00331E9E
		public void EndUpdate()
		{
			this.exhaustFleckSystem.ForceDraw(this.drawBatch);
			this.drawBatch.Flush(true);
		}

		// Token: 0x06008CF3 RID: 36083 RVA: 0x00333CC0 File Offset: 0x00331EC0
		private void DrawGravship(Capture capture, Vector3 cameraPosition, Vector3 gravshipCenter, Vector3 gravEnginePos, Quaternion gravshipRotation, float cutsceneProgressPercent, float cutsceneHeightPercent, bool isTakeoff, Rot4 landingRotation = default(Rot4), bool isPollutedLanding = false)
		{
			Vector3 position = gravEnginePos + this.gravship.Engine.def.Size.ToVector3() * 0.25f;
			Vector3 v = Find.Camera.WorldToViewportPoint(this.RotateAroundPivot(position, gravshipCenter, gravshipRotation));
			this.distortionBlock.SetFloat(ShaderPropertyIDs.Progress, cutsceneProgressPercent);
			this.distortionBlock.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, cutsceneHeightPercent);
			this.distortionBlock.SetVector(ShaderPropertyIDs.DrawPos, v);
			this.distortionBlock.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
			this.DrawLayer(GravshipRenderer.MatGravshipDistortion, cameraPosition.SetToAltitude(AltitudeLayer.Weather).WithYOffset(0.07317074f), this.distortionBlock);
			GravshipRenderer.MatGravship.SetFloat(ShaderPropertyIDs.Progress, cutsceneProgressPercent);
			GravshipRenderer.MatGravship.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, cutsceneHeightPercent);
			GravshipRenderer.MatGravship.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
			GravshipRenderer.MatGravship.color = (isPollutedLanding ? Color.white.WithAlpha(cutsceneProgressPercent.Remap(0.9f, 1f, 1f, 0f)) : Color.white);
			GravshipRenderer.MatGravship.mainTexture = (Texture2D)capture.capture;
			GenDraw.DrawQuad(GravshipRenderer.MatGravship, gravshipCenter.SetToAltitude(AltitudeLayer.Skyfaller), gravshipRotation, capture.drawSize, null);
			foreach (LayerSubMesh layerSubMesh in this.gravship.bakedIndoorMasks)
			{
				Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.TRS(gravshipCenter + Altitudes.AltIncVect * 2f, gravshipRotation, Vector3.one), layerSubMesh.material, 0);
			}
			if ((isTakeoff && cutsceneProgressPercent <= 0f) || (!isTakeoff && cutsceneProgressPercent >= 1f))
			{
				return;
			}
			Color color = new Color(1f, 1f, 1f, 1f);
			color *= Mathf.Lerp(0.75f, 1f, Mathf.PerlinNoise1D(cutsceneProgressPercent * 100f));
			color.a = Mathf.InverseLerp(0f, 0.1f, isTakeoff ? cutsceneProgressPercent : (1f - cutsceneProgressPercent));
			GravshipRenderer.MatGravshipLensFlare.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.gravship.ThrusterPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				IntVec3 b = -this.gravship.launchDirection;
				Rot4 rotation = data2.rotation;
				if (!(rotation.AsIntVec3 == b))
				{
					CompProperties_GravshipThruster props = thing2.TryGetComp<CompGravshipThruster>().Props;
					float num = (float)thing2.def.size.x * props.flameSize;
					Vector3 b2 = thing2.Rotation.AsQuat * props.flameOffsetsPerDirection[thing2.Rotation.AsInt];
					Vector3 b3 = GenThing.TrueCenter(data2.local, thing2.Rotation, thing2.def.size, 0f) - thing2.Rotation.AsIntVec3.ToVector3() * ((float)thing2.def.size.z * 0.5f + num * 0.5f) + b2;
					Vector3 vector = this.RotateAroundPivot(gravEnginePos + b3, gravshipCenter, gravshipRotation).SetToAltitude(AltitudeLayer.Skyfaller).WithYOffset(0.07317074f);
					Material material = MaterialPool.MatFrom(new MaterialRequest(props.FlameShaderType.Shader)
					{
						renderQueue = 3201
					});
					this.thrusterFlameBlock.Clear();
					this.thrusterFlameBlock.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
					foreach (ShaderParameter shaderParameter in props.flameShaderParameters)
					{
						shaderParameter.Apply(this.thrusterFlameBlock);
					}
					Material mat = material;
					Vector3 position2 = vector;
					rotation = data2.rotation;
					GenDraw.DrawQuad(mat, position2, gravshipRotation * rotation.AsQuat, num, this.thrusterFlameBlock);
					Vector3 v2 = Find.Camera.WorldToViewportPoint(vector);
					this.flareBlock.SetVector(ShaderPropertyIDs.DrawPos, v2);
					this.DrawLayer(GravshipRenderer.MatGravshipLensFlare, cameraPosition.SetToAltitude(AltitudeLayer.MetaOverlays).WithYOffset(0.03658537f), this.flareBlock);
					if (props.exhaustSettings.enabled)
					{
						EventQueue eventQueue = this.exhaustTimers[thing2];
						eventQueue.Push(Time.deltaTime);
						while (eventQueue.Pop())
						{
							CompProperties_GravshipThruster.ExhaustSettings exhaustSettings = props.exhaustSettings;
							Vector3 position3 = vector;
							rotation = data2.rotation;
							this.EmitSmoke(exhaustSettings, position3, gravshipRotation, rotation.AsQuat);
						}
					}
				}
			}
			GravshipRenderer.MatGravFieldExtenderGlow.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
			foreach (IntVec3 intVec in this.gravship.GravFieldExtenderPositions)
			{
				Vector3 b4 = intVec.ToVector3() + ThingDefOf.GravFieldExtender.graphicData.drawSize.ToVector3() * 0.5f;
				Vector3 position4 = this.RotateAroundPivot(gravEnginePos + b4, gravshipCenter, gravshipRotation).SetToAltitude(AltitudeLayer.MetaOverlays).WithYOffset(0.07317074f);
				GenDraw.DrawQuad(GravshipRenderer.MatGravFieldExtenderGlow, position4, Quaternion.identity, 8f, null);
			}
			GravshipRenderer.MatGravEngineGlow.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
			Vector3 position5 = this.RotateAroundPivot(position, gravshipCenter, gravshipRotation).SetToAltitude(AltitudeLayer.MetaOverlays).WithYOffset(0.07317074f);
			GenDraw.DrawQuad(GravshipRenderer.MatGravEngineGlow, position5, Quaternion.identity, 12.5f, null);
		}

		// Token: 0x06008CF4 RID: 36084 RVA: 0x0033433C File Offset: 0x0033253C
		private void DrawGravshipGroundEffects(Capture capture, Vector3 cameraCenter, Vector3 groundCenter, Quaternion rotation, float progress, float height, bool isTakeoff, Rot4 landingRotation = default(Rot4))
		{
			if (progress > 0f && !this.map.Biome.inVacuum)
			{
				GravshipRenderer.MatGravshipDownwash.SetFloat(ShaderPropertyIDs.Progress, progress);
				GravshipRenderer.MatGravshipDownwash.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, height);
				GravshipRenderer.MatGravshipDownwash.SetVector(ShaderPropertyIDs.DrawPos, Find.Camera.WorldToViewportPoint(groundCenter));
				GravshipRenderer.MatGravshipDownwash.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
				this.DrawLayer(GravshipRenderer.MatGravshipDownwash, cameraCenter.SetToAltitude(AltitudeLayer.Gas).WithYOffset(0.03658537f));
			}
			GravshipRenderer.MatGravshipShadow.SetFloat(ShaderPropertyIDs.Progress, 1f - progress);
			GravshipRenderer.MatGravshipShadow.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, height);
			GravshipRenderer.MatGravshipShadow.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
			GravshipRenderer.MatGravshipShadow.color = GravshipRenderer.MatGravshipShadow.color.WithAlpha(progress.RemapClamped(0.9f, 1f, 1f, 0f));
			GravshipRenderer.MatGravshipShadow.mainTexture = (Texture2D)capture.capture;
			GenDraw.DrawQuad(GravshipRenderer.MatGravshipShadow, groundCenter.SetToAltitude(AltitudeLayer.Gas).WithYOffset(0.03658537f), rotation, capture.drawSize * 1.05f, null);
		}

		// Token: 0x06008CF5 RID: 36085 RVA: 0x003344A4 File Offset: 0x003326A4
		private void EmitSmoke(CompProperties_GravshipThruster.ExhaustSettings settings, Vector3 position, Quaternion gravshipRotation, Quaternion thrusterRotation)
		{
			Quaternion quaternion = Quaternion.identity;
			if (settings.inheritThrusterRotation)
			{
				quaternion = thrusterRotation * quaternion;
			}
			if (settings.inheritGravshipRotation)
			{
				quaternion = gravshipRotation * quaternion;
			}
			this.exhaustFleckSystem.CreateFleck(new FleckCreationData
			{
				def = settings.ExhaustFleckDef,
				spawnPosition = position + quaternion * settings.spawnOffset + Rand.InsideUnitSphere.WithY(0f).normalized * settings.spawnRadiusRange.RandomInRange,
				scale = settings.scaleRange.RandomInRange,
				velocity = new Vector3?(quaternion * Quaternion.Euler(0f, settings.velocityRotationRange.RandomInRange, 0f) * (settings.velocity * settings.velocityMultiplierRange.RandomInRange)),
				rotationRate = settings.rotationOverTimeRange.RandomInRange,
				ageTicksOverride = -1
			});
		}

		// Token: 0x06008CF6 RID: 36086 RVA: 0x003345B1 File Offset: 0x003327B1
		private Vector3 RotateAroundPivot(Vector3 position, Vector3 pivot, Quaternion rotation)
		{
			return rotation * (position - pivot) + pivot;
		}

		// Token: 0x06008CF7 RID: 36087 RVA: 0x003345C6 File Offset: 0x003327C6
		private void DrawLayer(Material mat, Vector3 position)
		{
			this.DrawLayer(mat, position, Quaternion.identity);
		}

		// Token: 0x06008CF8 RID: 36088 RVA: 0x003345D8 File Offset: 0x003327D8
		private void DrawLayer(Material mat, Vector3 position, Quaternion rotation)
		{
			float num = Find.Camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
			Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x06008CF9 RID: 36089 RVA: 0x00334628 File Offset: 0x00332828
		private void DrawLayer(Material mat, Vector3 position, MaterialPropertyBlock props)
		{
			float num = Find.Camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
			Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0, null, 0, props);
		}

		// Token: 0x04005667 RID: 22119
		private Gravship gravship;

		// Token: 0x04005668 RID: 22120
		private MaterialPropertyBlock distortionBlock;

		// Token: 0x04005669 RID: 22121
		private MaterialPropertyBlock flareBlock;

		// Token: 0x0400566A RID: 22122
		private MaterialPropertyBlock thrusterFlameBlock;

		// Token: 0x0400566B RID: 22123
		private static EventQueue manualTicker;

		// Token: 0x0400566C RID: 22124
		private DrawBatch drawBatch = new DrawBatch();

		// Token: 0x0400566D RID: 22125
		private FleckSystem exhaustFleckSystem;

		// Token: 0x0400566E RID: 22126
		private Dictionary<Thing, EventQueue> exhaustTimers = new Dictionary<Thing, EventQueue>();

		// Token: 0x0400566F RID: 22127
		private Vector3 gravshipOffset;

		// Token: 0x04005670 RID: 22128
		private Vector3 takeoffOrLandingPosition;

		// Token: 0x04005671 RID: 22129
		private Vector3 takeoffOrLandingEnginePos;

		// Token: 0x04005672 RID: 22130
		private Rot4 landingRotation;

		// Token: 0x04005673 RID: 22131
		private Map map;

		// Token: 0x04005674 RID: 22132
		private const float GravFieldGlowSize = 8f;

		// Token: 0x04005675 RID: 22133
		private const float EngineGlowSize = 12.5f;

		// Token: 0x04005676 RID: 22134
		private const float GravshipMoveSpeed = 25f;

		// Token: 0x04005677 RID: 22135
		private const float ThrusterFlickerSpeed = 100f;

		// Token: 0x04005678 RID: 22136
		private const float ThrusterMinBrightness = 0.75f;

		// Token: 0x04005679 RID: 22137
		private const int ThrusterFlameRenderQueue = 3201;

		// Token: 0x0400567A RID: 22138
		private static readonly int ShaderPropertyColor2 = Shader.PropertyToID("_Color2");

		// Token: 0x0400567B RID: 22139
		private static readonly int ShaderPropertyGravshipHeight = Shader.PropertyToID("_GravshipHeight");

		// Token: 0x0400567C RID: 22140
		private static readonly int ShaderPropertyIsTakeoff = Shader.PropertyToID("_IsTakeoff");

		// Token: 0x0400567D RID: 22141
		private static readonly Material MatGravship = MatLoader.LoadMat("Map/Gravship/Gravship", -1);

		// Token: 0x0400567E RID: 22142
		private static readonly Material MatGravshipShadow = MatLoader.LoadMat("Map/Gravship/GravshipShadow", -1);

		// Token: 0x0400567F RID: 22143
		private static readonly Material MatGravshipDownwash = MatLoader.LoadMat("Map/Gravship/GravshipDownwash", -1);

		// Token: 0x04005680 RID: 22144
		private static readonly Material MatGravshipDistortion = MatLoader.LoadMat("Map/Gravship/GravshipDistortion", -1);

		// Token: 0x04005681 RID: 22145
		private static readonly Material MatGravshipLensFlare = MatLoader.LoadMat("Map/Gravship/GravshipLensFlare", -1);

		// Token: 0x04005682 RID: 22146
		private static readonly Material MatGravFieldExtenderGlow = MatLoader.LoadMat("Map/Gravship/GravFieldExtenderGlow", -1);

		// Token: 0x04005683 RID: 22147
		private static readonly Material MatGravEngineGlow = MatLoader.LoadMat("Map/Gravship/GravEngineGlow", -1);

		// Token: 0x04005684 RID: 22148
		public static readonly Material MatTerrainCurtain = MatLoader.LoadMat("Map/Gravship/FakeTerrain", -1);
	}
}
