using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D7 RID: 9175
	public class TransportersArrivalAction_FormCaravan : TransportersArrivalAction
	{
		// Token: 0x1700283F RID: 10303
		// (get) Token: 0x0600E4AA RID: 58538 RVA: 0x0054F313 File Offset: 0x0054D513
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E4AB RID: 58539 RVA: 0x0054F316 File Offset: 0x0054D516
		public TransportersArrivalAction_FormCaravan()
		{
		}

		// Token: 0x0600E4AC RID: 58540 RVA: 0x0054F329 File Offset: 0x0054D529
		public TransportersArrivalAction_FormCaravan(string arrivalMessageKey)
		{
			this.arrivalMessageKey = arrivalMessageKey;
		}

		// Token: 0x0600E4AD RID: 58541 RVA: 0x0054F344 File Offset: 0x0054D544
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			return TransportersArrivalAction_FormCaravan.CanFormCaravanAt(pods, destinationTile);
		}

		// Token: 0x0600E4AE RID: 58542 RVA: 0x0054F370 File Offset: 0x0054D570
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			TransportersArrivalAction_FormCaravan.tmpPawns.Clear();
			for (int i = 0; i < transporters.Count; i++)
			{
				ThingOwner innerContainer = transporters[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null)
					{
						TransportersArrivalAction_FormCaravan.tmpPawns.Add(pawn);
						innerContainer.Remove(pawn);
					}
				}
			}
			PlanetTile startingTile;
			if (!GenWorldClosest.TryFindClosestPassableTile(tile, out startingTile))
			{
				startingTile = tile;
			}
			Caravan caravan = CaravanMaker.MakeCaravan(TransportersArrivalAction_FormCaravan.tmpPawns, Faction.OfPlayer, startingTile, true);
			if (transporters.IsShuttle())
			{
				CaravanInventoryUtility.GiveThing(caravan, transporters[0].RemoveShuttle());
			}
			for (int k = 0; k < transporters.Count; k++)
			{
				TransportersArrivalAction_FormCaravan.tmpContainedThings.Clear();
				TransportersArrivalAction_FormCaravan.tmpContainedThings.AddRange(transporters[k].innerContainer);
				for (int l = 0; l < TransportersArrivalAction_FormCaravan.tmpContainedThings.Count; l++)
				{
					transporters[k].innerContainer.Remove(TransportersArrivalAction_FormCaravan.tmpContainedThings[l]);
					CaravanInventoryUtility.GiveThing(caravan, TransportersArrivalAction_FormCaravan.tmpContainedThings[l]);
				}
			}
			TransportersArrivalAction_FormCaravan.tmpPawns.Clear();
			TransportersArrivalAction_FormCaravan.tmpContainedThings.Clear();
			Messages.Message(this.arrivalMessageKey.Translate(), caravan, MessageTypeDefOf.TaskCompletion, true);
			PeaceTalks peaceTalks = Find.WorldObjects.WorldObjectAt<PeaceTalks>(tile);
			if (peaceTalks != null)
			{
				peaceTalks.Notify_CaravanArrived(caravan);
			}
		}

		// Token: 0x0600E4AF RID: 58543 RVA: 0x0054F4EB File Offset: 0x0054D6EB
		public static bool CanFormCaravanAt(IEnumerable<IThingHolder> pods, PlanetTile tile)
		{
			return TransportersArrivalActionUtility.AnyPotentialCaravanOwner(pods, Faction.OfPlayer) && !Find.World.Impassable(tile) && tile.LayerDef.canFormCaravans;
		}

		// Token: 0x0600E4B0 RID: 58544 RVA: 0x0054F515 File Offset: 0x0054D715
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.arrivalMessageKey, "arrivalMessageKey", "MessageTransportPodsArrived", false);
		}

		// Token: 0x04009167 RID: 37223
		private string arrivalMessageKey = "MessageTransportPodsArrived";

		// Token: 0x04009168 RID: 37224
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04009169 RID: 37225
		private static readonly List<Thing> tmpContainedThings = new List<Thing>();
	}
}
