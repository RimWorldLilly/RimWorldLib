using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001008 RID: 4104
	public class TraitMentalStateGiver
	{
		// Token: 0x0600678F RID: 26511 RVA: 0x00249F40 File Offset: 0x00248140
		public virtual bool CheckGive(Pawn pawn, int checkInterval)
		{
			if (this.traitDegreeData.forcedMentalState != null)
			{
				float forcedMentalStateMtbDays = this.traitDegreeData.forcedMentalStateMtbDays;
				if (forcedMentalStateMtbDays > 0f && Rand.MTBEventOccurs(forcedMentalStateMtbDays, 60000f, (float)checkInterval) && this.traitDegreeData.forcedMentalState.Worker.StateCanOccur(pawn))
				{
					return pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.forcedMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.label), false, false, false, null, false, false, false);
				}
			}
			if (this.traitDegreeData.randomMentalState != null)
			{
				float curMood = pawn.mindState.mentalBreaker.CurMood;
				float num = this.traitDegreeData.randomMentalStateMtbDaysMoodCurve.Evaluate(curMood);
				if (num > 0f && Rand.MTBEventOccurs(num, 60000f, (float)checkInterval) && this.traitDegreeData.randomMentalState.Worker.StateCanOccur(pawn))
				{
					return pawn.mindState.mentalStateHandler.TryStartMentalState(this.traitDegreeData.randomMentalState, "MentalStateReason_Trait".Translate(this.traitDegreeData.GetLabelFor(pawn)), false, false, false, null, false, false, false);
				}
			}
			return false;
		}

		// Token: 0x040041B0 RID: 16816
		public TraitDegreeData traitDegreeData;
	}
}
