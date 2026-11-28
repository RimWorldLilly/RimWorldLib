using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7B RID: 2939
	public class JoyGiver_GoSwimming : JoyGiver
	{
		// Token: 0x06005564 RID: 21860 RVA: 0x001FBF8A File Offset: 0x001FA18A
		public static bool HappyToSwimOutsideOnMap(Map map)
		{
			return map.mapTemperature.OutdoorTemp >= 10f && !map.Biome.inVacuum && map.gameConditionManager.AllowEnjoyableOutsideNow(map);
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x001FBFC0 File Offset: 0x001FA1C0
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_GoSwimming.<>c__DisplayClass2_0 CS$<>8__locals1 = new JoyGiver_GoSwimming.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			if (!ModLister.CheckOdyssey("Swimming"))
			{
				return null;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(CS$<>8__locals1.pawn, 0.05f))
			{
				return null;
			}
			Region reg;
			if (!CellFinder.TryFindClosestRegionWith(CS$<>8__locals1.pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), new Predicate<Region>(CS$<>8__locals1.<TryGiveJob>g__RegionValidator|2), 100, out reg, RegionType.Set_Passable))
			{
				return null;
			}
			IntVec3 root;
			if (!reg.TryFindRandomCellInRegionUnforbidden(CS$<>8__locals1.pawn, new Predicate<IntVec3>(CS$<>8__locals1.<TryGiveJob>g__CellValidator|1), out root))
			{
				return null;
			}
			List<IntVec3> list;
			if (!SwimPathFinder.TryFindSwimPath(CS$<>8__locals1.pawn, root, out list))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, list[0]);
			job.targetQueueA = new List<LocalTargetInfo>();
			for (int i = 1; i < list.Count; i++)
			{
				job.targetQueueA.Add(list[i]);
			}
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x001FC0CF File Offset: 0x001FA2CF
		[CompilerGenerated]
		internal static bool <TryGiveJob>g__EnjoyableInsideNow|2_0(Room room)
		{
			return room.Temperature > 10f;
		}

		// Token: 0x040032D6 RID: 13014
		private const float MinSwimmingTemperature = 10f;
	}
}
