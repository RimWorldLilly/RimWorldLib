using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D35 RID: 3381
	[StaticConstructorOnStartup]
	public abstract class ShipJob_Wait : ShipJob
	{
		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06005EC9 RID: 24265 RVA: 0x0022A8BE File Offset: 0x00228ABE
		public override bool ShowGizmos
		{
			get
			{
				return this.showGizmos;
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06005ECA RID: 24266 RVA: 0x0022A8C6 File Offset: 0x00228AC6
		private int MaxLaunchDistance
		{
			get
			{
				return this.transportShip.def.maxLaunchDistance;
			}
		}

		// Token: 0x06005ECB RID: 24267 RVA: 0x0022A8D8 File Offset: 0x00228AD8
		public override IEnumerable<Gizmo> GetJobGizmos()
		{
			ShipJob_Wait.<GetJobGizmos>d__12 <GetJobGizmos>d__ = new ShipJob_Wait.<GetJobGizmos>d__12(-2);
			<GetJobGizmos>d__.<>4__this = this;
			return <GetJobGizmos>d__;
		}

		// Token: 0x06005ECC RID: 24268 RVA: 0x0022A8E8 File Offset: 0x00228AE8
		private Command_Action LaunchAction()
		{
			return new Command_Action
			{
				defaultLabel = "CommandLaunchGroup".Translate(),
				defaultDesc = "CommandLaunchGroupDesc".Translate(),
				icon = CompLaunchable.LaunchCommandTex,
				alsoClickIfOtherInGroupClicked = false,
				action = delegate()
				{
					CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.transportShip.shipThing), CameraJumper.MovementMode.Pan);
					Find.WorldSelector.ClearSelection();
					PlanetTile tile = this.transportShip.shipThing.Map.Tile;
					Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
					{
						PlanetTile planetTile = tile;
						if (planetTile.Layer != Find.WorldSelector.SelectedLayer)
						{
							planetTile = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(planetTile, false);
						}
						GenDraw.DrawWorldRadiusRing(planetTile, this.MaxLaunchDistance, null);
					}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistance, Gen.YieldSingle<IThingHolder>(this.transportShip.TransporterComp), new Action<PlanetTile, TransportersArrivalAction>(this.Launch), null, null), null, null, false);
				}
			};
		}

		// Token: 0x06005ECD RID: 24269 RVA: 0x0022A948 File Offset: 0x00228B48
		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			PlanetTile tile = this.transportShip.shipThing.Map.Tile;
			return CompLaunchable.ChoseWorldTarget(target, tile, Gen.YieldSingle<IThingHolder>(this.transportShip.TransporterComp), this.MaxLaunchDistance, new Action<PlanetTile, TransportersArrivalAction>(this.Launch), null, null);
		}

		// Token: 0x06005ECE RID: 24270 RVA: 0x0022A9A0 File Offset: 0x00228BA0
		private void Launch(PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.destinationTile = destinationTile;
			shipJob_FlyAway.arrivalAction = arrivalAction;
			this.transportShip.SetNextJob(shipJob_FlyAway);
			this.transportShip.TryGetNextJob();
			CameraJumper.TryHideWorld();
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x0022A9E8 File Offset: 0x00228BE8
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.transportShip.ShipExistsAndIsSpawned)
			{
				if (this.leaveImmediatelyWhenSatisfied && this.transportShip.ShuttleComp.AllRequiredThingsLoaded)
				{
					this.SendAway();
					return;
				}
				if (this.transportShip.shipThing.IsHashIntervalTick(60, delta))
				{
					bool flag = false;
					if (!this.sendAwayIfAnyDespawnedDownedOrDead.NullOrEmpty<Thing>())
					{
						foreach (Thing thing in this.sendAwayIfAnyDespawnedDownedOrDead)
						{
							Pawn pawn = thing as Pawn;
							if ((!thing.Spawned || (pawn != null && (pawn.Dead || pawn.Downed))) && !this.transportShip.TransporterComp.innerContainer.Contains(thing))
							{
								flag = true;
								this.SendAway();
								break;
							}
						}
					}
					if (!flag && !this.sendAwayIfAllDespawned.NullOrEmpty<Thing>())
					{
						bool flag2 = false;
						foreach (Thing thing2 in this.sendAwayIfAllDespawned)
						{
							if (thing2.MapHeld == this.transportShip.shipThing.Map || thing2.MapHeld.PocketMapParent.sourceMap == this.transportShip.shipThing.Map || this.transportShip.TransporterComp.innerContainer.Contains(thing2))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.SendAway();
						}
					}
				}
			}
		}

		// Token: 0x06005ED0 RID: 24272 RVA: 0x0022AB90 File Offset: 0x00228D90
		protected virtual void SendAway()
		{
			this.transportShip.SetNextJob(ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway));
			this.End();
		}

		// Token: 0x06005ED1 RID: 24273 RVA: 0x0022ABB0 File Offset: 0x00228DB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.showGizmos, "showGizmos", true, false);
			Scribe_Values.Look<bool>(ref this.targetPlayerSettlement, "targetPlayerSettlement", false, false);
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawned", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.sendAwayIfAllDespawned;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
				List<Thing> list2 = this.sendAwayIfAnyDespawnedDownedOrDead;
				if (list2 == null)
				{
					return;
				}
				list2.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040035AC RID: 13740
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x040035AD RID: 13741
		public bool showGizmos = true;

		// Token: 0x040035AE RID: 13742
		public bool targetPlayerSettlement;

		// Token: 0x040035AF RID: 13743
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x040035B0 RID: 13744
		public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;

		// Token: 0x040035B1 RID: 13745
		private static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Commands/DismissShuttle", true);

		// Token: 0x040035B2 RID: 13746
		protected static readonly Texture2D SendCommandTex = CompLaunchable.LaunchCommandTex;

		// Token: 0x040035B3 RID: 13747
		private const int CheckAllDespawnedInterval = 60;
	}
}
