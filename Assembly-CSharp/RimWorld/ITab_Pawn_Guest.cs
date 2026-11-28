using System;

namespace RimWorld
{
	// Token: 0x02001D74 RID: 7540
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x1700243A RID: 9274
		// (get) Token: 0x0600C385 RID: 50053 RVA: 0x004782E1 File Offset: 0x004764E1
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.HostFaction == Faction.OfPlayer && !this.SelPawn.IsPrisoner && !this.SelPawn.IsSlaveOfColony;
			}
		}

		// Token: 0x0600C386 RID: 50054 RVA: 0x00478312 File Offset: 0x00476512
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}
	}
}
