using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200239A RID: 9114
	public static class CaravanUIUtility
	{
		// Token: 0x0600E2C4 RID: 58052 RVA: 0x005435E0 File Offset: 0x005417E0
		public static void CreateCaravanTransferableWidgets(List<TransferableOneWay> transferables, out TransferableOneWayWidget pawnsTransfer, out TransferableOneWayWidget itemsTransfer, out TransferableOneWayWidget travelSuppliesTransfer, string thingCountTip, IgnorePawnsInventoryMode ignorePawnInventoryMass, Func<float> availableMassGetter, bool ignoreSpawnedCorpsesGearAndInventoryMass, PlanetTile tile, bool playerPawnsReadOnly = false)
		{
			pawnsTransfer = new TransferableOneWayWidget(null, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, new PlanetTile?(tile), true, true, true, true, false, true, false, playerPawnsReadOnly, false, false);
			CaravanUIUtility.AddPawnsSections(pawnsTransfer, transferables);
			itemsTransfer = new TransferableOneWayWidget(from x in transferables
			where CaravanUIUtility.GetTransferableCategory(x) == CaravanUIUtility.TransferableCategory.Item
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, new PlanetTile?(tile), true, false, false, false, true, false, true, false, false, false);
			travelSuppliesTransfer = new TransferableOneWayWidget(from x in transferables
			where CaravanUIUtility.GetTransferableCategory(x) == CaravanUIUtility.TransferableCategory.TravelSupplies
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, new PlanetTile?(tile), true, false, false, false, true, false, true, false, false, false);
		}

		// Token: 0x0600E2C5 RID: 58053 RVA: 0x005436BC File Offset: 0x005418BC
		private static CaravanUIUtility.TransferableCategory GetTransferableCategory(Transferable t)
		{
			if (t.ThingDef.category == ThingCategory.Pawn)
			{
				return CaravanUIUtility.TransferableCategory.Pawn;
			}
			if ((!t.ThingDef.thingCategories.NullOrEmpty<ThingCategoryDef>() && t.ThingDef.thingCategories.Contains(ThingCategoryDefOf.Medicine)) || (t.ThingDef.IsIngestible && !t.ThingDef.IsDrug && !t.ThingDef.IsCorpse && (t.ThingDef.plant == null || !t.ThingDef.plant.IsTree)) || (t.AnyThing.GetInnerIfMinified().def.IsBed && t.AnyThing.GetInnerIfMinified().def.building.bed_caravansCanUse))
			{
				return CaravanUIUtility.TransferableCategory.TravelSupplies;
			}
			return CaravanUIUtility.TransferableCategory.Item;
		}

		// Token: 0x0600E2C6 RID: 58054 RVA: 0x00543780 File Offset: 0x00541980
		public static void AddPawnsSections(TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
		{
			IEnumerable<TransferableOneWay> source = from x in transferables
			where x.ThingDef.category == ThingCategory.Pawn
			select x;
			widget.AddSection("ColonistsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsFreeNonSlaveColonist
			select x);
			if (ModsConfig.IdeologyActive)
			{
				widget.AddSection("SlavesSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsSlave
				select x);
			}
			widget.AddSection("PrisonersSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsPrisoner
			select x);
			widget.AddSection("CaptureSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).Downed && CaravanUtility.ShouldAutoCapture((Pawn)x.AnyThing, Faction.OfPlayer)
			select x);
			widget.AddSection("AnimalsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsAnimal
			select x);
			if (ModsConfig.BiotechActive)
			{
				widget.AddSection("MechsSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsColonyMech && ((Pawn)x.AnyThing).OverseerSubject != null && ((Pawn)x.AnyThing).OverseerSubject.State == OverseerSubjectState.Overseen
				select x);
			}
			if (ModsConfig.AnomalyActive)
			{
				widget.AddSection("EntitiesSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsColonySubhuman && ((Pawn)x.AnyThing).mutant.Def.canTravelInCaravan
				select x);
			}
		}

		// Token: 0x0600E2C7 RID: 58055 RVA: 0x00543960 File Offset: 0x00541B60
		private static string GetDaysWorthOfFoodLabel([TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})] ValueTuple<float, float> daysWorthOfFood, bool multiline)
		{
			if (daysWorthOfFood.Item1 >= 600f)
			{
				return "InfiniteDaysWorthOfFoodInfo".Translate();
			}
			string text = daysWorthOfFood.Item1.ToString("0.#");
			string str = multiline ? "\n" : " ";
			if (daysWorthOfFood.Item2 < 600f && daysWorthOfFood.Item2 < daysWorthOfFood.Item1)
			{
				string t = text;
				string t2 = str + "(";
				string key = "DaysWorthOfFoodInfoRot";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendFormatted<float>(daysWorthOfFood.Item2, "0.#");
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = t + (t2 + key.Translate(defaultInterpolatedStringHandler.ToStringAndClear()));
			}
			return text;
		}

		// Token: 0x0600E2C8 RID: 58056 RVA: 0x00543A24 File Offset: 0x00541C24
		private static Color GetDaysWorthOfFoodColor([TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})] ValueTuple<float, float> daysWorthOfFood, int? ticksToArrive)
		{
			if (daysWorthOfFood.Item1 >= 600f)
			{
				return Color.white;
			}
			float num = Mathf.Min(daysWorthOfFood.Item1, daysWorthOfFood.Item2);
			if (ticksToArrive != null)
			{
				return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodKnownRouteColor, num / ((float)ticksToArrive.Value / 60000f));
			}
			return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodColor, num);
		}

		// Token: 0x0600E2C9 RID: 58057 RVA: 0x00543A88 File Offset: 0x00541C88
		private static string GetMovementSpeedLabel(float tilesPerDay, bool immobile, bool shortVersion)
		{
			if (immobile)
			{
				return shortVersion ? "TilesPerDayImmobileShort".Translate() : "TilesPerDayImmobile".Translate();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<float>(tilesPerDay, "0.#");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			return defaultInterpolatedStringHandler.ToStringAndClear() + "TilesPerDay".Translate();
		}

		// Token: 0x0600E2CA RID: 58058 RVA: 0x00543AF4 File Offset: 0x00541CF4
		private static Color GetMovementSpeedColor(float tilesPerDay, bool immobile)
		{
			if (!immobile)
			{
				return GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, tilesPerDay);
			}
			return Color.red;
		}

		// Token: 0x0600E2CB RID: 58059 RVA: 0x00543B0C File Offset: 0x00541D0C
		public static void DrawCaravanInfo(CaravanUIUtility.CaravanInfo info, CaravanUIUtility.CaravanInfo? info2, PlanetTile currentTile, int? ticksToArrive, float lastMassFlashTime, Rect rect, bool lerpMassColor = true, string extraDaysWorthOfFoodTipInfo = null, bool multiline = false, bool isCaravan = true)
		{
			CaravanUIUtility.tmpInfo.Clear();
			isCaravan = (isCaravan && currentTile.Valid && currentTile.LayerDef.SurfaceTiles);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendFormatted(info.massUsage.ToStringEnsureThreshold(info.massCapacity, 0));
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(info.massCapacity, "F0");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			TaggedString taggedString = defaultInterpolatedStringHandler.ToStringAndClear() + "kg".Translate();
			TaggedString taggedString2;
			if (info2 == null)
			{
				taggedString2 = null;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler2.AppendFormatted(info2.Value.massUsage.ToStringEnsureThreshold(info2.Value.massCapacity, 0));
				defaultInterpolatedStringHandler2.AppendLiteral(" / ");
				defaultInterpolatedStringHandler2.AppendFormatted<float>(info2.Value.massCapacity, "F0");
				defaultInterpolatedStringHandler2.AppendLiteral(" ");
				taggedString2 = defaultInterpolatedStringHandler2.ToStringAndClear() + "kg".Translate();
			}
			TaggedString taggedString3 = taggedString2;
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Mass".Translate(), taggedString, CaravanUIUtility.GetMassColor(info.massUsage, info.massCapacity, lerpMassColor), CaravanUIUtility.GetMassTip(info.massUsage, info.massCapacity, info.massCapacityExplanation, (info2 != null) ? new float?(info2.GetValueOrDefault().massUsage) : null, (info2 != null) ? new float?(info2.GetValueOrDefault().massCapacity) : null, (info2 != null) ? info2.GetValueOrDefault().massCapacityExplanation : null, isCaravan), taggedString3, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.massUsage, info2.Value.massCapacity, lerpMassColor) : Color.white, lastMassFlashTime));
			if (!Mathf.Approximately(info.extraMassUsage, -1f) && isCaravan)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(4, 2);
				defaultInterpolatedStringHandler3.AppendFormatted(info.extraMassUsage.ToStringEnsureThreshold(info.extraMassCapacity, 0));
				defaultInterpolatedStringHandler3.AppendLiteral(" / ");
				defaultInterpolatedStringHandler3.AppendFormatted<float>(info.extraMassCapacity, "F0");
				defaultInterpolatedStringHandler3.AppendLiteral(" ");
				TaggedString taggedString4 = defaultInterpolatedStringHandler3.ToStringAndClear() + "kg".Translate();
				TaggedString taggedString5;
				if (info2 == null)
				{
					taggedString5 = null;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler4.AppendFormatted(info2.Value.extraMassUsage.ToStringEnsureThreshold(info2.Value.extraMassCapacity, 0));
					defaultInterpolatedStringHandler4.AppendLiteral(" / ");
					defaultInterpolatedStringHandler4.AppendFormatted<float>(info2.Value.extraMassCapacity, "F0");
					defaultInterpolatedStringHandler4.AppendLiteral(" ");
					taggedString5 = defaultInterpolatedStringHandler4.ToStringAndClear() + "kg".Translate();
				}
				TaggedString taggedString6 = taggedString5;
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMass".Translate(), taggedString4, CaravanUIUtility.GetMassColor(info.extraMassUsage, info.extraMassCapacity, true), CaravanUIUtility.GetMassTip(info.extraMassUsage, info.extraMassCapacity, info.extraMassCapacityExplanation, (info2 != null) ? new float?(info2.GetValueOrDefault().extraMassUsage) : null, (info2 != null) ? new float?(info2.GetValueOrDefault().extraMassCapacity) : null, (info2 != null) ? info2.GetValueOrDefault().extraMassCapacityExplanation : null, isCaravan), taggedString6, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.extraMassUsage, info2.Value.extraMassCapacity, true) : Color.white, -9999f));
			}
			if (isCaravan)
			{
				string text = "CaravanMovementSpeedTip".Translate();
				if (!info.tilesPerDayExplanation.NullOrEmpty())
				{
					text = text + "\n\n" + info.tilesPerDayExplanation;
				}
				if (info2 != null && !info2.Value.tilesPerDayExplanation.NullOrEmpty())
				{
					text = text + "\n\n-----\n\n" + info2.Value.tilesPerDayExplanation;
				}
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMovementSpeed".Translate(), CaravanUIUtility.GetMovementSpeedLabel(info.tilesPerDay, info.massUsage > info.massCapacity, info2 != null), CaravanUIUtility.GetMovementSpeedColor(info.tilesPerDay, info.massUsage > info.massCapacity), text, (info2 != null) ? CaravanUIUtility.GetMovementSpeedLabel(info2.Value.tilesPerDay, info2.Value.massUsage > info2.Value.massCapacity, true) : null, (info2 != null) ? CaravanUIUtility.GetMovementSpeedColor(info2.Value.tilesPerDay, info2.Value.massUsage > info2.Value.massCapacity) : Color.white, -9999f));
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("DaysWorthOfFood".Translate(), CaravanUIUtility.GetDaysWorthOfFoodLabel(info.daysWorthOfFood, multiline), CaravanUIUtility.GetDaysWorthOfFoodColor(info.daysWorthOfFood, ticksToArrive), "DaysWorthOfFoodTooltip".Translate() + extraDaysWorthOfFoodTipInfo + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(currentTile, Find.TickManager.TicksAbs), (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodLabel(info2.Value.daysWorthOfFood, multiline) : null, (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodColor(info2.Value.daysWorthOfFood, ticksToArrive) : Color.white, -9999f));
			string text2 = info.foragedFoodPerDay.Item2.ToString("0.#");
			string text3 = (info2 != null) ? info2.GetValueOrDefault().foragedFoodPerDay.Item2.ToString("0.#") : null;
			TaggedString taggedString7 = "ForagedFoodPerDayTip".Translate();
			taggedString7 += "\n\n" + info.foragedFoodPerDayExplanation;
			if (info2 != null)
			{
				taggedString7 += "\n\n-----\n\n" + info2.Value.foragedFoodPerDayExplanation;
			}
			if (info.foragedFoodPerDay.Item2 > 0f || (info2 != null && info2.Value.foragedFoodPerDay.Item2 > 0f))
			{
				string text4 = multiline ? "\n" : " ";
				if (info2 == null)
				{
					text2 = string.Concat(new string[]
					{
						text2,
						text4,
						"(",
						info.foragedFoodPerDay.Item1.label,
						")"
					});
				}
				else
				{
					text3 = string.Concat(new string[]
					{
						text3,
						text4,
						"(",
						info2.Value.foragedFoodPerDay.Item1.label.Truncate(50f, null),
						")"
					});
				}
			}
			if (isCaravan)
			{
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("ForagedFoodPerDay".Translate(), text2, Color.white, taggedString7, text3, Color.white, -9999f));
				string text5 = "CaravanVisibilityTip".Translate();
				if (!info.visibilityExplanation.NullOrEmpty())
				{
					text5 = text5 + "\n\n" + info.visibilityExplanation;
				}
				if (info2 != null && !info2.Value.visibilityExplanation.NullOrEmpty())
				{
					text5 = text5 + "\n\n-----\n\n" + info2.Value.visibilityExplanation;
				}
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Visibility".Translate(), info.visibility.ToStringPercent(), GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info.visibility), text5, (info2 != null) ? info2.GetValueOrDefault().visibility.ToStringPercent() : null, (info2 != null) ? GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info2.Value.visibility) : Color.white, -9999f));
			}
			TransferableUIUtility.DrawExtraInfo(CaravanUIUtility.tmpInfo, rect);
		}

		// Token: 0x0600E2CC RID: 58060 RVA: 0x00544396 File Offset: 0x00542596
		private static Color GetMassColor(float massUsage, float massCapacity, bool lerpMassColor)
		{
			if (massCapacity == 0f)
			{
				return Color.white;
			}
			if (massUsage > massCapacity)
			{
				return Color.red;
			}
			if (lerpMassColor)
			{
				return GenUI.LerpColor(CaravanUIUtility.MassColor, massUsage / massCapacity);
			}
			return Color.white;
		}

		// Token: 0x0600E2CD RID: 58061 RVA: 0x005443C8 File Offset: 0x005425C8
		private static string GetMassTip(float massUsage, float massCapacity, string massCapacityExplanation, float? massUsage2, float? massCapacity2, string massCapacity2Explanation, bool isCaravan)
		{
			TaggedString taggedString = "MassCarriedSimple".Translate() + ": " + massUsage.ToStringEnsureThreshold(massCapacity, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity.ToString("F2") + " " + "kg".Translate();
			if (massUsage2 != null)
			{
				taggedString += "\n <-> \n" + "MassCarriedSimple".Translate() + ": " + massUsage2.Value.ToStringEnsureThreshold(massCapacity2.Value, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity2.Value.ToString("F2") + " " + "kg".Translate();
			}
			if (isCaravan)
			{
				taggedString += "\n\n" + "CaravanMassUsageTooltip".Translate();
			}
			if (!massCapacityExplanation.NullOrEmpty())
			{
				taggedString += "\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacityExplanation;
			}
			if (!massCapacity2Explanation.NullOrEmpty())
			{
				taggedString += "\n\n-----\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacity2Explanation;
			}
			return taggedString;
		}

		// Token: 0x040090B5 RID: 37045
		private static readonly List<Pair<float, Color>> MassColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.37f, Color.green),
			new Pair<float, Color>(0.82f, Color.yellow),
			new Pair<float, Color>(1f, new Color(1f, 0.6f, 0f))
		};

		// Token: 0x040090B6 RID: 37046
		private static readonly List<Pair<float, Color>> TilesPerDayColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.001f, ColorLibrary.RedReadable),
			new Pair<float, Color>(1f, Color.yellow),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x040090B7 RID: 37047
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(1f, Color.red),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x040090B8 RID: 37048
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodKnownRouteColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.3f, ColorLibrary.RedReadable),
			new Pair<float, Color>(0.9f, Color.yellow),
			new Pair<float, Color>(1.02f, Color.green)
		};

		// Token: 0x040090B9 RID: 37049
		private static readonly List<Pair<float, Color>> VisibilityColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.01f, Color.green),
			new Pair<float, Color>(0.2f, Color.green),
			new Pair<float, Color>(1f, Color.white),
			new Pair<float, Color>(1.2f, ColorLibrary.RedReadable)
		};

		// Token: 0x040090BA RID: 37050
		private static readonly List<TransferableUIUtility.ExtraInfo> tmpInfo = new List<TransferableUIUtility.ExtraInfo>();

		// Token: 0x02003E86 RID: 16006
		private enum TransferableCategory
		{
			// Token: 0x0400F670 RID: 63088
			Pawn,
			// Token: 0x0400F671 RID: 63089
			Item,
			// Token: 0x0400F672 RID: 63090
			TravelSupplies
		}

		// Token: 0x02003E87 RID: 16007
		public struct CaravanInfo
		{
			// Token: 0x0601622B RID: 90667 RVA: 0x006E51C4 File Offset: 0x006E33C4
			public CaravanInfo(float massUsage, float massCapacity, string massCapacityExplanation, float tilesPerDay, string tilesPerDayExplanation, [TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})] ValueTuple<float, float> daysWorthOfFood, [TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})] ValueTuple<ThingDef, float> foragedFoodPerDay, string foragedFoodPerDayExplanation, float visibility, string visibilityExplanation, float extraMassUsage = -1f, float extraMassCapacity = -1f, string extraMassCapacityExplanation = null)
			{
				this.massUsage = massUsage;
				this.massCapacity = massCapacity;
				this.massCapacityExplanation = massCapacityExplanation;
				this.tilesPerDay = tilesPerDay;
				this.tilesPerDayExplanation = tilesPerDayExplanation;
				this.daysWorthOfFood = daysWorthOfFood;
				this.foragedFoodPerDay = foragedFoodPerDay;
				this.foragedFoodPerDayExplanation = foragedFoodPerDayExplanation;
				this.visibility = visibility;
				this.visibilityExplanation = visibilityExplanation;
				this.extraMassUsage = extraMassUsage;
				this.extraMassCapacity = extraMassCapacity;
				this.extraMassCapacityExplanation = extraMassCapacityExplanation;
			}

			// Token: 0x0400F673 RID: 63091
			public float massUsage;

			// Token: 0x0400F674 RID: 63092
			public float massCapacity;

			// Token: 0x0400F675 RID: 63093
			public string massCapacityExplanation;

			// Token: 0x0400F676 RID: 63094
			public float tilesPerDay;

			// Token: 0x0400F677 RID: 63095
			public string tilesPerDayExplanation;

			// Token: 0x0400F678 RID: 63096
			[TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			public ValueTuple<float, float> daysWorthOfFood;

			// Token: 0x0400F679 RID: 63097
			[TupleElementNames(new string[]
			{
				"food",
				"perDay"
			})]
			public ValueTuple<ThingDef, float> foragedFoodPerDay;

			// Token: 0x0400F67A RID: 63098
			public string foragedFoodPerDayExplanation;

			// Token: 0x0400F67B RID: 63099
			public float visibility;

			// Token: 0x0400F67C RID: 63100
			public string visibilityExplanation;

			// Token: 0x0400F67D RID: 63101
			public float extraMassUsage;

			// Token: 0x0400F67E RID: 63102
			public float extraMassCapacity;

			// Token: 0x0400F67F RID: 63103
			public string extraMassCapacityExplanation;
		}
	}
}
