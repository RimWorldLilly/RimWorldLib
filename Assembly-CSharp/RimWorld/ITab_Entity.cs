using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6A RID: 7530
	public class ITab_Entity : ITab
	{
		// Token: 0x17002428 RID: 9256
		// (get) Token: 0x0600C323 RID: 49955 RVA: 0x004726DC File Offset: 0x004708DC
		public override bool IsVisible
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				Building_HoldingPlatform building_HoldingPlatform = base.SelThing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					return building_HoldingPlatform.HeldPawn != null;
				}
				return base.SelThing is Pawn && base.SelThing.IsOnHoldingPlatform;
			}
		}

		// Token: 0x17002429 RID: 9257
		// (get) Token: 0x0600C324 RID: 49956 RVA: 0x00472728 File Offset: 0x00470928
		protected override Pawn SelPawn
		{
			get
			{
				Pawn selPawn = base.SelPawn;
				if (selPawn != null)
				{
					return selPawn;
				}
				Building_HoldingPlatform building_HoldingPlatform = base.SelThing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					return building_HoldingPlatform.HeldPawn;
				}
				return null;
			}
		}

		// Token: 0x0600C325 RID: 49957 RVA: 0x00472758 File Offset: 0x00470958
		public ITab_Entity()
		{
			this.size = new Vector2(280f, 0f);
			this.labelKey = "TabEntity";
			this.tutorTag = "Entity";
		}

		// Token: 0x0600C326 RID: 49958 RVA: 0x0047278B File Offset: 0x0047098B
		public override void OnOpen()
		{
			if (!ModLister.CheckAnomaly("entity itab"))
			{
				this.CloseTab();
				return;
			}
			base.OnOpen();
		}

		// Token: 0x0600C327 RID: 49959 RVA: 0x004727A8 File Offset: 0x004709A8
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.maxOneColumn = true;
			listing_Standard.Begin(rect);
			Thing thing = this.SelPawn.ParentHolder as Thing;
			CompEntityHolder compEntityHolder;
			if (thing != null && thing.TryGetComp(out compEntityHolder))
			{
				StatDef containmentStrength = StatDefOf.ContainmentStrength;
				float containmentStrength2 = compEntityHolder.ContainmentStrength;
				string text = containmentStrength.description + "\n\n" + containmentStrength.Worker.GetExplanationFull(StatRequest.For(thing), containmentStrength.toStringNumberSense, containmentStrength2);
				Widgets.DrawHighlightIfMouseover(listing_Standard.Label(containmentStrength.LabelCap + ": " + containmentStrength.ValueToString(containmentStrength2, ToStringNumberSense.Absolute, true), -1f, new TipSignal?(new TipSignal(text, this.GetHashCode() + 1))));
			}
			StringBuilder stringBuilder = new StringBuilder();
			float num = ContainmentUtility.InitiateEscapeMtbDays(this.SelPawn, stringBuilder);
			int numTicks = Mathf.FloorToInt(num * 60000f);
			TaggedString taggedString = "HoldingPlatformEscapeMTBDays".Translate() + ": ";
			if (num < 0f)
			{
				taggedString += "Never".Translate();
			}
			else
			{
				taggedString += numTicks.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			string text2 = "HoldingPlatformEscapeMTBDaysDesc".Translate();
			if (stringBuilder.Length > 0)
			{
				string str = text2;
				string str2 = "\n\n";
				StringBuilder stringBuilder2 = stringBuilder;
				text2 = str + str2 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
			}
			Widgets.DrawHighlightIfMouseover(listing_Standard.Label(taggedString, -1f, new TipSignal?(new TipSignal(text2, this.GetHashCode() + 2))));
			CompStudiable compStudiable = this.SelPawn.TryGetComp<CompStudiable>();
			if (compStudiable != null)
			{
				ITab_Entity.DoStudyPeriodListing(listing_Standard, compStudiable);
			}
			if (compStudiable != null)
			{
				ITab_Entity.DoKnowledgeGainListing(listing_Standard, compStudiable);
			}
			listing_Standard.Gap(1f);
			Rect rect2 = listing_Standard.GetRect(24f, 1f).Rounded();
			TooltipHandler.TipRegionByKey(rect2, "MedicineQualityDescriptionEntity");
			Widgets.DrawHighlightIfMouseover(rect2);
			Rect rect3 = rect2;
			rect3.xMax = rect2.center.x - 4f;
			Rect rect4 = rect2;
			rect4.xMin = rect2.center.x + 4f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect5 = rect3;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AllowMedicine".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":");
			Widgets.Label(rect5, defaultInterpolatedStringHandler.ToStringAndClear());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawButtonGraphic(rect4);
			MedicalCareUtility.MedicalCareSelectButton(rect4, this.SelPawn);
			listing_Standard.Gap(4f);
			CompHoldingPlatformTarget compHoldingPlatformTarget = this.SelPawn.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null)
			{
				float height = 132f;
				Rect rect6 = listing_Standard.GetRect(height, 1f).Rounded();
				Widgets.DrawMenuSection(rect6);
				Rect rect7 = rect6.ContractedBy(10f);
				Widgets.BeginGroup(rect7);
				Rect rect8 = new Rect(0f, 0f, rect7.width, 28f);
				if (Widgets.RadioButtonLabeled(rect8, "EntityStudyMode_MaintainOnly".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.MaintainOnly, false))
				{
					compHoldingPlatformTarget.containmentMode = EntityContainmentMode.MaintainOnly;
				}
				Widgets.DrawHighlightIfMouseover(rect8);
				TooltipHandler.TipRegion(rect8, "EntityStudyMode_MaintainOnlyDesc".Translate());
				rect8.y += 28f;
				if (Widgets.RadioButtonLabeled(rect8, "EntityStudyMode_Study".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Study, false))
				{
					compHoldingPlatformTarget.containmentMode = EntityContainmentMode.Study;
				}
				Widgets.DrawHighlightIfMouseover(rect8);
				TooltipHandler.TipRegion(rect8, "EntityStudyMode_StudyDesc".Translate());
				rect8.y += 28f;
				if (Widgets.RadioButtonLabeled(rect8, "EntityStudyMode_Release".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release, false))
				{
					compHoldingPlatformTarget.containmentMode = EntityContainmentMode.Release;
				}
				Widgets.DrawHighlightIfMouseover(rect8);
				TooltipHandler.TipRegion(rect8, "EntityStudyMode_ReleaseDesc".Translate());
				rect8.y += 28f;
				if (Widgets.RadioButtonLabeled(rect8, "EntityStudyMode_Execute".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Execute, !compHoldingPlatformTarget.Props.canBeExecuted))
				{
					if (!compHoldingPlatformTarget.Props.canBeExecuted)
					{
						Messages.Message("CantBeExecuted".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						compHoldingPlatformTarget.containmentMode = EntityContainmentMode.Execute;
					}
				}
				Widgets.DrawHighlightIfMouseover(rect8);
				TooltipHandler.TipRegion(rect8, "EntityStudyMode_ExecuteDesc".Translate() + (compHoldingPlatformTarget.Props.canBeExecuted ? "" : ("\n\n" + "CantBeExecuted".Translate().ToString())));
				rect8.y += 28f;
				Widgets.EndGroup();
				listing_Standard.Gap(12f);
				height = 48f;
				Rect rect9 = listing_Standard.GetRect(height, 1f).Rounded();
				Widgets.DrawMenuSection(rect9);
				rect7 = rect9.ContractedBy(10f);
				Widgets.BeginGroup(rect7);
				string text3 = null;
				if (!ResearchProjectDefOf.BioferriteExtraction.IsFinished)
				{
					text3 = "RequiresBioferriteExtraction".Translate();
				}
				else
				{
					Building_HoldingPlatform heldPlatform = compHoldingPlatformTarget.HeldPlatform;
					if (heldPlatform != null && heldPlatform.HasAttachedBioferriteHarvester)
					{
						text3 = "BioferriteHarvesterAttached".Translate();
					}
				}
				Rect rect10 = new Rect(0f, 0f, rect7.width, 28f);
				Widgets.CheckboxLabeled(rect10, "EntityStudyMode_Extract".Translate(), ref compHoldingPlatformTarget.extractBioferrite, text3 != null, null, null, false, false);
				Widgets.DrawHighlightIfMouseover(rect10);
				TaggedString taggedString2 = "EntityStudyMode_ExtractDesc".Translate();
				if (text3 != null)
				{
					taggedString2 += "\n\n" + text3.Colorize(ColoredText.WarningColor);
				}
				TooltipHandler.TipRegion(rect10, taggedString2);
				Widgets.EndGroup();
			}
			listing_Standard.End();
			this.size = new Vector2(280f, listing_Standard.CurHeight + 10f + 24f);
		}

		// Token: 0x0600C328 RID: 49960 RVA: 0x00472DE8 File Offset: 0x00470FE8
		public static void DoStudyPeriodListing(Listing_Standard listing, CompStudiable studiable)
		{
			if (studiable.Props.frequencyTicks > 0)
			{
				Widgets.DrawHighlightIfMouseover(listing.Label("StudyInterval".Translate() + ": " + studiable.Props.frequencyTicks.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor), -1f, "StudyIntervalDesc".Translate()));
			}
		}

		// Token: 0x0600C329 RID: 49961 RVA: 0x00472E5C File Offset: 0x0047105C
		public static void DoKnowledgeGainListing(Listing_Standard listing, CompStudiable studiable)
		{
			string text = "StudyKnowledgeGainDesc".Translate();
			CompHoldingPlatformTarget compHoldingPlatformTarget = studiable.Pawn.TryGetComp<CompHoldingPlatformTarget>();
			string text2 = "";
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
			{
				CompEntityHolder comp = compHoldingPlatformTarget.HeldPlatform.GetComp<CompEntityHolder>();
				float studyKnowledgeAmountMultiplier = ContainmentUtility.GetStudyKnowledgeAmountMultiplier(studiable.Pawn, comp);
				string str = text2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 2);
				defaultInterpolatedStringHandler.AppendLiteral("  - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FactorContainmentStrength".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": x");
				defaultInterpolatedStringHandler.AppendFormatted<float>(studyKnowledgeAmountMultiplier, "F1");
				text2 = str + defaultInterpolatedStringHandler.ToStringAndClear();
				if (compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
				{
					string str2 = text2;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(8, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("\n  - ");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("FactorElectroharvester".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": x");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(0.5f, "F1");
					text2 = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
				}
			}
			CompActivity compActivity;
			if (studiable.Pawn.TryGetComp(out compActivity))
			{
				if (text2 != "")
				{
					text2 += "\n";
				}
				string str3 = text2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(7, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("  - ");
				defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("FactorActivity".Translate());
				defaultInterpolatedStringHandler3.AppendLiteral(": x");
				defaultInterpolatedStringHandler3.AppendFormatted<float>(compActivity.ActivityResearchFactor, "F1");
				text2 = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			Widgets.DrawHighlightIfMouseover(listing.Label("StudyKnowledgeGain".Translate() + ": " + (studiable.AdjustedAnomalyKnowledgePerStudy * 5f).ToStringDecimalIfSmall() + " (" + studiable.KnowledgeCategory.label + ")", -1f, text));
		}
	}
}
