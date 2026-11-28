using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B51 RID: 6993
	public class FlyShipLeaving : Skyfaller, IActiveTransporter, IThingHolder
	{
		// Token: 0x17002077 RID: 8311
		// (get) Token: 0x0600B383 RID: 45955 RVA: 0x003F4555 File Offset: 0x003F2755
		// (set) Token: 0x0600B384 RID: 45956 RVA: 0x003F456D File Offset: 0x003F276D
		public ActiveTransporterInfo Contents
		{
			get
			{
				return ((ActiveTransporter)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveTransporter)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600B385 RID: 45957 RVA: 0x003F4588 File Offset: 0x003F2788
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alreadyLeft, "alreadyLeft", false, false);
			Scribe_Values.Look<bool>(ref this.createWorldObject, "createWorldObject", true, false);
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjectDef");
		}

		// Token: 0x0600B386 RID: 45958 RVA: 0x003F4610 File Offset: 0x003F2810
		protected override void LeaveMap()
		{
			if (this.alreadyLeft || !this.createWorldObject)
			{
				if (this.Contents != null)
				{
					foreach (Thing thing in ((IEnumerable<Thing>)this.Contents.innerContainer))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							pawn.ExitMap(false, Rot4.Invalid);
						}
					}
					this.Contents.innerContainer.ClearAndDestroyContentsOrPassToWorld(DestroyMode.QuestLogic);
				}
				base.LeaveMap();
				return;
			}
			if (this.groupID < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Drop pod left the map, but its group ID is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.groupID);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!this.destinationTile.Valid)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(51, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Drop pod left the map, but its destination tile is ");
				defaultInterpolatedStringHandler2.AppendFormatted<PlanetTile>(this.destinationTile);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Lord lord = TransporterUtility.FindLord(this.groupID, base.Map);
			if (lord != null)
			{
				base.Map.lordManager.RemoveLord(lord);
			}
			TravellingTransporters travellingTransporters = (TravellingTransporters)WorldObjectMaker.MakeWorldObject(this.worldObjectDef ?? WorldObjectDefOf.TravellingTransporters);
			travellingTransporters.SetFaction(Faction.OfPlayer);
			travellingTransporters.destinationTile = this.destinationTile;
			travellingTransporters.arrivalAction = this.arrivalAction;
			PlanetTile planetTile = base.Map.Tile;
			if (planetTile.Layer != this.destinationTile.Layer)
			{
				planetTile = this.destinationTile.Layer.GetClosestTile_NewTemp(planetTile, false);
			}
			travellingTransporters.Tile = planetTile;
			Find.WorldObjects.Add(travellingTransporters);
			FlyShipLeaving.tmpActiveTransporters.Clear();
			FlyShipLeaving.tmpActiveTransporters.AddRange(base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ActiveTransporter));
			for (int i = 0; i < FlyShipLeaving.tmpActiveTransporters.Count; i++)
			{
				FlyShipLeaving flyShipLeaving = FlyShipLeaving.tmpActiveTransporters[i] as FlyShipLeaving;
				if (flyShipLeaving != null && flyShipLeaving.groupID == this.groupID)
				{
					flyShipLeaving.alreadyLeft = true;
					travellingTransporters.AddTransporter(flyShipLeaving.Contents, true);
					flyShipLeaving.Contents = null;
					flyShipLeaving.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0400678B RID: 26507
		public int groupID = -1;

		// Token: 0x0400678C RID: 26508
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x0400678D RID: 26509
		public TransportersArrivalAction arrivalAction;

		// Token: 0x0400678E RID: 26510
		public bool createWorldObject = true;

		// Token: 0x0400678F RID: 26511
		public WorldObjectDef worldObjectDef;

		// Token: 0x04006790 RID: 26512
		private bool alreadyLeft;

		// Token: 0x04006791 RID: 26513
		private static List<Thing> tmpActiveTransporters = new List<Thing>();
	}
}
