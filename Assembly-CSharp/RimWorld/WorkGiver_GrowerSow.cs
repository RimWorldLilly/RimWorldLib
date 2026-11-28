using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C13 RID: 3091
	public class WorkGiver_GrowerSow : WorkGiver_Grower
	{
		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x0600583C RID: 22588 RVA: 0x0020A32D File Offset: 0x0020852D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x0020A330 File Offset: 0x00208530
		public static void ResetStaticData()
		{
			WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans = "CantSowCavePlantBecauseOfLight".Translate();
			WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans = "CantSowCavePlantBecauseUnroofed".Translate();
		}

		// Token: 0x0600583E RID: 22590 RVA: 0x0020A35C File Offset: 0x0020855C
		protected override bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			if (!settable.CanAcceptSowNow())
			{
				return false;
			}
			Zone_Growing zone_Growing = settable as Zone_Growing;
			IntVec3 c;
			if (zone_Growing != null)
			{
				if (!zone_Growing.allowSow)
				{
					return false;
				}
				c = zone_Growing.Cells[0];
			}
			else
			{
				c = ((Thing)settable).Position;
			}
			WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
			return WorkGiver_Grower.wantedPlantDef != null;
		}

		// Token: 0x0600583F RID: 22591 RVA: 0x0020A3C0 File Offset: 0x002085C0
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Map map = pawn.Map;
			if (c.GetVacuum(pawn.Map) >= 0.5f)
			{
				return null;
			}
			if (WorkGiver_Grower.wantedPlantDef == null)
			{
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, map);
				if (WorkGiver_Grower.wantedPlantDef == null)
				{
					return null;
				}
			}
			if (!PlantUtility.GrowthSeasonNow(c, map, WorkGiver_Grower.wantedPlantDef))
			{
				return null;
			}
			List<Thing> thingList = c.GetThingList(map);
			Zone_Growing zone_Growing = c.GetZone(map) as Zone_Growing;
			bool flag = false;
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def == WorkGiver_Grower.wantedPlantDef)
				{
					return null;
				}
				if ((thing is Blueprint || thing is Frame) && thing.Faction == pawn.Faction)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Thing edifice = c.GetEdifice(map);
				if (edifice == null || edifice.def.fertility < 0f)
				{
					return null;
				}
			}
			if (WorkGiver_Grower.wantedPlantDef.plant.diesToLight)
			{
				if (!c.Roofed(map) && !map.GameConditionManager.IsAlwaysDarkOutside)
				{
					JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans, null);
					return null;
				}
				if (map.glowGrid.GroundGlowAt(c, true, false) > 0f)
				{
					JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans, null);
					return null;
				}
			}
			if (WorkGiver_Grower.wantedPlantDef.plant.interferesWithRoof && c.Roofed(pawn.Map))
			{
				return null;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.blockAdjacentSow)
			{
				if (!pawn.CanReserve(plant, 1, -1, null, forced) || plant.IsForbidden(pawn))
				{
					return null;
				}
				if (zone_Growing != null && !zone_Growing.allowCut)
				{
					return null;
				}
				CompPlantPreventCutting compPlantPreventCutting;
				if (!forced && plant.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
				{
					return null;
				}
				if (!PlantUtility.PawnWillingToCutPlant_Job(plant, pawn))
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.CutPlant, plant);
			}
			else
			{
				Thing thing2 = PlantUtility.AdjacentSowBlocker(WorkGiver_Grower.wantedPlantDef, c, map);
				if (thing2 != null)
				{
					Plant plant2 = thing2 as Plant;
					if (plant2 != null && pawn.CanReserveAndReach(plant2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, forced) && !plant2.IsForbidden(pawn))
					{
						IPlantToGrowSettable plantToGrowSettable = plant2.Position.GetPlantToGrowSettable(plant2.Map);
						if (plantToGrowSettable == null || plantToGrowSettable.GetPlantDefToGrow() != plant2.def)
						{
							Zone_Growing zone_Growing2 = c.GetZone(map) as Zone_Growing;
							Zone_Growing zone_Growing3 = plant2.Position.GetZone(map) as Zone_Growing;
							if ((zone_Growing2 != null && !zone_Growing2.allowCut) || (zone_Growing3 != null && !zone_Growing3.allowCut && plant2.def == zone_Growing3.GetPlantDefToGrow()))
							{
								return null;
							}
							CompPlantPreventCutting compPlantPreventCutting2;
							if (!forced && thing2.TryGetComp(out compPlantPreventCutting2) && compPlantPreventCutting2.PreventCutting)
							{
								return null;
							}
							if (PlantUtility.TreeMarkedForExtraction(plant2))
							{
								return null;
							}
							if (!PlantUtility.PawnWillingToCutPlant_Job(plant2, pawn))
							{
								return null;
							}
							return JobMaker.MakeJob(JobDefOf.CutPlant, plant2);
						}
					}
					return null;
				}
				if (WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill > 0 && ((pawn.skills != null && pawn.skills.GetSkill(SkillDefOf.Plants).Level < WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill) || (pawn.IsColonyMech && pawn.RaceProps.mechFixedSkillLevel < WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill)))
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill), this.def.label);
					return null;
				}
				int j = 0;
				while (j < thingList.Count)
				{
					Thing thing3 = thingList[j];
					if (thing3.def.BlocksPlanting(false))
					{
						if (!pawn.CanReserve(thing3, 1, -1, null, forced))
						{
							return null;
						}
						if (thing3.def.category == ThingCategory.Plant)
						{
							if (thing3.IsForbidden(pawn))
							{
								return null;
							}
							if (zone_Growing != null && !zone_Growing.allowCut)
							{
								return null;
							}
							CompPlantPreventCutting compPlantPreventCutting3;
							if (!forced && plant.TryGetComp(out compPlantPreventCutting3) && compPlantPreventCutting3.PreventCutting)
							{
								return null;
							}
							if (!PlantUtility.PawnWillingToCutPlant_Job(thing3, pawn))
							{
								return null;
							}
							if (PlantUtility.TreeMarkedForExtraction(thing3))
							{
								return null;
							}
							return JobMaker.MakeJob(JobDefOf.CutPlant, thing3);
						}
						else
						{
							if (thing3.def.EverHaulable)
							{
								return HaulAIUtility.HaulAsideJobFor(pawn, thing3);
							}
							return null;
						}
					}
					else
					{
						j++;
					}
				}
				if (!WorkGiver_Grower.wantedPlantDef.CanNowPlantAt(c, map, false) || !PlantUtility.GrowthSeasonNow(c, map, WorkGiver_Grower.wantedPlantDef) || !pawn.CanReserve(c, 1, -1, null, forced))
				{
					return null;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Sow, c);
				job.plantDefToSow = WorkGiver_Grower.wantedPlantDef;
				return job;
			}
		}

		// Token: 0x0400334C RID: 13132
		protected static string CantSowCavePlantBecauseOfLightTrans;

		// Token: 0x0400334D RID: 13133
		protected static string CantSowCavePlantBecauseUnroofedTrans;
	}
}
