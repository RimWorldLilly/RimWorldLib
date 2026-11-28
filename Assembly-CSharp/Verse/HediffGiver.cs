using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020003F9 RID: 1017
	public abstract class HediffGiver
	{
		// Token: 0x06001DB3 RID: 7603 RVA: 0x000B71CC File Offset: 0x000B53CC
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000B71CE File Offset: 0x000B53CE
		public virtual float ChanceFactor(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && this.hediff == HediffDefOf.Carcinoma)
			{
				return pawn.GetStatValue(StatDefOf.CancerRate, true, -1);
			}
			return 1f;
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000B71F7 File Offset: 0x000B53F7
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x000B71FC File Offset: 0x000B53FC
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return (this.allowOnLodgers || !pawn.IsQuestLodger()) && (this.allowOnQuestRewardPawns || !pawn.IsWorldPawn() || !pawn.IsQuestReward()) && (this.allowOnQuestReservedPawns || !pawn.IsWorldPawn() || Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.ReservedByQuest) && (!ModsConfig.IdeologyActive || this.allowOnBeggars || pawn.kindDef != PawnKindDefOf.Beggar) && (pawn.ageTracker.CurLifeStage != LifeStageDefOf.HumanlikeBaby || !Find.Storyteller.difficulty.babiesAreHealthy) && (pawn.genes == null || pawn.genes.HediffGiversCanGive(this.hediff)) && (!pawn.IsMutant || pawn.mutant.HediffGiversCanGive(this.hediff)) && HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs, true);
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x000B72F4 File Offset: 0x000B54F4
		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShortCap, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, this.hediff.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), "LetterHealthComplications".Translate(pawn.LabelShortCap, this.hediff.LabelCap, cause.LabelCap, pawn.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000B7434 File Offset: 0x000B5634
		public virtual IEnumerable<string> ConfigErrors()
		{
			HediffGiver.<ConfigErrors>d__13 <ConfigErrors>d__ = new HediffGiver.<ConfigErrors>d__13(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040017C2 RID: 6082
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x040017C3 RID: 6083
		public List<BodyPartDef> partsToAffect;

		// Token: 0x040017C4 RID: 6084
		public bool canAffectAnyLivePart;

		// Token: 0x040017C5 RID: 6085
		public bool allowOnLodgers = true;

		// Token: 0x040017C6 RID: 6086
		public bool allowOnQuestRewardPawns = true;

		// Token: 0x040017C7 RID: 6087
		public bool allowOnQuestReservedPawns = true;

		// Token: 0x040017C8 RID: 6088
		public bool allowOnBeggars = true;

		// Token: 0x040017C9 RID: 6089
		public int countToAffect = 1;
	}
}
