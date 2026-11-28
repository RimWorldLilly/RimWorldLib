using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E46 RID: 7750
	public class BedInteractionCellSearchPattern : BedCellSearchPattern
	{
		// Token: 0x0600C8F0 RID: 51440 RVA: 0x0049BDB4 File Offset: 0x00499FB4
		public static void BedCellOffsets1x1(List<IntVec3> offsets)
		{
			offsets.Add(IntVec3.West);
			offsets.Add(IntVec3.East);
			offsets.Add(IntVec3.South);
			offsets.Add(IntVec3.North);
			offsets.Add(IntVec3.South + IntVec3.West);
			offsets.Add(IntVec3.South + IntVec3.East);
			offsets.Add(IntVec3.North + IntVec3.West);
			offsets.Add(IntVec3.North + IntVec3.East);
			offsets.Add(IntVec3.Zero);
		}

		// Token: 0x0600C8F1 RID: 51441 RVA: 0x0049BE4C File Offset: 0x0049A04C
		public static void BedCellOffsets2xN(List<IntVec3> offsets, bool rightEdge, bool leftEdge)
		{
			if (rightEdge)
			{
				offsets.Add(IntVec3.West);
			}
			if (leftEdge)
			{
				offsets.Add(IntVec3.East);
			}
			if (rightEdge)
			{
				offsets.Add(IntVec3.West + IntVec3.South);
			}
			if (leftEdge)
			{
				offsets.Add(IntVec3.East + IntVec3.South);
			}
			offsets.Add(IntVec3.North);
			offsets.Add(IntVec3.North + IntVec3.West);
			offsets.Add(IntVec3.North + IntVec3.East);
			offsets.Add(2 * IntVec3.South);
			offsets.Add(2 * IntVec3.South + IntVec3.West);
			offsets.Add(2 * IntVec3.South + IntVec3.East);
			offsets.Add(IntVec3.South);
			offsets.Add(IntVec3.Zero);
		}

		// Token: 0x0600C8F2 RID: 51442 RVA: 0x0049BF38 File Offset: 0x0049A138
		public override void BedCellOffsets(List<IntVec3> offsets, IntVec2 size, int slot)
		{
			if (size.z == 1 && size.x == 1)
			{
				BedInteractionCellSearchPattern.BedCellOffsets1x1(offsets);
				return;
			}
			if (size.z == 2)
			{
				bool rightEdge = slot == 0;
				bool leftEdge = slot == BedUtility.GetSleepingSlotsCount(size) - 1;
				BedInteractionCellSearchPattern.BedCellOffsets2xN(offsets, rightEdge, leftEdge);
				return;
			}
			throw new NotImplementedException("No offsets defined for bed of size {size}");
		}
	}
}
