using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EE RID: 4334
	public class QuestPart_RequirementsToAcceptThroneRoom : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06006C3B RID: 27707 RVA: 0x0025FBFE File Offset: 0x0025DDFE
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_RequirementsToAcceptThroneRoom.<get_Hyperlinks>d__4 <get_Hyperlinks>d__ = new QuestPart_RequirementsToAcceptThroneRoom.<get_Hyperlinks>d__4(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x0025FC10 File Offset: 0x0025DE10
		public override AcceptanceReport CanAccept()
		{
			this.acceptanceReportUnmetRequirements.Clear();
			if (this.forTitle.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return true;
			}
			Building_Throne assignedThrone = this.forPawn.ownership.AssignedThrone;
			if (assignedThrone == null)
			{
				return "QuestNoThroneRoom".Translate(this.forPawn.Named("PAWN"));
			}
			foreach (RoomRequirement roomRequirement in this.forTitle.throneRoomRequirements)
			{
				if (!roomRequirement.MetOrDisabled(assignedThrone.GetRoom(RegionType.Set_All), this.forPawn))
				{
					this.acceptanceReportUnmetRequirements.Add(roomRequirement.LabelCap(assignedThrone.GetRoom(RegionType.Set_All)));
				}
			}
			if (this.acceptanceReportUnmetRequirements.Count != 0)
			{
				return new AcceptanceReport("QuestThroneRoomRequirementsUnsatisfied".Translate(this.forPawn.Named("PAWN"), this.forTitle.GetLabelFor(this.forPawn).Named("TITLE")) + ":\n\n" + this.acceptanceReportUnmetRequirements.ToLineList("- "));
			}
			return true;
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x0025FD5C File Offset: 0x0025DF5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.forTitle, "forTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.forPawn, "forPawn", false);
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x0025FD96 File Offset: 0x0025DF96
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.forPawn == replace)
			{
				this.forPawn = with;
			}
		}

		// Token: 0x040044E6 RID: 17638
		public RoyalTitleDef forTitle;

		// Token: 0x040044E7 RID: 17639
		public Faction faction;

		// Token: 0x040044E8 RID: 17640
		public Pawn forPawn;

		// Token: 0x040044E9 RID: 17641
		private List<string> acceptanceReportUnmetRequirements = new List<string>();
	}
}
