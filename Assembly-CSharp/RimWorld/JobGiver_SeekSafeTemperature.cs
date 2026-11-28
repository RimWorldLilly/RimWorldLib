using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B65 RID: 2917
	public class JobGiver_SeekSafeTemperature : ThinkNode_JobGiver
	{
		// Token: 0x06005500 RID: 21760 RVA: 0x001FA018 File Offset: 0x001F8218
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.requiresInjury && !pawn.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Serious))
			{
				return null;
			}
			FloatRange tempRange = pawn.ComfortableTemperatureRange();
			if (tempRange.Includes(pawn.AmbientTemperature))
			{
				if (!this.waitInSafeTemp)
				{
					return null;
				}
				return JobMaker.MakeJob(JobDefOf.Wait_SafeTemperature, 500, true);
			}
			else
			{
				Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(pawn.Position, pawn.MapHeld, pawn, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), RegionType.Set_Passable, this.maxRadius);
				if (region != null)
				{
					IntVec3 c;
					JobGiver_SeekSafeTemperature.TryGetAllowedCellInRegion(region, pawn, out c, -1f);
					return JobMaker.MakeJob(JobDefOf.GotoSafeTemperature, c);
				}
				return null;
			}
		}

		// Token: 0x06005501 RID: 21761 RVA: 0x001FA0C0 File Offset: 0x001F82C0
		public static Region ClosestRegionWithinTemperatureRange(IntVec3 root, Map map, Pawn pawn, FloatRange tempRange, TraverseParms traverseParms, RegionType traversableRegionTypes = RegionType.Set_Passable, float maxRadius = -1f)
		{
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region foundReg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				IntVec3 intVec;
				if (!JobGiver_SeekSafeTemperature.TryGetAllowedCellInRegion(r, pawn, out intVec, -1f))
				{
					return false;
				}
				if (tempRange.Includes(r.Room.Temperature))
				{
					foundReg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, traversableRegionTypes);
			return foundReg;
		}

		// Token: 0x06005502 RID: 21762 RVA: 0x001FA130 File Offset: 0x001F8330
		public static bool TryGetAllowedCellInRegion(Region region, Pawn pawn, out IntVec3 cell, float maxRadius = -1f)
		{
			cell = IntVec3.Invalid;
			for (int i = 0; i < 100; i++)
			{
				IntVec3 randomCell = region.RandomCell;
				if (randomCell.InAllowedArea(pawn) && (maxRadius < 0f || randomCell.InHorDistOf(pawn.Position, maxRadius)))
				{
					cell = randomCell;
					return true;
				}
			}
			foreach (IntVec3 intVec in region.Cells.InRandomOrder(JobGiver_SeekSafeTemperature.tmpWorkingCellList))
			{
				if (intVec.InAllowedArea(pawn) && (maxRadius < 0f || intVec.InHorDistOf(pawn.Position, maxRadius)))
				{
					cell = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040032B7 RID: 12983
		public float maxRadius = -1f;

		// Token: 0x040032B8 RID: 12984
		public bool requiresInjury = true;

		// Token: 0x040032B9 RID: 12985
		public bool waitInSafeTemp = true;

		// Token: 0x040032BA RID: 12986
		public static List<IntVec3> tmpWorkingCellList = new List<IntVec3>();
	}
}
