using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D5 RID: 5589
	public class Thought_IdeoDisrespectedBuilding : Thought_Situational
	{
		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x06008ADE RID: 35550 RVA: 0x00325ECC File Offset: 0x003240CC
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x06008ADF RID: 35551 RVA: 0x00325EF8 File Offset: 0x003240F8
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x06008AE0 RID: 35552 RVA: 0x00325F24 File Offset: 0x00324124
		protected override ThoughtState CurrentStateInternal()
		{
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return ThoughtState.Inactive;
			}
			Map mapHeld = this.pawn.MapHeld;
			return mapHeld != null && this.demand.AppliesTo(mapHeld) && this.demand.BuildingPresent(mapHeld) && !this.demand.RequirementsSatisfied(mapHeld);
		}

		// Token: 0x06008AE1 RID: 35553 RVA: 0x00325F9C File Offset: 0x0032419C
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding = other as Thought_IdeoDisrespectedBuilding;
			return thought_IdeoDisrespectedBuilding != null && thought_IdeoDisrespectedBuilding.demand == this.demand && !this.pawn.IsQuestLodger();
		}

		// Token: 0x06008AE2 RID: 35554 RVA: 0x00325FD1 File Offset: 0x003241D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<IdeoBuildingPresenceDemand>(ref this.demand, "demand", false);
		}

		// Token: 0x0400557A RID: 21882
		public IdeoBuildingPresenceDemand demand;
	}
}
