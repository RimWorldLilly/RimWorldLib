using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA4 RID: 3236
	public class PsychicRitualDef_SummonShamblers : PsychicRitualDef_InvocationCircle
	{
		// Token: 0x06005B98 RID: 23448 RVA: 0x0021A624 File Offset: 0x00218824
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonShamblers(this.InvokerRole, this.combatPointsFromQualityRange));
			return list;
		}

		// Token: 0x06005B99 RID: 23449 RVA: 0x0021A645 File Offset: 0x00218845
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			PsychicRitualDef_SummonShamblers.<BlockingIssues>d__3 <BlockingIssues>d__ = new PsychicRitualDef_SummonShamblers.<BlockingIssues>d__3(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__assignments = assignments;
			<BlockingIssues>d__.<>3__map = map;
			return <BlockingIssues>d__;
		}

		// Token: 0x06005B9A RID: 23450 RVA: 0x0021A664 File Offset: 0x00218864
		public override void InitializeCast(Map map)
		{
			base.InitializeCast(map);
			int num = 0;
			bool flag = false;
			while (!flag && num < 5)
			{
				IntVec3 root;
				RCellFinder.TryFindRandomPawnEntryCell(out root, map, CellFinder.EdgeRoadChance_Hostile, false, null);
				IntVec3 intVec;
				flag = RCellFinder.TryFindTravelDestFrom(root, map, out intVec);
				num++;
			}
			this.shamblersAllowed = (map.Biome != BiomeDefOf.Undercave && flag);
		}

		// Token: 0x0400346D RID: 13421
		private FloatRange combatPointsFromQualityRange;

		// Token: 0x0400346E RID: 13422
		private bool shamblersAllowed;
	}
}
