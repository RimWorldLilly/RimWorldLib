using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001153 RID: 4435
	public class GameCondition_VolcanicAsh : GameCondition
	{
		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06006F65 RID: 28517 RVA: 0x0026F889 File Offset: 0x0026DA89
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06006F66 RID: 28518 RVA: 0x0026F890 File Offset: 0x0026DA90
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006F67 RID: 28519 RVA: 0x0026F8A4 File Offset: 0x0026DAA4
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, GameCondition_VolcanicAsh.AshSkyColors, 1f, 0f));
		}

		// Token: 0x06006F68 RID: 28520 RVA: 0x0026F8C4 File Offset: 0x0026DAC4
		public override void Init()
		{
			if (!ModLister.CheckOdyssey("Volcanic ash"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x06006F69 RID: 28521 RVA: 0x0026F8DF File Offset: 0x0026DADF
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return GameCondition_VolcanicAsh.overlays;
		}

		// Token: 0x06006F6A RID: 28522 RVA: 0x0026F8E6 File Offset: 0x0026DAE6
		public override void GameConditionDraw(Map map)
		{
			if (!base.HiddenByOtherCondition(map))
			{
				GameCondition_VolcanicAsh.volcanicAshOverlay.DrawOverlay(map);
			}
		}

		// Token: 0x06006F6B RID: 28523 RVA: 0x0026F8FC File Offset: 0x0026DAFC
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame % 3251 == 0)
			{
				foreach (Map map in base.AffectedMaps)
				{
					foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
					{
						if (!pawn.Position.Roofed(map) && !pawn.kindDef.immuneToGameConditionEffects && GasUtility.IsAffectedByExposure(pawn))
						{
							this.GiveOrUpdateHediff(pawn);
						}
					}
				}
			}
			foreach (Map map2 in base.AffectedMaps)
			{
				if (!base.HiddenByOtherCondition(map2))
				{
					GameCondition_VolcanicAsh.volcanicAshOverlay.TickOverlay(map2, 1f);
				}
			}
		}

		// Token: 0x06006F6C RID: 28524 RVA: 0x0026FA18 File Offset: 0x0026DC18
		private void GiveOrUpdateHediff(Pawn target)
		{
			Hediff hediff = target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VolcanicAsh, false);
			if (hediff == null)
			{
				hediff = target.health.AddHediff(HediffDefOf.VolcanicAsh, null, null, null);
			}
			hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = 3256;
		}

		// Token: 0x04004729 RID: 18217
		private static readonly SkyColorSet AshSkyColors = new SkyColorSet(new Color(0.482f, 0.403f, 0.402f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);

		// Token: 0x0400472A RID: 18218
		private static readonly GameCondition_VolcanicAsh.VolcanicAshOverlay volcanicAshOverlay = new GameCondition_VolcanicAsh.VolcanicAshOverlay();

		// Token: 0x0400472B RID: 18219
		private static readonly List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			GameCondition_VolcanicAsh.volcanicAshOverlay
		};

		// Token: 0x0400472C RID: 18220
		private const int CheckInterval = 3251;

		// Token: 0x02003087 RID: 12423
		[StaticConstructorOnStartup]
		private class VolcanicAshOverlay : WeatherOverlayDualPanner
		{
			// Token: 0x0601255C RID: 75100 RVA: 0x00630F1F File Offset: 0x0062F11F
			public override void DrawOverlay(Map map)
			{
				SkyOverlay.DrawWorldOverlay(map, GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer, 0);
				SkyOverlay.DrawWorldOverlay(map, GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer, 0);
			}

			// Token: 0x0601255D RID: 75101 RVA: 0x00630F39 File Offset: 0x0062F139
			public override void SetOverlayColor(Color color)
			{
				GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer.color = color;
				GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer.color = color;
			}

			// Token: 0x0601255E RID: 75102 RVA: 0x00630F51 File Offset: 0x0062F151
			public override void TickOverlay(Map map, float lerpFactor)
			{
				this.panner0.Tick();
				this.panner1.Tick();
				this.panner2.Tick();
				this.panner3.Tick();
			}

			// Token: 0x0400C528 RID: 50472
			private static readonly Material CloudLayer = MatLoader.LoadMat("Weather/VolcanicAsh/CloudLayer", -1);

			// Token: 0x0400C529 RID: 50473
			private static readonly Material ParticleLayer = MatLoader.LoadMat("Weather/VolcanicAsh/ParticleLayer", -1);

			// Token: 0x0400C52A RID: 50474
			private static readonly ComplexCurve speedCurve = new ComplexCurve(new UnityEngine.Keyframe[]
			{
				new UnityEngine.Keyframe(0f, 0f),
				new UnityEngine.Keyframe(1f, 1f),
				new UnityEngine.Keyframe(2f, 1.15f),
				new UnityEngine.Keyframe(3f, 1.3f)
			});

			// Token: 0x0400C52B RID: 50475
			private TexturePannerSpeedCurve panner0 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer, "_MainTex", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(1f, -0.2f), 0.007f);

			// Token: 0x0400C52C RID: 50476
			private TexturePannerSpeedCurve panner1 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.CloudLayer, "_MainTex2", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(1f, -0.28f), 0.002f);

			// Token: 0x0400C52D RID: 50477
			private TexturePannerSpeedCurve panner2 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer, "_MainTex", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(-0.28f, -1f), 0.0025f);

			// Token: 0x0400C52E RID: 50478
			private TexturePannerSpeedCurve panner3 = new TexturePannerSpeedCurve(GameCondition_VolcanicAsh.VolcanicAshOverlay.ParticleLayer, "_MainTex2", GameCondition_VolcanicAsh.VolcanicAshOverlay.speedCurve, new Vector2(-0.1f, -1f), 0.002f);
		}
	}
}
