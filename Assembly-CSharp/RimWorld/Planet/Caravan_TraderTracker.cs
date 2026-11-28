using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023AA RID: 9130
	public class Caravan_TraderTracker : IExposable
	{
		// Token: 0x170027EA RID: 10218
		// (get) Token: 0x0600E344 RID: 58180 RVA: 0x00548790 File Offset: 0x00546990
		public TraderKindDef TraderKind
		{
			get
			{
				List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (this.caravan.IsOwner(pawn) && pawn.TraderKind != null)
					{
						return pawn.TraderKind;
					}
				}
				return null;
			}
		}

		// Token: 0x170027EB RID: 10219
		// (get) Token: 0x0600E345 RID: 58181 RVA: 0x005487E0 File Offset: 0x005469E0
		public IEnumerable<Thing> Goods
		{
			get
			{
				Caravan_TraderTracker.<get_Goods>d__5 <get_Goods>d__ = new Caravan_TraderTracker.<get_Goods>d__5(-2);
				<get_Goods>d__.<>4__this = this;
				return <get_Goods>d__;
			}
		}

		// Token: 0x170027EC RID: 10220
		// (get) Token: 0x0600E346 RID: 58182 RVA: 0x005487F0 File Offset: 0x005469F0
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		// Token: 0x170027ED RID: 10221
		// (get) Token: 0x0600E347 RID: 58183 RVA: 0x00548807 File Offset: 0x00546A07
		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		// Token: 0x170027EE RID: 10222
		// (get) Token: 0x0600E348 RID: 58184 RVA: 0x00548814 File Offset: 0x00546A14
		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		// Token: 0x0600E349 RID: 58185 RVA: 0x00548861 File Offset: 0x00546A61
		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600E34A RID: 58186 RVA: 0x0054887C File Offset: 0x00546A7C
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600E34B RID: 58187 RVA: 0x005488D2 File Offset: 0x00546AD2
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan_TraderTracker.<ColonyThingsWillingToBuy>d__14 <ColonyThingsWillingToBuy>d__ = new Caravan_TraderTracker.<ColonyThingsWillingToBuy>d__14(-2);
			<ColonyThingsWillingToBuy>d__.<>3__playerNegotiator = playerNegotiator;
			return <ColonyThingsWillingToBuy>d__;
		}

		// Token: 0x0600E34C RID: 58188 RVA: 0x005488E4 File Offset: 0x00546AE4
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + ((toGive != null) ? toGive.ToString() : null) + " to stock (pawn's trader tracker), but it's already here.");
				return;
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				this.caravan.AddPawn(pawn, false);
				if (pawn.IsWorldPawn() && !this.caravan.Spawned)
				{
					Find.WorldPawns.RemovePawn(pawn);
				}
				if (pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
					return;
				}
			}
			else
			{
				Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, this.caravan.PawnsListForReading, null, null);
				if (pawn2 == null)
				{
					string str = "Could not find pawn to move sold thing to (sold by player). thing=";
					Thing thing2 = thing;
					Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
				if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error("Could not add item to inventory.");
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600E34D RID: 58189 RVA: 0x005489FC File Offset: 0x00546BFC
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, this.caravan.PawnsListForReading, null);
				caravan.AddPawn(pawn, true);
				if (!pawn.IsWorldPawn() && caravan.Spawned)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.soldPrisoners.Remove(pawn);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				string str = "Could not find pawn to move bought thing to (bought by player). thing=";
				Thing thing2 = thing;
				Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add item to inventory.");
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040090F6 RID: 37110
		private Caravan caravan;

		// Token: 0x040090F7 RID: 37111
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
