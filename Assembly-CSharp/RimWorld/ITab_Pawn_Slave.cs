using System;

namespace RimWorld
{
	// Token: 0x02001D76 RID: 7542
	public class ITab_Pawn_Slave : ITab_Pawn_Visitor
	{
		// Token: 0x1700243C RID: 9276
		// (get) Token: 0x0600C389 RID: 50057 RVA: 0x0047835B File Offset: 0x0047655B
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.IsSlaveOfColony;
			}
		}

		// Token: 0x0600C38A RID: 50058 RVA: 0x00478368 File Offset: 0x00476568
		public ITab_Pawn_Slave()
		{
			this.labelKey = "TabSlave";
			this.tutorTag = "Slave";
		}
	}
}
