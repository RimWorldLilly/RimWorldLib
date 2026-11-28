using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A15 RID: 2581
	public class JobDriver_BestowingCeremony : JobDriver
	{
		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x001DC204 File Offset: 0x001DA404
		private Pawn Bestower
		{
			get
			{
				return base.TargetA.Pawn;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06004DF4 RID: 19956 RVA: 0x001DC21F File Offset: 0x001DA41F
		private LordJob_BestowingCeremony CeremonyJob
		{
			get
			{
				return (LordJob_BestowingCeremony)this.Bestower.GetLord().LordJob;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x001DC236 File Offset: 0x001DA436
		private LocalTargetInfo BestowSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x001DC244 File Offset: 0x001DA444
		public static bool AnalyzeThroneRoom(Pawn bestower, Pawn target)
		{
			RoyalTitleDef titleAwardedWhenUpdating = target.royalty.GetTitleAwardedWhenUpdating(bestower.Faction, target.royalty.GetFavor(bestower.Faction));
			if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null)
			{
				using (List<RoomRequirement>.Enumerator enumerator = titleAwardedWhenUpdating.throneRoomRequirements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.MetOrDisabled(bestower.GetRoom(RegionType.Set_All), target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06004DF7 RID: 19959 RVA: 0x001DC2D4 File Offset: 0x001DA4D4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.Bestower.ClearAllReservations(true);
			return this.pawn.Reserve(this.BestowSpot, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Bestower, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DF8 RID: 19960 RVA: 0x001DC32E File Offset: 0x001DA52E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BestowingCeremony.<MakeNewToils>d__15 <MakeNewToils>d__ = new JobDriver_BestowingCeremony.<MakeNewToils>d__15(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400308A RID: 12426
		public const float WarmupSoundLength = 5.125f;

		// Token: 0x0400308B RID: 12427
		public const int BestowTimeTicks = 5000;

		// Token: 0x0400308C RID: 12428
		public const int PlayWarmupSoundAfterTicks = 307;

		// Token: 0x0400308D RID: 12429
		protected const TargetIndex BestowerInd = TargetIndex.A;

		// Token: 0x0400308E RID: 12430
		protected const TargetIndex BestowSpotInd = TargetIndex.B;

		// Token: 0x0400308F RID: 12431
		private Mote mote;

		// Token: 0x04003090 RID: 12432
		private Sustainer sound;
	}
}
