using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C0D RID: 3085
	public class WorkGiver_FillFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x0600581A RID: 22554 RVA: 0x002099EA File Offset: 0x00207BEA
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x0600581B RID: 22555 RVA: 0x002099F6 File Offset: 0x00207BF6
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x002099F9 File Offset: 0x00207BF9
		public static void ResetStaticData()
		{
			WorkGiver_FillFermentingBarrel.TemperatureTrans = "BadTemperature".Translate();
			WorkGiver_FillFermentingBarrel.NoWortTrans = "NoWort".Translate();
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x00209A24 File Offset: 0x00207C24
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			if (building_FermentingBarrel == null || building_FermentingBarrel.Fermented || building_FermentingBarrel.SpaceLeftForWort <= 0)
			{
				return false;
			}
			float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
			CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
			if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.TemperatureTrans, null);
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (this.FindWort(pawn, building_FermentingBarrel) == null)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.NoWortTrans, null);
				return false;
			}
			return !t.IsBurning();
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x00209ADC File Offset: 0x00207CDC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel barrel = (Building_FermentingBarrel)t;
			Thing t2 = this.FindWort(pawn, barrel);
			return JobMaker.MakeJob(JobDefOf.FillFermentingBarrel, t, t2);
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x00209B10 File Offset: 0x00207D10
		private Thing FindWort(Pawn pawn, Building_FermentingBarrel barrel)
		{
			Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Wort), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x04003346 RID: 13126
		private static string TemperatureTrans;

		// Token: 0x04003347 RID: 13127
		private static string NoWortTrans;
	}
}
