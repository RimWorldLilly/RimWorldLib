using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000861 RID: 2145
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		// Token: 0x0600434A RID: 17226 RVA: 0x001A8C21 File Offset: 0x001A6E21
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x001A8C3B File Offset: 0x001A6E3B
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x001A8C4C File Offset: 0x001A6E4C
		public override void MentalStateTick(int delta)
		{
			if (this.target != null && !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, true))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(250, delta) && (this.target == null || this.insultedTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick(delta);
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x001A8CAC File Offset: 0x001A6EAC
		private void ChooseNextTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_InsultingSpreeAll.candidates, true);
			if (!MentalState_InsultingSpreeAll.candidates.Any<Pawn>())
			{
				this.target = null;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
				return;
			}
			Pawn pawn;
			if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_InsultingSpreeAll.candidates.Any((Pawn x) => x != this.target))
			{
				pawn = (from x in MentalState_InsultingSpreeAll.candidates
				where x != this.target
				select x).RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			else
			{
				pawn = MentalState_InsultingSpreeAll.candidates.RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
			}
			if (pawn != this.target)
			{
				this.target = pawn;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x001A8D8C File Offset: 0x001A6F8C
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = Mathf.Min(this.pawn.Position.DistanceTo(candidate.Position) / 40f, 1f);
			return 1f - num + 0.01f;
		}

		// Token: 0x04002BF6 RID: 11254
		private int targetFoundTicks;

		// Token: 0x04002BF7 RID: 11255
		private const int CheckChooseNewTargetIntervalTicks = 250;

		// Token: 0x04002BF8 RID: 11256
		private const int MaxSameTargetChaseTicks = 1250;

		// Token: 0x04002BF9 RID: 11257
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
