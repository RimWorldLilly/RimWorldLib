using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AE3 RID: 2787
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		// Token: 0x06005355 RID: 21333 RVA: 0x001F06B7 File Offset: 0x001EE8B7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AITrashBuildingsDistant jobGiver_AITrashBuildingsDistant = (JobGiver_AITrashBuildingsDistant)base.DeepCopy(resolve);
			jobGiver_AITrashBuildingsDistant.attackAllInert = this.attackAllInert;
			return jobGiver_AITrashBuildingsDistant;
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x001F06D4 File Offset: 0x001EE8D4
		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count == 0)
			{
				return null;
			}
			JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.Clear();
			foreach (Building item in allBuildingsColonist)
			{
				JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.Add(item);
			}
			if (JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < 75; i++)
			{
				Building building = JobGiver_AITrashBuildingsDistant.tmpTrashableBuildingCandidates.RandomElement<Building>();
				if (TrashUtility.ShouldTrashBuilding(pawn, building, this.attackAllInert))
				{
					Job job = TrashUtility.TrashJob(pawn, building, this.attackAllInert, false);
					if (job != null)
					{
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x04003215 RID: 12821
		public bool attackAllInert;

		// Token: 0x04003216 RID: 12822
		private static readonly List<Building> tmpTrashableBuildingCandidates = new List<Building>();
	}
}
