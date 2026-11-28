using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class JobDriver_PredatorHunt : JobDriver
	{
		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06004B45 RID: 19269 RVA: 0x001D2EE4 File Offset: 0x001D10E4
		public Pawn Prey
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06004B46 RID: 19270 RVA: 0x001D2F1C File Offset: 0x001D111C
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x001D2F42 File Offset: 0x001D1142
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
			Scribe_Values.Look<bool>(ref this.notifiedPlayerAttacking, "notifiedPlayerAttacking", false, false);
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x001D2F6E File Offset: 0x001D116E
		public override string GetReport()
		{
			if (this.Corpse != null)
			{
				return this.ReportStringProcessed(JobDefOf.Ingest.reportString);
			}
			return base.GetReport();
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x001D2F8F File Offset: 0x001D118F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x001D2F92 File Offset: 0x001D1192
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PredatorHunt.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_PredatorHunt.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x001D2FA4 File Offset: 0x001D11A4
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.isRanged && dinfo.Instigator != null && dinfo.Instigator != this.Prey && !this.pawn.InMentalState && !this.pawn.Downed)
			{
				this.pawn.mindState.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
			}
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x001D3024 File Offset: 0x001D1224
		private void CheckWarnPlayerInterval(int delta)
		{
			if (this.notifiedPlayerAttacking)
			{
				return;
			}
			Pawn prey = this.Prey;
			if (!prey.Spawned || prey.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame <= this.pawn.mindState.lastPredatorHuntingPlayerNotificationTick + 2500)
			{
				return;
			}
			if (!prey.Position.InHorDistOf(this.pawn.Position, 60f))
			{
				return;
			}
			if (prey.RaceProps.Humanlike)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPredatorHuntingColonist".Translate(this.pawn.LabelShort, prey.LabelDefinite(), this.pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), "LetterPredatorHuntingColonist".Translate(this.pawn.LabelIndefinite(), prey.LabelDefinite(), this.pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), LetterDefOf.ThreatBig, this.pawn, null, null, null, null, 0, true);
			}
			else
			{
				Messages.Message((prey.Name.Numerical ? "LetterPredatorHuntingColonist" : "MessagePredatorHuntingPlayerAnimal").Translate(this.pawn.Named("PREDATOR"), prey.Named("PREY")), this.pawn, MessageTypeDefOf.ThreatBig, true);
			}
			this.pawn.mindState.Notify_PredatorHuntingPlayerNotification();
			this.notifiedPlayerAttacking = true;
		}

		// Token: 0x04002FBE RID: 12222
		private bool notifiedPlayerAttacked;

		// Token: 0x04002FBF RID: 12223
		private bool notifiedPlayerAttacking;

		// Token: 0x04002FC0 RID: 12224
		private bool firstHit = true;

		// Token: 0x04002FC1 RID: 12225
		public const TargetIndex PreyInd = TargetIndex.A;

		// Token: 0x04002FC2 RID: 12226
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04002FC3 RID: 12227
		private const int MaxHuntTicks = 5000;
	}
}
