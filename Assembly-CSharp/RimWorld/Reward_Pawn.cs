using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001F3E RID: 7998
	public class Reward_Pawn : Reward
	{
		// Token: 0x17002535 RID: 9525
		// (get) Token: 0x0600CC0E RID: 52238 RVA: 0x004B2F9F File Offset: 0x004B119F
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				Reward_Pawn.<get_StackElements>d__6 <get_StackElements>d__ = new Reward_Pawn.<get_StackElements>d__6(-2);
				<get_StackElements>d__.<>4__this = this;
				return <get_StackElements>d__;
			}
		}

		// Token: 0x0600CC0F RID: 52239 RVA: 0x004B2FB0 File Offset: 0x004B11B0
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false);
			this.pawn = PawnGenerator.GeneratePawn(request);
			this.arrivalMode = (Rand.Bool ? Reward_Pawn.ArrivalMode.WalkIn : Reward_Pawn.ArrivalMode.DropPod);
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x0600CC10 RID: 52240 RVA: 0x004B306D File Offset: 0x004B126D
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Reward_Pawn.<GenerateQuestParts>d__8 <GenerateQuestParts>d__ = new Reward_Pawn.<GenerateQuestParts>d__8(-2);
			<GenerateQuestParts>d__.<>4__this = this;
			<GenerateQuestParts>d__.<>3__parms = parms;
			<GenerateQuestParts>d__.<>3__customLetterLabel = customLetterLabel;
			<GenerateQuestParts>d__.<>3__customLetterText = customLetterText;
			<GenerateQuestParts>d__.<>3__customLetterLabelRules = customLetterLabelRules;
			<GenerateQuestParts>d__.<>3__customLetterTextRules = customLetterTextRules;
			return <GenerateQuestParts>d__;
		}

		// Token: 0x0600CC11 RID: 52241 RVA: 0x004B30A4 File Offset: 0x004B12A4
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Pawn_Caravan".Translate(this.pawn);
			}
			Reward_Pawn.ArrivalMode arrivalMode = this.arrivalMode;
			if (arrivalMode == Reward_Pawn.ArrivalMode.WalkIn)
			{
				return "Reward_Pawn_WalkIn".Translate(this.pawn);
			}
			if (arrivalMode != Reward_Pawn.ArrivalMode.DropPod)
			{
				throw new Exception("Unknown arrival mode: " + this.arrivalMode.ToString());
			}
			return "Reward_Pawn_DropPod".Translate(this.pawn);
		}

		// Token: 0x0600CC12 RID: 52242 RVA: 0x004B313C File Offset: 0x004B133C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" (",
				this.pawn.MarketValue.ToStringMoney(null),
				" pawn=",
				this.pawn.ToStringSafe<Pawn>(),
				", arrivalMode=",
				this.arrivalMode.ToString(),
				")"
			});
		}

		// Token: 0x0600CC13 RID: 52243 RVA: 0x004B31B8 File Offset: 0x004B13B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Values.Look<Reward_Pawn.ArrivalMode>(ref this.arrivalMode, "arrivalMode", Reward_Pawn.ArrivalMode.WalkIn, false);
			Scribe_Values.Look<bool>(ref this.detailsHidden, "detailsHidden", false, false);
		}

		// Token: 0x04008205 RID: 33285
		public Pawn pawn;

		// Token: 0x04008206 RID: 33286
		public Reward_Pawn.ArrivalMode arrivalMode;

		// Token: 0x04008207 RID: 33287
		public bool detailsHidden;

		// Token: 0x04008208 RID: 33288
		private const string RootSymbol = "root";

		// Token: 0x02003BEE RID: 15342
		public enum ArrivalMode
		{
			// Token: 0x0400ED64 RID: 60772
			WalkIn,
			// Token: 0x0400ED65 RID: 60773
			DropPod
		}
	}
}
