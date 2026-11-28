using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C9E RID: 7326
	public class Dialog_ChooseThingsForNewColony : Window
	{
		// Token: 0x1700220C RID: 8716
		// (get) Token: 0x0600BB63 RID: 47971 RVA: 0x0042686A File Offset: 0x00424A6A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(540f, 800f);
			}
		}

		// Token: 0x1700220D RID: 8717
		// (get) Token: 0x0600BB64 RID: 47972 RVA: 0x0042687C File Offset: 0x00424A7C
		public int ColonistCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.colonists.Count; i++)
				{
					if (this.selected.Contains(this.colonists[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700220E RID: 8718
		// (get) Token: 0x0600BB65 RID: 47973 RVA: 0x004268C0 File Offset: 0x00424AC0
		public int SlaveCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.colonists.Count; i++)
				{
					if (this.selected.Contains(this.colonists[i]))
					{
						Pawn pawn = this.colonists[i] as Pawn;
						if (pawn != null && pawn.IsSlaveOfColony)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x1700220F RID: 8719
		// (get) Token: 0x0600BB66 RID: 47974 RVA: 0x00426920 File Offset: 0x00424B20
		public int AnimalCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.animals.Count; i++)
				{
					if (this.selected.Contains(this.animals[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002210 RID: 8720
		// (get) Token: 0x0600BB67 RID: 47975 RVA: 0x00426964 File Offset: 0x00424B64
		public int RelicCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.relics.Count; i++)
				{
					if (this.selected.Contains(this.relics[i]))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002211 RID: 8721
		// (get) Token: 0x0600BB68 RID: 47976 RVA: 0x004269A8 File Offset: 0x00424BA8
		private AcceptanceReport AcceptanceReport
		{
			get
			{
				int colonistCount = this.ColonistCount;
				if (colonistCount > this.maxColonists)
				{
					return "MessageNewColonyMax".Translate(this.maxColonists, "People".Translate());
				}
				if (colonistCount == 0)
				{
					return "MessageNewColoyRequiresOneColonist".Translate();
				}
				if (this.AnimalCount > this.maxAnimals)
				{
					return "MessageNewColonyMax".Translate(this.maxAnimals, "AnimalsLower".Translate());
				}
				if (this.RelicCount > this.maxRelics)
				{
					return "MessageNewColonyMax".Translate(this.maxRelics, (this.maxRelics == 1) ? "RelicLower".Translate() : "RelicsLower".Translate());
				}
				if (this.selectedItemCount > this.maxItems)
				{
					return "MessageNewColonyMax".Translate(this.maxItems, "ItemsLower".Translate());
				}
				return AcceptanceReport.WasAccepted;
			}
		}

		// Token: 0x0600BB69 RID: 47977 RVA: 0x00426AC4 File Offset: 0x00424CC4
		public Dialog_ChooseThingsForNewColony(Action<List<Thing>> postAccepted, int maxColonists = 5, int maxAnimals = 5, int maxRelics = 1, int maxItems = 7, Action cancel = null) : base(null)
		{
			if (!ModLister.CheckIdeology("Choose new colony"))
			{
				return;
			}
			this.postAccepted = postAccepted;
			this.maxAnimals = maxAnimals;
			this.maxColonists = maxColonists;
			this.maxRelics = maxRelics;
			this.maxItems = maxItems;
			this.forcePause = true;
			this.closeOnCancel = true;
			this.absorbInputAroundWindow = true;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.preventSave = true;
			this.cancel = cancel;
		}

		// Token: 0x0600BB6A RID: 47978 RVA: 0x00426BC4 File Offset: 0x00424DC4
		public override void PostOpen()
		{
			Dialog_ChooseThingsForNewColony.<>c__DisplayClass42_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this.colonists.Clear();
			this.animals.Clear();
			this.relics.Clear();
			this.items.Clear();
			this.selected.Clear();
			this.selectedItemCount = 0;
			this.itemArchonexusAllowedStackCount.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (!pawn.IsQuestLodger())
				{
					if (pawn.IsColonist)
					{
						this.colonists.Add(pawn);
					}
					else if (pawn.IsAnimal)
					{
						this.animals.Add(pawn);
					}
				}
			}
			foreach (Precept precept in Faction.OfPlayer.ideos.PrimaryIdeo.PreceptsListForReading)
			{
				Precept_Relic precept_Relic = precept as Precept_Relic;
				if (precept_Relic != null && precept_Relic.RelicInPlayerPossession)
				{
					this.relics.Add(precept_Relic.GeneratedRelic);
				}
			}
			CS$<>8__locals1.itemDefMap = new Dictionary<ThingDef, List<Thing>>();
			CS$<>8__locals1.distinctItem = new List<Thing>();
			List<Thing> list = new List<Thing>();
			foreach (TravellingTransporters travellingTransporters in Find.WorldObjects.TravellingTransporters)
			{
				if (travellingTransporters.IsPlayerControlled)
				{
					ThingOwnerUtility.GetAllThingsRecursively(travellingTransporters, list, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter));
					foreach (Thing t4 in list)
					{
						this.<PostOpen>g__CountThing|42_0(t4, ref CS$<>8__locals1);
					}
				}
			}
			List<Thing> list2 = new List<Thing>();
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				if (caravan.IsPlayerControlled)
				{
					ThingOwnerUtility.GetAllThingsRecursively(caravan, list2, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter));
					foreach (Thing t2 in list2)
					{
						this.<PostOpen>g__CountThing|42_0(t2, ref CS$<>8__locals1);
					}
				}
			}
			List<Thing> list3 = new List<Thing>();
			foreach (Map holder in Find.Maps)
			{
				ThingOwnerUtility.GetAllThingsRecursively(holder, list3, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter));
				foreach (Thing t3 in list3)
				{
					this.<PostOpen>g__CountThing|42_0(t3, ref CS$<>8__locals1);
				}
			}
			foreach (List<Thing> list4 in CS$<>8__locals1.itemDefMap.Values)
			{
				foreach (Thing item in list4)
				{
					this.items.Add(item);
				}
			}
			foreach (Thing item2 in CS$<>8__locals1.distinctItem)
			{
				this.items.Add(item2);
			}
			this.colonists.SortBy((Thing t) => t.Label);
			this.animals.SortBy((Thing t) => t.Label);
			this.relics.SortBy((Thing t) => t.Label);
			this.items = this.items.OrderByDescending((Thing i) => i.def, this.itemComparator).ThenBy((Thing i) => i.def.label).ThenByDescending(delegate(Thing i)
			{
				QualityCategory result;
				i.TryGetQuality(out result);
				return result;
			}).ToList<Thing>();
		}

		// Token: 0x0600BB6B RID: 47979 RVA: 0x004270D4 File Offset: 0x004252D4
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 40f), "ChooseThingsForNewColonyTitle".Translate());
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(0f, 40f, rect.width, 25f), "ChooseThingsForNewColonyDesc".Translate());
			float num = rect.width;
			Rect outRect = new Rect(rect.x, rect.y + 40f + 25f + 10f, rect.width, rect.height - 40f - 25f - 70f);
			float num2 = Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.colonists.Count) + Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.animals.Count) + Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.relics.Count) + Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.items.Count);
			if (num2 > outRect.height)
			{
				num -= 16f;
			}
			Rect rect2 = new Rect(0f, 0f, num, num2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			if (this.colonists.Count > 0)
			{
				this.DrawThingList(this.colonists, rect2, "ChoosePeopleDesc".Translate(this.maxColonists), false);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.colonists.Count);
			}
			if (this.animals.Count > 0)
			{
				this.DrawThingList(this.animals, rect2, "ChooseThingsDesc".Translate(this.maxAnimals, "AnimalsLower".Translate()), false);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.animals.Count);
			}
			if (this.relics.Count > 0)
			{
				TaggedString value = (this.maxRelics == 1) ? "RelicLower".Translate() : "RelicsLower".Translate();
				this.DrawThingList(this.relics, rect2, "ChooseThingsDesc".Translate(this.maxRelics, value), false);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.relics.Count);
			}
			if (this.items.Count > 0)
			{
				this.DrawThingList(this.items, rect2, "ChooseThingsDesc".Translate(this.maxItems, "ItemsLower".Translate()), true);
				rect2.yMin += Dialog_ChooseThingsForNewColony.<DoWindowContents>g__CalculateSectionHeight|43_0(this.items.Count);
			}
			Widgets.EndScrollView();
			Rect rect3 = new Rect(0f, rect.yMax - 70f, rect.width, 70f);
			Rect rect4 = new Rect(rect3.xMax - this.BottomButtonSize.x, rect3.yMax - this.BottomButtonSize.y, this.BottomButtonSize.x, this.BottomButtonSize.y);
			AcceptanceReport acceptanceReport = this.AcceptanceReport;
			if (!acceptanceReport.Accepted)
			{
				Rect rect5 = rect4;
				rect5.x -= 162f;
				rect5.width = 160f;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleRight;
				GUI.color = Color.red;
				Widgets.Label(rect5, acceptanceReport.Reason);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			int slaveCount = this.SlaveCount;
			bool flag = slaveCount > 0 && slaveCount == this.ColonistCount;
			if (acceptanceReport.Accepted && flag)
			{
				Rect rect6 = rect4;
				rect6.x -= 162f;
				rect6.width = 160f;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleLeft;
				GUI.color = Color.yellow;
				Widgets.Label(rect6, "ChooseOnlySlavesInfo".Translate());
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			if (Widgets.ButtonText(rect4, "AcceptButton".Translate(), true, true, true, null))
			{
				if (acceptanceReport.Accepted)
				{
					this.ConfirmArchonexusSettlementConsequences(this.selected.ToList<Thing>(), flag);
				}
				else
				{
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				}
			}
			Rect rect7 = rect4;
			rect7.x = rect3.x;
			if (Widgets.ButtonText(rect7, "Cancel".Translate(), true, true, true, null))
			{
				if (this.cancel != null)
				{
					this.cancel();
				}
				this.Close(true);
			}
		}

		// Token: 0x0600BB6C RID: 47980 RVA: 0x004275A6 File Offset: 0x004257A6
		public override void OnCancelKeyPressed()
		{
			base.OnCancelKeyPressed();
			if (this.cancel != null)
			{
				this.cancel();
			}
		}

		// Token: 0x0600BB6D RID: 47981 RVA: 0x004275C4 File Offset: 0x004257C4
		private void ConfirmArchonexusSettlementConsequences(List<Thing> selectedList, bool onlySlavesSelected)
		{
			Dialog_ChooseThingsForNewColony.<>c__DisplayClass45_0 CS$<>8__locals1 = new Dialog_ChooseThingsForNewColony.<>c__DisplayClass45_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onlySlavesSelected = onlySlavesSelected;
			CS$<>8__locals1.selectedList = selectedList;
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			if (this.items.Count > this.selectedItemCount && this.selectedItemCount < this.maxItems)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("ArchonexusItemsNotChosen".Translate(this.selectedItemCount, Mathf.Min(this.items.Count, this.maxItems)).Colorize(ColoredText.WarningColor));
			}
			List<Thing> abandonedRelicsCarriedByPawns = MoveColonyUtility.GetAbandonedRelicsCarriedByPawns(CS$<>8__locals1.selectedList);
			if (this.relics.Count > this.RelicCount && this.RelicCount <= 0)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("ArchonexusRelicNotChosen".Translate().Colorize(ColoredText.WarningColor));
			}
			else if (abandonedRelicsCarriedByPawns.Count > 0)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("RemoveEquippedRelicsDescription".Translate().Colorize(ColoredText.WarningColor) + "\n\n" + (from r in abandonedRelicsCarriedByPawns
				select r.Label.Colorize(ColoredText.WarningColor)).ToLineList(" - ".Colorize(ColoredText.WarningColor), false));
			}
			CS$<>8__locals1.escapeShip = Find.WorldObjects.AllWorldObjects.FirstOrDefault((WorldObject wo) => wo.def == WorldObjectDefOf.EscapeShip);
			if (CS$<>8__locals1.escapeShip != null)
			{
				if (flag)
				{
					stringBuilder.Append("\n\n");
				}
				flag = true;
				stringBuilder.Append("ShipEscapeWillEndDescription".Translate());
			}
			if (flag)
			{
				stringBuilder.Append("\n\n");
			}
			stringBuilder.Append("WantToProceed".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), new Action(CS$<>8__locals1.<ConfirmArchonexusSettlementConsequences>g__CloseAction|2), false, "ConfirmDecisionsTitle".Translate(), WindowLayer.Dialog));
		}

		// Token: 0x0600BB6E RID: 47982 RVA: 0x004277F8 File Offset: 0x004259F8
		private void DrawThingList(List<Thing> things, Rect rect, string title, bool isItem)
		{
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 30f), title);
			Widgets.DrawLineHorizontal(rect.x, rect.y + 21f, Text.CalcSize(title).x);
			Text.Anchor = TextAnchor.UpperLeft;
			int count = things.Count;
			float num = rect.y + 30f;
			for (int i = 0; i < count; i++)
			{
				Rect rect2 = new Rect(rect.x, num + (float)i * 30f, rect.width, 30f);
				this.DoRow(rect2, things[i], i, isItem);
			}
		}

		// Token: 0x0600BB6F RID: 47983 RVA: 0x004278B0 File Offset: 0x00425AB0
		private void DoRow(Rect rect, Thing t, int index, bool isItem)
		{
			Dialog_ChooseThingsForNewColony.<>c__DisplayClass47_0 CS$<>8__locals1 = new Dialog_ChooseThingsForNewColony.<>c__DisplayClass47_0();
			CS$<>8__locals1.t = t;
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Widgets.BeginGroup(rect);
			float num = 4f;
			CS$<>8__locals1.p = (CS$<>8__locals1.t as Pawn);
			CS$<>8__locals1.label = CS$<>8__locals1.t.LabelCap;
			Pawn p = CS$<>8__locals1.p;
			bool flag;
			if (p == null)
			{
				flag = false;
			}
			else
			{
				RaceProperties raceProps = p.RaceProps;
				flag = ((raceProps != null) ? new bool?(raceProps.Animal) : null).GetValueOrDefault();
			}
			if (flag)
			{
				Dialog_ChooseThingsForNewColony.<>c__DisplayClass47_0 CS$<>8__locals2 = CS$<>8__locals1;
				string labelCap = CS$<>8__locals1.p.LabelCap;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(CS$<>8__locals1.p.GetGenderLabel());
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Mathf.FloorToInt(CS$<>8__locals1.p.ageTracker.AgeBiologicalYearsFloat));
				defaultInterpolatedStringHandler.AppendLiteral(")");
				CS$<>8__locals2.label = labelCap + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else if (isItem)
			{
				CS$<>8__locals1.label = GenLabel.ThingLabel(CS$<>8__locals1.t, 1, false, true).CapitalizeFirst();
			}
			float num2 = isItem ? 250f : 196f;
			Rect rect2 = new Rect(0f, 0f, 80f + num2, rect.height);
			Color color = (CS$<>8__locals1.p != null && CS$<>8__locals1.p.IsSlave) ? PawnNameColorUtility.PawnNameColorOf(CS$<>8__locals1.p) : Color.white;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, new TipSignal(delegate()
				{
					string text = CS$<>8__locals1.label;
					string text2 = (CS$<>8__locals1.p != null) ? (CS$<>8__locals1.p.MainDesc(true, true) + "\n\n" + CS$<>8__locals1.p.def.description) : CS$<>8__locals1.t.DescriptionDetailed;
					if (!text2.NullOrEmpty())
					{
						text = text + ": " + text2 + TransferableUIUtility.ContentSourceDescription(CS$<>8__locals1.t);
					}
					return text;
				}, CS$<>8__locals1.t.GetHashCode()));
			}
			Widgets.InfoCardButton(0f, 0f, CS$<>8__locals1.t);
			Widgets.ThingIcon(new Rect(36f, 0f, 27f, 27f), CS$<>8__locals1.t, 1f, null, false, 1f, false);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect3 = new Rect(80f, 0f, num2, rect2.height);
			Text.WordWrap = false;
			GUI.color = color;
			Widgets.Label(rect3, CS$<>8__locals1.label);
			GUI.color = Color.white;
			Text.WordWrap = true;
			num += 320f;
			if (CS$<>8__locals1.p != null && CS$<>8__locals1.p.Ideo != null && !Find.IdeoManager.classicMode)
			{
				Rect rect4 = new Rect(num, 0f, 27f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect4);
				CS$<>8__locals1.p.Ideo.DrawIcon(rect4);
				TooltipHandler.TipRegion(rect4, CS$<>8__locals1.p.Ideo.name);
			}
			else
			{
				Pawn p2 = CS$<>8__locals1.p;
				bool flag2;
				if (p2 == null)
				{
					flag2 = false;
				}
				else
				{
					RaceProperties raceProps2 = p2.RaceProps;
					flag2 = ((raceProps2 != null) ? new bool?(raceProps2.Animal) : null).GetValueOrDefault();
				}
				if (flag2)
				{
					if (CS$<>8__locals1.p.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
					{
						TransferableUIUtility.DrawPregnancyIcon(CS$<>8__locals1.p, new Rect(num, 2f, 23f, rect.height - 4f));
						num += 27f;
					}
					if (CS$<>8__locals1.p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
					{
						TransferableUIUtility.DrawBondedIcon(CS$<>8__locals1.p, new Rect(num, 2f, 23f, rect.height - 4f));
						num += 27f;
					}
				}
			}
			num += 27f;
			if (CS$<>8__locals1.p != null && CS$<>8__locals1.p.IsSlave)
			{
				Rect rect5 = new Rect(num, 0f, 27f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect5);
				GUI.DrawTexture(rect5, CS$<>8__locals1.p.guest.GetIcon());
				TooltipHandler.TipRegion(rect5, CS$<>8__locals1.p.guest.GetLabel());
				num += 27f;
			}
			if (isItem)
			{
				Rect rect6 = new Rect(rect.width - 27f - 4f - 60f - 10f, 1f, 60f, rect.height);
				if (Mouse.IsOver(rect6))
				{
					Widgets.DrawHighlight(rect6);
					TooltipHandler.TipRegion(rect6, new TipSignal(() => "ItemQuantityTooltip".Translate(CS$<>8__locals1.label, CS$<>8__locals1.t.def.ArchonexusMaxAllowedCount), CS$<>8__locals1.t.GetHashCode()));
				}
				GUI.color = Color.white;
				Text.WordWrap = false;
				TextAnchor anchor = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleRight;
				int num3 = MoveColonyUtility.IsDistinctArchonexusItem(CS$<>8__locals1.t.def) ? CS$<>8__locals1.t.stackCount : this.itemArchonexusAllowedStackCount[CS$<>8__locals1.t];
				Widgets.Label(rect6.LeftPartPixels(rect6.width - 10f), num3.ToString());
				Text.Anchor = anchor;
				Text.WordWrap = true;
			}
			Rect rect7 = new Rect(rect.width - 27f - 4f, 1f, 27f, 27f);
			Rect butRect = new Rect(rect7);
			butRect.xMin -= 6f;
			butRect.width += 12f;
			butRect.yMin -= 1.5f;
			butRect.height += 3f;
			if (Widgets.ButtonInvisible(butRect, true))
			{
				if (this.selected.Contains(CS$<>8__locals1.t))
				{
					this.selected.Remove(CS$<>8__locals1.t);
					if (isItem)
					{
						this.selectedItemCount--;
					}
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				else
				{
					this.selected.Add(CS$<>8__locals1.t);
					if (isItem)
					{
						this.selectedItemCount++;
					}
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
			GUI.DrawTexture(rect7.ContractedBy(1f), this.selected.Contains(CS$<>8__locals1.t) ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			GenUI.ResetLabelAlign();
			Widgets.EndGroup();
		}

		// Token: 0x0600BB70 RID: 47984 RVA: 0x00427ED0 File Offset: 0x004260D0
		[CompilerGenerated]
		private void <PostOpen>g__CountThing|42_0(Thing t, ref Dialog_ChooseThingsForNewColony.<>c__DisplayClass42_0 A_2)
		{
			if (t.def.ArchonexusMaxAllowedCount == 0)
			{
				return;
			}
			if (!MoveColonyUtility.IsBringableItem(t))
			{
				return;
			}
			if (!A_2.itemDefMap.ContainsKey(t.def))
			{
				A_2.itemDefMap[t.def] = new List<Thing>();
			}
			if (MoveColonyUtility.IsDistinctArchonexusItem(t.def))
			{
				A_2.distinctItem.Add(t);
				return;
			}
			foreach (Thing thing in A_2.itemDefMap[t.def])
			{
				if (t.CanStackWith(thing))
				{
					this.itemArchonexusAllowedStackCount[thing] = Mathf.Min(t.def.ArchonexusMaxAllowedCount, t.stackCount + this.itemArchonexusAllowedStackCount[thing]);
					return;
				}
			}
			this.itemArchonexusAllowedStackCount[t] = Mathf.Min(t.def.ArchonexusMaxAllowedCount, t.stackCount);
			A_2.itemDefMap[t.def].Add(t);
		}

		// Token: 0x0600BB71 RID: 47985 RVA: 0x00427FF4 File Offset: 0x004261F4
		[CompilerGenerated]
		internal static float <DoWindowContents>g__CalculateSectionHeight|43_0(int numberOfThings)
		{
			if (numberOfThings == 0)
			{
				return 0f;
			}
			return 40f + (float)numberOfThings * 30f;
		}

		// Token: 0x04006AA8 RID: 27304
		private int maxColonists;

		// Token: 0x04006AA9 RID: 27305
		private int maxAnimals;

		// Token: 0x04006AAA RID: 27306
		private int maxRelics;

		// Token: 0x04006AAB RID: 27307
		private int maxItems;

		// Token: 0x04006AAC RID: 27308
		private Action<List<Thing>> postAccepted;

		// Token: 0x04006AAD RID: 27309
		private List<Thing> colonists = new List<Thing>();

		// Token: 0x04006AAE RID: 27310
		private List<Thing> animals = new List<Thing>();

		// Token: 0x04006AAF RID: 27311
		private List<Thing> relics = new List<Thing>();

		// Token: 0x04006AB0 RID: 27312
		private List<Thing> items = new List<Thing>();

		// Token: 0x04006AB1 RID: 27313
		private Dictionary<Thing, int> itemArchonexusAllowedStackCount = new Dictionary<Thing, int>();

		// Token: 0x04006AB2 RID: 27314
		private HashSet<Thing> selected = new HashSet<Thing>();

		// Token: 0x04006AB3 RID: 27315
		private int selectedItemCount;

		// Token: 0x04006AB4 RID: 27316
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04006AB5 RID: 27317
		private Action cancel;

		// Token: 0x04006AB6 RID: 27318
		private const float TitleHeight = 40f;

		// Token: 0x04006AB7 RID: 27319
		private const float DescriptionHeight = 25f;

		// Token: 0x04006AB8 RID: 27320
		private const float BottomAreaHeight = 70f;

		// Token: 0x04006AB9 RID: 27321
		private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04006ABA RID: 27322
		private const float RowHeight = 30f;

		// Token: 0x04006ABB RID: 27323
		private const float CategoryHeaderHeight = 30f;

		// Token: 0x04006ABC RID: 27324
		private const float MiscIconSize = 27f;

		// Token: 0x04006ABD RID: 27325
		private const float CheckboxSize = 27f;

		// Token: 0x04006ABE RID: 27326
		private const float ItemCountSize = 60f;

		// Token: 0x04006ABF RID: 27327
		private const float ErrorAndWarningTextWidth = 160f;

		// Token: 0x04006AC0 RID: 27328
		private const float MiscIconOffset = 320f;

		// Token: 0x04006AC1 RID: 27329
		private const float NameOffset = 80f;

		// Token: 0x04006AC2 RID: 27330
		private const float ItemNameWidth = 250f;

		// Token: 0x04006AC3 RID: 27331
		private const float ThingNameWidth = 196f;

		// Token: 0x04006AC4 RID: 27332
		private readonly Comparer<ThingDef> itemComparator = Comparer<ThingDef>.Create((ThingDef td1, ThingDef td2) => TransferableComparer_Archonexus.Compare(td1, td2));
	}
}
