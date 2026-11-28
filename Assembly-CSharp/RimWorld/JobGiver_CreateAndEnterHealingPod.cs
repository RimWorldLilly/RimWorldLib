using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD1 RID: 3025
	public class JobGiver_CreateAndEnterHealingPod : JobGiver_CreateAndEnterDryadHolder
	{
		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06005709 RID: 22281 RVA: 0x00204E78 File Offset: 0x00203078
		public override JobDef JobDef
		{
			get
			{
				return JobDefOf.CreateAndEnterHealingPod;
			}
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x00204E80 File Offset: 0x00203080
		public override bool ExtraValidator(Pawn pawn, CompTreeConnection connectionComp)
		{
			if (pawn.mindState != null && pawn.mindState.returnToHealingPod)
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					return true;
				}
				if (pawn.health.hediffSet.GetMissingPartsCommonAncestors().Any<Hediff_MissingPart>())
				{
					return true;
				}
			}
			return base.ExtraValidator(pawn, connectionComp);
		}
	}
}
