using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B77 RID: 7031
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x17002092 RID: 8338
		// (get) Token: 0x0600B47E RID: 46206 RVA: 0x003FA950 File Offset: 0x003F8B50
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x17002093 RID: 8339
		// (get) Token: 0x0600B47F RID: 46207 RVA: 0x003FA957 File Offset: 0x003F8B57
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x17002094 RID: 8340
		// (get) Token: 0x0600B480 RID: 46208 RVA: 0x003FA965 File Offset: 0x003F8B65
		public Map Map
		{
			get
			{
				PassingShipManager passingShipManager = this.passingShipManager;
				if (passingShipManager == null)
				{
					return null;
				}
				return passingShipManager.map;
			}
		}

		// Token: 0x17002095 RID: 8341
		// (get) Token: 0x0600B481 RID: 46209 RVA: 0x003FA978 File Offset: 0x003F8B78
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x0600B482 RID: 46210 RVA: 0x003FA980 File Offset: 0x003F8B80
		public PassingShip()
		{
		}

		// Token: 0x0600B483 RID: 46211 RVA: 0x003FA9A5 File Offset: 0x003F8BA5
		public PassingShip(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x0600B484 RID: 46212 RVA: 0x003FA9D4 File Offset: 0x003F8BD4
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x0600B485 RID: 46213 RVA: 0x003FAA28 File Offset: 0x003F8C28
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x0600B486 RID: 46214 RVA: 0x003FAA48 File Offset: 0x003F8C48
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(this.FullTitle), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		// Token: 0x0600B487 RID: 46215 RVA: 0x003FAAB7 File Offset: 0x003F8CB7
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600B488 RID: 46216 RVA: 0x003FAABE File Offset: 0x003F8CBE
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x0600B489 RID: 46217 RVA: 0x003FAAC6 File Offset: 0x003F8CC6
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x0600B48A RID: 46218 RVA: 0x003FAACE File Offset: 0x003F8CCE
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x0600B48B RID: 46219 RVA: 0x003FAAD1 File Offset: 0x003F8CD1
		protected virtual AcceptanceReport CanCommunicateWith(Pawn negotiator)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B48C RID: 46220 RVA: 0x003FAAD8 File Offset: 0x003F8CD8
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			string label = "CallOnRadio".Translate(this.GetCallLabel());
			Action action = null;
			AcceptanceReport canCommunicate = this.CanCommunicateWith(negotiator);
			if (!canCommunicate.Accepted)
			{
				if (!canCommunicate.Reason.NullOrEmpty())
				{
					action = delegate()
					{
						Messages.Message(canCommunicate.Reason, console, MessageTypeDefOf.RejectInput, false);
					};
				}
			}
			else
			{
				action = delegate()
				{
					if (!Building_OrbitalTradeBeacon.AllPowered(this.Map).Any<Building_OrbitalTradeBeacon>())
					{
						Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), console, MessageTypeDefOf.RejectInput, false);
						return;
					}
					console.GiveUseCommsJob(negotiator, this);
				};
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null, true, 0), negotiator, console, "ReservedBy", null);
		}

		// Token: 0x0600B48D RID: 46221 RVA: 0x003FAB97 File Offset: 0x003F8D97
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID.ToString();
		}

		// Token: 0x040067E4 RID: 26596
		public PassingShipManager passingShipManager;

		// Token: 0x040067E5 RID: 26597
		private Faction faction;

		// Token: 0x040067E6 RID: 26598
		public string name = "Nameless";

		// Token: 0x040067E7 RID: 26599
		protected int loadID = -1;

		// Token: 0x040067E8 RID: 26600
		public int ticksUntilDeparture = 40000;
	}
}
