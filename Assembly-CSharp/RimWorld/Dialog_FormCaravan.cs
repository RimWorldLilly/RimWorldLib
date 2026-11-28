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
	// Token: 0x02001CAE RID: 7342
	public class Dialog_FormCaravan : Window
	{
		// Token: 0x1700222E RID: 8750
		// (get) Token: 0x0600BC1F RID: 48159 RVA: 0x00431F3C File Offset: 0x0043013C
		public PlanetTile CurrentTile
		{
			get
			{
				return this.map.Tile;
			}
		}

		// Token: 0x1700222F RID: 8751
		// (get) Token: 0x0600BC20 RID: 48160 RVA: 0x00431F49 File Offset: 0x00430149
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1024f, (float)UI.screenHeight);
			}
		}

		// Token: 0x17002230 RID: 8752
		// (get) Token: 0x0600BC21 RID: 48161 RVA: 0x00431F5B File Offset: 0x0043015B
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002231 RID: 8753
		// (get) Token: 0x0600BC22 RID: 48162 RVA: 0x00431F62 File Offset: 0x00430162
		private bool AutoStripSpawnedCorpses
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x17002232 RID: 8754
		// (get) Token: 0x0600BC23 RID: 48163 RVA: 0x00431F6A File Offset: 0x0043016A
		private bool ListPlayerPawnsInventorySeparately
		{
			get
			{
				return this.reform;
			}
		}

		// Token: 0x17002233 RID: 8755
		// (get) Token: 0x0600BC24 RID: 48164 RVA: 0x00431F72 File Offset: 0x00430172
		private BiomeDef Biome
		{
			get
			{
				return this.map.Biome;
			}
		}

		// Token: 0x17002234 RID: 8756
		// (get) Token: 0x0600BC25 RID: 48165 RVA: 0x00431F7F File Offset: 0x0043017F
		private bool MustChooseRoute
		{
			get
			{
				return this.canChooseRoute && (!this.reform || this.map.Parent is Settlement);
			}
		}

		// Token: 0x17002235 RID: 8757
		// (get) Token: 0x0600BC26 RID: 48166 RVA: 0x00431FA8 File Offset: 0x004301A8
		private bool ShowCancelButton
		{
			get
			{
				if (!this.mapAboutToBeRemoved)
				{
					return true;
				}
				bool flag = false;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					Pawn pawn = this.transferables[i].AnyThing as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
		}

		// Token: 0x17002236 RID: 8758
		// (get) Token: 0x0600BC27 RID: 48167 RVA: 0x0043200B File Offset: 0x0043020B
		private IgnorePawnsInventoryMode IgnoreInventoryMode
		{
			get
			{
				if (!this.ListPlayerPawnsInventorySeparately)
				{
					return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload;
				}
				return IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn;
			}
		}

		// Token: 0x17002237 RID: 8759
		// (get) Token: 0x0600BC28 RID: 48168 RVA: 0x00432018 File Offset: 0x00430218
		public float MassUsage
		{
			get
			{
				if (this.massUsageDirty)
				{
					this.massUsageDirty = false;
					this.cachedMassUsage = CollectionsMassCalculator.MassUsageTransferables(this.transferables, this.IgnoreInventoryMode, false, this.AutoStripSpawnedCorpses);
				}
				return this.cachedMassUsage;
			}
		}

		// Token: 0x17002238 RID: 8760
		// (get) Token: 0x0600BC29 RID: 48169 RVA: 0x00432050 File Offset: 0x00430250
		public float MassCapacity
		{
			get
			{
				if (this.massCapacityDirty)
				{
					this.massCapacityDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedMassCapacity = CollectionsMassCalculator.CapacityTransferables(this.transferables, stringBuilder);
					this.cachedMassCapacityExplanation = stringBuilder.ToString();
				}
				return this.cachedMassCapacity;
			}
		}

		// Token: 0x17002239 RID: 8761
		// (get) Token: 0x0600BC2A RID: 48170 RVA: 0x00432098 File Offset: 0x00430298
		private float TilesPerDay
		{
			get
			{
				if (this.tilesPerDayDirty)
				{
					this.tilesPerDayDirty = false;
					StringBuilder stringBuilder = new StringBuilder();
					this.cachedTilesPerDay = TilesPerDayCalculator.ApproxTilesPerDay(this.transferables, this.MassUsage, this.MassCapacity, this.CurrentTile, this.startingTile, false, stringBuilder);
					this.cachedTilesPerDayExplanation = stringBuilder.ToString();
				}
				return this.cachedTilesPerDay;
			}
		}

		// Token: 0x1700223A RID: 8762
		// (get) Token: 0x0600BC2B RID: 48171 RVA: 0x004320F8 File Offset: 0x004302F8
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
					float item;
					float item2;
					if (this.destinationTile.Valid)
					{
						using (WorldPath worldPath = this.CurrentTile.Layer.Pather.FindPath(this.CurrentTile, this.destinationTile, null, null))
						{
							int ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
							item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, worldPath, 0f, ticksPerMove);
							item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, worldPath, 0f, ticksPerMove);
							goto IL_F1;
						}
					}
					item = DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, null, 0f, 3300);
					item2 = DaysUntilRotCalculator.ApproxDaysUntilRot(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, null, 0f, 3300);
					IL_F1:
					this.cachedDaysWorthOfFood = new ValueTuple<float, float>(item, item2);
				}
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700223B RID: 8763
		// (get) Token: 0x0600BC2C RID: 48172 RVA: 0x0043221C File Offset: 0x0043041C
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

		// Token: 0x1700223C RID: 8764
		// (get) Token: 0x0600BC2D RID: 48173 RVA: 0x00432270 File Offset: 0x00430470
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

		// Token: 0x1700223D RID: 8765
		// (get) Token: 0x0600BC2E RID: 48174 RVA: 0x004322B8 File Offset: 0x004304B8
		private int TicksToArrive
		{
			get
			{
				if (!this.destinationTile.Valid)
				{
					return 0;
				}
				if (this.ticksToArriveDirty)
				{
					this.ticksToArriveDirty = false;
					using (WorldPath worldPath = this.CurrentTile.Layer.Pather.FindPath(this.CurrentTile, this.destinationTile, null, null))
					{
						this.cachedTicksToArrive = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.CurrentTile, this.destinationTile, worldPath, 0f, CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null), Find.TickManager.TicksAbs);
					}
				}
				return this.cachedTicksToArrive;
			}
		}

		// Token: 0x1700223E RID: 8766
		// (get) Token: 0x0600BC2F RID: 48175 RVA: 0x00432360 File Offset: 0x00430560
		private bool MostFoodWillRotSoon
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < this.transferables.Count; i++)
				{
					TransferableOneWay transferableOneWay = this.transferables[i];
					if (transferableOneWay.HasAnyThing && transferableOneWay.CountToTransfer > 0 && transferableOneWay.ThingDef.IsNutritionGivingIngestible && !(transferableOneWay.AnyThing is Corpse))
					{
						float num3 = 600f;
						CompRottable compRottable = transferableOneWay.AnyThing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							num3 = (float)DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.CurrentTile, null) / 60000f;
						}
						float num4 = transferableOneWay.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)transferableOneWay.CountToTransfer;
						if (num3 < 5f)
						{
							num += num4;
						}
						else
						{
							num2 += num4;
						}
					}
				}
				return (num != 0f || num2 != 0f) && num / (num + num2) >= 0.75f;
			}
		}

		// Token: 0x0600BC30 RID: 48176 RVA: 0x00432450 File Offset: 0x00430650
		public Dialog_FormCaravan(Map map, bool reform = false, Action onClosed = null, bool mapAboutToBeRemoved = false, IntVec3? designatedMeetingPoint = null) : base(null)
		{
			this.map = map;
			this.reform = reform;
			this.onClosed = onClosed;
			this.mapAboutToBeRemoved = mapAboutToBeRemoved;
			this.designatedMeetingPoint = (designatedMeetingPoint ?? IntVec3.Invalid);
			this.canChooseRoute = (!reform || !map.retainedCaravanData.HasDestinationTile);
			this.closeOnAccept = true;
			this.closeOnCancel = !reform;
			this.autoSelectTravelSupplies = !reform;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC31 RID: 48177 RVA: 0x00432541 File Offset: 0x00430741
		public override void PostOpen()
		{
			base.PostOpen();
			this.choosingRoute = false;
			if (!this.thisWindowInstanceEverOpened)
			{
				this.thisWindowInstanceEverOpened = true;
				this.CalculateAndRecacheTransferables();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.FormCaravan, KnowledgeAmount.Total);
				Find.WorldRoutePlanner.Start(this);
			}
		}

		// Token: 0x0600BC32 RID: 48178 RVA: 0x0043257B File Offset: 0x0043077B
		public override void PostClose()
		{
			base.PostClose();
			if (this.onClosed != null && !this.choosingRoute)
			{
				this.onClosed();
			}
		}

		// Token: 0x0600BC33 RID: 48179 RVA: 0x0043259E File Offset: 0x0043079E
		public void Notify_NoLongerChoosingRoute()
		{
			this.choosingRoute = false;
			if (!Find.WindowStack.IsOpen(this) && this.onClosed != null)
			{
				this.onClosed();
			}
		}

		// Token: 0x0600BC34 RID: 48180 RVA: 0x004325C8 File Offset: 0x004307C8
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, inRect.width, 35f);
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, (this.reform ? "ReformCaravan" : "FormCaravan").Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			CaravanUIUtility.DrawCaravanInfo(new CaravanUIUtility.CaravanInfo(this.MassUsage, this.MassCapacity, this.cachedMassCapacityExplanation, this.TilesPerDay, this.cachedTilesPerDayExplanation, this.DaysWorthOfFood, this.ForagedFoodPerDay, this.cachedForagedFoodPerDayExplanation, this.Visibility, this.cachedVisibilityExplanation, -1f, -1f, null), null, this.CurrentTile, (!this.destinationTile.Valid) ? null : new int?(this.TicksToArrive), this.lastMassFlashTime, new Rect(12f, 35f, inRect.width - 24f, 40f), true, (!this.destinationTile.Valid) ? null : ("\n" + "DaysWorthOfFoodTooltip_OnlyFirstWaypoint".Translate()), false, true);
			Dialog_FormCaravan.tabsList.Clear();
			Dialog_FormCaravan.tabsList.Add(new TabRecord("PawnsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Pawns;
			}, this.tab == Dialog_FormCaravan.Tab.Pawns));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("ItemsTab".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.Items;
			}, this.tab == Dialog_FormCaravan.Tab.Items));
			Dialog_FormCaravan.tabsList.Add(new TabRecord("TravelSupplies".Translate(), delegate()
			{
				this.tab = Dialog_FormCaravan.Tab.TravelSupplies;
			}, this.tab == Dialog_FormCaravan.Tab.TravelSupplies));
			inRect.yMin += 119f;
			Widgets.DrawMenuSection(inRect);
			TabDrawer.DrawTabs<TabRecord>(inRect, Dialog_FormCaravan.tabsList, 200f);
			Dialog_FormCaravan.tabsList.Clear();
			inRect = inRect.ContractedBy(17f);
			inRect.height += 17f;
			Widgets.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect rect3 = rect2;
			rect3.yMax -= 76f;
			bool flag = false;
			switch (this.tab)
			{
			case Dialog_FormCaravan.Tab.Pawns:
				this.pawnsTransfer.OnGUI(rect3, out flag);
				break;
			case Dialog_FormCaravan.Tab.Items:
				this.itemsTransfer.OnGUI(rect3, out flag);
				break;
			case Dialog_FormCaravan.Tab.TravelSupplies:
				this.travelSuppliesTransfer.extraHeaderSpace = 35f;
				this.travelSuppliesTransfer.OnGUI(rect3, out flag);
				this.DrawAutoSelectCheckbox(rect3, ref flag);
				break;
			}
			if (flag)
			{
				this.Notify_TransferablesChanged();
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BC35 RID: 48181 RVA: 0x00432898 File Offset: 0x00430A98
		public void DrawAutoSelectCheckbox(Rect rect, ref bool anythingChanged)
		{
			rect.yMin += 37f;
			rect.height = 35f;
			bool flag = this.autoSelectTravelSupplies;
			Widgets.CheckboxLabeled(rect, "AutomaticallySelectTravelSupplies".Translate(), ref this.autoSelectTravelSupplies, false, null, null, true, false);
			this.travelSuppliesTransfer.readOnly = this.autoSelectTravelSupplies;
			if (flag != this.autoSelectTravelSupplies)
			{
				anythingChanged = true;
			}
		}

		// Token: 0x0600BC36 RID: 48182 RVA: 0x00432906 File Offset: 0x00430B06
		public override bool CausesMessageBackground()
		{
			return true;
		}

		// Token: 0x0600BC37 RID: 48183 RVA: 0x0043290C File Offset: 0x00430B0C
		public void Notify_ChoseRoute(PlanetTile destinationTile)
		{
			this.destinationTile = destinationTile;
			this.startingTile = CaravanExitMapUtility.BestExitTileToGoTo(destinationTile, this.map);
			this.ticksToArriveDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.soundAppear.PlayOneShotOnCamera(null);
			if (this.autoSelectTravelSupplies)
			{
				this.SelectApproximateBestTravelSupplies();
			}
		}

		// Token: 0x0600BC38 RID: 48184 RVA: 0x0043295C File Offset: 0x00430B5C
		private void AddToTransferables(Thing t, bool setToTransferMax = false)
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
			if (setToTransferMax)
			{
				transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + t.stackCount);
			}
		}

		// Token: 0x0600BC39 RID: 48185 RVA: 0x004329DC File Offset: 0x00430BDC
		private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width - Dialog_FormCaravan.BottomButtonSize.x, rect.height - 55f - 17f, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "Send".Translate(), true, true, true, null))
			{
				this.TrySend();
			}
			if (this.ShowCancelButton && Widgets.ButtonText(new Rect(0f, rect2.y, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y), "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width / 2f - Dialog_FormCaravan.BottomButtonSize.x - 8.5f, rect2.y, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y), "ResetButton".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				this.CalculateAndRecacheTransferables();
			}
			if (this.canChooseRoute)
			{
				if (Widgets.ButtonText(new Rect(rect.width / 2f + 8.5f, rect2.y, Dialog_FormCaravan.BottomButtonSize.x, Dialog_FormCaravan.BottomButtonSize.y), "ChangeRouteButton".Translate(), true, true, true, null))
				{
					this.soundClose.PlayOneShotOnCamera(null);
					Find.WorldRoutePlanner.Start(this);
				}
				if (this.destinationTile.Valid)
				{
					Rect rect3 = rect2;
					rect3.y += rect2.height + 4f;
					rect3.height = 200f;
					rect3.xMin -= 200f;
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect3, "CaravanEstimatedDaysToDestination".Translate(((float)this.TicksToArrive / 60000f).ToString("0.#")));
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (Prefs.DevMode)
			{
				float width = 200f;
				float height = Dialog_FormCaravan.BottomButtonSize.y / 2f;
				if (Widgets.ButtonText(new Rect(0f, rect2.yMax + 4f, width, height), "DEV: Send instantly", true, true, true, null) && this.DebugTryFormCaravanInstantly())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
				if (Widgets.ButtonText(new Rect(204f, rect2.yMax + 4f, width, height), "DEV: Select everything", true, true, true, null))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.SetToSendEverything();
				}
			}
		}

		// Token: 0x0600BC3A RID: 48186 RVA: 0x00432CC8 File Offset: 0x00430EC8
		private void TrySend()
		{
			if (this.reform)
			{
				if (this.TryReformCaravan())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
					return;
				}
			}
			else
			{
				List<string> list = new List<string>();
				List<Pawn> pawns = TransferableUtility.GetPawnsFromTransferables(this.transferables);
				ValueTuple<float, float> daysWorthOfFood = this.DaysWorthOfFood;
				if (daysWorthOfFood.Item1 < 5f)
				{
					list.Add((daysWorthOfFood.Item1 < 0.1f) ? "DaysWorthOfFoodWarningDialog_NoFood".Translate().ToString() : "DaysWorthOfFoodWarningDialog".Translate(daysWorthOfFood.Item1.ToString("0.#")).Resolve());
				}
				else if (this.MostFoodWillRotSoon)
				{
					list.Add("CaravanFoodWillRotSoonWarningDialog".Translate());
				}
				if (!pawns.Any((Pawn pawn) => CaravanUtility.IsOwner(pawn, Faction.OfPlayer) && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled))
				{
					list.Add("CaravanIncapableOfSocial".Translate());
				}
				if (this.ShouldShowWarningForUndesirableFood())
				{
					list.Add("DaysWorthOfFoodDietWarningDialog".Translate());
				}
				if (this.ShouldShowWarningForMechWithoutMechanitor())
				{
					list.Add("CaravanLacksMechMechanitorWarning".Translate());
				}
				if (ModsConfig.BiotechActive)
				{
					IEnumerable<Pawn> source = pawns.Where(delegate(Pawn x)
					{
						Hediff_PsychicBond hediff_PsychicBond = x.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
						return hediff_PsychicBond != null && ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(x, hediff_PsychicBond) && !pawns.Contains(hediff_PsychicBond.target);
					});
					if (source.Any<Pawn>())
					{
						list.Add("PsychicBondDistanceWillBeActive_Caravan".Translate().ToString() + ":\n" + (from x in source
						select x.NameFullColored.Resolve() + " (" + "Partner".Translate((x.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond).target).Resolve() + ")").ToLineList("  - ", true));
					}
				}
				if (list.Count > 0)
				{
					if (this.CheckForErrors(pawns))
					{
						string str2 = string.Concat((from str in list
						select str + "\n\n").ToArray<string>()) + "CaravanAreYouSure".Translate().ToString();
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str2, delegate()
						{
							if (this.TryFormAndSendCaravan())
							{
								this.Close(false);
							}
						}, false, null, WindowLayer.Dialog));
						return;
					}
				}
				else if (this.TryFormAndSendCaravan())
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Close(false);
				}
			}
		}

		// Token: 0x0600BC3B RID: 48187 RVA: 0x00432F48 File Offset: 0x00431148
		private void CalculateAndRecacheTransferables()
		{
			this.transferables = new List<TransferableOneWay>();
			this.AddPawnsToTransferables();
			this.AddItemsToTransferables();
			CaravanUIUtility.CreateCaravanTransferableWidgets(this.transferables, out this.pawnsTransfer, out this.itemsTransfer, out this.travelSuppliesTransfer, "FormCaravanColonyThingCountTip".Translate(), this.IgnoreInventoryMode, () => this.MassCapacity - this.MassUsage, this.AutoStripSpawnedCorpses, this.CurrentTile, this.mapAboutToBeRemoved);
			this.Notify_TransferablesChanged();
		}

		// Token: 0x0600BC3C RID: 48188 RVA: 0x00432FC4 File Offset: 0x004311C4
		private bool DebugTryFormCaravanInstantly()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer)))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			PlanetTile directionTile = this.startingTile;
			if (!directionTile.Valid)
			{
				directionTile = CaravanExitMapUtility.RandomBestExitTileFrom(this.map);
			}
			if (!directionTile.Valid)
			{
				directionTile = this.CurrentTile;
			}
			CaravanFormingUtility.FormAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, directionTile, this.destinationTile);
			return true;
		}

		// Token: 0x0600BC3D RID: 48189 RVA: 0x00433090 File Offset: 0x00431290
		private bool TryFormAndSendCaravan()
		{
			List<Pawn> pawns = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawns))
			{
				return false;
			}
			IntVec3 root = pawns.Aggregate(IntVec3.Zero, (IntVec3 acc, Pawn p) => acc + p.Position) / pawns.Count;
			Direction8Way direction8WayFromTo = Find.WorldGrid.GetDirection8WayFromTo(this.CurrentTile, this.startingTile);
			IntVec3 exitSpot;
			if (!RCellFinder.TryFindClosestEdgeCellTo(root, this.map, out exitSpot) && !this.TryFindExitSpot(pawns, true, out exitSpot))
			{
				if (!this.TryFindExitSpot(pawns, false, out exitSpot))
				{
					Messages.Message("CaravanCouldNotFindExitSpot".Translate(direction8WayFromTo.LabelShort()), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("CaravanCouldNotFindReachableExitSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(exitSpot, this.map, false), MessageTypeDefOf.CautionInput, false);
			}
			IntVec3 meetingPoint = this.designatedMeetingPoint;
			if (meetingPoint == IntVec3.Invalid && !this.TryFindRandomPackingSpot(exitSpot, out meetingPoint))
			{
				Messages.Message("CaravanCouldNotFindPackingSpot".Translate(direction8WayFromTo.LabelShort()), new GlobalTargetInfo(exitSpot, this.map, false), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			Pawn pawn = pawns.Find((Pawn ropee) => AnimalPenUtility.NeedsToBeManagedByRope(ropee) && !pawns.Any((Pawn roper) => roper.IsColonist && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(roper, ropee, exitSpot) && GatherAnimalsAndSlavesForCaravanUtility.CanRoperTakeAnimalToDest(roper, ropee, meetingPoint)));
			if (pawn != null)
			{
				Messages.Message("CaravanRoamerCannotReachSpots".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.CautionInput, false);
				return false;
			}
			CaravanFormingUtility.StartFormingCaravan((from x in pawns
			where !x.Downed
			select x).ToList<Pawn>(), (from x in pawns
			where x.Downed
			select x).ToList<Pawn>(), Faction.OfPlayer, this.transferables, meetingPoint, exitSpot, this.startingTile, this.destinationTile);
			Messages.Message("CaravanFormationProcessStarted".Translate(), pawns[0], MessageTypeDefOf.PositiveEvent, false);
			if (ModsConfig.BiotechActive)
			{
				if (pawns.Any((Pawn p) => p.RaceProps.IsMechanoid))
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.MechsInCaravans, OpportunityType.GoodToKnow);
				}
			}
			return true;
		}

		// Token: 0x0600BC3E RID: 48190 RVA: 0x0043336C File Offset: 0x0043156C
		private bool TryReformCaravan()
		{
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!this.CheckForErrors(pawnsFromTransferables))
			{
				return false;
			}
			this.AddItemsFromTransferablesToRandomInventories(pawnsFromTransferables);
			Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawnsFromTransferables, Faction.OfPlayer, this.CurrentTile, this.CurrentTile, this.destinationTile, false);
			this.map.Parent.CheckRemoveMapNow();
			TaggedString taggedString = "MessageReformedCaravan".Translate();
			if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
			{
				taggedString += " " + "MessageFormedCaravan_Orders".Translate() + ": " + caravan.pather.ArrivalAction.Label + ".";
			}
			Messages.Message(taggedString, caravan, MessageTypeDefOf.TaskCompletion, false);
			return true;
		}

		// Token: 0x0600BC3F RID: 48191 RVA: 0x00433448 File Offset: 0x00431648
		private void AddItemsFromTransferablesToRandomInventories(List<Pawn> pawns)
		{
			this.transferables.RemoveAll((TransferableOneWay x) => x.AnyThing is Pawn);
			if (this.ListPlayerPawnsInventorySeparately)
			{
				for (int i = 0; i < pawns.Count; i++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(pawns[i]))
					{
						ThingOwner<Thing> innerContainer = pawns[i].inventory.innerContainer;
						for (int j = innerContainer.Count - 1; j >= 0; j--)
						{
							this.RemoveCarriedItemFromTransferablesOrDrop(innerContainer[j], pawns[i], this.transferables);
						}
					}
				}
				for (int k = 0; k < this.transferables.Count; k++)
				{
					if (this.transferables[k].things.Any((Thing x) => !x.Spawned))
					{
						this.transferables[k].things.SortBy((Thing x) => x.Spawned);
					}
				}
			}
			Action<Thing, IThingHolder> <>9__3;
			for (int l = 0; l < this.transferables.Count; l++)
			{
				if (!(this.transferables[l].AnyThing is Corpse))
				{
					List<Thing> things = this.transferables[l].things;
					int countToTransfer = this.transferables[l].CountToTransfer;
					Action<Thing, IThingHolder> transferred;
					if ((transferred = <>9__3) == null)
					{
						transferred = (<>9__3 = delegate(Thing splitPiece, IThingHolder originalHolder)
						{
							if (splitPiece.Faction != null && splitPiece.Faction != Faction.OfPlayer)
							{
								splitPiece.SetFactionDirect(Faction.OfPlayer);
							}
							Thing item = splitPiece.TryMakeMinified();
							CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.TryAddAndUnforbid(item);
						});
					}
					TransferableUtility.Transfer(things, countToTransfer, transferred);
				}
			}
			Action<Thing, int> <>9__4;
			for (int m = 0; m < this.transferables.Count; m++)
			{
				if (this.transferables[m].AnyThing is Corpse)
				{
					List<Thing> things2 = this.transferables[m].things;
					int countToTransfer2 = this.transferables[m].CountToTransfer;
					Action<Thing, int> transfer;
					if ((transfer = <>9__4) == null)
					{
						transfer = (<>9__4 = delegate(Thing originalThing, int numToTake)
						{
							if (this.AutoStripSpawnedCorpses)
							{
								Corpse corpse = originalThing as Corpse;
								if (corpse != null && corpse.Spawned)
								{
									corpse.Strip(true);
								}
							}
							Thing item = originalThing.SplitOff(numToTake);
							CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, pawns, null, null).inventory.TryAddAndUnforbid(item);
						});
					}
					TransferableUtility.TransferNoSplit(things2, countToTransfer2, transfer, true, true);
				}
			}
		}

		// Token: 0x0600BC40 RID: 48192 RVA: 0x00433698 File Offset: 0x00431898
		private bool CheckForErrors(List<Pawn> pawns)
		{
			if (this.MustChooseRoute && !this.destinationTile.Valid)
			{
				Messages.Message("MessageMustChooseRouteFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.reform && !this.startingTile.Valid)
			{
				Messages.Message("MessageNoValidExitTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!pawns.Any((Pawn x) => CaravanUtility.IsOwner(x, Faction.OfPlayer) && !x.Downed))
			{
				if (ModsConfig.IdeologyActive)
				{
					Messages.Message("CaravanMustHaveAtLeastOneNonSlaveColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					Messages.Message("CaravanMustHaveAtLeastOneColonist".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.reform && this.MassUsage > this.MassCapacity)
			{
				this.FlashMass();
				Messages.Message("TooBigCaravanMassUsage".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
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
							if (!t.Spawned || pawns.Any((Pawn x) => x.IsColonist && x.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
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
								Messages.Message("CaravanItemIsUnreachableSingle".Translate(this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Messages.Message("CaravanItemIsUnreachableMulti".Translate(countToTransfer, this.transferables[i].ThingDef.label), MessageTypeDefOf.RejectInput, false);
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600BC41 RID: 48193 RVA: 0x004338F4 File Offset: 0x00431AF4
		private bool ShouldShowWarningForUndesirableFood()
		{
			Dialog_FormCaravan.tmpFood.Clear();
			Dialog_FormCaravan.tmpPawns.Clear();
			int num = 0;
			foreach (TransferableOneWay transferableOneWay in this.transferables)
			{
				if (transferableOneWay.HasAnyThing)
				{
					if (transferableOneWay.AnyThing is Pawn)
					{
						for (int i = 0; i < transferableOneWay.CountToTransfer; i++)
						{
							Pawn pawn = (Pawn)transferableOneWay.things[i];
							if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.IgnoreInventoryMode))
							{
								ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
								int j = 0;
								int count = innerContainer.Count;
								while (j < count)
								{
									Thing thing = innerContainer[j];
									if (thing.def.IsNutritionGivingIngestible && ThingDefOf.Human.race.CanEverEat(thing))
									{
										Dialog_FormCaravan.tmpFood.Add(new ThingCount(thing, thing.stackCount, false));
										num += thing.stackCount;
									}
									j++;
								}
							}
							if (pawn.RaceProps.Humanlike)
							{
								Pawn_NeedsTracker needs = pawn.needs;
								if (((needs != null) ? needs.food : null) != null)
								{
									Dialog_FormCaravan.tmpPawns.Add(pawn);
								}
							}
						}
					}
					else if (transferableOneWay.ThingDef.IsNutritionGivingIngestible && ThingDefOf.Human.race.CanEverEat(transferableOneWay.AnyThing) && transferableOneWay.CountToTransfer > 0)
					{
						Dialog_FormCaravan.tmpFood.Add(new ThingCount(transferableOneWay.AnyThing, transferableOneWay.CountToTransfer, true));
						num += transferableOneWay.CountToTransfer;
					}
				}
			}
			int num2 = 0;
			foreach (ThingCount thingCount in Dialog_FormCaravan.tmpFood)
			{
				foreach (Pawn pawn2 in Dialog_FormCaravan.tmpPawns)
				{
					if (FoodUtility.WillGiveNegativeThoughts(thingCount.Thing, pawn2))
					{
						num2 += thingCount.Count;
						break;
					}
				}
			}
			return (double)((float)num2 / (float)num) >= 0.5;
		}

		// Token: 0x0600BC42 RID: 48194 RVA: 0x00433B80 File Offset: 0x00431D80
		private bool ShouldShowWarningForMechWithoutMechanitor()
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Dialog_FormCaravan.tmpPawnsToTransfer.Clear();
			foreach (TransferableOneWay transferableOneWay in this.transferables)
			{
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int i = 0; i < transferableOneWay.CountToTransfer; i++)
					{
						Dialog_FormCaravan.tmpPawnsToTransfer.Add((Pawn)transferableOneWay.things[i]);
					}
				}
			}
			for (int j = 0; j < Dialog_FormCaravan.tmpPawnsToTransfer.Count; j++)
			{
				Pawn pawn = Dialog_FormCaravan.tmpPawnsToTransfer[j];
				if (pawn.IsColonyMech && MechanitorUtility.EverControllable(pawn))
				{
					Pawn overseer = pawn.GetOverseer();
					if (overseer != null && !Dialog_FormCaravan.tmpPawnsToTransfer.Contains(overseer))
					{
						Dialog_FormCaravan.tmpPawnsToTransfer.Clear();
						return true;
					}
				}
			}
			Dialog_FormCaravan.tmpPawnsToTransfer.Clear();
			return false;
		}

		// Token: 0x0600BC43 RID: 48195 RVA: 0x00433C88 File Offset: 0x00431E88
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, out IntVec3 spot)
		{
			Rot4 rot;
			Rot4 rot2;
			CaravanExitMapUtility.GetExitMapEdges(Find.WorldGrid, this.CurrentTile, this.startingTile, out rot, out rot2);
			return (rot != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot, out spot)) || (rot2 != Rot4.Invalid && this.TryFindExitSpot(pawns, reachableForEveryColonist, rot2, out spot)) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Clockwise), out spot) || this.TryFindExitSpot(pawns, reachableForEveryColonist, rot.Rotated(RotationDirection.Counterclockwise), out spot);
		}

		// Token: 0x0600BC44 RID: 48196 RVA: 0x00433D08 File Offset: 0x00431F08
		private bool TryFindExitSpot(List<Pawn> pawns, bool reachableForEveryColonist, Rot4 exitDirection, out IntVec3 spot)
		{
			Dialog_FormCaravan.<>c__DisplayClass102_0 CS$<>8__locals1 = new Dialog_FormCaravan.<>c__DisplayClass102_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawns = pawns;
			if (!this.startingTile.Valid)
			{
				Log.Error("Can't find exit spot because startingTile is not set.");
				spot = IntVec3.Invalid;
				return false;
			}
			if (reachableForEveryColonist)
			{
				return CellFinder.TryFindRandomEdgeCellWith(delegate(IntVec3 x)
				{
					if (!base.<TryFindExitSpot>g__Validator|0(x))
					{
						return false;
					}
					for (int j = 0; j < CS$<>8__locals1.pawns.Count; j++)
					{
						if (CS$<>8__locals1.pawns[j].IsColonist && !CS$<>8__locals1.pawns[j].Downed && !CS$<>8__locals1.pawns[j].CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							return false;
						}
					}
					return true;
				}, this.map, exitDirection, CellFinder.EdgeRoadChance_Always, out spot);
			}
			IntVec3 intVec = IntVec3.Invalid;
			int num = -1;
			foreach (IntVec3 intVec2 in CellRect.WholeMap(this.map).GetEdgeCells(exitDirection).InRandomOrder(null))
			{
				if (CS$<>8__locals1.<TryFindExitSpot>g__Validator|0(intVec2))
				{
					int num2 = 0;
					for (int i = 0; i < CS$<>8__locals1.pawns.Count; i++)
					{
						if (CS$<>8__locals1.pawns[i].IsColonist && !CS$<>8__locals1.pawns[i].Downed && CS$<>8__locals1.pawns[i].CanReach(intVec2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							num2++;
						}
					}
					if (num2 > num)
					{
						num = num2;
						intVec = intVec2;
					}
				}
			}
			spot = intVec;
			return intVec.IsValid;
		}

		// Token: 0x0600BC45 RID: 48197 RVA: 0x00433E60 File Offset: 0x00432060
		private bool TryFindRandomPackingSpot(IntVec3 exitSpot, out IntVec3 packingSpot)
		{
			Dialog_FormCaravan.tmpPackingSpots.Clear();
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.CaravanPackingSpot);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			for (int i = 0; i < list.Count; i++)
			{
				if (this.map.reachability.CanReach(exitSpot, list[i], PathEndMode.OnCell, traverseParams))
				{
					Dialog_FormCaravan.tmpPackingSpots.Add(list[i]);
				}
			}
			if (Dialog_FormCaravan.tmpPackingSpots.Any<Thing>())
			{
				Thing thing = Dialog_FormCaravan.tmpPackingSpots.RandomElement<Thing>();
				Dialog_FormCaravan.tmpPackingSpots.Clear();
				packingSpot = thing.Position;
				return true;
			}
			return RCellFinder.TryFindRandomSpotJustOutsideColony(exitSpot, this.map, out packingSpot);
		}

		// Token: 0x0600BC46 RID: 48198 RVA: 0x00433F1C File Offset: 0x0043211C
		private void AddPawnsToTransferables()
		{
			List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				bool setToTransferMax = (this.reform || this.mapAboutToBeRemoved) && !CaravanUtility.ShouldAutoCapture(list[i], Faction.OfPlayer);
				this.AddToTransferables(list[i], setToTransferMax);
				Pawn_CarryTracker carryTracker = list[i].carryTracker;
				Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
				if (thing != null)
				{
					this.AddToTransferables(thing, setToTransferMax);
				}
			}
		}

		// Token: 0x0600BC47 RID: 48199 RVA: 0x00433FA8 File Offset: 0x004321A8
		private void AddItemsToTransferables()
		{
			List<Thing> list = CaravanFormingUtility.AllReachableColonyItems(this.map, this.reform, this.reform, this.reform);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddToTransferables(list[i], false);
			}
			if (this.AutoStripSpawnedCorpses)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Spawned)
					{
						this.TryAddCorpseInventoryAndGearToTransferables(list[j]);
					}
				}
			}
			if (this.ListPlayerPawnsInventorySeparately)
			{
				List<Pawn> list2 = Dialog_FormCaravan.AllSendablePawns(this.map, this.reform);
				for (int k = 0; k < list2.Count; k++)
				{
					if (Dialog_FormCaravan.CanListInventorySeparately(list2[k]))
					{
						ThingOwner<Thing> innerContainer = list2[k].inventory.innerContainer;
						for (int l = 0; l < innerContainer.Count; l++)
						{
							this.AddToTransferables(innerContainer[l], true);
							if (this.AutoStripSpawnedCorpses && innerContainer[l].Spawned)
							{
								this.TryAddCorpseInventoryAndGearToTransferables(innerContainer[l]);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600BC48 RID: 48200 RVA: 0x004340C8 File Offset: 0x004322C8
		private void TryAddCorpseInventoryAndGearToTransferables(Thing potentiallyCorpse)
		{
			Corpse corpse = potentiallyCorpse as Corpse;
			if (corpse != null)
			{
				this.AddCorpseInventoryAndGearToTransferables(corpse);
			}
		}

		// Token: 0x0600BC49 RID: 48201 RVA: 0x004340E8 File Offset: 0x004322E8
		private void AddCorpseInventoryAndGearToTransferables(Corpse corpse)
		{
			Pawn_InventoryTracker inventory = corpse.InnerPawn.inventory;
			Pawn_ApparelTracker apparel = corpse.InnerPawn.apparel;
			Pawn_EquipmentTracker equipment = corpse.InnerPawn.equipment;
			for (int i = 0; i < inventory.innerContainer.Count; i++)
			{
				if (Dialog_FormCaravan.<AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(inventory.innerContainer[i]))
				{
					this.AddToTransferables(inventory.innerContainer[i], false);
				}
			}
			if (apparel != null)
			{
				List<Apparel> wornApparel = apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					if (Dialog_FormCaravan.<AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(wornApparel[j]))
					{
						this.AddToTransferables(wornApparel[j], false);
					}
				}
			}
			if (equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = equipment.AllEquipmentListForReading;
				for (int k = 0; k < allEquipmentListForReading.Count; k++)
				{
					if (Dialog_FormCaravan.<AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(allEquipmentListForReading[k]))
					{
						this.AddToTransferables(allEquipmentListForReading[k], false);
					}
				}
			}
		}

		// Token: 0x0600BC4A RID: 48202 RVA: 0x004341D8 File Offset: 0x004323D8
		private void RemoveCarriedItemFromTransferablesOrDrop(Thing carried, Pawn carrier, List<TransferableOneWay> transferables)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(carried, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			int num;
			if (transferableOneWay == null)
			{
				num = carried.stackCount;
			}
			else if (transferableOneWay.CountToTransfer >= carried.stackCount)
			{
				transferableOneWay.AdjustBy(-carried.stackCount);
				transferableOneWay.things.Remove(carried);
				num = 0;
			}
			else
			{
				num = carried.stackCount - transferableOneWay.CountToTransfer;
				transferableOneWay.AdjustTo(0);
			}
			if (num > 0)
			{
				Thing thing = carried.SplitOff(num);
				if (carrier.SpawnedOrAnyParentSpawned)
				{
					GenPlace.TryPlaceThing(thing, carrier.PositionHeld, carrier.MapHeld, ThingPlaceMode.Near, null, null, null, 1);
					return;
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600BC4B RID: 48203 RVA: 0x00434277 File Offset: 0x00432477
		private void FlashMass()
		{
			this.lastMassFlashTime = Time.time;
		}

		// Token: 0x0600BC4C RID: 48204 RVA: 0x00434284 File Offset: 0x00432484
		public static bool CanListInventorySeparately(Pawn p)
		{
			return p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
		}

		// Token: 0x0600BC4D RID: 48205 RVA: 0x004342A4 File Offset: 0x004324A4
		private void SetToSendEverything()
		{
			for (int i = 0; i < this.transferables.Count; i++)
			{
				this.transferables[i].AdjustTo(this.transferables[i].GetMaximumToTransfer());
			}
			this.Notify_TransferablesChanged();
		}

		// Token: 0x0600BC4E RID: 48206 RVA: 0x004342F0 File Offset: 0x004324F0
		private void Notify_TransferablesChanged()
		{
			if (this.autoSelectTravelSupplies)
			{
				this.SelectApproximateBestTravelSupplies();
			}
			if (ModsConfig.BiotechActive)
			{
				foreach (TransferableOneWay transferableOneWay in this.transferables)
				{
					for (int i = 0; i < transferableOneWay.things.Count; i++)
					{
						if (transferableOneWay.IsThing)
						{
							Pawn pawn = transferableOneWay.things[i] as Pawn;
							if (pawn != null && pawn.IsColonyMech)
							{
								Pawn overseer = pawn.GetOverseer();
								if (overseer != null && pawn.GetMechControlGroup().WorkMode == MechWorkModeDefOf.Escort)
								{
									bool flag = false;
									foreach (TransferableOneWay transferableOneWay2 in this.transferables)
									{
										if (transferableOneWay2.IsThing && transferableOneWay2.things.Contains(overseer) && transferableOneWay2.CountToTransferToDestination > 0)
										{
											flag = true;
											break;
										}
									}
									if (flag && transferableOneWay.CountToTransferToDestination <= 0)
									{
										Messages.Message("MessageCaravanAddingEscortingMech".Translate(pawn.Named("MECH"), overseer.Named("OVERSEER")), pawn, MessageTypeDefOf.RejectInput, false);
										transferableOneWay.ForceToDestination(1);
									}
									if (!flag && transferableOneWay.CountToTransferToDestination > 0)
									{
										Messages.Message("MessageCaravanRemovingEscortingMech".Translate(pawn.Named("MECH"), overseer.Named("OVERSEER")), pawn, MessageTypeDefOf.RejectInput, false);
										transferableOneWay.ForceToDestination(0);
									}
								}
							}
						}
					}
				}
			}
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			this.tilesPerDayDirty = true;
			this.daysWorthOfFoodDirty = true;
			this.foragedFoodPerDayDirty = true;
			this.visibilityDirty = true;
			this.ticksToArriveDirty = true;
		}

		// Token: 0x0600BC4F RID: 48207 RVA: 0x00434510 File Offset: 0x00432710
		private void SelectApproximateBestTravelSupplies()
		{
			this.daysWorthOfFoodDirty = true;
			this.massUsageDirty = true;
			this.massCapacityDirty = true;
			List<TransferableOneWay> list = (from x in this.transferables
			where x.HasAnyThing && x.ThingDef.category != ThingCategory.Pawn && !x.ThingDef.thingCategories.NullOrEmpty<ThingCategoryDef>() && x.ThingDef.thingCategories.Contains(ThingCategoryDefOf.Medicine)
			select x).ToList<TransferableOneWay>();
			List<TransferableOneWay> list2 = (from x in this.transferables
			where x.HasAnyThing && x.ThingDef.IsIngestible && !x.ThingDef.IsDrug && !x.ThingDef.IsCorpse
			select x).ToList<TransferableOneWay>();
			IEnumerable<TransferableOneWay> enumerable = Enumerable.Empty<TransferableOneWay>();
			if (ModsConfig.BiotechActive)
			{
				enumerable = from x in this.transferables
				where x.ThingDef == ThingDefOf.HemogenPack
				select x;
			}
			Dialog_FormCaravan.tmpBeds.Clear();
			for (int i = 0; i < this.transferables.Count; i++)
			{
				for (int j = 0; j < this.transferables[i].things.Count; j++)
				{
					Thing thing = this.transferables[i].things[j];
					for (int k = 0; k < thing.stackCount; k++)
					{
						Building_Bed building_Bed = thing.GetInnerIfMinified() as Building_Bed;
						if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
						{
							for (int l = 0; l < building_Bed.SleepingSlotsCount; l++)
							{
								Dialog_FormCaravan.tmpBeds.Add(this.transferables[i]);
							}
						}
					}
				}
			}
			Dialog_FormCaravan.tmpBeds.SortByDescending((TransferableOneWay x) => x.AnyThing.GetInnerIfMinified().GetStatValue(StatDefOf.BedRestEffectiveness, true, -1));
			foreach (TransferableOneWay transferableOneWay in list)
			{
				transferableOneWay.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay2 in list2)
			{
				transferableOneWay2.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay3 in Dialog_FormCaravan.tmpBeds)
			{
				transferableOneWay3.AdjustTo(0);
			}
			foreach (TransferableOneWay transferableOneWay4 in enumerable)
			{
				transferableOneWay4.AdjustTo(0);
			}
			List<Pawn> pawnsFromTransferables = TransferableUtility.GetPawnsFromTransferables(this.transferables);
			if (!pawnsFromTransferables.Any<Pawn>())
			{
				return;
			}
			pawnsFromTransferables.SortByDescending((Pawn x) => x.RaceProps.Humanlike);
			foreach (Pawn pawn in pawnsFromTransferables)
			{
				TransferableOneWay transferableOneWay5 = this.BestBedFor(pawn);
				if (transferableOneWay5 != null)
				{
					Dialog_FormCaravan.tmpBeds.Remove(transferableOneWay5);
					if (transferableOneWay5.CanAdjustBy(1).Accepted)
					{
						this.AddOneIfMassAllows(transferableOneWay5);
					}
				}
				if (!pawn.AnimalOrWildMan() && (pawn.guest == null || !pawn.guest.IsPrisoner) && (!ModsConfig.BiotechActive || !pawn.IsColonyMech))
				{
					for (int m = 0; m < 2; m++)
					{
						Transferable transferable = this.BestMedicineItemFor(pawn, list);
						if (transferable != null)
						{
							this.AddOneIfMassAllows(transferable);
						}
					}
				}
			}
			if (!this.destinationTile.Valid || !DaysWorthOfFoodCalculator.AnyFoodEatingPawn(pawnsFromTransferables) || !list2.Any<TransferableOneWay>())
			{
				return;
			}
			try
			{
				Dialog_FormCaravan.<>c__DisplayClass117_0 CS$<>8__locals1;
				CS$<>8__locals1.path = this.CurrentTile.Layer.Pather.FindPath(this.CurrentTile, this.destinationTile, null, null);
				try
				{
					CS$<>8__locals1.ticksPerMove = CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(this), null);
					CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(this.CurrentTile, this.destinationTile, CS$<>8__locals1.path, 0f, CS$<>8__locals1.ticksPerMove, Find.TickManager.TicksAbs, Dialog_FormCaravan.tmpTicksToArrive);
					float num = (float)Dialog_FormCaravan.tmpTicksToArrive.Last<ValueTuple<PlanetTile, int>>().Item2 / 60000f;
					float num2 = num + Dialog_FormCaravan.ExtraFoodDaysRange.ClampToRange(num * 0.35f);
					foreach (Pawn pawn2 in pawnsFromTransferables)
					{
						if (ModsConfig.BiotechActive && !pawn2.AnimalOrWildMan() && !pawn2.IsPrisoner && pawn2.genes != null && num >= 0.5f && enumerable.Any<TransferableOneWay>() && pawn2.genes.GetFirstGeneOfType<Gene_Hemogen>() != null)
						{
							int num3 = Mathf.CeilToInt(num / 2f);
							for (int n = 0; n < num3; n++)
							{
								Transferable hemogenPack = this.GetHemogenPack(enumerable);
								if (hemogenPack != null)
								{
									this.AddOneIfMassAllows(hemogenPack);
								}
							}
						}
						if (VirtualPlantsUtility.CanEverEatVirtualPlants(pawn2))
						{
							for (float num4 = 0f; num4 < num; num4 += 0.25f)
							{
								int ticksAbs = Find.TickManager.TicksGame + (int)(60000f * num4);
								if (VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(CaravanArrivalTimeEstimator.TileIllBeInAt(ticksAbs, Dialog_FormCaravan.tmpTicksToArrive, Find.TickManager.TicksGame), ticksAbs))
								{
									Dialog_FormCaravan.tmpPawnNutritionDays.SetOrAdd(pawn2, 0.25f);
								}
							}
						}
					}
					float num5 = this.<SelectApproximateBestTravelSupplies>g__DaysOfFood|117_5(ref CS$<>8__locals1);
					Dialog_FormCaravan.cachedTicksToRot.Clear();
					while (num5 < num2 && this.MassUsage < this.MassCapacity)
					{
						bool flag = false;
						foreach (Pawn pawn3 in pawnsFromTransferables)
						{
							if (pawn3.RaceProps.EatsFood && pawn3.needs.food != null && pawn3.needs.food.MaxLevel > 0f && pawn3.needs.food.FoodFallPerTick != 0f && Dialog_FormCaravan.tmpPawnNutritionDays.TryGetValue(pawn3, 0f) < num2)
							{
								Transferable transferable2 = this.BestFoodItemFor(pawn3, list2, Dialog_FormCaravan.tmpTicksToArrive);
								if (transferable2 != null && this.AddFoodItem(pawn3, transferable2, num2))
								{
									flag = true;
								}
							}
						}
						if (!flag)
						{
							break;
						}
						num5 = this.<SelectApproximateBestTravelSupplies>g__DaysOfFood|117_5(ref CS$<>8__locals1);
					}
				}
				finally
				{
					if (CS$<>8__locals1.path != null)
					{
						((IDisposable)CS$<>8__locals1.path).Dispose();
					}
				}
			}
			finally
			{
				Dialog_FormCaravan.tmpTicksToArrive.Clear();
				Dialog_FormCaravan.tmpPawnNutritionDays.Clear();
				this.daysWorthOfFoodDirty = true;
				this.massUsageDirty = true;
				this.massCapacityDirty = true;
			}
		}

		// Token: 0x0600BC50 RID: 48208 RVA: 0x00434C78 File Offset: 0x00432E78
		private bool AddFoodItem(Pawn pawn, Transferable transferable, float tripDays)
		{
			int num = transferable.GetMaximumToTransfer() - transferable.CountToTransfer;
			if (num <= 0)
			{
				return false;
			}
			float num2 = Dialog_FormCaravan.tmpPawnNutritionDays.TryGetValue(pawn, 0f);
			float num3 = pawn.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true) * 60000f;
			float statValueAbstract = transferable.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
			int num4 = Mathf.Min(num, Mathf.CeilToInt((tripDays - num2) * num3 / statValueAbstract));
			if (num4 <= 0 || !transferable.CanAdjustBy(num4).Accepted)
			{
				return false;
			}
			int destination = transferable.CountToTransfer + Mathf.Min(num4, Mathf.FloorToInt((this.MassCapacity - this.MassUsage) / transferable.ThingDef.BaseMass));
			transferable.AdjustTo(destination);
			Dialog_FormCaravan.tmpPawnNutritionDays.SetOrAdd(pawn, num2 + num3 / (statValueAbstract * (float)num4));
			this.daysWorthOfFoodDirty = true;
			this.massUsageDirty = true;
			return true;
		}

		// Token: 0x0600BC51 RID: 48209 RVA: 0x00434D5C File Offset: 0x00432F5C
		private bool AddOneIfMassAllows(Transferable transferable)
		{
			if (transferable.CanAdjustBy(1).Accepted && this.MassUsage + transferable.ThingDef.BaseMass < this.MassCapacity)
			{
				transferable.AdjustBy(1);
				this.massUsageDirty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600BC52 RID: 48210 RVA: 0x00434DA8 File Offset: 0x00432FA8
		private TransferableOneWay BestBedFor(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.IsColonyMech)
			{
				return null;
			}
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.rest : null) == null)
			{
				return null;
			}
			for (int i = 0; i < Dialog_FormCaravan.tmpBeds.Count; i++)
			{
				Thing innerIfMinified = Dialog_FormCaravan.tmpBeds[i].AnyThing.GetInnerIfMinified();
				if (RestUtility.CanUseBedEver(pawn, innerIfMinified.def))
				{
					return Dialog_FormCaravan.tmpBeds[i];
				}
			}
			return null;
		}

		// Token: 0x0600BC53 RID: 48211 RVA: 0x00434E24 File Offset: 0x00433024
		private Transferable BestFoodItemFor(Pawn pawn, List<TransferableOneWay> food, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> ticksToArrive)
		{
			Transferable result = null;
			float num = 0f;
			for (int i = 0; i < food.Count; i++)
			{
				TransferableOneWay transferableOneWay = food[i];
				if (transferableOneWay.CanAdjustBy(1).Accepted)
				{
					float foodScore = this.GetFoodScore(pawn, transferableOneWay.AnyThing, ticksToArrive);
					if (foodScore > num)
					{
						result = transferableOneWay;
						num = foodScore;
					}
				}
			}
			return result;
		}

		// Token: 0x0600BC54 RID: 48212 RVA: 0x00434E80 File Offset: 0x00433080
		private float GetFoodScore(Pawn pawn, Thing food, [TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})] List<ValueTuple<PlanetTile, int>> ticksToArrive)
		{
			if (!CaravanPawnsNeedsUtility.CanEatForNutritionEver(food.def, pawn))
			{
				return 0f;
			}
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true, 1));
			if (FoodUtility.WillGiveNegativeThoughts(food, pawn))
			{
				num *= 0.4f;
			}
			if (ModsConfig.BiotechActive && food.def == ThingDefOf.BabyFood && !pawn.DevelopmentalStage.Baby())
			{
				num *= 0.1f;
			}
			int num2;
			if (!Dialog_FormCaravan.cachedTicksToRot.TryGetValue(food, out num2))
			{
				if (food.TryGetComp<CompRottable>() != null)
				{
					num2 = DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(food.TryGetComp<CompRottable>(), this.CurrentTile, ticksToArrive);
				}
				else
				{
					num2 = -1;
				}
				Dialog_FormCaravan.cachedTicksToRot[food] = num2;
			}
			if (num2 >= 0 && num2 < ticksToArrive.Last<ValueTuple<PlanetTile, int>>().Item2)
			{
				num *= 0.1f;
			}
			return num;
		}

		// Token: 0x0600BC55 RID: 48213 RVA: 0x00434F4C File Offset: 0x0043314C
		private Transferable BestMedicineItemFor(Pawn pawn, IEnumerable<TransferableOneWay> medicine)
		{
			Transferable transferable = null;
			float num = 0f;
			foreach (TransferableOneWay transferableOneWay in medicine)
			{
				Thing anyThing = transferableOneWay.AnyThing;
				if (transferableOneWay.CanAdjustBy(1).Accepted && pawn.playerSettings.medCare.AllowsMedicine(anyThing.def))
				{
					float statValue = anyThing.GetStatValue(StatDefOf.MedicalPotency, true, -1);
					if (transferable == null || statValue > num)
					{
						transferable = transferableOneWay;
						num = statValue;
					}
				}
			}
			return transferable;
		}

		// Token: 0x0600BC56 RID: 48214 RVA: 0x00434FE8 File Offset: 0x004331E8
		private Transferable GetHemogenPack(IEnumerable<TransferableOneWay> hemogen)
		{
			foreach (TransferableOneWay transferableOneWay in hemogen)
			{
				if (transferableOneWay.CanAdjustBy(1).Accepted)
				{
					return transferableOneWay;
				}
			}
			return null;
		}

		// Token: 0x0600BC57 RID: 48215 RVA: 0x00435044 File Offset: 0x00433244
		public static List<Pawn> AllSendablePawns(Map map, bool reform)
		{
			return CaravanFormingUtility.AllSendablePawns(map, true, reform, reform, reform, false, -1);
		}

		// Token: 0x0600BC58 RID: 48216 RVA: 0x00435052 File Offset: 0x00433252
		public override void OnAcceptKeyPressed()
		{
			this.TrySend();
		}

		// Token: 0x0600BC5F RID: 48223 RVA: 0x00435126 File Offset: 0x00433326
		[CompilerGenerated]
		internal static bool <AddCorpseInventoryAndGearToTransferables>g__CanAdd|108_0(Thing thing)
		{
			return !thing.def.destroyOnDrop && thing.GetInnerIfMinified().def.canLoadIntoCaravan;
		}

		// Token: 0x0600BC60 RID: 48224 RVA: 0x00435147 File Offset: 0x00433347
		[CompilerGenerated]
		private float <SelectApproximateBestTravelSupplies>g__DaysOfFood|117_5(ref Dialog_FormCaravan.<>c__DisplayClass117_0 A_1)
		{
			return DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this.transferables, this.CurrentTile, this.IgnoreInventoryMode, Faction.OfPlayer, A_1.path, 0f, A_1.ticksPerMove);
		}

		// Token: 0x04006B72 RID: 27506
		private readonly Map map;

		// Token: 0x04006B73 RID: 27507
		private readonly bool reform;

		// Token: 0x04006B74 RID: 27508
		private readonly Action onClosed;

		// Token: 0x04006B75 RID: 27509
		private readonly bool canChooseRoute;

		// Token: 0x04006B76 RID: 27510
		private readonly bool mapAboutToBeRemoved;

		// Token: 0x04006B77 RID: 27511
		public bool choosingRoute;

		// Token: 0x04006B78 RID: 27512
		private bool thisWindowInstanceEverOpened;

		// Token: 0x04006B79 RID: 27513
		public List<TransferableOneWay> transferables;

		// Token: 0x04006B7A RID: 27514
		private TransferableOneWayWidget pawnsTransfer;

		// Token: 0x04006B7B RID: 27515
		private TransferableOneWayWidget itemsTransfer;

		// Token: 0x04006B7C RID: 27516
		private TransferableOneWayWidget travelSuppliesTransfer;

		// Token: 0x04006B7D RID: 27517
		private Dialog_FormCaravan.Tab tab;

		// Token: 0x04006B7E RID: 27518
		private float lastMassFlashTime = -9999f;

		// Token: 0x04006B7F RID: 27519
		private PlanetTile startingTile = PlanetTile.Invalid;

		// Token: 0x04006B80 RID: 27520
		private PlanetTile destinationTile = PlanetTile.Invalid;

		// Token: 0x04006B81 RID: 27521
		private readonly IntVec3 designatedMeetingPoint = IntVec3.Invalid;

		// Token: 0x04006B82 RID: 27522
		private bool massUsageDirty = true;

		// Token: 0x04006B83 RID: 27523
		private float cachedMassUsage;

		// Token: 0x04006B84 RID: 27524
		private bool massCapacityDirty = true;

		// Token: 0x04006B85 RID: 27525
		private float cachedMassCapacity;

		// Token: 0x04006B86 RID: 27526
		private string cachedMassCapacityExplanation;

		// Token: 0x04006B87 RID: 27527
		private bool tilesPerDayDirty = true;

		// Token: 0x04006B88 RID: 27528
		private float cachedTilesPerDay;

		// Token: 0x04006B89 RID: 27529
		private string cachedTilesPerDayExplanation;

		// Token: 0x04006B8A RID: 27530
		private bool daysWorthOfFoodDirty = true;

		// Token: 0x04006B8B RID: 27531
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x04006B8C RID: 27532
		private bool foragedFoodPerDayDirty = true;

		// Token: 0x04006B8D RID: 27533
		[TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		private ValueTuple<ThingDef, float> cachedForagedFoodPerDay;

		// Token: 0x04006B8E RID: 27534
		private string cachedForagedFoodPerDayExplanation;

		// Token: 0x04006B8F RID: 27535
		private bool visibilityDirty = true;

		// Token: 0x04006B90 RID: 27536
		private float cachedVisibility;

		// Token: 0x04006B91 RID: 27537
		private string cachedVisibilityExplanation;

		// Token: 0x04006B92 RID: 27538
		private bool ticksToArriveDirty = true;

		// Token: 0x04006B93 RID: 27539
		private int cachedTicksToArrive;

		// Token: 0x04006B94 RID: 27540
		private bool autoSelectTravelSupplies;

		// Token: 0x04006B95 RID: 27541
		private const int AutoMedicinePerColonist = 2;

		// Token: 0x04006B96 RID: 27542
		private const float TitleRectHeight = 35f;

		// Token: 0x04006B97 RID: 27543
		private const float BottomAreaHeight = 55f;

		// Token: 0x04006B98 RID: 27544
		private const float AutoSelectCheckBoxHeight = 35f;

		// Token: 0x04006B99 RID: 27545
		private const float AdjustedTravelTimeFactor = 0.35f;

		// Token: 0x04006B9A RID: 27546
		private const float MaxDaysWorthOfFoodToShowWarningDialog = 5f;

		// Token: 0x04006B9B RID: 27547
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04006B9C RID: 27548
		private static FloatRange ExtraFoodDaysRange = new FloatRange(1f, 5f);

		// Token: 0x04006B9D RID: 27549
		private static readonly List<TabRecord> tabsList = new List<TabRecord>();

		// Token: 0x04006B9E RID: 27550
		private static readonly List<ThingCount> tmpFood = new List<ThingCount>();

		// Token: 0x04006B9F RID: 27551
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04006BA0 RID: 27552
		private static readonly List<Pawn> tmpPawnsToTransfer = new List<Pawn>();

		// Token: 0x04006BA1 RID: 27553
		private static readonly List<Thing> tmpPackingSpots = new List<Thing>();

		// Token: 0x04006BA2 RID: 27554
		[TupleElementNames(new string[]
		{
			"tile",
			"ticks"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTicksToArrive = new List<ValueTuple<PlanetTile, int>>();

		// Token: 0x04006BA3 RID: 27555
		private static readonly Dictionary<Pawn, float> tmpPawnNutritionDays = new Dictionary<Pawn, float>();

		// Token: 0x04006BA4 RID: 27556
		private static readonly List<TransferableOneWay> tmpBeds = new List<TransferableOneWay>();

		// Token: 0x04006BA5 RID: 27557
		private static Dictionary<Thing, int> cachedTicksToRot = new Dictionary<Thing, int>();

		// Token: 0x02003954 RID: 14676
		private enum Tab
		{
			// Token: 0x0400E496 RID: 58518
			Pawns,
			// Token: 0x0400E497 RID: 58519
			Items,
			// Token: 0x0400E498 RID: 58520
			TravelSupplies
		}
	}
}
