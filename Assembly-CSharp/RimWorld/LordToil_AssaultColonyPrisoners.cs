using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC5 RID: 3269
	public class LordToil_AssaultColonyPrisoners : LordToil
	{
		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06005CBE RID: 23742 RVA: 0x00220D61 File Offset: 0x0021EF61
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06005CC0 RID: 23744 RVA: 0x00220D6C File Offset: 0x0021EF6C
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x00220D6F File Offset: 0x0021EF6F
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x00220D84 File Offset: 0x0021EF84
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrisonerAssaultColony);
			}
		}
	}
}
