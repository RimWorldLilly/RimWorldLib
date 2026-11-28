using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021BC RID: 8636
	public class QuestNode_GetDropSpot : QuestNode
	{
		// Token: 0x0600D716 RID: 55062 RVA: 0x004FD1D8 File Offset: 0x004FB3D8
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return true;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 var;
			if (this.TryFindDropSpot(map, this.minDistanceFromEdge.GetValue(slate), out var))
			{
				slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600D717 RID: 55063 RVA: 0x004FD248 File Offset: 0x004FB448
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.storeAs.GetValue(slate), false))
			{
				return;
			}
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			IntVec3 var;
			if (this.TryFindDropSpot(map, this.minDistanceFromEdge.GetValue(slate), out var))
			{
				QuestGen.slate.Set<IntVec3>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x0600D718 RID: 55064 RVA: 0x004FD2BC File Offset: 0x004FB4BC
		private bool TryFindDropSpot(Map map, float minDistFromEdge, out IntVec3 spawnSpot)
		{
			return CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map) && (float)x.DistanceToEdge(map) >= minDistFromEdge && map.reachability.CanReachColony(x), map, 1000, out spawnSpot);
		}

		// Token: 0x0400886F RID: 34927
		[NoTranslate]
		public SlateRef<string> storeAs = "dropSpot";

		// Token: 0x04008870 RID: 34928
		public SlateRef<float> minDistanceFromEdge;
	}
}
