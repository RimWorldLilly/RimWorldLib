using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000871 RID: 2161
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x0600439C RID: 17308 RVA: 0x001A9AA8 File Offset: 0x001A7CA8
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x0600439D RID: 17309 RVA: 0x001A9AE0 File Offset: 0x001A7CE0
		private bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				if (!this.otherPawn.InMentalState)
				{
					return false;
				}
				MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
				return mentalState_SocialFighting != null && mentalState_SocialFighting.otherPawn == this.pawn;
			}
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x001A9B23 File Offset: 0x001A7D23
		public override void MentalStateTick(int delta)
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
				return;
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x001A9B3C File Offset: 0x001A7D3C
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.pawn.jobs != null)
			{
				this.pawn.jobs.StopAll(false, true);
			}
			this.pawn.mindState.meleeThreat = null;
			if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && this.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("MessageNoLongerSocialFighting".Translate(this.pawn.LabelShort, this.otherPawn.LabelShort, this.pawn.Named("PAWN1"), this.otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
			if (!this.pawn.Dead && this.pawn.needs.mood != null && !this.otherPawn.Dead)
			{
				ThoughtDef def;
				if (Rand.Value < 0.5f)
				{
					def = ThoughtDefOf.HadAngeringFight;
				}
				else
				{
					def = ThoughtDefOf.HadCatharticFight;
				}
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn, null);
			}
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x001A9C9B File Offset: 0x001A7E9B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x001A9CB4 File Offset: 0x001A7EB4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002C0A RID: 11274
		public Pawn otherPawn;
	}
}
