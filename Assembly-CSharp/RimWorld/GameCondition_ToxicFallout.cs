using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001150 RID: 4432
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x06006F42 RID: 28482 RVA: 0x0026F036 File Offset: 0x0026D236
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06006F43 RID: 28483 RVA: 0x0026F03D File Offset: 0x0026D23D
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x06006F44 RID: 28484 RVA: 0x0026F058 File Offset: 0x0026D258
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			if (Find.TickManager.TicksGame % 3451 == 0)
			{
				for (int i = 0; i < affectedMaps.Count; i++)
				{
					this.DoPawnsToxicDamage(affectedMaps[i]);
				}
			}
			for (int j = 0; j < this.overlays.Count; j++)
			{
				for (int k = 0; k < affectedMaps.Count; k++)
				{
					this.overlays[j].TickOverlay(affectedMaps[k], 1f);
				}
			}
		}

		// Token: 0x06006F45 RID: 28485 RVA: 0x0026F0E0 File Offset: 0x0026D2E0
		private void DoPawnsToxicDamage(Map map)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (!allPawnsSpawned[i].kindDef.immuneToGameConditionEffects)
				{
					ToxicUtility.DoAirbornePawnToxicDamage(allPawnsSpawned[i], 1f);
				}
			}
		}

		// Token: 0x06006F46 RID: 28486 RVA: 0x0026F130 File Offset: 0x0026D330
		public override void DoCellSteadyEffects(IntVec3 c, Map map)
		{
			if (!c.Roofed(map))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is Plant)
					{
						if (thing.def.plant.dieFromToxicFallout && Rand.Value < 0.0065f)
						{
							Plant plant = thing as Plant;
							if (plant != null && plant.IsCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + thing.def.defName, 240f))
							{
								Messages.Message("MessagePlantDiedOfPoison".Translate(plant.GetCustomLabelNoCount(false)), new TargetInfo(plant.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
							}
							thing.Kill(null, null);
						}
					}
					else if (thing.def.category == ThingCategory.Item)
					{
						CompRottable compRottable = thing.TryGetComp<CompRottable>();
						if (compRottable != null && compRottable.Stage < RotStage.Dessicated)
						{
							compRottable.RotProgress += 3000f;
						}
					}
				}
			}
		}

		// Token: 0x06006F47 RID: 28487 RVA: 0x0026F254 File Offset: 0x0026D454
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06006F48 RID: 28488 RVA: 0x0026F289 File Offset: 0x0026D489
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x0026F29D File Offset: 0x0026D49D
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x06006F4A RID: 28490 RVA: 0x0026F2BE File Offset: 0x0026D4BE
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x0026F2C5 File Offset: 0x0026D4C5
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06006F4C RID: 28492 RVA: 0x0026F2CC File Offset: 0x0026D4CC
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x06006F4D RID: 28493 RVA: 0x0026F2CF File Offset: 0x0026D4CF
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x0400471D RID: 18205
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x0400471E RID: 18206
		private const float SkyGlow = 0.85f;

		// Token: 0x0400471F RID: 18207
		private SkyColorSet ToxicFalloutColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new Color(0.6f, 0.8f, 0.5f), 0.85f);

		// Token: 0x04004720 RID: 18208
		private readonly List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_Fallout()
		};

		// Token: 0x04004721 RID: 18209
		private const float PlantKillChance = 0.0065f;

		// Token: 0x04004722 RID: 18210
		private const float CorpseRotProgressAdd = 3000f;
	}
}
