using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet
{
	// Token: 0x0200237D RID: 9085
	[StaticConstructorOnStartup]
	public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ILoadReferenceable, ITrader
	{
		// Token: 0x170027B2 RID: 10162
		// (get) Token: 0x0600E198 RID: 57752 RVA: 0x0053B57E File Offset: 0x0053977E
		public List<Pawn> PawnsListForReading
		{
			get
			{
				return this.pawns.InnerListForReading;
			}
		}

		// Token: 0x170027B3 RID: 10163
		// (get) Token: 0x0600E199 RID: 57753 RVA: 0x0053B58B File Offset: 0x0053978B
		public override float DrawAltitude
		{
			get
			{
				return 0.035f;
			}
		}

		// Token: 0x170027B4 RID: 10164
		// (get) Token: 0x0600E19A RID: 57754 RVA: 0x0053B594 File Offset: 0x00539794
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction == null)
					{
						color = Color.white;
					}
					else if (base.Faction.IsPlayer)
					{
						color = Caravan.PlayerCaravanColor;
					}
					else
					{
						color = base.Faction.Color;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3600);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x170027B5 RID: 10165
		// (get) Token: 0x0600E19B RID: 57755 RVA: 0x0053B607 File Offset: 0x00539807
		// (set) Token: 0x0600E19C RID: 57756 RVA: 0x0053B60F File Offset: 0x0053980F
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x170027B6 RID: 10166
		// (get) Token: 0x0600E19D RID: 57757 RVA: 0x0053B618 File Offset: 0x00539818
		public override Vector3 DrawPos
		{
			get
			{
				return this.tweener.TweenedPos;
			}
		}

		// Token: 0x170027B7 RID: 10167
		// (get) Token: 0x0600E19E RID: 57758 RVA: 0x0053B625 File Offset: 0x00539825
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170027B8 RID: 10168
		// (get) Token: 0x0600E19F RID: 57759 RVA: 0x0053B634 File Offset: 0x00539834
		public bool ImmobilizedByMass
		{
			get
			{
				if (Find.TickManager.TicksGame - this.cachedImmobilizedForTicks < 60)
				{
					return this.cachedImmobilized;
				}
				this.cachedImmobilized = (this.MassUsage > this.MassCapacity);
				this.cachedImmobilizedForTicks = Find.TickManager.TicksGame;
				return this.cachedImmobilized;
			}
		}

		// Token: 0x170027B9 RID: 10169
		// (get) Token: 0x0600E1A0 RID: 57760 RVA: 0x0053B688 File Offset: 0x00539888
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		public ValueTuple<float, float> DaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (Find.TickManager.TicksGame - this.cachedDaysWorthOfFoodForTicks < 3000)
				{
					return this.cachedDaysWorthOfFood;
				}
				this.cachedDaysWorthOfFood = new ValueTuple<float, float>(DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this), DaysUntilRotCalculator.ApproxDaysUntilRot(this));
				this.cachedDaysWorthOfFoodForTicks = Find.TickManager.TicksGame;
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x170027BA RID: 10170
		// (get) Token: 0x0600E1A1 RID: 57761 RVA: 0x0053B6E1 File Offset: 0x005398E1
		public bool CantMove
		{
			get
			{
				return this.NightResting || this.pather.Paused || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass || this.Shuttle != null;
			}
		}

		// Token: 0x170027BB RID: 10171
		// (get) Token: 0x0600E1A2 RID: 57762 RVA: 0x0053B71B File Offset: 0x0053991B
		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, null);
			}
		}

		// Token: 0x170027BC RID: 10172
		// (get) Token: 0x0600E1A3 RID: 57763 RVA: 0x0053B72C File Offset: 0x0053992C
		public string MassCapacityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170027BD RID: 10173
		// (get) Token: 0x0600E1A4 RID: 57764 RVA: 0x0053B752 File Offset: 0x00539952
		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage<Pawn>(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		// Token: 0x170027BE RID: 10174
		// (get) Token: 0x0600E1A5 RID: 57765 RVA: 0x0053B764 File Offset: 0x00539964
		public bool AllOwnersDowned
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].Downed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170027BF RID: 10175
		// (get) Token: 0x0600E1A6 RID: 57766 RVA: 0x0053B7B4 File Offset: 0x005399B4
		public bool AllOwnersHaveMentalBreak
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170027C0 RID: 10176
		// (get) Token: 0x0600E1A7 RID: 57767 RVA: 0x0053B804 File Offset: 0x00539A04
		public bool NightResting
		{
			get
			{
				return base.Spawned && this.needs.AnyPawnsNeedRest && (!this.pather.Moving || !(this.pather.nextTile == this.pather.Destination) || !Caravan_PathFollower.IsValidFinalPushDestination(this.pather.Destination) || Mathf.CeilToInt(this.pather.nextTileCostLeft / 1f) > 10000) && CaravanNightRestUtility.RestingNowAt(base.Tile);
			}
		}

		// Token: 0x170027C1 RID: 10177
		// (get) Token: 0x0600E1A8 RID: 57768 RVA: 0x0053B890 File Offset: 0x00539A90
		public int LeftRestTicks
		{
			get
			{
				if (!this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftRestTicksAt(base.Tile);
			}
		}

		// Token: 0x170027C2 RID: 10178
		// (get) Token: 0x0600E1A9 RID: 57769 RVA: 0x0053B8A7 File Offset: 0x00539AA7
		public int LeftNonRestTicks
		{
			get
			{
				if (this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftNonRestTicksAt(base.Tile);
			}
		}

		// Token: 0x170027C3 RID: 10179
		// (get) Token: 0x0600E1AA RID: 57770 RVA: 0x0053B8BE File Offset: 0x00539ABE
		public override string Label
		{
			get
			{
				if (this.nameInt != null)
				{
					return this.nameInt;
				}
				return base.Label;
			}
		}

		// Token: 0x170027C4 RID: 10180
		// (get) Token: 0x0600E1AB RID: 57771 RVA: 0x0053B8D5 File Offset: 0x00539AD5
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x170027C5 RID: 10181
		// (get) Token: 0x0600E1AC RID: 57772 RVA: 0x0053B8E5 File Offset: 0x00539AE5
		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this, null);
			}
		}

		// Token: 0x170027C6 RID: 10182
		// (get) Token: 0x0600E1AD RID: 57773 RVA: 0x0053B8EE File Offset: 0x00539AEE
		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170027C7 RID: 10183
		// (get) Token: 0x0600E1AE RID: 57774 RVA: 0x0053B8F1 File Offset: 0x00539AF1
		public float Visibility
		{
			get
			{
				return CaravanVisibilityCalculator.Visibility(this, null);
			}
		}

		// Token: 0x170027C8 RID: 10184
		// (get) Token: 0x0600E1AF RID: 57775 RVA: 0x0053B8FC File Offset: 0x00539AFC
		public string VisibilityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanVisibilityCalculator.Visibility(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170027C9 RID: 10185
		// (get) Token: 0x0600E1B0 RID: 57776 RVA: 0x0053B920 File Offset: 0x00539B20
		public string TicksPerMoveExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170027CA RID: 10186
		// (get) Token: 0x0600E1B1 RID: 57777 RVA: 0x0053B941 File Offset: 0x00539B41
		public IEnumerable<Thing> AllThings
		{
			get
			{
				return CaravanInventoryUtility.AllInventoryItems(this).Concat(this.pawns);
			}
		}

		// Token: 0x170027CB RID: 10187
		// (get) Token: 0x0600E1B2 RID: 57778 RVA: 0x0053B954 File Offset: 0x00539B54
		public Building_PassengerShuttle Shuttle
		{
			get
			{
				if (this.hasShuttleDirty)
				{
					this.hasShuttleDirty = false;
					this.cachedShuttle = CaravanInventoryUtility.FindShuttle(this);
				}
				return this.cachedShuttle;
			}
		}

		// Token: 0x170027CC RID: 10188
		// (get) Token: 0x0600E1B3 RID: 57779 RVA: 0x0053B977 File Offset: 0x00539B77
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueId ^ 728241121;
			}
		}

		// Token: 0x170027CD RID: 10189
		// (get) Token: 0x0600E1B4 RID: 57780 RVA: 0x0053B985 File Offset: 0x00539B85
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x170027CE RID: 10190
		// (get) Token: 0x0600E1B5 RID: 57781 RVA: 0x0053B98D File Offset: 0x00539B8D
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050);
				return null;
			}
		}

		// Token: 0x170027CF RID: 10191
		// (get) Token: 0x0600E1B6 RID: 57782 RVA: 0x0053B9A0 File Offset: 0x00539BA0
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return 0f;
				}
				float num = 0f;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(this.pawns[i]);
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						float num2 = this.pawns[i].MarketValue;
						if (this.pawns[i].IsSlave)
						{
							num2 *= 0.75f;
						}
						num += num2;
					}
				}
				return num * 0.7f;
			}
		}

		// Token: 0x170027D0 RID: 10192
		// (get) Token: 0x0600E1B7 RID: 57783 RVA: 0x0053BA3B File Offset: 0x00539C3B
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from x in this.PawnsListForReading
				where x.Faction == Faction.OfPlayer
				select x;
			}
		}

		// Token: 0x170027D1 RID: 10193
		// (get) Token: 0x0600E1B8 RID: 57784 RVA: 0x0053BA75 File Offset: 0x00539C75
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.CaravanPointsRandomFactorRange;
			}
		}

		// Token: 0x0600E1B9 RID: 57785 RVA: 0x0053BA7C File Offset: 0x00539C7C
		public void SetUniqueId(int newId)
		{
			if (this.uniqueId != -1 || newId < 0)
			{
				Log.Error("Tried to set caravan with uniqueId " + this.uniqueId.ToString() + " to have uniqueId " + newId.ToString());
			}
			this.uniqueId = newId;
		}

		// Token: 0x170027D2 RID: 10194
		// (get) Token: 0x0600E1BA RID: 57786 RVA: 0x0053BAB8 File Offset: 0x00539CB8
		public TraderKindDef TraderKind
		{
			get
			{
				return this.trader.TraderKind;
			}
		}

		// Token: 0x170027D3 RID: 10195
		// (get) Token: 0x0600E1BB RID: 57787 RVA: 0x0053BAC5 File Offset: 0x00539CC5
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x170027D4 RID: 10196
		// (get) Token: 0x0600E1BC RID: 57788 RVA: 0x0053BAD2 File Offset: 0x00539CD2
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x170027D5 RID: 10197
		// (get) Token: 0x0600E1BD RID: 57789 RVA: 0x0053BADF File Offset: 0x00539CDF
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x170027D6 RID: 10198
		// (get) Token: 0x0600E1BE RID: 57790 RVA: 0x0053BAEC File Offset: 0x00539CEC
		public bool CanTradeNow
		{
			get
			{
				return this.trader.CanTradeNow;
			}
		}

		// Token: 0x170027D7 RID: 10199
		// (get) Token: 0x0600E1BF RID: 57791 RVA: 0x0053BAF9 File Offset: 0x00539CF9
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170027D8 RID: 10200
		// (get) Token: 0x0600E1C0 RID: 57792 RVA: 0x0053BB00 File Offset: 0x00539D00
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x0600E1C1 RID: 57793 RVA: 0x0053BB0D File Offset: 0x00539D0D
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x0600E1C2 RID: 57794 RVA: 0x0053BB1B File Offset: 0x00539D1B
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0600E1C3 RID: 57795 RVA: 0x0053BB2B File Offset: 0x00539D2B
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0600E1C4 RID: 57796 RVA: 0x0053BB3C File Offset: 0x00539D3C
		public Caravan()
		{
			this.pawns = new ThingOwner<Pawn>(this, false, LookMode.Reference, true);
			this.pather = new Caravan_PathFollower(this);
			this.gotoMote = new Caravan_GotoMoteRenderer();
			this.tweener = new Caravan_Tweener(this);
			this.trader = new Caravan_TraderTracker(this);
			this.forage = new Caravan_ForageTracker(this);
			this.needs = new Caravan_NeedsTracker(this);
			this.carryTracker = new Caravan_CarryTracker(this);
			this.beds = new Caravan_BedsTracker(this);
			if (ModsConfig.BiotechActive)
			{
				this.babies = new Caravan_BabyTracker(this);
			}
			this.storyState = new StoryState(this);
		}

		// Token: 0x0600E1C5 RID: 57797 RVA: 0x0053BC00 File Offset: 0x00539E00
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawns, "pawns", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.autoJoinable, "autoJoinable", false, false);
			Scribe_Deep.Look<Caravan_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_ForageTracker>(ref this.forage, "forage", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_BedsTracker>(ref this.beds, "beds", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Caravan_BabyTracker caravan_BabyTracker = this.babies;
				if (caravan_BabyTracker == null)
				{
					return;
				}
				caravan_BabyTracker.Recache();
			}
		}

		// Token: 0x0600E1C6 RID: 57798 RVA: 0x0053BD6A File Offset: 0x00539F6A
		public override void PostAdd()
		{
			base.PostAdd();
			this.carryTracker.Notify_CaravanSpawned();
			this.beds.Notify_CaravanSpawned();
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker != null)
			{
				caravan_BabyTracker.Recache();
			}
			Find.ColonistBar.MarkColonistsDirty();
			this.hasShuttleDirty = true;
		}

		// Token: 0x0600E1C7 RID: 57799 RVA: 0x0053BDAA File Offset: 0x00539FAA
		public override void PostRemove()
		{
			base.PostRemove();
			this.pather.StopDead();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x0600E1C8 RID: 57800 RVA: 0x0053BDC8 File Offset: 0x00539FC8
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.CheckAnyNonWorldPawns();
			this.pather.PatherTickInterval(delta);
			this.tweener.TweenerTickInterval(delta);
			this.forage.ForageTrackerTickInterval(delta);
			this.carryTracker.CarryTrackerTickInterval(delta);
			this.beds.BedsTrackerTickInterval(delta);
			this.needs.NeedsTrackerTickInterval(delta);
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker != null)
			{
				caravan_BabyTracker.TickInterval(delta);
			}
			CaravanDrugPolicyUtility.CheckTakeScheduledDrugs(this, delta);
			CaravanTendUtility.CheckTend(this, delta);
			if (ModsConfig.BiotechActive)
			{
				CaravanPollutionUtility.CheckDamageFromPollution(this, delta);
			}
		}

		// Token: 0x0600E1C9 RID: 57801 RVA: 0x0053BE58 File Offset: 0x0053A058
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x0600E1CA RID: 57802 RVA: 0x0053BE6B File Offset: 0x0053A06B
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.gotoMote.RenderMote();
		}

		// Token: 0x0600E1CB RID: 57803 RVA: 0x0053BEA4 File Offset: 0x0053A0A4
		public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (p == null)
			{
				Log.Warning("Tried to add a null pawn to " + ((this != null) ? this.ToString() : null));
				return;
			}
			if (p.Dead)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					(p != null) ? p.ToString() : null,
					" to ",
					(this != null) ? this.ToString() : null,
					", but this pawn is dead."
				}));
				return;
			}
			Pawn pawn = p.carryTracker.CarriedThing as Pawn;
			if (pawn != null)
			{
				p.carryTracker.innerContainer.Remove(pawn);
			}
			p.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (this.pawns.TryAdd(p, true))
			{
				if (this.ShouldAutoCapture(p))
				{
					p.guest.CapturedBy(base.Faction, null);
				}
				if (pawn != null)
				{
					if (this.ShouldAutoCapture(pawn))
					{
						pawn.guest.CapturedBy(base.Faction, p);
					}
					this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
					if (addCarriedPawnToWorldPawnsIfAny)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return;
					}
				}
			}
			else
			{
				Log.Error("Couldn't add pawn " + ((p != null) ? p.ToString() : null) + " to caravan.");
			}
		}

		// Token: 0x0600E1CC RID: 57804 RVA: 0x0053BFD4 File Offset: 0x0053A1D4
		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + ((this != null) ? this.ToString() : null));
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
				return;
			}
			CaravanInventoryUtility.GiveThing(this, thing);
		}

		// Token: 0x0600E1CD RID: 57805 RVA: 0x0053C01B File Offset: 0x0053A21B
		public bool ContainsPawn(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600E1CE RID: 57806 RVA: 0x0053C029 File Offset: 0x0053A229
		public void RemovePawn(Pawn p)
		{
			this.pawns.Remove(p);
		}

		// Token: 0x0600E1CF RID: 57807 RVA: 0x0053C038 File Offset: 0x0053A238
		public void RemoveAllPawns()
		{
			this.pawns.Clear();
		}

		// Token: 0x0600E1D0 RID: 57808 RVA: 0x0053C045 File Offset: 0x0053A245
		public bool IsOwner(Pawn p)
		{
			return this.pawns.Contains(p) && CaravanUtility.IsOwner(p, base.Faction);
		}

		// Token: 0x0600E1D1 RID: 57809 RVA: 0x0053C064 File Offset: 0x0053A264
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].IsColonist)
				{
					num++;
				}
				else if (this.pawns[i].RaceProps.Animal)
				{
					num2++;
				}
				else if (this.pawns[i].IsPrisoner)
				{
					num3++;
				}
				else if (this.pawns[i].RaceProps.IsMechanoid)
				{
					num6++;
				}
				if (this.pawns[i].Downed && !this.pawns[i].ageTracker.CurLifeStage.alwaysDowned)
				{
					num4++;
				}
				if (this.pawns[i].InMentalState)
				{
					num5++;
				}
			}
			stringBuilder.Append("CaravanColonistsCount".Translate(num, (num == 1) ? Faction.OfPlayer.def.pawnSingular : Faction.OfPlayer.def.pawnsPlural));
			if (num2 == 1)
			{
				stringBuilder.Append(", " + "CaravanAnimal".Translate());
			}
			else if (num2 > 1)
			{
				stringBuilder.Append(", " + "CaravanAnimalsCount".Translate(num2));
			}
			if (num3 == 1)
			{
				stringBuilder.Append(", " + "CaravanPrisoner".Translate());
			}
			else if (num3 > 1)
			{
				stringBuilder.Append(", " + "CaravanPrisonersCount".Translate(num3));
			}
			if (ModsConfig.BiotechActive)
			{
				if (num6 == 1)
				{
					stringBuilder.Append(", " + "CaravanMech".Translate());
				}
				else if (num6 > 1)
				{
					stringBuilder.Append(", " + "CaravanMechsCount".Translate(num6));
				}
			}
			stringBuilder.AppendLine();
			if (num5 > 0)
			{
				stringBuilder.Append("CaravanPawnsInMentalState".Translate(num5));
			}
			if (num4 > 0)
			{
				if (num5 > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("CaravanPawnsDowned".Translate(num4));
			}
			if (num5 > 0 || num4 > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.pather.Moving)
			{
				if (this.pather.ArrivalAction != null)
				{
					stringBuilder.Append(this.pather.ArrivalAction.ReportString);
				}
				else
				{
					stringBuilder.Append("CaravanTraveling".Translate());
				}
			}
			else
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(this);
				if (settlement != null)
				{
					stringBuilder.Append("CaravanVisiting".Translate(settlement.Label));
				}
				else
				{
					stringBuilder.Append("CaravanWaiting".Translate());
				}
			}
			if (this.pather.Moving)
			{
				float num7 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, true) / 60000f;
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(num7.ToString("0.#")));
			}
			if (this.AllOwnersDowned)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersDowned".Translate());
			}
			else if (this.AllOwnersHaveMentalBreak)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
			}
			else if (this.ImmobilizedByMass)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanImmobilizedByMass".Translate());
			}
			string text;
			if (this.needs.AnyPawnOutOfFood(out text))
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanOutOfFood".Translate());
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(text);
					stringBuilder.Append(".");
				}
			}
			if (!this.pather.MovingNow)
			{
				int usedBedCount = this.beds.GetUsedBedCount();
				stringBuilder.AppendLine();
				stringBuilder.Append(CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), usedBedCount));
			}
			else
			{
				string inspectStringLine = this.carryTracker.GetInspectStringLine();
				if (!inspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inspectStringLine);
				}
				string inBedForMedicalReasonsInspectStringLine = this.beds.GetInBedForMedicalReasonsInspectStringLine();
				if (!inBedForMedicalReasonsInspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inBedForMedicalReasonsInspectStringLine);
				}
			}
			if (ModsConfig.OdysseyActive && this.Shuttle != null)
			{
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CaravanShuttleFuel".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.Shuttle.FuelLevel, "F0");
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.Shuttle.MaxFuelLevel, "F0");
				stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600E1D2 RID: 57810 RVA: 0x0053C5ED File Offset: 0x0053A7ED
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Caravan.<GetGizmos>d__123 <GetGizmos>d__ = new Caravan.<GetGizmos>d__123(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E1D3 RID: 57811 RVA: 0x0053C600 File Offset: 0x0053A800
		private Gizmo GetLaunchGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandLaunchGroup".Translate();
			command_Action.defaultDesc = "CommandLaunchGroupDesc".Translate();
			command_Action.icon = CompLaunchable.LaunchCommandTex;
			command_Action.action = delegate()
			{
				this.Shuttle.LaunchableComp.StartChoosingDestination(delegate(PlanetTile tile, TransportersArrivalAction action)
				{
					CaravanShuttleUtility.LaunchShuttle(this, tile, action);
				}, null);
			};
			AcceptanceReport acceptanceReport = CaravanShuttleUtility.CanLaunchCaravanShuttle(this);
			if (!acceptanceReport.Accepted)
			{
				command_Action.Disable(acceptanceReport.Reason);
			}
			return command_Action;
		}

		// Token: 0x0600E1D4 RID: 57812 RVA: 0x0053C678 File Offset: 0x0053A878
		private Gizmo GetRefuelGizmo()
		{
			float fuelInCaravan = CaravanShuttleUtility.FuelInCaravan(this);
			string text = null;
			if (fuelInCaravan <= 0f)
			{
				text = "NoFuelInCaravan".Translate();
			}
			if (Mathf.Approximately(this.Shuttle.FuelLevel, this.Shuttle.MaxFuelLevel))
			{
				text = "ShuttleFullyFueled".Translate();
			}
			Action<int> <>9__2;
			return new Command_Action
			{
				defaultLabel = "CommandRefuelShuttleFromCargo".Translate(),
				defaultDesc = "CommandRefuelShuttleFromCargoDesc".Translate(),
				icon = Building_PassengerShuttle.RefuelFromCargoIcon.Texture,
				action = delegate()
				{
					int num = Mathf.FloorToInt(Mathf.Min(fuelInCaravan, this.Shuttle.MaxFuelLevel - this.Shuttle.FuelLevel));
					Func<int, string> textGetter = (int val) => "RefuelShuttleCount".Translate(val);
					int from = 1;
					int to = num;
					Action<int> confirmAction;
					if ((confirmAction = <>9__2) == null)
					{
						confirmAction = (<>9__2 = delegate(int count)
						{
							CaravanShuttleUtility.ConsumeFuelFromCaravanInventory(this, count);
							this.Shuttle.RefuelableComp.Refuel((float)count);
						});
					}
					Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue, 1f);
					Find.WindowStack.Add(window);
				},
				Disabled = !text.NullOrEmpty(),
				disabledReason = text
			};
		}

		// Token: 0x0600E1D5 RID: 57813 RVA: 0x0053C751 File Offset: 0x0053A951
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			Caravan.<GetTransportersFloatMenuOptions>d__126 <GetTransportersFloatMenuOptions>d__ = new Caravan.<GetTransportersFloatMenuOptions>d__126(-2);
			<GetTransportersFloatMenuOptions>d__.<>4__this = this;
			<GetTransportersFloatMenuOptions>d__.<>3__pods = pods;
			<GetTransportersFloatMenuOptions>d__.<>3__launchAction = launchAction;
			return <GetTransportersFloatMenuOptions>d__;
		}

		// Token: 0x0600E1D6 RID: 57814 RVA: 0x0053C76F File Offset: 0x0053A96F
		public void RecacheInventory()
		{
			this.cachedImmobilizedForTicks = -99999;
			this.cachedDaysWorthOfFoodForTicks = -99999;
			this.hasShuttleDirty = true;
		}

		// Token: 0x0600E1D7 RID: 57815 RVA: 0x0053C790 File Offset: 0x0053A990
		public virtual void Notify_MemberDied(Pawn member)
		{
			if (!base.Spawned)
			{
				Log.Error("Caravan member died in an unspawned caravan. Unspawned caravans shouldn't be kept for more than a single frame.");
			}
			if (!this.PawnsListForReading.Any((Pawn x) => x != member && this.IsOwner(x)))
			{
				List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Notify_AbandonedAtTile(base.Tile);
				}
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					if (ModsConfig.IdeologyActive && this.PawnsListForReading.Any((Pawn x) => x != member && x.IsSlave))
					{
						Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterOnlySlaveCaravanColonistsLeft".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null, 0, true);
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null, 0, true);
					}
				}
				this.pawns.Clear();
				this.Destroy();
				return;
			}
			member.Strip(true);
			this.RemovePawn(member);
		}

		// Token: 0x0600E1D8 RID: 57816 RVA: 0x0053C909 File Offset: 0x0053AB09
		public virtual void Notify_Merged(List<Caravan> group)
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x0600E1D9 RID: 57817 RVA: 0x0053C912 File Offset: 0x0053AB12
		public virtual void Notify_StartedTrading()
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x0600E1DA RID: 57818 RVA: 0x0053C91C File Offset: 0x0053AB1C
		private void CheckAnyNonWorldPawns()
		{
			for (int i = this.pawns.Count - 1; i >= 0; i--)
			{
				if (!this.pawns[i].IsWorldPawn())
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Caravan member ");
					defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawns[i]);
					defaultInterpolatedStringHandler.AppendLiteral(" is not a world pawn. Removing...");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					this.pawns.Remove(this.pawns[i]);
				}
			}
		}

		// Token: 0x0600E1DB RID: 57819 RVA: 0x0053C9AD File Offset: 0x0053ABAD
		private bool ShouldAutoCapture(Pawn p)
		{
			return CaravanUtility.ShouldAutoCapture(p, base.Faction);
		}

		// Token: 0x0600E1DC RID: 57820 RVA: 0x0053C9BC File Offset: 0x0053ABBC
		public void Notify_PawnRemoved(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheInventory();
			this.carryTracker.Notify_PawnRemoved();
			this.beds.Notify_PawnRemoved();
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker != null)
			{
				caravan_BabyTracker.Recache();
			}
			Pawn_MindState mindState = p.mindState;
			if (mindState == null)
			{
				return;
			}
			mindState.ClearBreastfeedCaravan();
		}

		// Token: 0x0600E1DD RID: 57821 RVA: 0x0053CA10 File Offset: 0x0053AC10
		public void Notify_PawnAdded(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheInventory();
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker == null)
			{
				return;
			}
			caravan_BabyTracker.Recache();
		}

		// Token: 0x0600E1DE RID: 57822 RVA: 0x0053CA32 File Offset: 0x0053AC32
		public void Notify_DestinationOrPauseStatusChanged()
		{
			this.cachedDaysWorthOfFoodForTicks = -99999;
		}

		// Token: 0x0600E1DF RID: 57823 RVA: 0x0053CA3F File Offset: 0x0053AC3F
		public void Notify_Teleported()
		{
			this.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
		}

		// Token: 0x0600E1E0 RID: 57824 RVA: 0x0053CA57 File Offset: 0x0053AC57
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		// Token: 0x0600E1E1 RID: 57825 RVA: 0x0053CA5F File Offset: 0x0053AC5F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04009043 RID: 36931
		private int uniqueId = -1;

		// Token: 0x04009044 RID: 36932
		private string nameInt;

		// Token: 0x04009045 RID: 36933
		public ThingOwner<Pawn> pawns;

		// Token: 0x04009046 RID: 36934
		public bool autoJoinable;

		// Token: 0x04009047 RID: 36935
		public Caravan_PathFollower pather;

		// Token: 0x04009048 RID: 36936
		public Caravan_GotoMoteRenderer gotoMote;

		// Token: 0x04009049 RID: 36937
		public Caravan_Tweener tweener;

		// Token: 0x0400904A RID: 36938
		public Caravan_TraderTracker trader;

		// Token: 0x0400904B RID: 36939
		public Caravan_ForageTracker forage;

		// Token: 0x0400904C RID: 36940
		public Caravan_NeedsTracker needs;

		// Token: 0x0400904D RID: 36941
		public Caravan_CarryTracker carryTracker;

		// Token: 0x0400904E RID: 36942
		public Caravan_BedsTracker beds;

		// Token: 0x0400904F RID: 36943
		public Caravan_BabyTracker babies;

		// Token: 0x04009050 RID: 36944
		public StoryState storyState;

		// Token: 0x04009051 RID: 36945
		private Material cachedMat;

		// Token: 0x04009052 RID: 36946
		private bool cachedImmobilized;

		// Token: 0x04009053 RID: 36947
		private int cachedImmobilizedForTicks = -99999;

		// Token: 0x04009054 RID: 36948
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x04009055 RID: 36949
		private int cachedDaysWorthOfFoodForTicks = -99999;

		// Token: 0x04009056 RID: 36950
		public bool notifiedOutOfFood;

		// Token: 0x04009057 RID: 36951
		private const int ImmobilizedCacheDuration = 60;

		// Token: 0x04009058 RID: 36952
		private const int DaysWorthOfFoodCacheDuration = 3000;

		// Token: 0x04009059 RID: 36953
		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		// Token: 0x0400905A RID: 36954
		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);

		// Token: 0x0400905B RID: 36955
		public bool hasShuttleDirty = true;

		// Token: 0x0400905C RID: 36956
		private Building_PassengerShuttle cachedShuttle;
	}
}
