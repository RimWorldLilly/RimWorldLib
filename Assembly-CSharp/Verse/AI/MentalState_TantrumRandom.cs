using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000889 RID: 2185
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x06004408 RID: 17416
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06004409 RID: 17417 RVA: 0x001AB207 File Offset: 0x001A9407
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x001AB20A File Offset: 0x001A940A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x001AB224 File Offset: 0x001A9424
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x001AB234 File Offset: 0x001A9434
		public override void MentalStateTick(int delta)
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || (this.target is Pawn && ((Pawn)this.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(500, delta) && (this.target == null || this.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x001AB2C8 File Offset: 0x001A94C8
		private void ChooseNextTarget()
		{
			MentalState_TantrumRandom.candidates.Clear();
			this.GetPotentialTargets(MentalState_TantrumRandom.candidates);
			if (!MentalState_TantrumRandom.candidates.Any<Thing>())
			{
				this.target = null;
				this.hitTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Thing thing;
				if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_TantrumRandom.candidates.Any((Thing x) => x != this.target))
				{
					thing = (from x in MentalState_TantrumRandom.candidates
					where x != this.target
					select x).RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				else
				{
					thing = MentalState_TantrumRandom.candidates.RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				if (thing != this.target)
				{
					this.target = thing;
					this.hitTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
			MentalState_TantrumRandom.candidates.Clear();
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x001AB3BC File Offset: 0x001A95BC
		private float GetCandidateWeight(Thing candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return (1f - num) * (1f - num) + 0.01f;
		}

		// Token: 0x04002C35 RID: 11317
		private int targetFoundTicks;

		// Token: 0x04002C36 RID: 11318
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x04002C37 RID: 11319
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04002C38 RID: 11320
		private static List<Thing> candidates = new List<Thing>();
	}
}
