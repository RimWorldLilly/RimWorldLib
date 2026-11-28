using System;

namespace Verse
{
	// Token: 0x020004EA RID: 1258
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000E317B File Offset: 0x000E137B
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000E317E File Offset: 0x000E137E
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000E31A7 File Offset: 0x000E13A7
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.startedTick = Find.TickManager.TicksGame;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000E31DF File Offset: 0x000E13DF
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000E31EF File Offset: 0x000E13EF
		private void SetPieSizeFactor()
		{
			if (this.ticksLeft < 300)
			{
				this.pieSizeFactor = 1f;
				return;
			}
			if (this.ticksLeft < 450)
			{
				this.pieSizeFactor = 0.75f;
				return;
			}
			this.pieSizeFactor = 0.5f;
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000E3230 File Offset: 0x000E1430
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
			Scribe_TargetInfo.Look(ref this.focusTarg, "focusTarg");
			Scribe_Values.Look<bool>(ref this.neverAimWeapon, "neverAimWeapon", false, false);
			Scribe_References.Look<Verb>(ref this.verb, "verb", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetPieSizeFactor();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verb != null && this.verb.BuggedAfterLoading)
			{
				this.verb = null;
				Type type = base.GetType();
				Log.Warning(((type != null) ? type.ToString() : null) + " had a bugged verb after loading.");
			}
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000E32ED File Offset: 0x000E14ED
		public override void StanceTick()
		{
			if (this.stanceTracker.stunner.Stunned)
			{
				return;
			}
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000E331F File Offset: 0x000E151F
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x04001AF8 RID: 6904
		public int ticksLeft;

		// Token: 0x04001AF9 RID: 6905
		public int startedTick;

		// Token: 0x04001AFA RID: 6906
		public Verb verb;

		// Token: 0x04001AFB RID: 6907
		public LocalTargetInfo focusTarg;

		// Token: 0x04001AFC RID: 6908
		public bool neverAimWeapon;

		// Token: 0x04001AFD RID: 6909
		protected float pieSizeFactor = 1f;
	}
}
