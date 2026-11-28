using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021A2 RID: 8610
	public class QuestPart_AddShipJob_Arrive : QuestPart_AddShipJob
	{
		// Token: 0x0600D6C1 RID: 54977 RVA: 0x004FBC25 File Offset: 0x004F9E25
		public override bool QuestPartReserves(Faction faction)
		{
			return this.factionForArrival != null && faction == this.factionForArrival;
		}

		// Token: 0x0600D6C2 RID: 54978 RVA: 0x004FBC3A File Offset: 0x004F9E3A
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.factionForArrival == faction)
			{
				this.factionForArrival = null;
			}
		}

		// Token: 0x0600D6C3 RID: 54979 RVA: 0x004FBC4C File Offset: 0x004F9E4C
		public override ShipJob GetShipJob()
		{
			if (this.mapOfPawn == null && (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent)))
			{
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
				this.cell = DropCellFinder.TryFindSafeLandingSpotCloseToColony(this.mapParent.Map, ThingDefOf.Shuttle.Size + new IntVec2(2, 2), null, 2);
			}
			ShipJob_Arrive shipJob_Arrive = (ShipJob_Arrive)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_Arrive.cell = this.cell;
			shipJob_Arrive.mapParent = this.mapParent;
			shipJob_Arrive.mapOfPawn = this.mapOfPawn;
			shipJob_Arrive.factionForArrival = this.factionForArrival;
			return shipJob_Arrive;
		}

		// Token: 0x0600D6C4 RID: 54980 RVA: 0x004FBD08 File Offset: 0x004F9F08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_References.Look<Faction>(ref this.factionForArrival, "factionForArrival", false);
		}

		// Token: 0x04008809 RID: 34825
		public IntVec3 cell;

		// Token: 0x0400880A RID: 34826
		public MapParent mapParent;

		// Token: 0x0400880B RID: 34827
		public Pawn mapOfPawn;

		// Token: 0x0400880C RID: 34828
		public Faction factionForArrival;
	}
}
