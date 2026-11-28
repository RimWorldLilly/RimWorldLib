using System;

namespace RimWorld
{
	// Token: 0x02001D75 RID: 7541
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x1700243B RID: 9275
		// (get) Token: 0x0600C387 RID: 50055 RVA: 0x00478330 File Offset: 0x00476530
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x0600C388 RID: 50056 RVA: 0x0047833D File Offset: 0x0047653D
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}
	}
}
