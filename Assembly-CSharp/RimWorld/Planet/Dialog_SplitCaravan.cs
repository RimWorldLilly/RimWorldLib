using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023E7 RID: 9191
	public class Dialog_SplitCaravan : Window
	{
		// Token: 0x17002877 RID: 10359
		// (get) Token: 0x0600E578 RID: 58744 RVA: 0x0055392E File Offset: 0x00551B2E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17002878 RID: 10360
		// (get) Token: 0x0600E579 RID: 58745 RVA: 0x00553940 File Offset: 0x00551B40
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002879 RID: 10361
		// (get) Token: 0x0600E57A RID: 58746 RVA: 0x00553947 File Offset: 0x00551B47
		private BiomeDef Biome
		{
			get
			{
				return this.caravan.Biome;
			}
		}

		// Token: 0x1700287A RID: 10362
		// (get) Token: 0x0600E57B RID: 58747 RVA: 0x00553954 File Offset: 0x00551B54
		private float SourceMassUsage
		{
			get
			{
				if (this.sourceMassUsageDirty)
				{
					this.sourceMassUsageDirty = false;
					this.cachedSourceMassUsage = CollectionsMassCalculator.MassUsageLeftAfterTransfer(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedSourceMassUsage;
			}
		}

		// Token: 0x1700287B RID: 10363
		// (get) Token: 0x0600E57C RID: 58748 RVA: 0x00553980 File Offset: 0x00551B80
		private float SourceMassCapacity
		{
			get
			{
				if (this.sourceMassCapacityDirty)
				{
					this.sourceMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceMassCapacity = CollectionsMassCalculator.CapacityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceMassCapacity;
			}
		}

		// Token: 0x1700287C RID: 10364
		// (get) Token: 0x0600E57D RID: 58749 RVA: 0x005539C8 File Offset: 0x00551BC8
		private float SourceTilesPerDay
		{
			get
			{
				if (this.sourceTilesPerDayDirty)
				{
					this.sourceTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDayLeftAfterTransfer(this.transferables, this.SourceMassUsage, this.SourceMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : PlanetTile.Invalid, this.caravan.Shuttle != null, stringBuilder);
					this.cachedSourceTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceTilesPerDay;
			}
		}

		// Token: 0x1700287D RID: 10365
		// (get) Token: 0x0600E57E RID: 58750 RVA: 0x00553A5C File Offset: 0x00551C5C
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> SourceDaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.sourceDaysWorthOfFoodDirty)
				{
					this.sourceDaysWorthOfFoodDirty = false;
					float item;
					float item2;
					if (this.caravan.pather.Moving)
					{
						using (this.caravan.Tile.Layer.Pather.FindPath(this.caravan.Tile, this.caravan.pather.Destination, null, null))
						{
							item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							item2 = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
							goto IL_150;
						}
					}
					item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFoodLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
					item2 = DaysUntilRotCalculator.ApproxDaysUntilRotLeftAfterTransfer(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					IL_150:
					this.cachedSourceDaysWorthOfFood = new ValueTuple<float, float>(item, item2);
				}
				return this.cachedSourceDaysWorthOfFood;
			}
		}

		// Token: 0x1700287E RID: 10366
		// (get) Token: 0x0600E57F RID: 58751 RVA: 0x00553BDC File Offset: 0x00551DDC
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> SourceForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.sourceForagedFoodPerDayDirty)
				{
					this.sourceForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDayLeftAfterTransfer(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedSourceForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceForagedFoodPerDay;
			}
		}

		// Token: 0x1700287F RID: 10367
		// (get) Token: 0x0600E580 RID: 58752 RVA: 0x00553C30 File Offset: 0x00551E30
		private float SourceVisibility
		{
			get
			{
				if (this.sourceVisibilityDirty)
				{
					this.sourceVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedSourceVisibility = CaravanVisibilityCalculator.VisibilityLeftAfterTransfer(this.transferables, stringBuilder);
					this.cachedSourceVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedSourceVisibility;
			}
		}

		// Token: 0x17002880 RID: 10368
		// (get) Token: 0x0600E581 RID: 58753 RVA: 0x00553C76 File Offset: 0x00551E76
		private float DestMassUsage
		{
			get
			{
				if (this.destMassUsageDirty)
				{
					this.destMassUsageDirty = false;
					this.cachedDestMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.Ignore, false, false);
				}
				return this.cachedDestMassUsage;
			}
		}

		// Token: 0x17002881 RID: 10369
		// (get) Token: 0x0600E582 RID: 58754 RVA: 0x00553CA4 File Offset: 0x00551EA4
		private float DestMassCapacity
		{
			get
			{
				if (this.destMassCapacityDirty)
				{
					this.destMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedDestMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestMassCapacity;
			}
		}

		// Token: 0x17002882 RID: 10370
		// (get) Token: 0x0600E583 RID: 58755 RVA: 0x00553CEC File Offset: 0x00551EEC
		private float DestTilesPerDay
		{
			get
			{
				if (this.destTilesPerDayDirty)
				{
					this.destTilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.DestMassUsage, this.DestMassCapacity, this.caravan.Tile, this.caravan.pather.Moving ? this.caravan.pather.nextTile : PlanetTile.Invalid, this.caravan.Shuttle != null, stringBuilder);
					this.cachedDestTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestTilesPerDay;
			}
		}

		// Token: 0x17002883 RID: 10371
		// (get) Token: 0x0600E584 RID: 58756 RVA: 0x00553D80 File Offset: 0x00551F80
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> DestDaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.destDaysWorthOfFoodDirty)
				{
					this.destDaysWorthOfFoodDirty = false;
					float item;
					float item2;
					if (this.caravan.pather.Moving)
					{
						item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
						item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.pather.curPath, this.caravan.pather.nextTileCostLeft, this.caravan.TicksPerMove);
					}
					else
					{
						item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, this.caravan.Faction, null, 0f, 3300);
						item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.caravan.Tile, IgnorePawnsInventoryMode.Ignore, null, 0f, 3300);
					}
					this.cachedDestDaysWorthOfFood = new ValueTuple<float, float>(item, item2);
				}
				return this.cachedDestDaysWorthOfFood;
			}
		}

		// Token: 0x17002884 RID: 10372
		// (get) Token: 0x0600E585 RID: 58757 RVA: 0x00553EAC File Offset: 0x005520AC
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> DestForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.destForagedFoodPerDayDirty)
				{
					this.destForagedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedDestForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedDestForagedFoodPerDay;
			}
		}

		// Token: 0x17002885 RID: 10373
		// (get) Token: 0x0600E586 RID: 58758 RVA: 0x00553F00 File Offset: 0x00552100
		private float DestVisibility
		{
			get
			{
				if (this.destVisibilityDirty)
				{
					this.destVisibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedDestVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedDestVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedDestVisibility;
			}
		}

		// Token: 0x17002886 RID: 10374
		// (get) Token: 0x0600E587 RID: 58759 RVA: 0x00553F46 File Offset: 0x00552146
		private int TicksToArrive
		{
			get
			{
				if (!this.caravan.pather.Moving)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.caravan, false);
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x0600E588 RID: 58760 RVA: 0x00553F84 File Offset: 0x00552184
		public Dialog_SplitCaravan(Caravan caravan) : base(null)
		{
			this.caravan = caravan;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600E589 RID: 58761 RVA: 0x0055401D File Offset: 0x0055221D
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
		}

		// Token: 0x0600E58A RID: 58762 RVA: 0x0055402C File Offset: 0x0055222C
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "SplitCaravan".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.SourceMassUsage, this.SourceMassCapacity, this.cachedSourceMassCapacityExplanation, this.SourceTilesPerDay, this.cachedSourceTilesPerDayExplanation, this.SourceDaysWorthOfFood, this.SourceForagedFoodPerDay, this.cachedSourceForagedFoodPerDayExplanation, this.SourceVisibility, this.cachedSourceVisibilityExplanation, -1f, -1f, null), new CaravanUIUtility.CaravanInfo?(new CaravanUIUtility.CaravanInfo(this.DestMassUsage, this.DestMassCapacity, this.cachedDestMassCapacityExplanation, this.DestTilesPerDay, this.cachedDestTilesPerDayExplanation, this.DestDaysWorthOfFood, this.DestForagedFoodPerDay, this.cachedDestForagedFoodPerDayExplanation, this.DestVisibility, this.cachedDestVisibilityExplanation, -1f, -1f, null)), this.caravan.Tile, this.caravan.pather.Moving ? new int?(this.TicksToArrive) : null, -9999f, new Rect(12f, 35f, inRect.width - 24f, 40f), true, null, false, true);
			Dialog_SplitCaravan.tabsList.Clear();
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Pawns;
			}, this.tab == Dialog_SplitCaravan.Tab.Pawns));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.Items;
			}, this.tab == Dialog_SplitCaravan.Tab.Items));
			Dialog_SplitCaravan.tabsList.Add(new TabRecord("TravelSupplies".Translate(), delegate()
			{
				this.tab = Dialog_SplitCaravan.Tab.FoodAndMedicine;
			}, this.tab == Dialog_SplitCaravan.Tab.FoodAndMedicine));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_SplitCaravan.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 59f;
			bool flag = false;
			switch (this.tab)
			{
			case Dialog_SplitCaravan.Tab.Pawns:
				this.pawnsTransfer.OnGUI(inRect2, out flag);
				break;
			case Dialog_SplitCaravan.Tab.Items:
				this.itemsTransfer.OnGUI(inRect2, out flag);
				break;
			case Dialog_SplitCaravan.Tab.FoodAndMedicine:
				this.foodAndMedicineTransfer.OnGUI(inRect2, out flag);
				break;
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600E58B RID: 58763 RVA: 0x005542D8 File Offset: 0x005524D8
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x0600E58C RID: 58764 RVA: 0x005542DC File Offset: 0x005524DC
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.Normal);
			if (transferableOneWay == null)
			{
				transferableOneWay = new TransferableOneWay();
				this.transferables.Add(transferableOneWay);
			}
			if (transferableOneWay.things.Contains(t))
			{
				Log.Error("Tried to add the same thing twice to TransferableOneWay: " + ((t != null) ? t.ToString() : null));
				return;
			}
			transferableOneWay.things.Add(t);
		}

		// Token: 0x0600E58D RID: 58765 RVA: 0x00554344 File Offset: 0x00552544
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true, null) && this.TrySplitCaravan())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
			if (Widgets.ButtonText(new Rect(rect2.x - 10f - this.BottomButtonSize.x, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 10f, rect2.y, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600E58E RID: 58766 RVA: 0x005544A0 File Offset: 0x005526A0
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.foodAndMedicineTransfer, "SplitCaravanThingCountTip".Translate(), IgnorePawnsInventoryMode.Ignore, () => this.DestMassCapacity - this.DestMassUsage, false, this.caravan.Tile, false);
			this.CountToTransferChanged();
		}

		// Token: 0x0600E58F RID: 58767 RVA: 0x00554510 File Offset: 0x00552710
		private bool TrySplitCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			for (int i = 0; i < pawnsFromTransferables.Count; i++)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawnsFromTransferables[i], this.caravan.PawnsListForReading, pawnsFromTransferables);
			}
			for (int j = 0; j < pawnsFromTransferables.Count; j++)
			{
				this.caravan.RemovePawn(pawnsFromTransferables[j]);
			}
			Caravan newCaravan = CaravanMaker.MakeCaravan(pawnsFromTransferables, this.caravan.Faction, this.caravan.Tile, true);
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			Action<Thing, int> <>9__1;
			for (int k = 0; k < this.transferables.Count; k++)
			{
				List<Thing> things = this.transferables[k].things;
				int countToTransfer = this.transferables[k].CountToTransfer;
				Action<Thing, int> transfer;
				if ((transfer = <>9__1) == null)
				{
					transfer = (<>9__1 = delegate(Thing thing, int numToTake)
					{
						Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
						if (ownerOf == null)
						{
							Log.Error("Error while splitting a caravan: Thing " + ((thing != null) ? thing.ToString() : null) + " has no owner. Where did it come from then?");
							return;
						}
						CaravanInventoryUtility.MoveInventoryToSomeoneElse(ownerOf, thing, newCaravan.PawnsListForReading, null, numToTake);
					});
				}
				TransferableUtility.TransferNoSplit(things, countToTransfer, transfer, true, true);
			}
			return true;
		}

		// Token: 0x0600E590 RID: 58768 RVA: 0x00554644 File Offset: 0x00552844
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.AnyNonDownedColonistLeftInSourceCaravan(pawns))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("SourceCaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("SourceCaravanMustHaveAtLeastOneColonist".Translate(), this.caravan, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600E591 RID: 58769 RVA: 0x00554724 File Offset: 0x00552924
		private void AddPawnsToTransferables()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				this.AddToTransferables(pawnsListForReading[i]);
			}
		}

		// Token: 0x0600E592 RID: 58770 RVA: 0x0055475C File Offset: 0x0055295C
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i]);
			}
		}

		// Token: 0x0600E593 RID: 58771 RVA: 0x00554794 File Offset: 0x00552994
		private bool AnyNonDownedColonistLeftInSourceCaravan(List<Pawn> pawnsToTransfer)
		{
			Predicate<Thing> <>9__1;
			return this.transferables.Any(delegate(TransferableOneWay x)
			{
				List<Thing> things = x.things;
				Predicate<Thing> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(Thing y)
					{
						Pawn pawn = y as Pawn;
						return pawn != null && CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.Downed && !pawnsToTransfer.Contains(pawn);
					});
				}
				return things.Any(predicate);
			});
		}

		// Token: 0x0600E594 RID: 58772 RVA: 0x005547C8 File Offset: 0x005529C8
		private void CountToTransferChanged()
		{
			this.sourceMassUsageDirty = true;
			this.sourceMassCapacityDirty = true;
			this.sourceTilesPerDayDirty = true;
			this.sourceDaysWorthOfFoodDirty = true;
			this.sourceForagedFoodPerDayDirty = true;
			this.sourceVisibilityDirty = true;
			this.destMassUsageDirty = true;
			this.destMassCapacityDirty = true;
			this.destTilesPerDayDirty = true;
			this.destDaysWorthOfFoodDirty = true;
			this.destForagedFoodPerDayDirty = true;
			this.destVisibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x040091BE RID: 37310
		private Caravan caravan;

		// Token: 0x040091BF RID: 37311
		private List<TransferableOneWay> transferables;

		// Token: 0x040091C0 RID: 37312
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x040091C1 RID: 37313
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x040091C2 RID: 37314
		private TransferableOneWayWidget foodAndMedicineTransfer;

		// Token: 0x040091C3 RID: 37315
		private Dialog_SplitCaravan.Tab tab;

		// Token: 0x040091C4 RID: 37316
		private bool sourceMassUsageDirty = true;

		// Token: 0x040091C5 RID: 37317
		private float cachedSourceMassUsage;

		// Token: 0x040091C6 RID: 37318
		private bool sourceMassCapacityDirty = true;

		// Token: 0x040091C7 RID: 37319
		private float cachedSourceMassCapacity;

		// Token: 0x040091C8 RID: 37320
		private string cachedSourceMassCapacityExplanation;

		// Token: 0x040091C9 RID: 37321
		private bool sourceTilesPerDayDirty = true;

		// Token: 0x040091CA RID: 37322
		private float cachedSourceTilesPerDay;

		// Token: 0x040091CB RID: 37323
		private string cachedSourceTilesPerDayExplanation;

		// Token: 0x040091CC RID: 37324
		private bool sourceDaysWorthOfFoodDirty = true;

		// Token: 0x040091CD RID: 37325
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedSourceDaysWorthOfFood;

		// Token: 0x040091CE RID: 37326
		private bool sourceForagedFoodPerDayDirty = true;

		// Token: 0x040091CF RID: 37327
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedSourceForagedFoodPerDay;

		// Token: 0x040091D0 RID: 37328
		private string cachedSourceForagedFoodPerDayExplanation;

		// Token: 0x040091D1 RID: 37329
		private bool sourceVisibilityDirty = true;

		// Token: 0x040091D2 RID: 37330
		private float cachedSourceVisibility;

		// Token: 0x040091D3 RID: 37331
		private string cachedSourceVisibilityExplanation;

		// Token: 0x040091D4 RID: 37332
		private bool destMassUsageDirty = true;

		// Token: 0x040091D5 RID: 37333
		private float cachedDestMassUsage;

		// Token: 0x040091D6 RID: 37334
		private bool destMassCapacityDirty = true;

		// Token: 0x040091D7 RID: 37335
		private float cachedDestMassCapacity;

		// Token: 0x040091D8 RID: 37336
		private string cachedDestMassCapacityExplanation;

		// Token: 0x040091D9 RID: 37337
		private bool destTilesPerDayDirty = true;

		// Token: 0x040091DA RID: 37338
		private float cachedDestTilesPerDay;

		// Token: 0x040091DB RID: 37339
		private string cachedDestTilesPerDayExplanation;

		// Token: 0x040091DC RID: 37340
		private bool destDaysWorthOfFoodDirty = true;

		// Token: 0x040091DD RID: 37341
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDestDaysWorthOfFood;

		// Token: 0x040091DE RID: 37342
		private bool destForagedFoodPerDayDirty = true;

		// Token: 0x040091DF RID: 37343
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedDestForagedFoodPerDay;

		// Token: 0x040091E0 RID: 37344
		private string cachedDestForagedFoodPerDayExplanation;

		// Token: 0x040091E1 RID: 37345
		private bool destVisibilityDirty = true;

		// Token: 0x040091E2 RID: 37346
		private float cachedDestVisibility;

		// Token: 0x040091E3 RID: 37347
		private string cachedDestVisibilityExplanation;

		// Token: 0x040091E4 RID: 37348
		private bool ticksToArriveDirty = true;

		// Token: 0x040091E5 RID: 37349
		private int cachedTicksToArrive;

		// Token: 0x040091E6 RID: 37350
		private const float TitleRectHeight = 35f;

		// Token: 0x040091E7 RID: 37351
		private const float BottomAreaHeight = 55f;

		// Token: 0x040091E8 RID: 37352
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x040091E9 RID: 37353
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x02003EF0 RID: 16112
		private enum Tab
		{
			// Token: 0x0400F854 RID: 63572
			Pawns,
			// Token: 0x0400F855 RID: 63573
			Items,
			// Token: 0x0400F856 RID: 63574
			FoodAndMedicine
		}
	}
}
