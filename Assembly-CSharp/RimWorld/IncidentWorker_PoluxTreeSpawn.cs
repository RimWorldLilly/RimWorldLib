using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A0 RID: 4512
	public class IncidentWorker_PoluxTreeSpawn : IncidentWorker_SpecialTreeSpawn
	{
		// Token: 0x060070DB RID: 28891 RVA: 0x00277D3C File Offset: 0x00275F3C
		protected override bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => x.IsPolluted(map) && this.GenStep.CanSpawnAt(x, map, 40, 0, 0f, 22, 10, 0, -1, 15), map, out cell) || CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.GenStep.CanSpawnAt(x, map, 10, 0, 0f, 18, 20, 0, -1, 15), map, out cell);
		}
	}
}
