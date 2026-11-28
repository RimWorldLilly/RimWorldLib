using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000927 RID: 2343
	public class PsychicRitualToil_ImbueDeathRefusal : PsychicRitualToil
	{
		// Token: 0x060047AF RID: 18351 RVA: 0x001C186E File Offset: 0x001BFA6E
		protected PsychicRitualToil_ImbueDeathRefusal()
		{
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x001C1876 File Offset: 0x001BFA76
		public PsychicRitualToil_ImbueDeathRefusal(PsychicRitualRoleDef targetRole)
		{
			this.targetRole = targetRole;
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x001C1888 File Offset: 0x001BFA88
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			float skillOffsetPct = ((PsychicRitualDef_ImbueDeathRefusal)psychicRitual.def).skillOffsetPercentFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
			PsychicRitualToil_ImbueDeathRefusal.tmpTargetPawns.Clear();
			PsychicRitualToil_ImbueDeathRefusal.tmpTargetPawns.AddRange(psychicRitual.assignments.AssignedPawns(this.targetRole));
			foreach (Pawn pawn in PsychicRitualToil_ImbueDeathRefusal.tmpTargetPawns)
			{
				this.ApplyOutcome(psychicRitual, pawn, skillOffsetPct);
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x001C1920 File Offset: 0x001BFB20
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn pawn, float skillOffsetPct)
		{
			Hediff_DeathRefusal firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null)
			{
				firstHediff.SetUseAmountDirect(firstHediff.UsesLeft + 1, false);
			}
			else
			{
				Hediff_DeathRefusal hediff_DeathRefusal = (Hediff_DeathRefusal)HediffMaker.MakeHediff(HediffDefOf.DeathRefusal, pawn, null);
				hediff_DeathRefusal.SetUseAmountDirect(1, false);
				pawn.health.AddHediff(hediff_DeathRefusal, null, null, null);
			}
			float num = 0f;
			foreach (SkillRecord skillRecord in pawn.skills.skills)
			{
				float num2 = skillRecord.XpTotalEarned * skillOffsetPct;
				skillRecord.Learn(num2, false, false);
				num += num2;
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "ImbueDeathRefuralCompleteText".Translate(pawn, Mathf.Abs((int)num)), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x001C1A40 File Offset: 0x001BFC40
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
		}

		// Token: 0x04002E65 RID: 11877
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04002E66 RID: 11878
		private static List<Pawn> tmpTargetPawns = new List<Pawn>(4);
	}
}
