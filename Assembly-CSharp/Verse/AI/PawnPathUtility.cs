using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008A7 RID: 2215
	public static class PawnPathUtility
	{
		// Token: 0x06004491 RID: 17553 RVA: 0x001AD2C0 File Offset: 0x001AB4C0
		public static Thing FirstBlockingBuilding(this PawnPath path, out IntVec3 cellBefore, Pawn pawn)
		{
			if (!path.Found)
			{
				cellBefore = IntVec3.Invalid;
				return null;
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			if (nodesReversed.Count <= 1)
			{
				cellBefore = ((nodesReversed.Count == 1) ? nodesReversed[0] : IntVec3.Invalid);
				return null;
			}
			Building building = null;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = nodesReversed.Count - 2; i >= 0; i--)
			{
				Building edifice = nodesReversed[i].GetEdifice(pawn.Map);
				if (edifice != null)
				{
					Building_Door building_Door = edifice as Building_Door;
					bool flag = building_Door != null && !building_Door.FreePassage && !building_Door.PawnCanOpen(pawn);
					bool flag2 = edifice.def.IsFence && !pawn.CanPassFences;
					if (flag || flag2 || edifice.def.passability == Traversability.Impassable)
					{
						if (building != null)
						{
							cellBefore = intVec;
							return building;
						}
						cellBefore = nodesReversed[i + 1];
						return edifice;
					}
				}
				if (edifice != null && edifice.def.passability == Traversability.PassThroughOnly && edifice.def.Fillage == FillCategory.Full)
				{
					if (building == null)
					{
						building = edifice;
						intVec = nodesReversed[i + 1];
					}
				}
				else if (edifice == null || edifice.def.passability != Traversability.PassThroughOnly)
				{
					building = null;
				}
			}
			cellBefore = nodesReversed[0];
			return null;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x001AD414 File Offset: 0x001AB614
		public static bool TryFindLastCellBeforeBlockingDoor(this PawnPath path, Pawn pawn, out IntVec3 result)
		{
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
				return false;
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = nodesReversed.Count - 2; i >= 1; i--)
			{
				Building_Door building_Door = nodesReversed[i].GetEdifice(pawn.Map) as Building_Door;
				if (building_Door != null && !building_Door.CanPhysicallyPass(pawn))
				{
					result = nodesReversed[i + 1];
					return true;
				}
			}
			result = nodesReversed[0];
			return false;
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x001AD4A0 File Offset: 0x001AB6A0
		public static bool TryFindCellAtIndex(PawnPath path, int index, out IntVec3 result)
		{
			if (path.NodesReversed.Count <= index || index < 0)
			{
				result = IntVec3.Invalid;
				return false;
			}
			result = path.NodesReversed[path.NodesReversed.Count - 1 - index];
			return true;
		}
	}
}
