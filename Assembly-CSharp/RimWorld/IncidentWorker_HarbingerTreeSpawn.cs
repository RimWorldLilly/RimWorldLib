using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A1 RID: 4513
	public class IncidentWorker_HarbingerTreeSpawn : IncidentWorker_SpecialTreeSpawn
	{
		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x060070DD RID: 28893 RVA: 0x00277D9D File Offset: 0x00275F9D
		protected override int MaxTreesPerIncident
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x00277DA0 File Offset: 0x00275FA0
		protected override bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return IncidentWorker_HarbingerTreeSpawn.TryGetHarbingerTreeSpawnCell(map, out cell, null);
		}

		// Token: 0x060070DF RID: 28895 RVA: 0x00277DBD File Offset: 0x00275FBD
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			bool flag = base.TryExecuteWorker(parms);
			if (flag)
			{
				Find.Anomaly.Notify_HarbingerTreeSpawned();
			}
			return flag;
		}

		// Token: 0x060070E0 RID: 28896 RVA: 0x00277DD4 File Offset: 0x00275FD4
		public static bool TryGetHarbingerTreeSpawnCell(Map map, out IntVec3 cell, IntVec3? nearLoc = null)
		{
			GenStep_SpecialTrees genStep = (GenStep_SpecialTrees)GenStepDefOf.HarbingerTrees.genStep;
			Map anyPlayerHomeMap = Current.Game.AnyPlayerHomeMap;
			int num = (anyPlayerHomeMap != null) ? anyPlayerHomeMap.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeHarbinger).Count : 0;
			int maxProximityToSameTree = (num > 0) ? 7 : -1;
			if (nearLoc != null)
			{
				if (RCellFinder.TryFindRandomCellNearWith(nearLoc.Value, (IntVec3 x) => genStep.CanSpawnAt(x, map, 20, 0, 0.5f, 18, 20, 3, maxProximityToSameTree, 15), map, out cell, 5, maxProximityToSameTree))
				{
					return true;
				}
				if (RCellFinder.TryFindRandomCellNearWith(nearLoc.Value, (IntVec3 x) => genStep.CanSpawnAt(x, map, 2, 0, 0.5f, 22, 10, 3, maxProximityToSameTree, 15), map, out cell, 5, maxProximityToSameTree))
				{
					return true;
				}
			}
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 20, 0, 0.5f, 18, 20, 3, maxProximityToSameTree, 15), map, 1000, out cell) || CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 2, 0, 0.5f, 22, 10, 3, maxProximityToSameTree, 15), map, 10000, out cell) || CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 20, 0, 0.5f, 18, 20, 3, -1, 15), map, 1000, out cell) || CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => genStep.CanSpawnAt(x, map, 2, 0, 0.5f, 22, 10, 3, -1, 15), map, 1000, out cell);
		}
	}
}
