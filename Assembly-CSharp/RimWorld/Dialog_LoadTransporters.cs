using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CB5 RID: 7349
	public class Dialog_LoadTransporters : Window
	{
		// Token: 0x1700224D RID: 8781
		// (get) Token: 0x0600BC85 RID: 48261 RVA: 0x0043688F File Offset: 0x00434A8F
		public bool CanChangeAssignedThingsAfterStarting
		{
			get
			{
				return this.transporters[0].Props.canChangeAssignedThingsAfterStarting;
			}
		}

		// Token: 0x1700224E RID: 8782
		// (get) Token: 0x0600BC86 RID: 48262 RVA: 0x004368A7 File Offset: 0x00434AA7
		public bool LoadingInProgressOrReadyToLaunch
		{
			get
			{
				return this.transporters[0].LoadingInProgressOrReadyToLaunch;
			}
		}

		// Token: 0x1700224F RID: 8783
		// (get) Token: 0x0600BC87 RID: 48263 RVA: 0x004368BA File Offset: 0x00434ABA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17002250 RID: 8784
		// (get) Token: 0x0600BC88 RID: 48264 RVA: 0x004368CC File Offset: 0x00434ACC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002251 RID: 8785
		// (get) Token: 0x0600BC89 RID: 48265 RVA: 0x004368D4 File Offset: 0x00434AD4
		private float MassCapacity
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.transporters.Count; i++)
				{
					num += this.transporters[i].MassCapacity;
				}
				return num;
			}
		}

		// Token: 0x17002252 RID: 8786
		// (get) Token: 0x0600BC8A RID: 48266 RVA: 0x00436914 File Offset: 0x00434B14
		private float CaravanMassCapacity
		{
			get
			{
				if (this.caravanMassCapacityDirty)
				{
					this.caravanMassCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedCaravanMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedCaravanMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedCaravanMassCapacity;
			}
		}

		// Token: 0x17002253 RID: 8787
		// (get) Token: 0x0600BC8B RID: 48267 RVA: 0x0043695C File Offset: 0x00434B5C
		private string TransportersLabel
		{
			get
			{
				if (this.transporters[0].Props.max1PerGroup)
				{
					return this.transporters[0].parent.Label;
				}
				return Find.ActiveLanguageWorker.Pluralize(this.transporters[0].parent.Label, -1);
			}
		}

		// Token: 0x17002254 RID: 8788
		// (get) Token: 0x0600BC8C RID: 48268 RVA: 0x004369B9 File Offset: 0x00434BB9
		private string TransportersLabelCap
		{
			get
			{
				return this.TransportersLabel.CapitalizeFirst();
			}
		}

		// Token: 0x17002255 RID: 8789
		// (get) Token: 0x0600BC8D RID: 48269 RVA: 0x004369C6 File Offset: 0x00434BC6
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17002256 RID: 8790
		// (get) Token: 0x0600BC8E RID: 48270 RVA: 0x004369D4 File Offset: 0x00434BD4
		private float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					CompShuttle compShuttle = this.transporters.AsShuttle();
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, compShuttle == null || compShuttle.requiredColonistCount == 0, false);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17002257 RID: 8791
		// (get) Token: 0x0600BC8F RID: 48271 RVA: 0x00436A24 File Offset: 0x00434C24
		public float CaravanMassUsage
		{
			get
			{
				if (this.caravanMassUsageDirty)
				{
					this.caravanMassUsageDirty = false;
					this.cachedCaravanMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
				}
				return this.cachedCaravanMassUsage;
			}
		}

		// Token: 0x17002258 RID: 8792
		// (get) Token: 0x0600BC90 RID: 48272 RVA: 0x00436A50 File Offset: 0x00434C50
		private float TilesPerDay
		{
			get
			{
				if (!this.map.Tile.Valid || !this.map.Tile.LayerDef.SurfaceTiles)
				{
					return 0f;
				}
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.map.Tile, PlanetTile.Invalid, this.transporters.IsShuttle(), stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x17002259 RID: 8793
		// (get) Token: 0x0600BC91 RID: 48273 RVA: 0x00436AF4 File Offset: 0x00434CF4
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> DaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (this.daysWorthOfFoodDirty)
				{
					this.daysWorthOfFoodDirty = false;
					float item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, Faction.OfPlayer, null, 0f, 3300);
					this.cachedDaysWorthOfFood = new ValueTuple<float, float>(item, DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.map.Tile, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, null, 0f, 3300));
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700225A RID: 8794
		// (get) Token: 0x0600BC92 RID: 48274 RVA: 0x00436B6C File Offset: 0x00434D6C
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> ForagedFoodPerDay
		{
			[return: TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			get
			{
				if (this.foragedFoodPerDayDirty)
				{
					this.foragedFoodPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedForagedFoodPerDay = ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.transferables, this.Biome, Faction.OfPlayer, stringBuilder);
					this.cachedForagedFoodPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedForagedFoodPerDay;
			}
		}

		// Token: 0x1700225B RID: 8795
		// (get) Token: 0x0600BC93 RID: 48275 RVA: 0x00436BC0 File Offset: 0x00434DC0
		private float Visibility
		{
			get
			{
				if (this.visibilityDirty)
				{
					this.visibilityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedVisibility = CaravanVisibilityCalculator.Visibility(this.transferables, stringBuilder);
					this.cachedVisibilityExplanation = stringBuilder.ToString();
				}
				return this.cachedVisibility;
			}
		}

		// Token: 0x0600BC94 RID: 48276 RVA: 0x00436C08 File Offset: 0x00434E08
		public Dialog_LoadTransporters(Map map, List<CompTransporter> transporters) : base(null)
		{
			this.map = map;
			this.transporters = new List<CompTransporter>();
			this.transporters.AddRange(transporters);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC95 RID: 48277 RVA: 0x00436C99 File Offset: 0x00434E99
		public override void PostOpen()
		{
			base.PostOpen();
			this.CalculateAndRecacheTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				this.SetLoadedItemsToLoad();
			}
		}

		// Token: 0x0600BC96 RID: 48278 RVA: 0x00436CC0 File Offset: 0x00434EC0
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "LoadTransporters".Translate(this.TransportersLabel).CapitalizeFirst());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.transporters[0].Props.showOverallStats && this.transporters[0].Map.Tile.Valid)
			{
				CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, "", this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, this.CaravanMassUsage, this.CaravanMassCapacity, this.cachedCaravanMassCapacityExplanation), null, this.map.Tile, null, this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), false, null, false, !this.transporters.IsShuttle());
				inRect.yMin += 52f;
			}
			Dialog_LoadTransporters.tabsList.Clear();
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_LoadTransporters.Tab.Pawns;
			}, this.tab == Dialog_LoadTransporters.Tab.Pawns));
			Dialog_LoadTransporters.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_LoadTransporters.Tab.Items;
			}, this.tab == Dialog_LoadTransporters.Tab.Items));
			inRect.yMin += 67f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_LoadTransporters.tabsList, 200f);
			inRect = inRect.ContractedBy(17f);
			inRect.height += 17f;
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect inRect2 = rect2;
			inRect2.yMax -= 76f;
			bool flag = false;
			Dialog_LoadTransporters.Tab tab = this.tab;
			if (tab != Dialog_LoadTransporters.Tab.Pawns)
			{
				if (tab == Dialog_LoadTransporters.Tab.Items)
				{
					this.itemsTransfer.OnGUI(inRect2, out flag);
				}
			}
			else
			{
				this.pawnsTransfer.OnGUI(inRect2, out flag);
			}
			if (flag)
			{
				this.CountToTransferChanged();
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BC97 RID: 48279 RVA: 0x00436F4B File Offset: 0x0043514B
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x0600BC98 RID: 48280 RVA: 0x00436F50 File Offset: 0x00435150
		private void AddToTransferables(Thing t)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
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

		// Token: 0x0600BC99 RID: 48281 RVA: 0x00436FB8 File Offset: 0x004351B8
		private bool ErrorsRequireConfirmation(List<Pawn> pawns)
		{
			if (this.transporters.IsShuttle())
			{
				return false;
			}
			if (this.CaravanMassUsage <= this.CaravanMassCapacity || this.CaravanMassCapacity == 0f)
			{
				return false;
			}
			if (this.transporters[0].parent != null && this.transporters[0].parent.def == ThingDefOf.TransportPod)
			{
				return pawns.Any((Pawn x) => x.IsFreeColonist);
			}
			return true;
		}

		// Token: 0x0600BC9A RID: 48282 RVA: 0x00437048 File Offset: 0x00435248
		private void DoBottomButtons(Rect rect)
		{
			if (Widgets.ButtonText(new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (Widgets.ButtonText(new Rect(0f, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			Rect rect2 = new Rect(rect.width - this.BottomButtonSize.x, rect.height - 55f - 17f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "AcceptButton".Translate(), true, true, true, null))
			{
				this.onAcceptButton();
			}
			if (Prefs.DevMode)
			{
				float width = 200f;
				float height = this.BottomButtonSize.y / 2f;
				if (!this.LoadingInProgressOrReadyToLaunch && Widgets.ButtonText(new Rect(0f, rect2.yMax + 4f, width, height), "DEV: Load instantly", true, true, true, null) && this.DebugTryLoadInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(0f, rect2.yMax + 4f, width, height), "DEV: Select everything", true, true, true, null))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToLoadEverything();
				}
			}
		}

		// Token: 0x0600BC9B RID: 48283 RVA: 0x00437254 File Offset: 0x00435454
		private void onAcceptButton()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (this.ErrorsRequireConfirmation(pawnsFromTransferables))
			{
				if (this.CheckForErrors(pawnsFromTransferables))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("TransportersCaravanWillBeImmobile".Translate(), delegate()
					{
						if (this.TryAccept())
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							this.Close(false);
						}
					}, false, null, WindowLayer.Dialog));
					return;
				}
			}
			else if (this.TryAccept())
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Close(false);
			}
		}

		// Token: 0x0600BC9C RID: 48284 RVA: 0x004372C4 File Offset: 0x004354C4
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			if (this.CanChangeAssignedThingsAfterStarting && this.LoadingInProgressOrReadyToLaunch)
			{
				for (int i = 0; i < this.transporters.Count; i++)
				{
					for (int j = 0; j < this.transporters[i].innerContainer.Count; j++)
					{
						this.AddToTransferables(this.transporters[i].innerContainer[j]);
					}
				}
				foreach (Thing t in TransporterUtility.ThingsBeingHauledTo(this.transporters, this.map))
				{
					this.AddToTransferables(t);
				}
			}
			this.pawnsTransfer = new TransferableOneWayWidget(null, null, null, "TransporterColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, new PlanetTile?(this.map.Tile), true, true, true, false, false, true, false, false, false, false);
			CaravanUIUtility.AddPawnsSections(this.pawnsTransfer, this.transferables);
			this.itemsTransfer = new TransferableOneWayWidget(from x in this.transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, "TransporterColonyThingCountTip".Translate(), true, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, () => this.MassCapacity - this.MassUsage, 0f, false, new PlanetTile?(this.map.Tile), true, false, false, false, true, false, true, false, false, false);
			this.CountToTransferChanged();
		}

		// Token: 0x0600BC9D RID: 48285 RVA: 0x00437478 File Offset: 0x00435678
		private bool DebugTryLoadInstantly()
		{
			TransporterUtility.InitiateLoading(this.transporters);
			int i;
			Action<Thing, IThingHolder> <>9__0;
			int j;
			for (i = 0; i < this.transferables.Count; i = j + 1)
			{
				List<Thing> things = this.transferables[i].things;
				int countToTransfer = this.transferables[i].CountToTransfer;
				Action<Thing, IThingHolder> transferred;
				if ((transferred = <>9__0) == null)
				{
					transferred = (<>9__0 = delegate(Thing splitPiece, IThingHolder originalThing)
					{
						this.transporters[i % this.transporters.Count].GetDirectlyHeldThings().TryAdd(splitPiece, true);
					});
				}
				TransferableUtility.Transfer(things, countToTransfer, transferred);
				j = i;
			}
			return true;
		}

		// Token: 0x0600BC9E RID: 48286 RVA: 0x0043751C File Offset: 0x0043571C
		private bool TryAccept()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				this.AssignTransferablesToRandomTransporters();
				TransporterUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.transporters, this.map);
				IReadOnlyList<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter && this.transporters.Contains(((JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver).Transporter))
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
			else
			{
				TransporterUtility.InitiateLoading(this.transporters);
				this.AssignTransferablesToRandomTransporters();
				TransporterUtility.MakeLordsAsAppropriate(pawnsFromTransferables, this.transporters, this.map);
				if (this.transporters.IsShuttle())
				{
					Messages.Message("MessageShuttleLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
				else if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("MessageTransporterSingleLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
				else
				{
					Messages.Message("MessageTransportersLoadingProcessStarted".Translate(), this.transporters[0].parent, MessageTypeDefOf.TaskCompletion, false);
				}
			}
			return true;
		}

		// Token: 0x0600BC9F RID: 48287 RVA: 0x004376B8 File Offset: 0x004358B8
		private void SetLoadedItemsToLoad()
		{
			int i;
			int num;
			for (i = 0; i < this.transporters.Count; i = num + 1)
			{
				int j;
				for (j = 0; j < this.transporters[i].innerContainer.Count; j = num + 1)
				{
					TransferableOneWay transferableOneWay = this.transferables.Find((TransferableOneWay x) => x.things.Contains(this.transporters[i].innerContainer[j]));
					if (transferableOneWay != null && transferableOneWay.CanAdjustBy(this.transporters[i].innerContainer[j].stackCount).Accepted)
					{
						transferableOneWay.AdjustBy(this.transporters[i].innerContainer[j].stackCount);
					}
					num = j;
				}
				if (this.transporters[i].leftToLoad != null)
				{
					for (int k = 0; k < this.transporters[i].leftToLoad.Count; k++)
					{
						TransferableOneWay transferableOneWay2 = this.transporters[i].leftToLoad[k];
						if (transferableOneWay2.CountToTransfer != 0 && transferableOneWay2.HasAnyThing)
						{
							TransferableOneWay transferableOneWay3 = TransferableUtility.TransferableMatchingDesperate(transferableOneWay2.AnyThing, this.transferables, TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay3 != null && transferableOneWay3.CanAdjustBy(transferableOneWay2.CountToTransferToDestination).Accepted)
							{
								transferableOneWay3.AdjustBy(transferableOneWay2.CountToTransferToDestination);
							}
						}
					}
				}
				num = i;
			}
		}

		// Token: 0x0600BCA0 RID: 48288 RVA: 0x0043789C File Offset: 0x00435A9C
		private void AssignTransferablesToRandomTransporters()
		{
			Dialog_LoadTransporters.tmpLeftToLoadCopy.Clear();
			for (int i = 0; i < this.transporters.Count; i++)
			{
				Dialog_LoadTransporters.tmpLeftToLoadCopy.Add((this.transporters[i].leftToLoad != null) ? this.transporters[i].leftToLoad.ToList<TransferableOneWay>() : new List<TransferableOneWay>());
				if (this.transporters[i].leftToLoad != null)
				{
					this.transporters[i].leftToLoad.Clear();
				}
			}
			Dialog_LoadTransporters.tmpLeftCountToTransfer.Clear();
			for (int j = 0; j < this.transferables.Count; j++)
			{
				Dialog_LoadTransporters.tmpLeftCountToTransfer.Add(this.transferables[j], this.transferables[j].CountToTransfer);
			}
			if (this.LoadingInProgressOrReadyToLaunch)
			{
				for (int k = 0; k < this.transferables.Count; k++)
				{
					if (this.transferables[k].HasAnyThing && Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[k]] > 0)
					{
						for (int l = 0; l < Dialog_LoadTransporters.tmpLeftToLoadCopy.Count; l++)
						{
							TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(this.transferables[k].AnyThing, Dialog_LoadTransporters.tmpLeftToLoadCopy[l], TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay != null)
							{
								int num = Mathf.Min(Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[k]], transferableOneWay.CountToTransfer);
								if (num > 0)
								{
									this.transporters[l].AddToTheToLoadList(this.transferables[k], num);
									Dictionary<TransferableOneWay, int> dictionary = Dialog_LoadTransporters.tmpLeftCountToTransfer;
									TransferableOneWay key = this.transferables[k];
									dictionary[key] -= num;
								}
							}
							for (int m = 0; m < this.transporters[l].innerContainer.Count; m++)
							{
								Thing thing = this.transporters[l].innerContainer[m];
								if (TransferableUtility.TransferAsOne(this.transferables[k].AnyThing, thing, TransferAsOneMode.PodsOrCaravanPacking))
								{
									int num2 = Mathf.Min(Dialog_LoadTransporters.tmpLeftCountToTransfer[this.transferables[k]], thing.stackCount);
									if (num2 > 0)
									{
										this.transporters[l].AddToTheToLoadList(this.transferables[k], num2);
										Dictionary<TransferableOneWay, int> dictionary = Dialog_LoadTransporters.tmpLeftCountToTransfer;
										TransferableOneWay key = this.transferables[k];
										dictionary[key] -= num2;
									}
								}
							}
						}
					}
				}
			}
			Dialog_LoadTransporters.tmpLeftToLoadCopy.Clear();
			if (this.transferables.Any<TransferableOneWay>())
			{
				this.transferables.SortByDescending((TransferableOneWay x) => x.AnyThing.GetStatValue(StatDefOf.Mass, true, -1) * (float)Dialog_LoadTransporters.tmpLeftCountToTransfer[x]);
				foreach (TransferableOneWay transferableOneWay2 in this.transferables)
				{
					if (Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay2] != 0)
					{
						TransferableOneWay transferableOneWay3 = transferableOneWay2;
						int num3 = Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay3];
						int num4 = Mathf.CeilToInt((float)num3 / (float)this.transporters.Count);
						int num5 = int.MaxValue;
						int num6 = 0;
						for (int n = 0; n < this.transporters.Count; n++)
						{
							if (this.transporters[n].leftToLoad == null)
							{
								num6 = n;
								break;
							}
							if (this.transporters[n].leftToLoad.Count < num5)
							{
								num5 = this.transporters[n].leftToLoad.Count;
								num6 = n;
							}
						}
						int num7 = 0;
						while (num7 < this.transporters.Count && num3 != 0)
						{
							int num8 = (num7 == this.transporters.Count - 1) ? num3 : num4;
							if (num8 > 0)
							{
								this.transporters[num6].AddToTheToLoadList(transferableOneWay3, num8);
							}
							num3 -= num8;
							num6 = (num6 + 1) % this.transporters.Count;
							num7++;
						}
						Dialog_LoadTransporters.tmpLeftCountToTransfer[transferableOneWay3] = 0;
					}
				}
			}
			Dialog_LoadTransporters.tmpLeftCountToTransfer.Clear();
			for (int num9 = 0; num9 < this.transporters.Count; num9++)
			{
				for (int num10 = this.transporters[num9].innerContainer.Count - 1; num10 >= 0; num10--)
				{
					Thing thing2 = this.transporters[num9].innerContainer[num10];
					int num11 = this.transporters[num9].SubtractFromToLoadList(thing2, thing2.stackCount, false);
					if (num11 < thing2.stackCount)
					{
						this.transporters[num9].Notify_ThingRemoved(thing2);
						Thing thing3;
						if (!this.transporters[num9].innerContainer.TryDrop(thing2, ThingPlaceMode.Near, thing2.stackCount - num11, out thing3, null, null))
						{
							string str = "Transporter failed to drop ";
							string str2 = (thing2.stackCount - num11).ToString();
							string str3 = " of ";
							Thing thing4 = thing2;
							Log.Error(str + str2 + str3 + ((thing4 != null) ? thing4.ToString() : null));
						}
					}
				}
			}
		}

		// Token: 0x0600BCA1 RID: 48289 RVA: 0x00437E24 File Offset: 0x00436024
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (!this.CanChangeAssignedThingsAfterStarting)
			{
				if (!this.transferables.Any((TransferableOneWay x) => x.CountToTransfer != 0))
				{
					if (this.transporters[0].Props.max1PerGroup)
					{
						Messages.Message("CantSendEmptyTransporterSingle".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message("CantSendEmptyTransportPods".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			if (this.transporters[0].Props.max1PerGroup)
			{
				CompShuttle compShuttle = this.transporters.AsShuttle();
				if (compShuttle != null && compShuttle.requiredColonistCount > 0 && pawns.Count > compShuttle.requiredColonistCount)
				{
					Messages.Message("TransporterSingleTooManyColonists".Translate(compShuttle.requiredColonistCount), MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			if (this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("TooBigTransporterSingleMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("TooBigTransportersMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			Pawn pawn = pawns.Find((Pawn x) => !x.MapHeld.reachability.CanReach(x.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)) && !this.transporters.Any((CompTransporter y) => y.innerContainer.Contains(x)));
			if (pawn != null)
			{
				if (this.transporters[0].Props.max1PerGroup)
				{
					Messages.Message("PawnCantReachTransporterSingle".Translate(pawn.LabelShort, pawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("PawnCantReachTransporters".Translate(pawn.LabelShort, pawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			Map map = this.transporters[0].parent.Map;
			for (int i = 0; i < this.transferables.Count; i++)
			{
				if (this.transferables[i].ThingDef.category == ThingCategory.Item)
				{
					int countToTransfer = this.transferables[i].CountToTransfer;
					int num = 0;
					if (countToTransfer > 0)
					{
						for (int j = 0; j < this.transferables[i].things.Count; j++)
						{
							Thing t = this.transferables[i].things[j];
							Pawn_CarryTracker pawn_CarryTracker = t.ParentHolder as Pawn_CarryTracker;
							if (map.reachability.CanReach(t.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)) || this.transporters.Any((CompTransporter x) => x.innerContainer.Contains(t)) || (pawn_CarryTracker != null && pawn_CarryTracker.pawn.MapHeld.reachability.CanReach(pawn_CarryTracker.pawn.PositionHeld, this.transporters[0].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false))))
							{
								num += t.stackCount;
								if (num >= countToTransfer)
								{
									break;
								}
							}
						}
						if (num < countToTransfer)
						{
							if (countToTransfer == 1)
							{
								if (this.transporters[0].Props.max1PerGroup)
								{
									Messages.Message("TransporterSingleItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
								}
								else
								{
									Messages.Message("TransporterItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
								}
							}
							else if (this.transporters[0].Props.max1PerGroup)
							{
								Messages.Message("TransporterSingleItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("TransporterItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600BCA2 RID: 48290 RVA: 0x004382E0 File Offset: 0x004364E0
		private void AddPawnsToTransferables()
		{
			foreach (Pawn t in TransporterUtility.AllSendablePawns(this.transporters, this.map))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x0600BCA3 RID: 48291 RVA: 0x00438338 File Offset: 0x00436538
		private void AddItemsToTransferables()
		{
			foreach (Thing t in TransporterUtility.AllSendableItems(this.transporters, this.map))
			{
				this.AddToTransferables(t);
			}
		}

		// Token: 0x0600BCA4 RID: 48292 RVA: 0x00438390 File Offset: 0x00436590
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x0600BCA5 RID: 48293 RVA: 0x004383A0 File Offset: 0x004365A0
		private void SetToLoadEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.CountToTransferChanged();
		}

		// Token: 0x0600BCA6 RID: 48294 RVA: 0x004383EB File Offset: 0x004365EB
		private void CountToTransferChanged()
		{
			this.massUsageDirty = true;
			this.caravanMassUsageDirty = true;
			this.caravanMassCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
		}

		// Token: 0x0600BCA7 RID: 48295 RVA: 0x0043841E File Offset: 0x0043661E
		public override void OnAcceptKeyPressed()
		{
			this.onAcceptButton();
		}

		// Token: 0x04006BC8 RID: 27592
		private Map map;

		// Token: 0x04006BC9 RID: 27593
		private List<CompTransporter> transporters;

		// Token: 0x04006BCA RID: 27594
		private List<TransferableOneWay> transferables;

		// Token: 0x04006BCB RID: 27595
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04006BCC RID: 27596
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04006BCD RID: 27597
		private Dialog_LoadTransporters.Tab tab;

		// Token: 0x04006BCE RID: 27598
		private float lastMassFlashTime = -9999f;

		// Token: 0x04006BCF RID: 27599
		private bool massUsageDirty = true;

		// Token: 0x04006BD0 RID: 27600
		private float cachedMassUsage;

		// Token: 0x04006BD1 RID: 27601
		private bool caravanMassUsageDirty = true;

		// Token: 0x04006BD2 RID: 27602
		private float cachedCaravanMassUsage;

		// Token: 0x04006BD3 RID: 27603
		private bool caravanMassCapacityDirty = true;

		// Token: 0x04006BD4 RID: 27604
		private float cachedCaravanMassCapacity;

		// Token: 0x04006BD5 RID: 27605
		private string cachedCaravanMassCapacityExplanation;

		// Token: 0x04006BD6 RID: 27606
		private bool tilesPerDayDirty = true;

		// Token: 0x04006BD7 RID: 27607
		private float cachedTilesPerDay;

		// Token: 0x04006BD8 RID: 27608
		private string cachedTilesPerDayExplanation;

		// Token: 0x04006BD9 RID: 27609
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04006BDA RID: 27610
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x04006BDB RID: 27611
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04006BDC RID: 27612
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04006BDD RID: 27613
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04006BDE RID: 27614
		private bool visibilityDirty = true;

		// Token: 0x04006BDF RID: 27615
		private float cachedVisibility;

		// Token: 0x04006BE0 RID: 27616
		private string cachedVisibilityExplanation;

		// Token: 0x04006BE1 RID: 27617
		private const float TitleRectHeight = 35f;

		// Token: 0x04006BE2 RID: 27618
		private const float BottomAreaHeight = 55f;

		// Token: 0x04006BE3 RID: 27619
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04006BE4 RID: 27620
		private static List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04006BE5 RID: 27621
		private static List<List<TransferableOneWay>> tmpLeftToLoadCopy = new List<List<TransferableOneWay>>();

		// Token: 0x04006BE6 RID: 27622
		private static Dictionary<TransferableOneWay, int> tmpLeftCountToTransfer = new Dictionary<TransferableOneWay, int>();

		// Token: 0x02003960 RID: 14688
		private enum Tab
		{
			// Token: 0x0400E4C3 RID: 58563
			Pawns,
			// Token: 0x0400E4C4 RID: 58564
			Items
		}
	}
}
