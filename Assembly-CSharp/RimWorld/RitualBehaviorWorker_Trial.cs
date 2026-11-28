using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001687 RID: 5767
	public class RitualBehaviorWorker_Trial : RitualBehaviorWorker
	{
		// Token: 0x0600909A RID: 37018 RVA: 0x00351084 File Offset: 0x0034F284
		public RitualBehaviorWorker_Trial()
		{
		}

		// Token: 0x0600909B RID: 37019 RVA: 0x00351093 File Offset: 0x0034F293
		public RitualBehaviorWorker_Trial(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600909C RID: 37020 RVA: 0x003510A4 File Offset: 0x0034F2A4
		public override void Cleanup(LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.PawnWithRole("convict");
			if (pawn.IsPrisonerOfColony)
			{
				pawn.guest.WaitInsteadOfEscapingFor(2500);
			}
		}

		// Token: 0x0600909D RID: 37021 RVA: 0x003510D8 File Offset: 0x0034F2D8
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("leader");
			Pawn pawn = ritual.PawnWithRole("convict");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}

		// Token: 0x0600909E RID: 37022 RVA: 0x0035111C File Offset: 0x0034F31C
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			if (ritual.StageIndex == 0)
			{
				return;
			}
			if (this.ticksSinceLastInteraction != -1 && this.ticksSinceLastInteraction <= 700)
			{
				this.ticksSinceLastInteraction++;
				return;
			}
			this.ticksSinceLastInteraction = 0;
			Pawn pawn = ritual.PawnWithRole("leader");
			Pawn pawn2 = ritual.PawnWithRole("convict");
			if (Rand.Bool)
			{
				pawn.interactions.TryInteractWith(pawn2, InteractionDefOf.Trial_Accuse);
				return;
			}
			pawn2.interactions.TryInteractWith(pawn, InteractionDefOf.Trial_Defend);
		}

		// Token: 0x0600909F RID: 37023 RVA: 0x003511A9 File Offset: 0x0034F3A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastInteraction, "ticksSinceLastInteraction", -1, false);
		}

		// Token: 0x04005838 RID: 22584
		private int ticksSinceLastInteraction = -1;

		// Token: 0x04005839 RID: 22585
		public const int SocialInteractionIntervalTicks = 700;
	}
}
