using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A16 RID: 2582
	public class JobDriver_CarryDownedPawn : JobDriver
	{
		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x001DC348 File Offset: 0x001DA548
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x001DC36E File Offset: 0x001DA56E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x001DC391 File Offset: 0x001DA591
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryDownedPawn.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_CarryDownedPawn.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x001DC3A4 File Offset: 0x001DA5A4
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && !this.Takee.HostileTo(Faction.OfPlayer) && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
			}
		}

		// Token: 0x04003091 RID: 12433
		private const TargetIndex TakeeIndex = TargetIndex.A;
	}
}
