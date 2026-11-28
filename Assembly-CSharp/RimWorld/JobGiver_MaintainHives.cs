using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB2 RID: 2738
	public class JobGiver_MaintainHives : JobGiver_AIFightEnemies
	{
		// Token: 0x06005280 RID: 21120 RVA: 0x001EBCB3 File Offset: 0x001E9EB3
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_MaintainHives jobGiver_MaintainHives = (JobGiver_MaintainHives)base.DeepCopy(resolve);
			jobGiver_MaintainHives.onlyIfDamagingState = this.onlyIfDamagingState;
			return jobGiver_MaintainHives;
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x001EBCD0 File Offset: 0x001E9ED0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			int num = 0;
			while ((float)num < JobGiver_MaintainHives.CellsInScanRadius)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[num];
				if (intVec.InBounds(pawn.Map) && intVec.GetRoom(pawn.Map) == room)
				{
					Hive hive = (Hive)pawn.Map.thingGrid.ThingAt(intVec, ThingDefOf.Hive);
					if (hive != null && pawn.CanReserve(hive, 1, -1, null, false))
					{
						CompMaintainable compMaintainable = hive.TryGetComp<CompMaintainable>();
						if (compMaintainable.CurStage != MaintainableStage.Healthy && (!this.onlyIfDamagingState || compMaintainable.CurStage == MaintainableStage.Damaging))
						{
							return JobMaker.MakeJob(JobDefOf.Maintain, hive);
						}
					}
				}
				num++;
			}
			return null;
		}

		// Token: 0x040031D7 RID: 12759
		private bool onlyIfDamagingState;

		// Token: 0x040031D8 RID: 12760
		private static readonly float CellsInScanRadius = (float)GenRadial.NumCellsInRadius(7.9f);
	}
}
