using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E4 RID: 4324
	public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06006C12 RID: 27666 RVA: 0x0025F3C6 File Offset: 0x0025D5C6
		public override bool RequiresAccepter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C13 RID: 27667 RVA: 0x0025F3CC File Offset: 0x0025D5CC
		public override AcceptanceReport CanAccept()
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (this.CanPawnAccept(p))
				{
					return true;
				}
			}
			return new AcceptanceReport("QuestNoColonistWithTitle".Translate(this.minimumTitle.GetLabelCapForBothGenders()));
		}

		// Token: 0x06006C14 RID: 27668 RVA: 0x0025F450 File Offset: 0x0025D650
		public override bool CanPawnAccept(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(this.faction);
			return currentTitle != null && this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.minimumTitle);
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x0025F4B4 File Offset: 0x0025D6B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.minimumTitle, "minimumTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06006C16 RID: 27670 RVA: 0x0025F4DD File Offset: 0x0025D6DD
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_RequirementsToAcceptColonistWithTitle.<get_Hyperlinks>d__8 <get_Hyperlinks>d__ = new QuestPart_RequirementsToAcceptColonistWithTitle.<get_Hyperlinks>d__8(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x040044D5 RID: 17621
		public RoyalTitleDef minimumTitle;

		// Token: 0x040044D6 RID: 17622
		public Faction faction;
	}
}
