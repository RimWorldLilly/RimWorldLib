using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020015AB RID: 5547
	public class Pawn_TraderTracker : IExposable
	{
		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06008958 RID: 35160 RVA: 0x0031BC01 File Offset: 0x00319E01
		public IEnumerable<Thing> Goods
		{
			get
			{
				Pawn_TraderTracker.<get_Goods>d__4 <get_Goods>d__ = new Pawn_TraderTracker.<get_Goods>d__4(-2);
				<get_Goods>d__.<>4__this = this;
				return <get_Goods>d__;
			}
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x06008959 RID: 35161 RVA: 0x0031BC11 File Offset: 0x00319E11
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x0600895A RID: 35162 RVA: 0x0031BC28 File Offset: 0x00319E28
		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x0600895B RID: 35163 RVA: 0x0031BC38 File Offset: 0x00319E38
		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false, false, false, false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && (this.Goods.Any((Thing x) => this.traderKind.WillTrade(x.def)) || this.traderKind.tradeCurrency == TradeCurrency.Favor);
			}
		}

		// Token: 0x0600895C RID: 35164 RVA: 0x0031BD06 File Offset: 0x00319F06
		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600895D RID: 35165 RVA: 0x0031BD20 File Offset: 0x00319F20
		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600895E RID: 35166 RVA: 0x0031BD86 File Offset: 0x00319F86
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Pawn_TraderTracker.<ColonyThingsWillingToBuy>d__13 <ColonyThingsWillingToBuy>d__ = new Pawn_TraderTracker.<ColonyThingsWillingToBuy>d__13(-2);
			<ColonyThingsWillingToBuy>d__.<>4__this = this;
			return <ColonyThingsWillingToBuy>d__;
		}

		// Token: 0x0600895F RID: 35167 RVA: 0x0031BD98 File Offset: 0x00319F98
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + ((toGive != null) ? toGive.ToString() : null) + " to stock (pawn's trader tracker), but it's already here.");
				return;
			}
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
				this.AddPawnToStock(pawn);
				return;
			}
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
			if (thing2 != null)
			{
				if (!thing2.TryAbsorbStack(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				this.AddThingToRandomInventory(thing);
			}
		}

		// Token: 0x06008960 RID: 35168 RVA: 0x0031BE34 File Offset: 0x0031A034
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Undefined, null);
				}
				if (this.soldPrisoners.Contains(pawn))
				{
					this.soldPrisoners.Remove(pawn);
					return;
				}
			}
			else
			{
				IntVec3 positionHeld = toGive.PositionHeld;
				Map mapHeld = toGive.MapHeld;
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null, null, 1))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
						return;
					}
				}
				else
				{
					string str = "Could not place bought thing ";
					Thing thing2 = thing;
					string str2 = (thing2 != null) ? thing2.ToString() : null;
					string str3 = " at ";
					IntVec3 intVec = positionHeld;
					Log.Error(str + str2 + str3 + intVec.ToString());
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06008961 RID: 35169 RVA: 0x0031BF30 File Offset: 0x0031A130
		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map, WipeMode.Vanish);
			}
			if (newPawn.Faction != this.pawn.Faction)
			{
				newPawn.SetFaction(this.pawn.Faction, null);
			}
			if (newPawn.RaceProps.Humanlike)
			{
				newPawn.kindDef = PawnKindDefOf.Slave;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				newPawn.Destroy(DestroyMode.Vanish);
				string[] array = new string[7];
				array[0] = "Tried to sell pawn ";
				array[1] = ((newPawn != null) ? newPawn.ToString() : null);
				array[2] = " to ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", but ";
				int num2 = 5;
				Pawn pawn2 = this.pawn;
				array[num2] = ((pawn2 != null) ? pawn2.ToString() : null);
				array[6] = " has no lord. Traders without lord can't buy pawns.";
				Log.Error(string.Concat(array));
				return;
			}
			if (newPawn.RaceProps.Humanlike)
			{
				this.soldPrisoners.Add(newPawn);
			}
			lord.AddPawn(newPawn);
		}

		// Token: 0x06008962 RID: 35170 RVA: 0x0031C044 File Offset: 0x0031A244
		private void AddThingToRandomInventory(Thing thing)
		{
			Lord lord = this.pawn.GetLord();
			IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
			if (lord != null)
			{
				source = from x in lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
			}
			if (source.Any<Pawn>())
			{
				if (!source.RandomElement<Pawn>().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06008963 RID: 35171 RVA: 0x0031C0DC File Offset: 0x0031A2DC
		private bool ReachableForTrade(Thing thing)
		{
			Thing thing2 = thing;
			if (HaulAIUtility.IsInHaulableInventory(thing))
			{
				thing2 = thing.SpawnedParentOrMe;
			}
			return this.pawn.Map == thing2.MapHeld && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing2, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}

		// Token: 0x04005497 RID: 21655
		private Pawn pawn;

		// Token: 0x04005498 RID: 21656
		public TraderKindDef traderKind;

		// Token: 0x04005499 RID: 21657
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
