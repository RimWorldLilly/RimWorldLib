using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E3 RID: 2531
	[StaticConstructorOnStartup]
	public class JobDriver_GiveSpeech : JobDriver
	{
		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x001D7D55 File Offset: 0x001D5F55
		private Building_Throne Throne
		{
			get
			{
				return (Building_Throne)base.TargetThingA;
			}
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x001D7D64 File Offset: 0x001D5F64
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Throne != null)
			{
				return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(base.TargetLocA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x001D7DBC File Offset: 0x001D5FBC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GiveSpeech.<MakeNewToils>d__11 <MakeNewToils>d__ = new JobDriver_GiveSpeech.<MakeNewToils>d__11(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x001D7DCC File Offset: 0x001D5FCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillSocialInteraction, "ticksTillSocialInteraction", 0, false);
			Scribe_Values.Look<int>(ref this.ticksTillFacingUpdate, "ticksTillFacingUpdate", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.facingCellCached, "facingCellCached", default(IntVec3), false);
		}

		// Token: 0x04003027 RID: 12327
		private int ticksTillSocialInteraction = 60;

		// Token: 0x04003028 RID: 12328
		private int ticksTillFacingUpdate;

		// Token: 0x04003029 RID: 12329
		private IntVec3 facingCellCached = IntVec3.Invalid;

		// Token: 0x0400302A RID: 12330
		private const TargetIndex StandIndex = TargetIndex.A;

		// Token: 0x0400302B RID: 12331
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x0400302C RID: 12332
		private static readonly int SocialInteractionInterval = 480;

		// Token: 0x0400302D RID: 12333
		private static readonly int FacingUpdateInterval = 20;

		// Token: 0x0400302E RID: 12334
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
