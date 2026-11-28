using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6B RID: 7531
	public class ITab_Fishing : ITab
	{
		// Token: 0x1700242A RID: 9258
		// (get) Token: 0x0600C32A RID: 49962 RVA: 0x00473052 File Offset: 0x00471252
		public Zone_Fishing SelZone
		{
			get
			{
				return base.SelObject as Zone_Fishing;
			}
		}

		// Token: 0x0600C32B RID: 49963 RVA: 0x0047305F File Offset: 0x0047125F
		public ITab_Fishing()
		{
			this.size = ITab_Fishing.WinSize;
			this.labelKey = "TabFishing";
		}

		// Token: 0x0600C32C RID: 49964 RVA: 0x00473080 File Offset: 0x00471280
		protected override void FillTab()
		{
			Zone_Fishing zone = this.SelZone;
			if (zone == null || zone.CellCount == 0)
			{
				return;
			}
			WaterBody waterBody = zone.Cells[0].GetWaterBody(zone.Map);
			Rect rect = base.TabRect.AtZero();
			rect.yMin += 22f;
			rect = rect.ContractedBy(10f);
			Listing_Standard listing_Standard = new Listing_Standard(GameFont.Small);
			listing_Standard.Begin(rect);
			Listing_Standard listing_Standard2 = listing_Standard.BeginSection(200f, 4f, 4f);
			if (listing_Standard2.ButtonText(this.RepeatModeLabel(zone.repeatMode), null, 1f))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator enumerator = Enum.GetValues(typeof(FishRepeatMode)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FishRepeatMode mode = (FishRepeatMode)enumerator.Current;
						list.Add(new FloatMenuOption(this.RepeatModeLabel(mode), delegate()
						{
							zone.targetCount = 1;
							zone.repeatCount = 100;
							zone.pauseWhenSatisfied = false;
							zone.unpauseAtCount = 50;
							zone.repeatMode = mode;
							zone.RecheckPausedDueToResourceCount();
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			listing_Standard2.Gap(10f);
			FishRepeatMode repeatMode = zone.repeatMode;
			if (repeatMode != FishRepeatMode.RepeatCount)
			{
				if (repeatMode == FishRepeatMode.TargetCount)
				{
					listing_Standard2.Label("CurrentlyHave".Translate() + ": " + zone.OwnedFishCount.ToString() + " / " + zone.targetCount.ToString(), -1f, null);
					int targetCount = zone.targetCount;
					listing_Standard2.IntEntry(ref zone.targetCount, ref this.targetCountEditBuffer, 1, 0);
					if (targetCount != zone.targetCount)
					{
						zone.unpauseAtCount = Mathf.Max(0, zone.unpauseAtCount + (zone.targetCount - targetCount));
						this.unpauseAtCountEditBuffer = zone.unpauseAtCount.ToStringCached();
						zone.RecheckPausedDueToResourceCount();
					}
					listing_Standard2.Gap(10f);
					bool pauseWhenSatisfied = zone.pauseWhenSatisfied;
					listing_Standard2.CheckboxLabeled("PauseWhenSatisfied".Translate(), ref zone.pauseWhenSatisfied, null, 0f, 1f);
					if (zone.pauseWhenSatisfied != pauseWhenSatisfied)
					{
						zone.RecheckPausedDueToResourceCount();
					}
					if (zone.pauseWhenSatisfied)
					{
						listing_Standard2.Label("UnpauseWhenYouHave".Translate() + ": " + zone.unpauseAtCount.ToString("F0"), -1f, null);
						listing_Standard2.IntEntry(ref zone.unpauseAtCount, ref this.unpauseAtCountEditBuffer, 1, 0);
						if (zone.unpauseAtCount >= zone.targetCount)
						{
							zone.unpauseAtCount = Mathf.Max(0, zone.targetCount - 1);
							this.unpauseAtCountEditBuffer = zone.unpauseAtCount.ToStringCached();
							zone.RecheckPausedDueToResourceCount();
						}
					}
				}
			}
			else
			{
				listing_Standard2.Label("RepeatCount".Translate() + " " + zone.repeatCount.ToString(), -1f, null);
				listing_Standard2.IntEntry(ref zone.repeatCount, ref this.repeatCountEditBuffer, 1, 0);
			}
			listing_Standard.EndSection(listing_Standard2);
			listing_Standard.Gap(10f);
			Listing_Standard listing_Standard3 = listing_Standard.BeginSection(80f, 4f, 4f);
			int num = (int)waterBody.Population;
			int num2 = (int)waterBody.MaxPopulation;
			float min = 10f / (float)num2;
			string value = FishingUtility.FishPopulationLabel((float)num);
			Listing_Standard listing_Standard4 = listing_Standard3;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 4);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FishPopulation".Translate().CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(value);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			listing_Standard4.Label(defaultInterpolatedStringHandler.ToStringAndClear(), -1f, new TipSignal?("FishPopulationDesc".Translate()));
			listing_Standard3.Gap(10f);
			listing_Standard3.Label("MinimumPopulation".Translate() + ": " + Mathf.Round(zone.targetPopulationPct * (float)num2).ToString(), -1f, new TipSignal?("MinimumPopulationDesc".Translate()));
			zone.targetPopulationPct = listing_Standard3.Slider(zone.targetPopulationPct, min, 1f);
			listing_Standard.EndSection(listing_Standard3);
			listing_Standard.Gap(10f);
			Listing_Standard listing = listing_Standard.BeginSection(rect.height - listing_Standard.CurHeight - 10f, 4f, 4f);
			foreach (ThingDef fish in waterBody.CommonFishIncludingExtras)
			{
				this.ListFish(fish, listing, false);
			}
			foreach (ThingDef fish2 in waterBody.UncommonFish)
			{
				this.ListFish(fish2, listing, true);
			}
			listing_Standard.EndSection(listing);
			listing_Standard.End();
		}

		// Token: 0x0600C32D RID: 49965 RVA: 0x004736CC File Offset: 0x004718CC
		private void ListFish(ThingDef fish, Listing_Standard listing, bool uncommon)
		{
			Rect rect = listing.GetRect(Text.LineHeight, 1f);
			Rect rect2 = new Rect(rect.x, rect.y, Text.LineHeight, Text.LineHeight);
			Widgets.InfoCardButton(rect2, fish);
			Rect rect3 = new Rect(rect2.xMax + 4f, rect.y, rect.height, rect.height);
			Widgets.ThingIcon(rect3, fish, null, null, 1f, null, null, 1f);
			Rect rect4 = new Rect(rect3.xMax + 4f, rect.y, rect.xMax - rect3.xMax - 4f, rect.height);
			TaggedString str = fish.LabelCap + (uncommon ? (" (" + "Uncommon".Translate().ToString() + ")") : string.Empty);
			Widgets.Label(rect4, str.Truncate(rect4.width, null));
			listing.Gap(2f);
		}

		// Token: 0x0600C32E RID: 49966 RVA: 0x004737F8 File Offset: 0x004719F8
		private string RepeatModeLabel(FishRepeatMode mode)
		{
			switch (mode)
			{
			case FishRepeatMode.RepeatCount:
				return "FishRepeatMode_RepeatCount".Translate().CapitalizeFirst();
			case FishRepeatMode.TargetCount:
				return "FishRepeatMode_TargetCount".Translate().CapitalizeFirst();
			case FishRepeatMode.DoForever:
				return "FishRepeatMode_Forever".Translate().CapitalizeFirst();
			default:
				return "Unknown";
			}
		}

		// Token: 0x04006F81 RID: 28545
		private string repeatCountEditBuffer;

		// Token: 0x04006F82 RID: 28546
		private string targetCountEditBuffer;

		// Token: 0x04006F83 RID: 28547
		private string unpauseAtCountEditBuffer;

		// Token: 0x04006F84 RID: 28548
		private static readonly Vector2 WinSize = new Vector2(300f, 450f);

		// Token: 0x04006F85 RID: 28549
		private const float TopSectionHeight = 200f;

		// Token: 0x04006F86 RID: 28550
		private const float MiddleSectionHeight = 80f;
	}
}
