using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009DC RID: 2524
	[StaticConstructorOnStartup]
	public class JobDriver_AcceptRole : JobDriver
	{
		// Token: 0x06004C89 RID: 19593 RVA: 0x001D7335 File Offset: 0x001D5535
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetLocA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x001D7358 File Offset: 0x001D5558
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AcceptRole.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_AcceptRole.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x001D7368 File Offset: 0x001D5568
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillSocialInteraction, "ticksTillSocialInteraction", 0, false);
			Scribe_Values.Look<int>(ref this.ticksTillFacingUpdate, "ticksTillFacingUpdate", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.facingCellCached, "facingCellCached", default(IntVec3), false);
		}

		// Token: 0x04003011 RID: 12305
		private int ticksTillSocialInteraction = 60;

		// Token: 0x04003012 RID: 12306
		private int ticksTillFacingUpdate;

		// Token: 0x04003013 RID: 12307
		private IntVec3 facingCellCached = IntVec3.Invalid;

		// Token: 0x04003014 RID: 12308
		private const TargetIndex StandIndex = TargetIndex.A;

		// Token: 0x04003015 RID: 12309
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x04003016 RID: 12310
		private static readonly int SocialInteractionInterval = 480;

		// Token: 0x04003017 RID: 12311
		private static readonly int FacingUpdateInterval = 20;

		// Token: 0x04003018 RID: 12312
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
