using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D34 RID: 3380
	public class ShipJob_Unload : ShipJob
	{
		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06005EBF RID: 24255 RVA: 0x0022A4FC File Offset: 0x002286FC
		protected override bool ShouldEnd
		{
			get
			{
				return this.dropMode == TransportShipDropMode.None;
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06005EC0 RID: 24256 RVA: 0x0022A507 File Offset: 0x00228707
		public override bool ShowGizmos
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06005EC1 RID: 24257 RVA: 0x0022A50A File Offset: 0x0022870A
		private Thing ShipThing
		{
			get
			{
				return this.transportShip.shipThing;
			}
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x0022A517 File Offset: 0x00228717
		public override bool TryStart()
		{
			return this.transportShip.ShipExistsAndIsSpawned && base.TryStart();
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x0022A52E File Offset: 0x0022872E
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return;
			}
			if (this.ShipThing.IsHashIntervalTick(60, delta))
			{
				this.Drop();
			}
		}

		// Token: 0x06005EC4 RID: 24260 RVA: 0x0022A55C File Offset: 0x0022875C
		private void Drop()
		{
			Thing thing = null;
			float num = 0f;
			Map map = this.ShipThing.Map;
			for (int i = 0; i < this.transportShip.TransporterComp.innerContainer.Count; i++)
			{
				Thing thing2 = this.transportShip.TransporterComp.innerContainer[i];
				float dropPriority = this.GetDropPriority(thing2);
				if (dropPriority > num)
				{
					thing = thing2;
					num = dropPriority;
				}
			}
			if (thing != null && (thing is Pawn || this.dropMode != TransportShipDropMode.PawnsOnly))
			{
				ShipJob_Unload.UnloadThingFromShuttle(this.transportShip, thing, this.droppedThings, this.unforbidAll);
				return;
			}
			this.transportShip.TransporterComp.TryRemoveLord(map);
			this.End();
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x0022A610 File Offset: 0x00228810
		public static void UnloadThingFromShuttle(TransportShip ship, Thing thingToDrop, List<Thing> droppedThings = null, bool unforbidAll = false)
		{
			Thing shipThing = ship.shipThing;
			if (shipThing == null || !shipThing.Spawned)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to unload ");
				defaultInterpolatedStringHandler.AppendFormatted<Thing>(thingToDrop);
				defaultInterpolatedStringHandler.AppendLiteral(" from unspawned shuttle");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			Map map = ship.shipThing.Map;
			IntVec3 interactionCell = ship.shipThing.InteractionCell;
			Thing thing;
			if (ship.TransporterComp.innerContainer.TryDrop(thingToDrop, interactionCell, map, ThingPlaceMode.Near, out thing, null, delegate(IntVec3 c)
			{
				if (c.Fogged(map))
				{
					return false;
				}
				Pawn pawn2 = thingToDrop as Pawn;
				return pawn2 == null || !pawn2.Downed || c.GetFirstPawn(map) == null;
			}, !(thingToDrop is Pawn)))
			{
				ship.TransporterComp.Notify_ThingRemoved(thingToDrop);
				if (droppedThings != null)
				{
					droppedThings.Add(thingToDrop);
				}
				if (unforbidAll)
				{
					thingToDrop.SetForbidden(false, false);
				}
				Pawn pawn = thingToDrop as Pawn;
				if (pawn != null)
				{
					if (pawn != null && pawn.IsPlayerControlled && pawn.Spawned && pawn.drafter != null && !map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
					Pawn_GuestTracker guest = pawn.guest;
					if (guest != null && guest.IsPrisoner)
					{
						pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					if (pawn != null && pawn.IsColonist && pawn.inventory != null && map.IsPlayerHome)
					{
						pawn.inventory.UnloadEverything = true;
					}
				}
			}
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x0022A7AC File Offset: 0x002289AC
		private float GetDropPriority(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null)
			{
				return 0.25f;
			}
			if (this.droppedThings.Contains(t))
			{
				return 0f;
			}
			if (this.dropMode == TransportShipDropMode.NonRequired && this.transportShip.ShuttleComp.IsRequired(t))
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (((lord != null) ? lord.CurLordToil : null) != null)
			{
				LordToil_EnterShuttleOrLeave lordToil_EnterShuttleOrLeave = lord.CurLordToil as LordToil_EnterShuttleOrLeave;
				if (lordToil_EnterShuttleOrLeave != null && lordToil_EnterShuttleOrLeave.shuttle == this.ShipThing)
				{
					return 0f;
				}
			}
			if (!pawn.AnimalOrWildMan())
			{
				return 1f;
			}
			return 0.5f;
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x0022A850 File Offset: 0x00228A50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.droppedThings, "droppedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
			Scribe_Values.Look<bool>(ref this.unforbidAll, "unforbidAll", true, false);
		}

		// Token: 0x040035A8 RID: 13736
		public TransportShipDropMode dropMode = TransportShipDropMode.All;

		// Token: 0x040035A9 RID: 13737
		private List<Thing> droppedThings = new List<Thing>();

		// Token: 0x040035AA RID: 13738
		public bool unforbidAll = true;

		// Token: 0x040035AB RID: 13739
		private const int DropInterval = 60;
	}
}
