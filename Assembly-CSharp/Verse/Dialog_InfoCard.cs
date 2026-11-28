using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000660 RID: 1632
	public class Dialog_InfoCard : Window
	{
		// Token: 0x060033B6 RID: 13238 RVA: 0x0014CB92 File Offset: 0x0014AD92
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<ThingDef> defs)
		{
			if (defs == null)
			{
				return null;
			}
			return from def in defs
			select new Dialog_InfoCard.Hyperlink(def, -1);
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x0014CBBE File Offset: 0x0014ADBE
		public static IEnumerable<Dialog_InfoCard.Hyperlink> DefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink(link.def, -1);
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x0014CBEA File Offset: 0x0014ADEA
		public static IEnumerable<Dialog_InfoCard.Hyperlink> TitleDefsToHyperlinks(IEnumerable<DefHyperlink> links)
		{
			if (links == null)
			{
				return null;
			}
			return from link in links
			select new Dialog_InfoCard.Hyperlink((RoyalTitleDef)link.def, link.faction, -1);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x0014CC18 File Offset: 0x0014AE18
		public static void PushCurrentToHistoryAndClose()
		{
			Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
			if (dialog_InfoCard == null)
			{
				return;
			}
			Dialog_InfoCard.history.Add(new Dialog_InfoCard.Hyperlink(dialog_InfoCard, StatsReportUtility.SelectedStatIndex));
			Find.WindowStack.TryRemove(dialog_InfoCard, false);
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x0014CC56 File Offset: 0x0014AE56
		private Def Def
		{
			get
			{
				if (this.thing != null)
				{
					return this.thing.def;
				}
				if (this.worldObject != null)
				{
					return this.worldObject.def;
				}
				return this.def;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060033BB RID: 13243 RVA: 0x0014CC86 File Offset: 0x0014AE86
		private Pawn ThingPawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x0014CC93 File Offset: 0x0014AE93
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(950f, 760f);
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x0014CCA4 File Offset: 0x0014AEA4
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x0014CCAB File Offset: 0x0014AEAB
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				if (this.tab != Dialog_InfoCard.InfoCardTab.Stats)
				{
					return null;
				}
				return StatsReportUtility.QuickSearchWidget;
			}
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x0014CCBC File Offset: 0x0014AEBC
		public Dialog_InfoCard(Thing thing, Precept_ThingStyle precept = null) : base(null)
		{
			this.thing = thing;
			this.precept = precept;
			this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			this.Setup();
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x0014CCE0 File Offset: 0x0014AEE0
		public Dialog_InfoCard(Def onlyDef, Precept_ThingStyle precept = null) : base(null)
		{
			this.def = onlyDef;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x0014CCFD File Offset: 0x0014AEFD
		public Dialog_InfoCard(ThingDef thingDef, ThingDef stuff, Precept_ThingStyle precept = null) : base(null)
		{
			this.def = thingDef;
			this.stuff = stuff;
			this.precept = precept;
			this.Setup();
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x0014CD21 File Offset: 0x0014AF21
		public Dialog_InfoCard(RoyalTitleDef titleDef, Faction faction, Pawn pawn = null) : base(null)
		{
			this.titleDef = titleDef;
			this.faction = faction;
			this.pawn = pawn;
			this.Setup();
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x0014CD45 File Offset: 0x0014AF45
		public Dialog_InfoCard(Hediff hediff) : base(null)
		{
			this.hediff = hediff;
			this.Setup();
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0014CD5B File Offset: 0x0014AF5B
		public Dialog_InfoCard(Faction faction) : base(null)
		{
			this.faction = faction;
			this.Setup();
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0014CD71 File Offset: 0x0014AF71
		public Dialog_InfoCard(WorldObject worldObject) : base(null)
		{
			this.worldObject = worldObject;
			this.Setup();
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x0014CD87 File Offset: 0x0014AF87
		public override void Notify_CommonSearchChanged()
		{
			StatsReportUtility.Notify_QuickSearchChanged();
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x0014CD8E File Offset: 0x0014AF8E
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Dialog_InfoCard.history.Clear();
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0014CDA4 File Offset: 0x0014AFA4
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.soundAppear = SoundDefOf.InfoCard_Open;
			this.soundClose = SoundDefOf.InfoCard_Close;
			StatsReportUtility.Reset();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InfoCard, KnowledgeAmount.Total);
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0014CDFA File Offset: 0x0014AFFA
		public void SetTab(Dialog_InfoCard.InfoCardTab infoCardTab)
		{
			this.tab = infoCardTab;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x0014CE04 File Offset: 0x0014B004
		private static bool ShowMaterialsButton(Rect containerRect, bool withBackButtonOffset = false)
		{
			float num = containerRect.x + containerRect.width - 14f - 200f - 16f;
			if (withBackButtonOffset)
			{
				num -= 136f;
			}
			return Widgets.ButtonText(new Rect(num, containerRect.y + 18f, 200f, 40f), "ShowMaterials".Translate(), true, true, true, null);
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x0014CE7C File Offset: 0x0014B07C
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect = rect.ContractedBy(18f);
			rect.height = 34f;
			rect.x += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.GetTitle());
			Rect rect2 = new Rect(inRect.x + 9f, rect.y, 34f, 34f);
			if (this.thing != null)
			{
				Widgets.ThingIcon(rect2, this.thing, 1f, null, false, 1f, false);
			}
			else
			{
				Widgets.DefIcon(rect2, this.def, this.stuff, 1f, null, true, null, null, null, 1f);
			}
			Rect rect3 = new Rect(inRect);
			rect3.yMin = rect.yMax;
			rect3.yMax -= 38f;
			Rect rect4 = rect3;
			List<TabRecord> list = new List<TabRecord>();
			TabRecord item = new TabRecord("TabStats".Translate(), delegate()
			{
				this.tab = Dialog_InfoCard.InfoCardTab.Stats;
			}, this.tab == Dialog_InfoCard.InfoCardTab.Stats);
			list.Add(item);
			if (this.ThingPawn != null)
			{
				if (this.ThingPawn.RaceProps.Humanlike)
				{
					TabRecord item2 = new TabRecord("TabCharacter".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Character;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Character);
					list.Add(item2);
				}
				TabRecord item3 = new TabRecord("TabHealth".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Health;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Health);
				list.Add(item3);
				if (ModsConfig.RoyaltyActive && this.ThingPawn.RaceProps.Humanlike && this.ThingPawn.Faction == Faction.OfPlayer && !this.ThingPawn.IsQuestLodger() && this.ThingPawn.royalty != null && PermitsCardUtility.selectedFaction != null)
				{
					TabRecord item4 = new TabRecord("TabPermits".Translate(), delegate()
					{
						this.tab = Dialog_InfoCard.InfoCardTab.Permits;
					}, this.tab == Dialog_InfoCard.InfoCardTab.Permits);
					list.Add(item4);
				}
				TabRecord item5 = new TabRecord("TabRecords".Translate(), delegate()
				{
					this.tab = Dialog_InfoCard.InfoCardTab.Records;
				}, this.tab == Dialog_InfoCard.InfoCardTab.Records);
				list.Add(item5);
			}
			if (list.Count > 1)
			{
				rect4.yMin += 45f;
				TabDrawer.DrawTabs<TabRecord>(rect4, list, 200f);
			}
			this.FillCard(rect4.ContractedBy(18f));
			if (this.def != null && this.def is BuildableDef)
			{
				IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor((BuildableDef)this.def, TechLevel.Undefined, false);
				if (enumerable.Count<ThingDef>() > 0 && Dialog_InfoCard.ShowMaterialsButton(inRect, Dialog_InfoCard.history.Count > 0))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (ThingDef thingDef in enumerable)
					{
						ThingDef localStuff = thingDef;
						list2.Add(new FloatMenuOption(thingDef.LabelCap, delegate()
						{
							this.stuff = localStuff;
							this.Setup();
						}, thingDef, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (Dialog_InfoCard.history.Count > 0 && Widgets.BackButtonFor(inRect))
			{
				Dialog_InfoCard.Hyperlink hyperlink = Dialog_InfoCard.history[Dialog_InfoCard.history.Count - 1];
				Dialog_InfoCard.history.RemoveAt(Dialog_InfoCard.history.Count - 1);
				Find.WindowStack.TryRemove(typeof(Dialog_InfoCard), false);
				hyperlink.ActivateHyperlink();
			}
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x0014D28C File Offset: 0x0014B48C
		protected void FillCard(Rect cardRect)
		{
			if (this.tab == Dialog_InfoCard.InfoCardTab.Stats)
			{
				if (this.thing != null)
				{
					Thing innerThing = this.thing;
					MinifiedThing minifiedThing = this.thing as MinifiedThing;
					if (minifiedThing != null)
					{
						innerThing = minifiedThing.InnerThing;
					}
					StatsReportUtility.DrawStatsReport(cardRect, innerThing);
				}
				else if (this.titleDef != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.titleDef, this.faction, this.pawn);
				}
				else if (this.hediff != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.hediff);
				}
				else if (this.faction != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.faction);
				}
				else if (this.worldObject != null)
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.worldObject);
				}
				else if (this.def is AbilityDef)
				{
					StatsReportUtility.DrawStatsReport(cardRect, (AbilityDef)this.def);
				}
				else
				{
					StatsReportUtility.DrawStatsReport(cardRect, this.def, this.stuff);
				}
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Character)
			{
				CharacterCardUtility.DrawCharacterCard(cardRect, (Pawn)this.thing, null, default(Rect), true);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Health)
			{
				HealthCardUtility.DrawPawnHealthCard(cardRect, (Pawn)this.thing, false, HealthCardUtility.ShowBloodLoss(this.thing), null);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Records)
			{
				RecordsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			else if (this.tab == Dialog_InfoCard.InfoCardTab.Permits)
			{
				PermitsCardUtility.DrawRecordsCard(cardRect, (Pawn)this.thing);
			}
			if (this.executeAfterFillCardOnce != null)
			{
				this.executeAfterFillCardOnce();
				this.executeAfterFillCardOnce = null;
			}
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x0014D420 File Offset: 0x0014B620
		private string GetTitle()
		{
			if (this.thing != null)
			{
				if (this.precept == null)
				{
					return this.thing.LabelCapNoCount;
				}
				return this.precept.LabelCap;
			}
			else
			{
				if (this.worldObject != null)
				{
					return this.worldObject.LabelCap;
				}
				if (this.hediff != null)
				{
					return this.hediff.def.LabelCap;
				}
				ThingDef thingDef = this.Def as ThingDef;
				if (thingDef != null)
				{
					if (this.precept == null)
					{
						return GenLabel.ThingLabel(thingDef, this.stuff, 1).CapitalizeFirst();
					}
					return this.precept.LabelCap;
				}
				else
				{
					AbilityDef abilityDef = this.Def as AbilityDef;
					if (abilityDef != null)
					{
						return abilityDef.LabelCap;
					}
					if (this.titleDef != null)
					{
						return this.titleDef.GetLabelCapForBothGenders();
					}
					if (this.faction != null)
					{
						return this.faction.Name;
					}
					if (this.precept == null)
					{
						return this.Def.LabelCap;
					}
					return this.precept.LabelCap;
				}
			}
		}

		// Token: 0x04002191 RID: 8593
		private const float ShowMaterialsButtonWidth = 200f;

		// Token: 0x04002192 RID: 8594
		private const float ShowMaterialsButtonHeight = 40f;

		// Token: 0x04002193 RID: 8595
		private const float ShowMaterialsMargin = 16f;

		// Token: 0x04002194 RID: 8596
		private Action executeAfterFillCardOnce;

		// Token: 0x04002195 RID: 8597
		private static List<Dialog_InfoCard.Hyperlink> history = new List<Dialog_InfoCard.Hyperlink>();

		// Token: 0x04002196 RID: 8598
		private Thing thing;

		// Token: 0x04002197 RID: 8599
		private ThingDef stuff;

		// Token: 0x04002198 RID: 8600
		private Precept_ThingStyle precept;

		// Token: 0x04002199 RID: 8601
		private Def def;

		// Token: 0x0400219A RID: 8602
		private WorldObject worldObject;

		// Token: 0x0400219B RID: 8603
		private RoyalTitleDef titleDef;

		// Token: 0x0400219C RID: 8604
		private Faction faction;

		// Token: 0x0400219D RID: 8605
		private Pawn pawn;

		// Token: 0x0400219E RID: 8606
		private Hediff hediff;

		// Token: 0x0400219F RID: 8607
		private Dialog_InfoCard.InfoCardTab tab;

		// Token: 0x020029F9 RID: 10745
		public enum InfoCardTab : byte
		{
			// Token: 0x0400AC2D RID: 44077
			Stats,
			// Token: 0x0400AC2E RID: 44078
			Character,
			// Token: 0x0400AC2F RID: 44079
			Health,
			// Token: 0x0400AC30 RID: 44080
			Records,
			// Token: 0x0400AC31 RID: 44081
			Permits
		}

		// Token: 0x020029FA RID: 10746
		public struct Hyperlink
		{
			// Token: 0x17002BB7 RID: 11191
			// (get) Token: 0x060104FE RID: 66814 RVA: 0x005C27D3 File Offset: 0x005C09D3
			public bool HasGeneOwnerThing
			{
				get
				{
					return this.thingIsGeneOwner && this.thing != null;
				}
			}

			// Token: 0x17002BB8 RID: 11192
			// (get) Token: 0x060104FF RID: 66815 RVA: 0x005C27E8 File Offset: 0x005C09E8
			public bool IsHidden
			{
				get
				{
					if (this.thing != null && Find.HiddenItemsManager.Hidden(this.thing.def))
					{
						return true;
					}
					ThingDef thingDef = this.def as ThingDef;
					return thingDef != null && Find.HiddenItemsManager.Hidden(thingDef);
				}
			}

			// Token: 0x17002BB9 RID: 11193
			// (get) Token: 0x06010500 RID: 66816 RVA: 0x005C2838 File Offset: 0x005C0A38
			public string Label
			{
				get
				{
					string result = null;
					if (this.IsHidden)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
						defaultInterpolatedStringHandler.AppendLiteral("(");
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NotYetDiscovered".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(")");
						result = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					else if (this.worldObject != null)
					{
						result = this.worldObject.Label;
					}
					else
					{
						if (this.def != null)
						{
							ThingDef thingDef = this.def as ThingDef;
							if (thingDef != null && this.stuff != null)
							{
								return thingDef.label;
							}
						}
						if (this.def != null)
						{
							result = this.def.label;
						}
						else if (this.thing != null && !this.thingIsGeneOwner)
						{
							result = this.thing.Label;
						}
						else if (this.titleDef != null)
						{
							result = this.titleDef.GetLabelCapForBothGenders();
						}
						else if (this.quest != null)
						{
							result = this.quest.name;
						}
						else if (this.ideo != null)
						{
							result = this.ideo.name;
						}
						else if (this.researchProject != null)
						{
							result = this.researchProject.label;
						}
						else if (this.faction != null)
						{
							result = this.faction.Name;
						}
						else if (this.HasGeneOwnerThing)
						{
							result = "InspectGenes".Translate();
						}
					}
					return result;
				}
			}

			// Token: 0x06010501 RID: 66817 RVA: 0x005C2994 File Offset: 0x005C0B94
			public Hyperlink(Dialog_InfoCard infoCard, int statIndex = -1)
			{
				this.def = infoCard.def;
				this.thing = infoCard.thing;
				this.stuff = infoCard.stuff;
				this.worldObject = infoCard.worldObject;
				this.titleDef = infoCard.titleDef;
				this.faction = infoCard.faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06010502 RID: 66818 RVA: 0x005C2A0C File Offset: 0x005C0C0C
			public Hyperlink(Def def, int statIndex = -1)
			{
				this.def = def;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06010503 RID: 66819 RVA: 0x005C2A68 File Offset: 0x005C0C68
			public Hyperlink(RoyalTitleDef titleDef, Faction faction, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = titleDef;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06010504 RID: 66820 RVA: 0x005C2AC4 File Offset: 0x005C0CC4
			public Hyperlink(Thing thing, int statIndex = -1, bool thingIsGeneOwner = false)
			{
				this.thing = thing;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = thingIsGeneOwner;
			}

			// Token: 0x06010505 RID: 66821 RVA: 0x005C2B20 File Offset: 0x005C0D20
			public Hyperlink(Faction faction, int statIndex = -1, bool thingIsGeneOwner = false)
			{
				this.thing = null;
				this.stuff = null;
				this.def = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = faction;
				this.selectedStatIndex = statIndex;
				this.quest = null;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = thingIsGeneOwner;
			}

			// Token: 0x06010506 RID: 66822 RVA: 0x005C2B7C File Offset: 0x005C0D7C
			public Hyperlink(Quest quest, int statIndex = -1)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = statIndex;
				this.quest = quest;
				this.ideo = null;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06010507 RID: 66823 RVA: 0x005C2BD8 File Offset: 0x005C0DD8
			public Hyperlink(Ideo ideo)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = 0;
				this.quest = null;
				this.ideo = ideo;
				this.researchProject = null;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06010508 RID: 66824 RVA: 0x005C2C34 File Offset: 0x005C0E34
			public Hyperlink(ResearchProjectDef researchProject)
			{
				this.def = null;
				this.thing = null;
				this.stuff = null;
				this.worldObject = null;
				this.titleDef = null;
				this.faction = null;
				this.selectedStatIndex = 0;
				this.quest = null;
				this.ideo = null;
				this.researchProject = researchProject;
				this.thingIsGeneOwner = false;
			}

			// Token: 0x06010509 RID: 66825 RVA: 0x005C2C90 File Offset: 0x005C0E90
			public void ActivateHyperlink()
			{
				if (this.IsHidden)
				{
					return;
				}
				if (this.ideo != null)
				{
					Dialog_InfoCard dialog_InfoCard = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard != null)
					{
						dialog_InfoCard.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
					IdeoUIUtility.SetSelected(this.ideo);
					return;
				}
				if (this.quest != null)
				{
					Dialog_InfoCard dialog_InfoCard2 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
					if (dialog_InfoCard2 != null)
					{
						dialog_InfoCard2.Close(true);
					}
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					return;
				}
				if (this.researchProject != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
					((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).Select(this.researchProject);
				}
				if (this.HasGeneOwnerThing)
				{
					if (ThingSelectionUtility.SelectableByMapClick(this.thing))
					{
						Find.Selector.Select(this.thing, true, true);
						InspectPaneUtility.OpenTab(typeof(ITab_Genes));
					}
					return;
				}
				Dialog_InfoCard dialog_InfoCard3 = null;
				if (this.def == null && this.thing == null && this.worldObject == null && this.titleDef == null && this.faction == null)
				{
					dialog_InfoCard3 = Find.WindowStack.WindowOfType<Dialog_InfoCard>();
				}
				else
				{
					Dialog_InfoCard.PushCurrentToHistoryAndClose();
					if (this.worldObject != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.worldObject);
					}
					else if (this.def != null && this.def is ThingDef && (this.stuff != null || GenStuff.DefaultStuffFor((ThingDef)this.def) != null))
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.def as ThingDef, this.stuff ?? GenStuff.DefaultStuffFor((ThingDef)this.def), null);
					}
					else if (this.def != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.def, null);
					}
					else if (this.thing != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.thing, null);
					}
					else if (this.titleDef != null)
					{
						dialog_InfoCard3 = new Dialog_InfoCard(this.titleDef, this.faction, null);
					}
					else if (this.faction != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
						((MainTabWindow_Factions)Find.MainTabsRoot.OpenTab.TabWindow).ScrollToFaction(this.faction);
						dialog_InfoCard3 = new Dialog_InfoCard(this.faction);
					}
				}
				if (dialog_InfoCard3 == null)
				{
					return;
				}
				int localSelectedStatIndex = this.selectedStatIndex;
				if (this.selectedStatIndex >= 0)
				{
					dialog_InfoCard3.executeAfterFillCardOnce = delegate()
					{
						StatsReportUtility.SelectEntry(localSelectedStatIndex);
					};
				}
				Find.WindowStack.Add(dialog_InfoCard3);
			}

			// Token: 0x0400AC32 RID: 44082
			public Thing thing;

			// Token: 0x0400AC33 RID: 44083
			public ThingDef stuff;

			// Token: 0x0400AC34 RID: 44084
			public Def def;

			// Token: 0x0400AC35 RID: 44085
			public WorldObject worldObject;

			// Token: 0x0400AC36 RID: 44086
			public RoyalTitleDef titleDef;

			// Token: 0x0400AC37 RID: 44087
			public Faction faction;

			// Token: 0x0400AC38 RID: 44088
			public Quest quest;

			// Token: 0x0400AC39 RID: 44089
			public Ideo ideo;

			// Token: 0x0400AC3A RID: 44090
			public ResearchProjectDef researchProject;

			// Token: 0x0400AC3B RID: 44091
			public int selectedStatIndex;

			// Token: 0x0400AC3C RID: 44092
			public bool thingIsGeneOwner;
		}
	}
}
