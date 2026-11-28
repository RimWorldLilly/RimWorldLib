using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E8 RID: 4328
	public class QuestPart_RequirementsToAcceptPawnOnColonyMap : QuestPart_RequirementsToAccept
	{
		// Token: 0x06006C24 RID: 27684 RVA: 0x0025F778 File Offset: 0x0025D978
		public override AcceptanceReport CanAccept()
		{
			if (this.pawn != null && this.pawn.Map != null && this.pawn.Map.IsPlayerHome)
			{
				return true;
			}
			return new AcceptanceReport("QuestPawnNotOnColonyMap".Translate(this.pawn.Named("PAWN")));
		}

		// Token: 0x06006C25 RID: 27685 RVA: 0x0025F7D7 File Offset: 0x0025D9D7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06006C26 RID: 27686 RVA: 0x0025F7F0 File Offset: 0x0025D9F0
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_RequirementsToAcceptPawnOnColonyMap.<get_Hyperlinks>d__4 <get_Hyperlinks>d__ = new QuestPart_RequirementsToAcceptPawnOnColonyMap.<get_Hyperlinks>d__4(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x040044DD RID: 17629
		public Pawn pawn;
	}
}
