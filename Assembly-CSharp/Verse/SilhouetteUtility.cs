using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DC RID: 988
	public static class SilhouetteUtility
	{
		// Token: 0x06001D02 RID: 7426 RVA: 0x000B19C3 File Offset: 0x000AFBC3
		public static void DrawGraphicSilhouette(Thing thing, Vector3 pos)
		{
			if (SilhouetteUtility.ShouldDrawSilhouette(thing))
			{
				SilhouetteUtility.DrawSilhouette(thing, pos);
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000B19D4 File Offset: 0x000AFBD4
		public static void DrawGUISilhouette(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && SilhouetteUtility.ShouldDrawPawnDotSilhouette(pawn))
			{
				SilhouetteUtility.DrawPawnDotSilhouette(pawn);
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000B19FC File Offset: 0x000AFBFC
		private static SilhouetteUtility.SilhouetteCacheKey GetSilhouetteCacheKey(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				return new SilhouetteUtility.SilhouetteCacheKey(pawn);
			}
			return new SilhouetteUtility.SilhouetteCacheKey(thing);
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000B1A20 File Offset: 0x000AFC20
		public static void NotifyGraphicDirty(Thing thing)
		{
			SilhouetteUtility.SilhouetteCacheKey silhouetteCacheKey = SilhouetteUtility.GetSilhouetteCacheKey(thing);
			if (SilhouetteUtility.materialCache.ContainsKey(silhouetteCacheKey))
			{
				SilhouetteUtility.materialCache.Remove(silhouetteCacheKey);
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000B1A50 File Offset: 0x000AFC50
		private static void DrawPawnDotSilhouette(Thing thing)
		{
			Color color = SilhouetteUtility.GetColor(thing);
			color.a = SilhouetteUtility.GetAlpha();
			GUI.DrawTexture(SilhouetteUtility.GetAdjustedScreenspaceRect(thing, 0.01f), TexUI.DotHighlight, ScaleMode.ScaleToFit, true, 0f, color, 0f, 0f);
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000B1A98 File Offset: 0x000AFC98
		public static void DrawSilhouetteJob(Thing thing, Matrix4x4 trs)
		{
			ValueTuple<Mesh, Material> cachedSilhouetteData = SilhouetteUtility.GetCachedSilhouetteData(thing);
			Mesh item = cachedSilhouetteData.Item1;
			Material item2 = cachedSilhouetteData.Item2;
			MaterialPropertyBlock cachedMaterialPropertyBlock = SilhouetteUtility.GetCachedMaterialPropertyBlock(SilhouetteUtility.GetColor(thing));
			GenDraw.DrawMeshNowOrLater(item, trs, item2, false, cachedMaterialPropertyBlock);
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000B1AD0 File Offset: 0x000AFCD0
		private static void DrawSilhouette(Thing thing, Vector3 pos)
		{
			ValueTuple<Mesh, Material> cachedSilhouetteData = SilhouetteUtility.GetCachedSilhouetteData(thing);
			Mesh item = cachedSilhouetteData.Item1;
			Material item2 = cachedSilhouetteData.Item2;
			Pawn pawn = thing as Pawn;
			Graphic graphic = (pawn != null) ? pawn.Drawer.renderer.SilhouetteGraphic : thing.Graphic;
			MaterialPropertyBlock cachedMaterialPropertyBlock = SilhouetteUtility.GetCachedMaterialPropertyBlock(SilhouetteUtility.GetColor(thing));
			Vector3 vector = thing.def.rotatable ? new Vector3(graphic.drawSize.y, 0f, graphic.drawSize.x) : new Vector3(graphic.drawSize.x, 0f, graphic.drawSize.y);
			Vector3 inverseFovScale = Find.CameraDriver.InverseFovScale;
			if (vector.x < 2.5f)
			{
				inverseFovScale.x *= vector.x + SilhouetteUtility.AdjustScale(vector.x);
			}
			else
			{
				inverseFovScale.x *= vector.x;
			}
			if (vector.z < 2.5f)
			{
				inverseFovScale.z *= vector.z + SilhouetteUtility.AdjustScale(vector.z);
			}
			else
			{
				inverseFovScale.z *= vector.z;
			}
			Matrix4x4 matrix = Matrix4x4.TRS(pos.SetToAltitude(AltitudeLayer.Silhouettes), Quaternion.AngleAxis(0f, Vector3.up), inverseFovScale);
			GenDraw.DrawMeshNowOrLater(item, matrix, item2, false, cachedMaterialPropertyBlock);
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000B1C2B File Offset: 0x000AFE2B
		public static bool ShouldDrawSilhouette(Thing thing)
		{
			return !WorldComponent_GravshipController.GravshipRenderInProgess && !Find.ScreenshotModeHandler.Active && Prefs.HighlightStyleMode == HighlightStyleMode.Silhouettes && thing.def.drawHighlight && SilhouetteUtility.ShouldHighlight(thing);
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000B1C68 File Offset: 0x000AFE68
		private static bool ShouldDrawPawnDotSilhouette(Thing thing)
		{
			return Prefs.HighlightStyleMode == HighlightStyleMode.Dots && thing.def.drawHighlight && SilhouetteUtility.ShouldHighlight(thing);
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000B1C88 File Offset: 0x000AFE88
		public static bool CanHighlightAny()
		{
			if (Prefs.DotHighlightDisplayMode == DotHighlightDisplayMode.None)
			{
				return false;
			}
			CameraDriver cameraDriver = Find.CameraDriver;
			return cameraDriver.ZoomRootSize >= cameraDriver.config.sizeRange.max * 0.9f;
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000B1CC8 File Offset: 0x000AFEC8
		private static bool ShouldHighlight(Thing thing)
		{
			if (!SilhouetteUtility.CanHighlightAny())
			{
				return false;
			}
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing.shouldHighlightCachedTick == GenTicks.TicksGame)
			{
				return thing.shouldHighlightCached;
			}
			thing.shouldHighlightCachedTick = GenTicks.TicksGame;
			return thing.shouldHighlightCached = SilhouetteUtility.ShouldHighlightInt(thing);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000B1D18 File Offset: 0x000AFF18
		private static bool ShouldHighlightInt(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (!pawn.IsPlayerControlled && pawn.Fogged())
				{
					return false;
				}
				if (Prefs.DotHighlightDisplayMode == DotHighlightDisplayMode.HighlightHostiles && !pawn.HostileTo(Faction.OfPlayer))
				{
					return false;
				}
				if (pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && pawn.Map.gameConditionManager.MapBrightness < 0.1f && thing.Map.glowGrid.GroundGlowAt(thing.Position, false, false) <= 0f && pawn.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (thing.Fogged())
				{
					return false;
				}
				if (thing.def.drawHighlightOnlyForHostile && !thing.HostileTo(Faction.OfPlayer))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000B1DD8 File Offset: 0x000AFFD8
		private static MaterialPropertyBlock GetCachedMaterialPropertyBlock(Color color)
		{
			float3 key = new float3(color.r, color.g, color.b);
			color.a = SilhouetteUtility.GetAlpha();
			MaterialPropertyBlock materialPropertyBlock;
			if (SilhouetteUtility.materialPropertyBlockCache.TryGetValue(key, out materialPropertyBlock))
			{
				materialPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			}
			else
			{
				materialPropertyBlock = new MaterialPropertyBlock();
				materialPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				SilhouetteUtility.materialPropertyBlockCache[key] = materialPropertyBlock;
			}
			return materialPropertyBlock;
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000B1E48 File Offset: 0x000B0048
		private static Color GetColor(Thing thing)
		{
			if (thing.highlightColorCachedTick == GenTicks.TicksGame)
			{
				return thing.highlightColorCached;
			}
			thing.highlightColorCachedTick = GenTicks.TicksGame;
			return thing.highlightColorCached = SilhouetteUtility.GetColorInt(thing);
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000B1E84 File Offset: 0x000B0084
		private static Color GetColorInt(Thing thing)
		{
			Color result;
			if (thing.def.highlightColor != null)
			{
				result = thing.def.highlightColor.Value;
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					if (pawn.IsColonyMech && !pawn.IsColonyMechPlayerControlled)
					{
						result = SilhouetteUtility.UncontrolledMechDotColor;
					}
					else
					{
						result = PawnNameColorUtility.PawnNameColorOf(pawn);
					}
				}
				else if (thing.HostileTo(Faction.OfPlayer))
				{
					result = PawnNameColorUtility.ColorBaseHostile;
				}
				else
				{
					result = SilhouetteUtility.ThingColor;
				}
			}
			return result;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000B1F00 File Offset: 0x000B0100
		private static float GetAlpha()
		{
			if (SilhouetteUtility.lastCachedAlphaFrame == RealTime.frameCount)
			{
				return SilhouetteUtility.lastCachedAlpha;
			}
			SilhouetteUtility.lastCachedAlphaFrame = RealTime.frameCount;
			bool flag = Prefs.HighlightStyleMode == HighlightStyleMode.Silhouettes;
			CameraDriver cameraDriver = Find.CameraDriver;
			float num = Mathf.Clamp01(Mathf.InverseLerp(cameraDriver.config.sizeRange.max * 0.84999996f, cameraDriver.config.sizeRange.max, cameraDriver.ZoomRootSize));
			if (flag)
			{
				return SilhouetteUtility.lastCachedAlpha = 0.9f * num;
			}
			return SilhouetteUtility.lastCachedAlpha = 0.75f * num;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000B1F8C File Offset: 0x000B018C
		[return: TupleElementNames(new string[]
		{
			"mesh",
			"material"
		})]
		private static ValueTuple<Mesh, Material> GetCachedSilhouetteData(Thing thing)
		{
			if (SilhouetteUtility.materialCache.Count > 512)
			{
				SilhouetteUtility.materialCache.Clear();
			}
			SilhouetteUtility.SilhouetteCacheKey silhouetteCacheKey = SilhouetteUtility.GetSilhouetteCacheKey(thing);
			if (!SilhouetteUtility.materialCache.ContainsKey(silhouetteCacheKey))
			{
				Pawn pawn = thing as Pawn;
				Graphic coloredVersion = ((pawn != null) ? pawn.Drawer.renderer.SilhouetteGraphic : thing.Graphic).GetColoredVersion(ShaderDatabase.Silhouette, Color.white, Color.white);
				SilhouetteUtility.materialCache[silhouetteCacheKey] = new SilhouetteUtility.SilhouetteCacheValue(coloredVersion.MatEast, coloredVersion.MatWest);
			}
			Material item;
			Mesh item2;
			if (thing.Rotation == Rot4.West)
			{
				item = SilhouetteUtility.materialCache[silhouetteCacheKey].west;
				item2 = MeshPool.GridPlaneFlip(Vector2.one);
			}
			else
			{
				item = SilhouetteUtility.materialCache[silhouetteCacheKey].east;
				item2 = MeshPool.GridPlane(Vector2.one);
			}
			return new ValueTuple<Mesh, Material>(item2, item);
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000B2071 File Offset: 0x000B0271
		public static float AdjustScale(float scale)
		{
			return Mathf.InverseLerp(2.5f, 0f, scale) * 0.75f;
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000B208C File Offset: 0x000B028C
		public static Rect GetAdjustedScreenspaceRect(Thing thing, float screenSizeRatio = 0.01f)
		{
			Vector2 vector = Find.Camera.WorldToScreenPoint(thing.DrawPos) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			float num = screenSizeRatio * (float)Mathf.Min(Screen.width, Screen.height) / Prefs.UIScale;
			return new Rect(vector.x - num, vector.y - num / 2f, num * 2f, num);
		}

		// Token: 0x0400175C RID: 5980
		private static readonly Dictionary<SilhouetteUtility.SilhouetteCacheKey, SilhouetteUtility.SilhouetteCacheValue> materialCache = new Dictionary<SilhouetteUtility.SilhouetteCacheKey, SilhouetteUtility.SilhouetteCacheValue>(512);

		// Token: 0x0400175D RID: 5981
		private static readonly Dictionary<float3, MaterialPropertyBlock> materialPropertyBlockCache = new Dictionary<float3, MaterialPropertyBlock>();

		// Token: 0x0400175E RID: 5982
		private static int lastCachedAlphaFrame = -1;

		// Token: 0x0400175F RID: 5983
		private static float lastCachedAlpha = 0f;

		// Token: 0x04001760 RID: 5984
		private const float DotHighlightSizeRatio = 0.01f;

		// Token: 0x04001761 RID: 5985
		private const float DotAlpha = 0.75f;

		// Token: 0x04001762 RID: 5986
		private const float SilhouetteAlpha = 0.9f;

		// Token: 0x04001763 RID: 5987
		private static readonly Color ThingColor = new Color(0.56f, 0.62f, 0.9f);

		// Token: 0x04001764 RID: 5988
		private static readonly Color UncontrolledMechDotColor = new Color(0.8f, 0.55f, 0.17f, 0.75f);

		// Token: 0x04001765 RID: 5989
		public const float DotHighlightStartRange = 0.9f;

		// Token: 0x04001766 RID: 5990
		private const int MaximumMaterialCache = 512;

		// Token: 0x020026B5 RID: 9909
		private readonly struct SilhouetteCacheKey
		{
			// Token: 0x0600F4AE RID: 62638 RVA: 0x00595290 File Offset: 0x00593490
			public SilhouetteCacheKey(Pawn pawn)
			{
				this.thingDef = pawn.def;
				this.lifeStageDef = pawn.ageTracker.CurLifeStage;
				this.graphicIndex = pawn.GetGraphicIndex();
				this.gender = pawn.gender;
				Pawn_MutantTracker mutant = pawn.mutant;
				this.rotMode = ((mutant != null) ? mutant.rotStage : RotStage.Fresh);
			}

			// Token: 0x0600F4AF RID: 62639 RVA: 0x005952EC File Offset: 0x005934EC
			public SilhouetteCacheKey(Thing thing)
			{
				this.thingDef = thing.def;
				this.lifeStageDef = null;
				this.graphicIndex = thing.OverrideGraphicIndex.GetValueOrDefault(-1);
				this.gender = Gender.None;
				this.rotMode = RotStage.Fresh;
			}

			// Token: 0x0600F4B0 RID: 62640 RVA: 0x00595330 File Offset: 0x00593530
			public override int GetHashCode()
			{
				int seed = this.thingDef.GetHashCode();
				if (this.lifeStageDef != null)
				{
					seed = Gen.HashCombineInt(seed, this.lifeStageDef.GetHashCode());
				}
				seed = Gen.HashCombineInt(seed, this.graphicIndex);
				seed = Gen.HashCombineInt(seed, this.gender.GetHashCode());
				return Gen.HashCombineInt(seed, this.rotMode.GetHashCode());
			}

			// Token: 0x04009EAE RID: 40622
			private readonly ThingDef thingDef;

			// Token: 0x04009EAF RID: 40623
			private readonly LifeStageDef lifeStageDef;

			// Token: 0x04009EB0 RID: 40624
			private readonly int graphicIndex;

			// Token: 0x04009EB1 RID: 40625
			private readonly Gender gender;

			// Token: 0x04009EB2 RID: 40626
			private readonly RotStage rotMode;
		}

		// Token: 0x020026B6 RID: 9910
		private class SilhouetteCacheValue : IDisposable
		{
			// Token: 0x0600F4B1 RID: 62641 RVA: 0x005953A7 File Offset: 0x005935A7
			public SilhouetteCacheValue(Material east, Material west)
			{
				this.east = east;
				this.west = west;
			}

			// Token: 0x0600F4B2 RID: 62642 RVA: 0x005953BD File Offset: 0x005935BD
			public void Dispose()
			{
				Object.Destroy(this.east);
				Object.Destroy(this.west);
			}

			// Token: 0x04009EB3 RID: 40627
			public readonly Material east;

			// Token: 0x04009EB4 RID: 40628
			public readonly Material west;
		}
	}
}
