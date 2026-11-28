using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D70 RID: 7536
	public class ITab_Pawn_Feeding : ITab
	{
		// Token: 0x17002433 RID: 9267
		// (get) Token: 0x0600C347 RID: 49991 RVA: 0x00473D70 File Offset: 0x00471F70
		public override bool IsVisible
		{
			get
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return ChildcareUtility.CanSuckle(this.SelPawn, out breastfeedFailReason) && ChildcareUtility.HasBreastfeedCompatibleFactions(Faction.OfPlayer, this.SelPawn);
			}
		}

		// Token: 0x17002434 RID: 9268
		// (get) Token: 0x0600C348 RID: 49992 RVA: 0x00473DA0 File Offset: 0x00471FA0
		public static List<ThingDef> BabyConsumableFoods
		{
			get
			{
				if (ITab_Pawn_Feeding.babyConsumableFoods.Count == 0)
				{
					ITab_Pawn_Feeding.babyConsumableFoods.AddRange(from t in DefDatabase<ThingDef>.AllDefs
					where t.ingestible != null && t.ingestible.babiesCanIngest
					select t);
				}
				return ITab_Pawn_Feeding.babyConsumableFoods;
			}
		}

		// Token: 0x0600C349 RID: 49993 RVA: 0x00473DF4 File Offset: 0x00471FF4
		public ITab_Pawn_Feeding()
		{
			this.size = new Vector2(500f, 500f);
			this.labelKey = "TabFeeding";
			this.tutorTag = "Feeding";
		}

		// Token: 0x0600C34A RID: 49994 RVA: 0x00473E48 File Offset: 0x00472048
		public static void FillTab(Pawn baby, Rect rect, ref Vector2 scrollPositionAutoBreastfeed, ref Vector2 scrollPositionBabyConsumables, List<Pawn> allowedPawns = null)
		{
			RectDivider rect2 = new RectDivider(rect.ContractedBy(10f, 10f), 2074307015, null);
			RectDivider rectDivider = rect2.NewRow(rect2.Rect.height * 0.66f, VerticalJustification.Top, null);
			Widgets.ListSeparator(ref rectDivider, "AutofeedSectionHeader".Translate().CapitalizeFirst());
			List<Pawn> list = ITab_Pawn_Feeding.tmpAllowedFeedingPawns;
			list.Clear();
			list.AddRange(PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction);
			list.Remove(baby);
			list.RemoveAll(delegate(Pawn feeder)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				int num;
				return !feeder.RaceProps.Humanlike || ChildcareUtility.CanSuckle(feeder, out breastfeedFailReason) || feeder.IsWorkTypeDisabledByAge(WorkTypeDefOf.Childcare, out num) || (allowedPawns != null && !allowedPawns.Contains(feeder));
			});
			Pawn surrogate = baby.GetBirthParent();
			Pawn mother = baby.GetMother();
			Pawn father = baby.GetFather();
			list.Sort(delegate(Pawn lhs, Pawn rhs)
			{
				int num = rhs.health.hediffSet.HasHediff(HediffDefOf.Lactating, false).CompareTo(lhs.health.hediffSet.HasHediff(HediffDefOf.Lactating, false));
				if (num != 0)
				{
					return num;
				}
				if (lhs == mother)
				{
					return -1;
				}
				if (rhs == mother)
				{
					return 1;
				}
				if (lhs == father)
				{
					return -1;
				}
				if (rhs == father)
				{
					return 1;
				}
				if (lhs == surrogate)
				{
					return -1;
				}
				if (rhs == surrogate)
				{
					return 1;
				}
				return 0;
			});
			if (list.Count == 0)
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rectDivider.Rect, "AutofeedNone".Translate());
					goto IL_1D3;
				}
			}
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				RectDivider rect3 = rectDivider.CreateViewRect(list.Count, 28f);
				Widgets.BeginScrollView(rectDivider.Rect, ref scrollPositionAutoBreastfeed, rect3, true);
				foreach (Pawn feeder2 in list)
				{
					RectDivider rectDivider2 = rect3.NewRow(28f, VerticalJustification.Top, null);
					ITab_Pawn_Feeding.DrawRow(baby, ref rectDivider2, feeder2, mother, father, surrogate);
				}
				Widgets.EndScrollView();
			}
			IL_1D3:
			Widgets.ListSeparator(ref rect2, "BabyFoodConsumables".Translate().CapitalizeFirst());
			List<ThingDef> list2 = ITab_Pawn_Feeding.BabyConsumableFoods;
			RectDivider rect4 = rect2.CreateViewRect(list2.Count, Text.LineHeight);
			Widgets.BeginScrollView(rect2, ref scrollPositionBabyConsumables, rect4, true);
			foreach (ThingDef thingDef in list2)
			{
				RectDivider rect5 = rect4.NewRow(Text.LineHeight, VerticalJustification.Top, null);
				Widgets.DefIcon(rect5.NewCol(Text.LineHeight, HorizontalJustification.Left, null), thingDef, null, 1f, null, false, null, null, null, 1f);
				Rect rect6 = rect5.NewCol(24f, HorizontalJustification.Right, null);
				Widgets.InfoCardButton(rect6.x, rect6.y, thingDef);
				if (baby.foodRestriction != null)
				{
					Rect rect7 = rect5.NewCol(24f, HorizontalJustification.Right, null);
					bool allowed = baby.foodRestriction.BabyFoodAllowed(thingDef);
					Widgets.Checkbox(rect7.x + 12f, rect7.y, ref allowed, 24f, false, true, null, null);
					baby.foodRestriction.SetBabyFoodAllowed(thingDef, allowed);
				}
				Widgets.Label(rect5, thingDef.LabelCap);
				if (Mouse.IsOver(rect5))
				{
					Widgets.DrawHighlight(rect5);
					Rect rect8 = rect5;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(thingDef.LabelCap);
					defaultInterpolatedStringHandler.AppendLiteral("\n\n");
					defaultInterpolatedStringHandler.AppendFormatted(thingDef.description);
					TooltipHandler.TipRegion(rect8, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600C34B RID: 49995 RVA: 0x00474284 File Offset: 0x00472484
		protected override void FillTab()
		{
			using (TextBlock.Default())
			{
				ITab_Pawn_Feeding.FillTab(this.SelPawn, new Rect(Vector2.zero, this.size), ref this.scrollPositionAutoBreastfeed, ref this.scrollPositionBabyConsumables, null);
			}
		}

		// Token: 0x0600C34C RID: 49996 RVA: 0x004742E0 File Offset: 0x004724E0
		private static FloatMenuOption GenerateFloatMenuOption(AutofeedMode setting, ITab_Pawn_Feeding.BabyFeederPair pair)
		{
			FloatMenuOption floatMenuOption = new FloatMenuOption(setting.Translate().CapitalizeFirst(), delegate()
			{
				pair.baby.mindState.SetAutofeeder(pair.feeder, setting);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			AutofeedMode setting2 = setting;
			Pawn feeder = pair.feeder;
			floatMenuOption.tooltip = new TipSignal?(setting2.GetTooltip(pair.baby, feeder));
			return floatMenuOption;
		}

		// Token: 0x0600C34D RID: 49997 RVA: 0x00474369 File Offset: 0x00472569
		private static IEnumerable<Widgets.DropdownMenuElement<AutofeedMode>> MenuGenerator(ITab_Pawn_Feeding.BabyFeederPair pair)
		{
			ITab_Pawn_Feeding.<MenuGenerator>d__17 <MenuGenerator>d__ = new ITab_Pawn_Feeding.<MenuGenerator>d__17(-2);
			<MenuGenerator>d__.<>3__pair = pair;
			return <MenuGenerator>d__;
		}

		// Token: 0x0600C34E RID: 49998 RVA: 0x0047437C File Offset: 0x0047257C
		private static void DrawRow(Pawn baby, ref RectDivider row, Pawn feeder, Pawn mother, Pawn father, Pawn surrogate)
		{
			AutofeedMode mode = baby.mindState.AutofeedSetting(feeder);
			string text = feeder.LabelShortCap;
			Hediff firstHediffOfDef = feeder.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
			if (firstHediffOfDef != null)
			{
				text = text + " (" + firstHediffOfDef.LabelBaseCap + ")";
			}
			Widgets.Label(row.NewCol(220f, HorizontalJustification.Left, null), text);
			Rect rect = row.NewCol(120f, HorizontalJustification.Left, null);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, mode.GetTooltip(baby, feeder));
			}
			ITab_Pawn_Feeding.BabyFeederPair target = new ITab_Pawn_Feeding.BabyFeederPair
			{
				baby = baby,
				feeder = feeder
			};
			Widgets.Dropdown<ITab_Pawn_Feeding.BabyFeederPair, AutofeedMode>(rect, target, (ITab_Pawn_Feeding.BabyFeederPair _target) => _target.baby.mindState.AutofeedSetting(_target.feeder), new Func<ITab_Pawn_Feeding.BabyFeederPair, IEnumerable<Widgets.DropdownMenuElement<AutofeedMode>>>(ITab_Pawn_Feeding.MenuGenerator), mode.Translate().CapitalizeFirst(), null, null, null, null, true);
			if (feeder == mother)
			{
				Widgets.Label(row, PawnRelationDefOf.Parent.labelFemale.CapitalizeFirst());
				return;
			}
			if (feeder == father)
			{
				Widgets.Label(row, PawnRelationDefOf.Parent.label.CapitalizeFirst());
				return;
			}
			if (feeder == surrogate)
			{
				Widgets.Label(row, PawnRelationDefOf.ParentBirth.GetGenderSpecificLabelCap(feeder));
			}
		}

		// Token: 0x04006F91 RID: 28561
		private const float FeederNameWidthPx = 220f;

		// Token: 0x04006F92 RID: 28562
		private const float BreastfeedOptWidthPx = 120f;

		// Token: 0x04006F93 RID: 28563
		private const int contextHash = 2074307015;

		// Token: 0x04006F94 RID: 28564
		public const float TabWidth = 500f;

		// Token: 0x04006F95 RID: 28565
		private Vector2 scrollPositionAutoBreastfeed = Vector2.zero;

		// Token: 0x04006F96 RID: 28566
		private Vector2 scrollPositionBabyConsumables = Vector2.zero;

		// Token: 0x04006F97 RID: 28567
		private static List<ThingDef> babyConsumableFoods = new List<ThingDef>();

		// Token: 0x04006F98 RID: 28568
		private static List<Pawn> tmpAllowedFeedingPawns = new List<Pawn>(8);

		// Token: 0x02003AC3 RID: 15043
		private struct BabyFeederPair
		{
			// Token: 0x0400E97C RID: 59772
			public Pawn baby;

			// Token: 0x0400E97D RID: 59773
			public Pawn feeder;
		}
	}
}
