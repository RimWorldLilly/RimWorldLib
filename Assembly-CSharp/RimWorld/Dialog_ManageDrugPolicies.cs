using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE1 RID: 7393
	[StaticConstructorOnStartup]
	public class Dialog_ManageDrugPolicies : Dialog_ManagePolicies<DrugPolicy>
	{
		// Token: 0x17002297 RID: 8855
		// (get) Token: 0x0600BE05 RID: 48645 RVA: 0x0044491C File Offset: 0x00442B1C
		protected override string TitleKey
		{
			get
			{
				return "DrugPolicyTitle";
			}
		}

		// Token: 0x17002298 RID: 8856
		// (get) Token: 0x0600BE06 RID: 48646 RVA: 0x00444923 File Offset: 0x00442B23
		protected override string TipKey
		{
			get
			{
				return "DrugPolicyTip";
			}
		}

		// Token: 0x17002299 RID: 8857
		// (get) Token: 0x0600BE07 RID: 48647 RVA: 0x0044492A File Offset: 0x00442B2A
		protected override float OffsetHeaderY
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x1700229A RID: 8858
		// (get) Token: 0x0600BE08 RID: 48648 RVA: 0x00444931 File Offset: 0x00442B31
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)Mathf.Min(Screen.width - 50, 1300), 720f);
			}
		}

		// Token: 0x0600BE09 RID: 48649 RVA: 0x00444950 File Offset: 0x00442B50
		public Dialog_ManageDrugPolicies(DrugPolicy policy) : base(policy)
		{
		}

		// Token: 0x0600BE0A RID: 48650 RVA: 0x00444959 File Offset: 0x00442B59
		protected override DrugPolicy CreateNewPolicy()
		{
			return Current.Game.drugPolicyDatabase.MakeNewDrugPolicy();
		}

		// Token: 0x0600BE0B RID: 48651 RVA: 0x0044496A File Offset: 0x00442B6A
		protected override DrugPolicy GetDefaultPolicy()
		{
			return Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
		}

		// Token: 0x0600BE0C RID: 48652 RVA: 0x0044497B File Offset: 0x00442B7B
		protected override void SetDefaultPolicy(DrugPolicy policy)
		{
			Current.Game.drugPolicyDatabase.SetDefault(policy);
		}

		// Token: 0x0600BE0D RID: 48653 RVA: 0x0044498D File Offset: 0x00442B8D
		protected override AcceptanceReport TryDeletePolicy(DrugPolicy policy)
		{
			return Current.Game.drugPolicyDatabase.TryDelete(policy);
		}

		// Token: 0x0600BE0E RID: 48654 RVA: 0x0044499F File Offset: 0x00442B9F
		protected override List<DrugPolicy> GetPolicies()
		{
			return Current.Game.drugPolicyDatabase.AllPolicies;
		}

		// Token: 0x0600BE0F RID: 48655 RVA: 0x004449B0 File Offset: 0x00442BB0
		protected override void DoContentsRect(Rect rect)
		{
			this.DoPolicyConfigArea(rect);
		}

		// Token: 0x0600BE10 RID: 48656 RVA: 0x004449B9 File Offset: 0x00442BB9
		public override void PostOpen()
		{
			base.PostOpen();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}

		// Token: 0x0600BE11 RID: 48657 RVA: 0x004449CC File Offset: 0x00442BCC
		private void DoPolicyConfigArea(Rect rect)
		{
			Rect rect2 = rect;
			rect2.y -= this.OffsetHeaderY;
			rect2.height = 36f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax;
			this.DoColumnLabels(rect2);
			Widgets.DrawMenuSection(rect3);
			rect3 = rect3.ContractedBy(1f);
			if (base.SelectedPolicy.Count == 0)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "NoDrugs".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			float height = (float)base.SelectedPolicy.Count * 35f;
			Rect viewRect = new Rect(0f, 0f, rect3.width, height);
			Widgets.AdjustRectsForScrollView(rect, ref rect3, ref viewRect);
			Widgets.BeginScrollView(rect3, ref this.scrollPosition, viewRect, true);
			DrugPolicy selectedPolicy = base.SelectedPolicy;
			for (int i = 0; i < selectedPolicy.Count; i++)
			{
				Rect rect4 = new Rect(0f, (float)i * 35f, viewRect.width, 35f);
				this.DoEntryRow(rect4, selectedPolicy[i], i);
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600BE12 RID: 48658 RVA: 0x00444B00 File Offset: 0x00442D00
		private void CalculateColumnsWidths(Rect rect, out float addictionWidth, out float allowJoyWidth, out float scheduledWidth, out float drugIconWidth, out float drugNameWidth, out float frequencyWidth, out float moodThresholdWidth, out float joyThresholdWidth, out float takeToInventoryWidth)
		{
			float num = rect.width - 27f - 108f;
			drugIconWidth = 27f;
			drugNameWidth = num * 0.25f;
			addictionWidth = 36f;
			allowJoyWidth = 36f;
			scheduledWidth = 36f;
			frequencyWidth = num * 0.3f;
			moodThresholdWidth = num * 0.15f;
			joyThresholdWidth = num * 0.15f;
			takeToInventoryWidth = num * 0.15f;
		}

		// Token: 0x0600BE13 RID: 48659 RVA: 0x00444B74 File Offset: 0x00442D74
		private void DoColumnLabels(Rect rect)
		{
			rect.width -= 16f;
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			float num9;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9);
			float num10 = rect.x;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(num10 + 4f, rect.y, num5 + num4, rect.height);
			Widgets.Label(rect2, "DrugColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect2, "DrugNameColumnDesc");
			num10 += num5 + num4;
			Rect rect3 = new Rect(num10, rect.y, num9, rect.height);
			Widgets.Label(rect3, "TakeToInventoryColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect3, "TakeToInventoryColumnDesc");
			num10 += num9;
			Rect rect4 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect4, Dialog_ManageDrugPolicies.IconForAddiction);
			TooltipHandler.TipRegionByKey(rect4, "DrugUsageTipForAddiction");
			num10 += num;
			Rect rect5 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect5, Dialog_ManageDrugPolicies.IconForJoy);
			TooltipHandler.TipRegionByKey(rect5, "DrugUsageTipForJoy");
			num10 += num2;
			Rect rect6 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect6, Dialog_ManageDrugPolicies.IconScheduled);
			TooltipHandler.TipRegionByKey(rect6, "DrugUsageTipScheduled");
			num10 += num3;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect7 = new Rect(num10, rect.y, num6, rect.height);
			Widgets.Label(rect7, "FrequencyColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect7, "FrequencyColumnDesc");
			num10 += num6;
			Rect rect8 = new Rect(num10, rect.y, num7, rect.height);
			Widgets.Label(rect8, "MoodThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect8, "MoodThresholdColumnDesc");
			num10 += num7;
			Rect rect9 = new Rect(num10, rect.y, num8, rect.height);
			Widgets.Label(rect9, "JoyThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect9, "JoyThresholdColumnDesc");
			num10 += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600BE14 RID: 48660 RVA: 0x00444D98 File Offset: 0x00442F98
		private void DoEntryRow(Rect rect, DrugPolicyEntry entry, int index)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			float num9;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9);
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			float num10 = rect.x;
			float num11 = (rect.height - num4) / 2f;
			Widgets.ThingIcon(new Rect(num10 + 5f, rect.y + num11, num4, num4), entry.drug, null, null, 1f, null, null, 1f);
			num10 += num4;
			Widgets.Label(new Rect(num10, rect.y, num5, rect.height).ContractedBy(4f), entry.drug.LabelCap);
			Widgets.InfoCardButton(new Rect
			{
				x = num10 + Text.CalcSize(entry.drug.LabelCap).x + 5f,
				y = rect.y + 8f,
				width = 16f,
				height = 16f
			}, entry.drug);
			num10 += num5;
			Widgets.TextFieldNumeric<int>(new Rect(num10, rect.y, num9, rect.height).ContractedBy(4f), ref entry.takeToInventory, ref entry.takeToInventoryTempBuffer, 0f, (float)PawnUtility.GetMaxAllowedToPickUp(entry.drug, null));
			num10 += num9;
			if (entry.drug.IsAddictiveDrug)
			{
				Widgets.Checkbox(num10, rect.y + 2f, ref entry.allowedForAddiction, 24f, false, true, null, null);
			}
			num10 += num;
			if (entry.drug.IsPleasureDrug)
			{
				Widgets.Checkbox(num10, rect.y + 2f, ref entry.allowedForJoy, 24f, false, true, null, null);
			}
			num10 += num2;
			Widgets.Checkbox(num10, rect.y + 2f, ref entry.allowScheduled, 24f, false, true, null, null);
			num10 += num3;
			if (entry.allowScheduled)
			{
				entry.daysFrequency = Widgets.FrequencyHorizontalSlider(new Rect(num10, rect.y + 2f, num6, rect.height).ContractedBy(4f), entry.daysFrequency, 0.1f, 25f, true);
				num10 += num6;
				string label;
				if (entry.onlyIfMoodBelow < 1f)
				{
					label = entry.onlyIfMoodBelow.ToStringPercent();
				}
				else
				{
					label = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfMoodBelow = Widgets.HorizontalSlider(new Rect(num10, rect.y + 2f, num7, rect.height).ContractedBy(4f), entry.onlyIfMoodBelow, 0.01f, 1f, true, label, null, null, -1f);
				num10 += num7;
				string label2;
				if (entry.onlyIfJoyBelow < 1f)
				{
					label2 = entry.onlyIfJoyBelow.ToStringPercent();
				}
				else
				{
					label2 = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfJoyBelow = Widgets.HorizontalSlider(new Rect(num10, rect.y + 2f, num8, rect.height).ContractedBy(4f), entry.onlyIfJoyBelow, 0.01f, 1f, true, label2, null, null, -1f);
				num10 += num8;
			}
			else
			{
				num10 += num6 + num7 + num8;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04006CE4 RID: 27876
		private Vector2 scrollPosition;

		// Token: 0x04006CE5 RID: 27877
		private const float DrugEntryRowHeight = 35f;

		// Token: 0x04006CE6 RID: 27878
		private const float CellsPadding = 4f;

		// Token: 0x04006CE7 RID: 27879
		private const float EntryLineYOffset = 2f;

		// Token: 0x04006CE8 RID: 27880
		private const float InfoButtonSize = 16f;

		// Token: 0x04006CE9 RID: 27881
		private const float HeaderHeight = 36f;

		// Token: 0x04006CEA RID: 27882
		private static readonly Texture2D IconForAddiction = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Addiction", true);

		// Token: 0x04006CEB RID: 27883
		private static readonly Texture2D IconForJoy = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Joy", true);

		// Token: 0x04006CEC RID: 27884
		private static readonly Texture2D IconScheduled = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Schedule", true);

		// Token: 0x04006CED RID: 27885
		private const float UsageSpacing = 12f;
	}
}
