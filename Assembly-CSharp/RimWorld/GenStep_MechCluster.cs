using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F6 RID: 4854
	public class GenStep_MechCluster : GenStep
	{
		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x0600786A RID: 30826 RVA: 0x002A95FC File Offset: 0x002A77FC
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x0600786B RID: 30827 RVA: 0x002A9604 File Offset: 0x002A7804
		public override void Generate(Map map, GenStepParams parms)
		{
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.sitePart.parms.threatPoints, map, true, this.forceNoConditionCauser);
			IntVec3 center = IntVec3.Invalid;
			CellRect cellRect;
			if (MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				center = cellRect.ExpandedBy(this.extraRangeToRectOfInterest).MaxBy((IntVec3 x) => MechClusterUtility.GetClusterPositionScore(x, map, sketch));
			}
			if (!center.IsValid)
			{
				center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0f);
			}
			List<Thing> list = MechClusterUtility.SpawnCluster(center, map, sketch, false, false, null);
			List<Pawn> list2 = new List<Pawn>();
			foreach (Thing thing in list)
			{
				if (thing is Pawn)
				{
					list2.Add((Pawn)thing);
				}
			}
			if (!list2.Any<Pawn>())
			{
				return;
			}
			GenStep_SleepingMechanoids.SendMechanoidsToSleepImmediately(list2);
		}

		// Token: 0x04004B76 RID: 19318
		public bool forceNoConditionCauser;

		// Token: 0x04004B77 RID: 19319
		public int extraRangeToRectOfInterest = 20;
	}
}
