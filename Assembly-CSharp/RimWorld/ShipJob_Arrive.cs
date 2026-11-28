using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D31 RID: 3377
	public class ShipJob_Arrive : ShipJob
	{
		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x00229FC6 File Offset: 0x002281C6
		protected override bool ShouldEnd
		{
			get
			{
				TransportShip transportShip = this.transportShip;
				return ((transportShip != null) ? transportShip.shipThing : null) != null && this.transportShip.shipThing.Spawned;
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06005EB5 RID: 24245 RVA: 0x00229FEE File Offset: 0x002281EE
		public override bool Interruptible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x00229FF4 File Offset: 0x002281F4
		public override bool TryStart()
		{
			if (!base.TryStart())
			{
				Log.Error("Failed to start arrive ship job");
				return false;
			}
			Pawn pawn = this.mapOfPawn;
			MapParent mapParent;
			if (pawn == null)
			{
				mapParent = null;
			}
			else
			{
				Map mapHeld = pawn.MapHeld;
				mapParent = ((mapHeld != null) ? mapHeld.Parent : null);
			}
			MapParent mapParent2 = mapParent ?? this.mapParent;
			PocketMapParent pocketMapParent = mapParent2 as PocketMapParent;
			if (pocketMapParent != null)
			{
				bool flag;
				if (pocketMapParent == null)
				{
					flag = (null != null);
				}
				else
				{
					Map sourceMap = pocketMapParent.sourceMap;
					flag = (((sourceMap != null) ? sourceMap.Parent : null) != null);
				}
				if (flag)
				{
					mapParent2 = pocketMapParent.sourceMap.Parent;
				}
			}
			if (mapParent2 == null || !mapParent2.HasMap)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap == null)
				{
					Log.Error("Trying to start ShipJob_Arrive with a null map.");
					return false;
				}
				mapParent2 = anyPlayerHomeMap.Parent;
			}
			if (!this.cell.IsValid)
			{
				this.cell = DropCellFinder.GetBestShuttleLandingSpot(mapParent2.Map, this.factionForArrival ?? Faction.OfPlayer);
			}
			ThingOwner innerContainer = this.transportShip.TransporterComp.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn2 = innerContainer[i] as Pawn;
				if (pawn2 != null && pawn2.IsWorldPawn())
				{
					Find.WorldPawns.RemovePawn(pawn2);
				}
			}
			GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(this.transportShip.def.arrivingSkyfaller, this.transportShip.shipThing), this.cell, mapParent2.Map, WipeMode.Vanish);
			QuestUtility.SendQuestTargetSignals(this.transportShip.questTags, "Arrived", this.transportShip.Named("SUBJECT"));
			return true;
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x0022A16C File Offset: 0x0022836C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapOfPawn, "mapOfPawn", false);
			Scribe_References.Look<Faction>(ref this.factionForArrival, "factionForArrival", false);
		}

		// Token: 0x0400359B RID: 13723
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x0400359C RID: 13724
		public Pawn mapOfPawn;

		// Token: 0x0400359D RID: 13725
		public MapParent mapParent;

		// Token: 0x0400359E RID: 13726
		public Faction factionForArrival;
	}
}
