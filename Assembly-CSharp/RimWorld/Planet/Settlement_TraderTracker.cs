using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023BB RID: 9147
	public class Settlement_TraderTracker : IThingHolderTickable, IThingHolder, IExposable
	{
		// Token: 0x17002817 RID: 10263
		// (get) Token: 0x0600E3F4 RID: 58356 RVA: 0x0054BA5C File Offset: 0x00549C5C
		protected virtual int RegenerateStockEveryDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x17002818 RID: 10264
		// (get) Token: 0x0600E3F5 RID: 58357 RVA: 0x0054BA60 File Offset: 0x00549C60
		public IThingHolder ParentHolder
		{
			get
			{
				return this.settlement;
			}
		}

		// Token: 0x17002819 RID: 10265
		// (get) Token: 0x0600E3F6 RID: 58358 RVA: 0x0054BA68 File Offset: 0x00549C68
		public bool ShouldTickContents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700281A RID: 10266
		// (get) Token: 0x0600E3F7 RID: 58359 RVA: 0x0054BA6B File Offset: 0x00549C6B
		public List<Thing> StockListForReading
		{
			get
			{
				if (this.stock == null || this.stock.InnerListForReading.Empty<Thing>())
				{
					this.RegenerateStock();
				}
				return this.stock.InnerListForReading;
			}
		}

		// Token: 0x1700281B RID: 10267
		// (get) Token: 0x0600E3F8 RID: 58360 RVA: 0x0054BA98 File Offset: 0x00549C98
		public TraderKindDef TraderKind
		{
			get
			{
				List<TraderKindDef> baseTraderKinds = this.settlement.Faction.def.baseTraderKinds;
				if (baseTraderKinds.NullOrEmpty<TraderKindDef>())
				{
					return null;
				}
				int index = Mathf.Abs(this.settlement.HashOffset()) % baseTraderKinds.Count;
				return baseTraderKinds[index];
			}
		}

		// Token: 0x1700281C RID: 10268
		// (get) Token: 0x0600E3F9 RID: 58361 RVA: 0x0054BAE4 File Offset: 0x00549CE4
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.settlement.ID, 1933327354);
			}
		}

		// Token: 0x1700281D RID: 10269
		// (get) Token: 0x0600E3FA RID: 58362 RVA: 0x0054BAFB File Offset: 0x00549CFB
		public bool EverVisited
		{
			get
			{
				return this.everGeneratedStock;
			}
		}

		// Token: 0x1700281E RID: 10270
		// (get) Token: 0x0600E3FB RID: 58363 RVA: 0x0054BB03 File Offset: 0x00549D03
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.everGeneratedStock && this.stock == null;
			}
		}

		// Token: 0x1700281F RID: 10271
		// (get) Token: 0x0600E3FC RID: 58364 RVA: 0x0054BB18 File Offset: 0x00549D18
		public int NextRestockTick
		{
			get
			{
				if (this.stock == null || !this.everGeneratedStock)
				{
					return -1;
				}
				return ((this.lastStockGenerationTicks == -1) ? 0 : this.lastStockGenerationTicks) + this.RegenerateStockEveryDays * 60000;
			}
		}

		// Token: 0x17002820 RID: 10272
		// (get) Token: 0x0600E3FD RID: 58365 RVA: 0x0054BB4C File Offset: 0x00549D4C
		public virtual string TraderName
		{
			get
			{
				if (this.settlement.Faction == null)
				{
					return this.settlement.LabelCap;
				}
				return "SettlementTrader".Translate(this.settlement.LabelCap, this.settlement.Faction.Name);
			}
		}

		// Token: 0x17002821 RID: 10273
		// (get) Token: 0x0600E3FE RID: 58366 RVA: 0x0054BBA8 File Offset: 0x00549DA8
		public virtual bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && (this.stock == null || this.stock.InnerListForReading.Empty<Thing>() || this.stock.InnerListForReading.Any((Thing x) => this.TraderKind.WillTrade(x.def)));
			}
		}

		// Token: 0x17002822 RID: 10274
		// (get) Token: 0x0600E3FF RID: 58367 RVA: 0x0054BBF7 File Offset: 0x00549DF7
		public virtual float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x0600E400 RID: 58368 RVA: 0x0054BBFE File Offset: 0x00549DFE
		public Settlement_TraderTracker(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E401 RID: 58369 RVA: 0x0054BC20 File Offset: 0x00549E20
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpSavedPawns.Clear();
				if (this.stock != null)
				{
					for (int i = this.stock.Count - 1; i >= 0; i--)
					{
						Pawn pawn = this.stock[i] as Pawn;
						if (pawn != null)
						{
							this.stock.Remove(pawn);
							this.tmpSavedPawns.Add(pawn);
						}
					}
				}
			}
			Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.stock, "stock", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastStockGenerationTicks, "lastStockGenerationTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everGeneratedStock, "wasStockGeneratedYet", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving)
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.stock.TryAdd(this.tmpSavedPawns[j], false);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x0600E402 RID: 58370 RVA: 0x0054BD27 File Offset: 0x00549F27
		public virtual IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Settlement_TraderTracker.<ColonyThingsWillingToBuy>d__32 <ColonyThingsWillingToBuy>d__ = new Settlement_TraderTracker.<ColonyThingsWillingToBuy>d__32(-2);
			<ColonyThingsWillingToBuy>d__.<>3__playerNegotiator = playerNegotiator;
			return <ColonyThingsWillingToBuy>d__;
		}

		// Token: 0x0600E403 RID: 58371 RVA: 0x0054BD38 File Offset: 0x00549F38
		public virtual void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.stock == null)
			{
				this.RegenerateStock();
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.settlement);
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				if (pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!this.stock.TryAdd(pawn, false))
				{
					pawn.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.stock.TryAdd(thing, false))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600E404 RID: 58372 RVA: 0x0054BDC4 File Offset: 0x00549FC4
		public virtual void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.settlement);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				caravan.AddPawn(pawn, true);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				Log.Error("Could not find any pawn to give sold thing to.");
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add sold thing to inventory.");
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600E405 RID: 58373 RVA: 0x0054BE4C File Offset: 0x0054A04C
		public virtual void TraderTrackerTick()
		{
			if (this.stock != null)
			{
				if (Find.TickManager.TicksGame - this.lastStockGenerationTicks > this.RegenerateStockEveryDays * 60000)
				{
					this.TryDestroyStock();
					return;
				}
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.stock[i] as Pawn;
					if (pawn != null && pawn.Destroyed)
					{
						this.stock.Remove(pawn);
					}
				}
				for (int j = this.stock.Count - 1; j >= 0; j--)
				{
					Pawn pawn2 = this.stock[j] as Pawn;
					if (pawn2 != null && !pawn2.IsWorldPawn())
					{
						Log.Error("Faction base has non-world-pawns in its stock. Removing...");
						this.stock.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x0600E406 RID: 58374 RVA: 0x0054BF18 File Offset: 0x0054A118
		public void TryDestroyStock()
		{
			if (this.stock != null)
			{
				for (int i = this.stock.Count - 1; i >= 0; i--)
				{
					Thing thing = this.stock[i];
					this.stock.Remove(thing);
					if (!(thing is Pawn) && !thing.Destroyed)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				this.stock = null;
			}
		}

		// Token: 0x0600E407 RID: 58375 RVA: 0x0054BF7D File Offset: 0x0054A17D
		public bool ContainsPawn(Pawn p)
		{
			return this.stock != null && this.stock.Contains(p);
		}

		// Token: 0x0600E408 RID: 58376 RVA: 0x0054BF98 File Offset: 0x0054A198
		protected virtual void RegenerateStock()
		{
			this.TryDestroyStock();
			this.stock = new ThingOwner<Thing>(this)
			{
				dontTickContents = true
			};
			this.everGeneratedStock = true;
			if (this.settlement.Faction == null || !this.settlement.Faction.IsPlayer)
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.traderDef = this.TraderKind;
				parms.tile = new PlanetTile?(this.settlement.Tile);
				parms.makingFaction = this.settlement.Faction;
				this.stock.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
			}
			for (int i = 0; i < this.stock.Count; i++)
			{
				Pawn pawn = this.stock[i] as Pawn;
				if (pawn != null)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
			}
			this.lastStockGenerationTicks = Find.TickManager.TicksGame;
		}

		// Token: 0x0600E409 RID: 58377 RVA: 0x0054C086 File Offset: 0x0054A286
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.stock;
		}

		// Token: 0x0600E40A RID: 58378 RVA: 0x0054C08E File Offset: 0x0054A28E
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0400911F RID: 37151
		public Settlement settlement;

		// Token: 0x04009120 RID: 37152
		private ThingOwner<Thing> stock;

		// Token: 0x04009121 RID: 37153
		private int lastStockGenerationTicks = -1;

		// Token: 0x04009122 RID: 37154
		private bool everGeneratedStock;

		// Token: 0x04009123 RID: 37155
		private const float DefaultTradePriceImprovement = 0.02f;

		// Token: 0x04009124 RID: 37156
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
