using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D4 RID: 5588
	public class Thought_IdeoMissingBuilding : Thought_Situational
	{
		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x06008AD8 RID: 35544 RVA: 0x00325D63 File Offset: 0x00323F63
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x06008AD9 RID: 35545 RVA: 0x00325D8F File Offset: 0x00323F8F
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x06008ADA RID: 35546 RVA: 0x00325DBC File Offset: 0x00323FBC
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return ThoughtState.Inactive;
			}
			if (!Faction.OfPlayer.ideos.IsPrimary(this.demand.parent.ideo) && this.demand.parent.ideo.ColonistBelieverCountCached < 3)
			{
				return ThoughtState.Inactive;
			}
			Map mapHeld = this.pawn.MapHeld;
			return mapHeld != null && this.demand.AppliesTo(mapHeld) && !this.demand.BuildingPresent(mapHeld) && !this.pawn.IsQuestLodger();
		}

		// Token: 0x06008ADB RID: 35547 RVA: 0x00325E84 File Offset: 0x00324084
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoMissingBuilding thought_IdeoMissingBuilding = other as Thought_IdeoMissingBuilding;
			return thought_IdeoMissingBuilding != null && thought_IdeoMissingBuilding.demand == this.demand;
		}

		// Token: 0x06008ADC RID: 35548 RVA: 0x00325EAB File Offset: 0x003240AB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<IdeoBuildingPresenceDemand>(ref this.demand, "demand", false);
		}

		// Token: 0x04005578 RID: 21880
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x04005579 RID: 21881
		public const int MinBelieversPresent = 3;
	}
}
