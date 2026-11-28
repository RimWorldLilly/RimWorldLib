using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CD RID: 4301
	public class QuestPart_LendColonistsToFaction : QuestPartActivable
	{
		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06006B7F RID: 27519 RVA: 0x0025B2CC File Offset: 0x002594CC
		public List<Thing> LentColonistsListForReading
		{
			get
			{
				return this.lentColonists;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x0025B2D4 File Offset: 0x002594D4
		public int ReturnPawnsInDurationTicks
		{
			get
			{
				return Mathf.Max(this.returnColonistsOnTick - GenTicks.TicksGame, 0);
			}
		}

		// Token: 0x06006B81 RID: 27521 RVA: 0x0025B2E8 File Offset: 0x002594E8
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
			if (this.lendColonistsToFaction != null && compTransporter != null)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)compTransporter.innerContainer))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.IsFreeColonist)
					{
						this.lentColonists.Add(pawn);
					}
				}
				this.returnColonistsOnTick = GenTicks.TicksGame + this.returnLentColonistsInTicks;
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06006B82 RID: 27522 RVA: 0x0025B37C File Offset: 0x0025957C
		public override string DescriptionPart
		{
			get
			{
				if (base.State == QuestPartState.Disabled || this.lentColonists.Count == 0)
				{
					return null;
				}
				return "PawnsLent".Translate((from t in this.lentColonists
				select t.LabelShort).ToCommaList(true, false), this.ReturnPawnsInDurationTicks.ToStringTicksToDays("0.0"));
			}
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x0025B3FC File Offset: 0x002595FC
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (ModsConfig.BiotechActive)
			{
				if (this.lentColonists.Any(delegate(Thing baby)
				{
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					return ChildcareUtility.CanSuckle(baby as Pawn, out breastfeedFailReason);
				}))
				{
					foreach (Thing thing in this.lentColonists)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
							if (firstHediffOfDef != null)
							{
								firstHediffOfDef.Severity = 1f;
							}
						}
					}
				}
			}
			if (Find.TickManager.TicksGame >= this.enableTick + this.returnLentColonistsInTicks)
			{
				base.Complete();
			}
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x0025B4D0 File Offset: 0x002596D0
		protected override void Complete(SignalArgs signalArgs)
		{
			MapParent mapParent = this.returnMap;
			Map map;
			if ((map = ((mapParent != null) ? mapParent.Map : null)) == null)
			{
				MapParent mapParent2 = this.quest.TryFindNewSuitableMapParentForRetarget();
				map = (((mapParent2 != null) ? mapParent2.Map : null) ?? Find.AnyPlayerHomeMap);
			}
			Map map2 = map;
			if (map2 == null)
			{
				return;
			}
			base.Complete(new SignalArgs(new LookTargets(this.lentColonists).Named("SUBJECT"), (from c in this.lentColonists
			select c.LabelShort).ToCommaList(true, false).Named("PAWNS")));
			if (this.lendColonistsToFaction != null && this.lendColonistsToFaction == Faction.OfEmpire)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				thing.SetFaction(Faction.OfEmpire, null);
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, this.lentColonists, thing);
				transportShip.ArriveAt(DropCellFinder.GetBestShuttleLandingSpot(map2, Faction.OfEmpire), map2.Parent);
				transportShip.AddJobs(new ShipJobDef[]
				{
					ShipJobDefOf.Unload,
					ShipJobDefOf.FlyAway
				});
				return;
			}
			DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(map2), map2, this.lentColonists, 110, false, false, false, false, true, null);
		}

		// Token: 0x06006B85 RID: 27525 RVA: 0x0025B600 File Offset: 0x00259800
		private void ReturnDead(Corpse corpse)
		{
			MapParent mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
			Map map = ((mapParent != null) ? mapParent.Map : null) ?? Find.AnyPlayerHomeMap;
			if (map != null)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(map), map, Gen.YieldSingle<Corpse>(corpse), 110, false, false, false, false, true, null);
			}
		}

		// Token: 0x06006B86 RID: 27526 RVA: 0x0025B64C File Offset: 0x0025984C
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.lentColonists.Contains(pawn))
			{
				Building_Grave assignedGrave = null;
				if (pawn.ownership != null)
				{
					assignedGrave = pawn.ownership.AssignedGrave;
				}
				Corpse corpse = pawn.MakeCorpse(assignedGrave, null);
				this.lentColonists.Remove(pawn);
				this.ReturnDead(corpse);
				if (!this.outSignalColonistsDied.NullOrEmpty() && this.lentColonists.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalColonistsDied, false));
				}
			}
		}

		// Token: 0x06006B87 RID: 27527 RVA: 0x0025B6CC File Offset: 0x002598CC
		public override void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
			if (this.lentColonists.Contains(birther))
			{
				Corpse corpse = baby as Corpse;
				if (corpse != null)
				{
					this.ReturnDead(corpse);
					return;
				}
				baby.SetFaction(birther.Faction, null);
				this.lentColonists.Add(baby);
			}
		}

		// Token: 0x06006B88 RID: 27528 RVA: 0x0025B714 File Offset: 0x00259914
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true, null))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06006B89 RID: 27529 RVA: 0x0025B784 File Offset: 0x00259984
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.lendColonistsToFaction, "lendColonistsToFaction", false);
			Scribe_Values.Look<int>(ref this.returnLentColonistsInTicks, "returnLentColonistsInTicks", 0, false);
			Scribe_Values.Look<int>(ref this.returnColonistsOnTick, "colonistsReturnOnTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.lentColonists, "lentPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.returnMap, "returnMap", false);
			Scribe_Values.Look<string>(ref this.outSignalColonistsDied, "outSignalColonistsDied", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lentColonists.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04004451 RID: 17489
		public Thing shuttle;

		// Token: 0x04004452 RID: 17490
		public Faction lendColonistsToFaction;

		// Token: 0x04004453 RID: 17491
		public int returnLentColonistsInTicks = -1;

		// Token: 0x04004454 RID: 17492
		public MapParent returnMap;

		// Token: 0x04004455 RID: 17493
		public string outSignalColonistsDied;

		// Token: 0x04004456 RID: 17494
		private int returnColonistsOnTick;

		// Token: 0x04004457 RID: 17495
		private List<Thing> lentColonists = new List<Thing>();
	}
}
