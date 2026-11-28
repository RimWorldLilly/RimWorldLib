using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001151 RID: 4433
	public class GameCondition_UnnaturalDarkness : GameCondition_ForceWeather
	{
		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x06006F4F RID: 28495 RVA: 0x0026F359 File Offset: 0x0026D559
		public override int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x06006F50 RID: 28496 RVA: 0x0026F360 File Offset: 0x0026D560
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Unnatural darkness"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x06006F51 RID: 28497 RVA: 0x0026F37B File Offset: 0x0026D57B
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006F52 RID: 28498 RVA: 0x0026F38F File Offset: 0x0026D58F
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, GameCondition_NoSunlight.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x06006F53 RID: 28499 RVA: 0x0026F3B0 File Offset: 0x0026D5B0
		public override WeatherDef ForcedWeather()
		{
			Map map = base.AffectedMaps.FirstOrDefault<Map>();
			if (map == null)
			{
				return null;
			}
			if (map.gameConditionManager.MapBrightness > 0.99f)
			{
				return WeatherDefOf.UnnaturalDarkness_Stage1;
			}
			return WeatherDefOf.UnnaturalDarkness_Stage2;
		}

		// Token: 0x06006F54 RID: 28500 RVA: 0x0026F3EC File Offset: 0x0026D5EC
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			List<Map> affectedMaps = base.AffectedMaps;
			for (int i = 0; i < affectedMaps.Count; i++)
			{
				for (int j = 0; j < this.overlays.Count; j++)
				{
					this.overlays[j].TickOverlay(affectedMaps[i], 1f);
				}
				if (GenTicks.IsTickInterval(60))
				{
					foreach (Pawn pawn in affectedMaps[i].mapPawns.AllHumanlikeSpawned)
					{
						if (GameCondition_UnnaturalDarkness.AffectedByDarkness(pawn) && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarknessExposure, false) == null)
						{
							pawn.health.AddHediff(HediffDefOf.DarknessExposure, null, null, null);
						}
					}
				}
			}
		}

		// Token: 0x06006F55 RID: 28501 RVA: 0x0026F4E4 File Offset: 0x0026D6E4
		public static bool AffectedByDarkness(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && (pawn.Spawned && pawn.RaceProps.Humanlike && !pawn.Downed) && (pawn.IsColonistPlayerControlled || pawn.IsColonySubhumanPlayerControlled);
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x0026F520 File Offset: 0x0026D720
		public override void End()
		{
			base.End();
			foreach (Map map in base.AffectedMaps)
			{
				map.weatherDecider.StartNextWeather();
			}
		}

		// Token: 0x06006F57 RID: 28503 RVA: 0x0026F57C File Offset: 0x0026D77C
		public override void GameConditionDraw(Map map)
		{
			if (map.GameConditionManager.MapBrightness > 0.5f)
			{
				return;
			}
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06006F58 RID: 28504 RVA: 0x0026F5C4 File Offset: 0x0026D7C4
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x06006F59 RID: 28505 RVA: 0x0026F5CC File Offset: 0x0026D7CC
		public static bool InUnnaturalDarkness(Pawn p)
		{
			return p.SpawnedOrAnyParentSpawned && GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(p.PositionHeld, p.MapHeld);
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x0026F5E9 File Offset: 0x0026D7E9
		public static bool UnnaturalDarknessOnMap(Map map)
		{
			return ModsConfig.AnomalyActive && map != null && map.gameConditionManager.MapBrightness <= 0.01f && map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness);
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x0026F624 File Offset: 0x0026D824
		public static bool UnnaturalDarknessAt(IntVec3 cell, Map map)
		{
			if (!GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(map))
			{
				return false;
			}
			Building_Door door = cell.GetDoor(map);
			if (door != null)
			{
				float num = 0f;
				int num2 = 0;
				foreach (IntVec3 c in door.OccupiedRect().ExpandedBy(1).EdgeCellsNoCorners)
				{
					if (c.InBounds(map) && !c.Filled(map))
					{
						num += map.glowGrid.GroundGlowAt(c, false, false);
						num2++;
					}
				}
				if (num2 > 0)
				{
					return num / (float)num2 <= 0f;
				}
			}
			return map.glowGrid.GroundGlowAt(cell, false, false) <= 0f;
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x0026F6F4 File Offset: 0x0026D8F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.anyColonistAttacked, "anyColonistAttacked", false, false);
		}

		// Token: 0x04004723 RID: 18211
		public bool anyColonistAttacked;

		// Token: 0x04004724 RID: 18212
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_UnnaturalDarkness()
		};
	}
}
