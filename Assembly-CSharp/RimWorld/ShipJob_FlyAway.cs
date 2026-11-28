using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D32 RID: 3378
	public class ShipJob_FlyAway : ShipJob
	{
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x0022A1DF File Offset: 0x002283DF
		protected override bool ShouldEnd
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06005EBA RID: 24250 RVA: 0x0022A1E7 File Offset: 0x002283E7
		public override bool Interruptible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x06005EBB RID: 24251 RVA: 0x0022A1EA File Offset: 0x002283EA
		public override bool HasDestination
		{
			get
			{
				return this.destinationTile.Valid;
			}
		}

		// Token: 0x06005EBC RID: 24252 RVA: 0x0022A1F8 File Offset: 0x002283F8
		public override bool TryStart()
		{
			if (!base.TryStart())
			{
				return false;
			}
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return false;
			}
			if (!this.transportShip.ShuttleComp.AllRequiredThingsLoaded && this.dropMode != TransportShipDropMode.None && this.transportShip.TransporterComp.innerContainer.Any)
			{
				ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
				shipJob_Unload.dropMode = this.dropMode;
				this.transportShip.SetNextJob(shipJob_Unload);
				return false;
			}
			IntVec3 position = this.transportShip.shipThing.Position;
			Map map = this.transportShip.shipThing.Map;
			if (!this.transportShip.TransporterComp.LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.transportShip.TransporterComp));
			}
			this.transportShip.TransporterComp.TryRemoveLord(map);
			this.transportShip.ShuttleComp.SendLaunchedSignals();
			QuestUtility.SendQuestTargetSignals(this.transportShip.questTags, "FlewAway", this.transportShip.Named("SUBJECT"));
			ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod, null);
			activeTransporter.Contents = new ActiveTransporterInfo();
			activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(this.transportShip.TransporterComp.GetDirectlyHeldThings(), true, true);
			activeTransporter.Rotation = this.transportShip.shipThing.Rotation;
			int groupID = this.transportShip.TransporterComp.groupID;
			if (!this.transportShip.shipThing.Destroyed)
			{
				this.transportShip.shipThing.DeSpawn(DestroyMode.Vanish);
			}
			FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(this.transportShip.def.leavingSkyfaller, activeTransporter);
			flyShipLeaving.groupID = groupID;
			if (!this.destinationTile.Valid)
			{
				flyShipLeaving.createWorldObject = false;
				if (!this.transportShip.shipThing.Destroyed && !Find.QuestManager.IsReservedByAnyQuest(this.transportShip))
				{
					this.transportShip.shipThing.Destroy(DestroyMode.QuestLogic);
				}
			}
			else
			{
				flyShipLeaving.createWorldObject = true;
				flyShipLeaving.worldObjectDef = this.transportShip.def.worldObject;
				flyShipLeaving.destinationTile = this.destinationTile;
				flyShipLeaving.arrivalAction = this.arrivalAction;
			}
			if (this.transportShip.shipThing.HasComp<CompShuttle>())
			{
				activeTransporter.Contents.SetShuttle(this.transportShip.shipThing);
			}
			GenSpawn.Spawn(flyShipLeaving, position, map, WipeMode.Vanish);
			this.initialized = true;
			return true;
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x0022A47C File Offset: 0x0022867C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlanetTile>(ref this.destinationTile, "destinationTile", default(PlanetTile), false);
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
			Scribe_Deep.Look<TransportersArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x0400359F RID: 13727
		public PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x040035A0 RID: 13728
		public TransportersArrivalAction arrivalAction;

		// Token: 0x040035A1 RID: 13729
		public TransportShipDropMode dropMode = TransportShipDropMode.All;

		// Token: 0x040035A2 RID: 13730
		private bool initialized;
	}
}
