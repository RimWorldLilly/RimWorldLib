using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001DD9 RID: 7641
	public class PawnColumnWorker_WorkPriority : PawnColumnWorker
	{
		// Token: 0x170024BE RID: 9406
		// (get) Token: 0x0600C6A0 RID: 50848 RVA: 0x0048C0A8 File Offset: 0x0048A2A8
		public override bool VisibleCurrently
		{
			get
			{
				return this.def.workType.VisibleCurrently;
			}
		}

		// Token: 0x0600C6A1 RID: 50849 RVA: 0x0048C0BC File Offset: 0x0048A2BC
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Dead || pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return;
			}
			Text.Font = GameFont.Medium;
			float x = rect.x + (rect.width - 25f) / 2f;
			float y = rect.y + 2.5f;
			bool incapable = this.IsIncapableOfWholeWorkType(pawn, this.def.workType);
			WidgetsWork.DrawWorkBoxFor(x, y, pawn, this.def.workType, incapable);
			Rect rect2 = new Rect(x, y, 25f, 25f);
			if (Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, () => WidgetsWork.TipForPawnWorker(pawn, this.def.workType, incapable), pawn.thingIDNumber ^ this.def.workType.GetHashCode());
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600C6A2 RID: 50850 RVA: 0x0048C1C4 File Offset: 0x0048A3C4
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Text.Font = GameFont.Small;
			if (this.cachedWorkLabelSize == default(Vector2))
			{
				this.cachedWorkLabelSize = Text.CalcSize(this.def.workType.labelShort.CapitalizeFirst());
			}
			Rect labelRect = this.GetLabelRect(rect);
			MouseoverSounds.DoRegion(labelRect);
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(labelRect, this.def.workType.labelShort.CapitalizeFirst());
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			Widgets.DrawLineVertical(labelRect.center.x, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
			Widgets.DrawLineVertical(labelRect.center.x + 1f, labelRect.yMax - 3f, rect.y + 50f - labelRect.yMax + 3f);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C6A3 RID: 50851 RVA: 0x0048C2EA File Offset: 0x0048A4EA
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return 50;
		}

		// Token: 0x0600C6A4 RID: 50852 RVA: 0x0048C2EE File Offset: 0x0048A4EE
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 32);
		}

		// Token: 0x0600C6A5 RID: 50853 RVA: 0x0048C2FE File Offset: 0x0048A4FE
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(39, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C6A6 RID: 50854 RVA: 0x0048C315 File Offset: 0x0048A515
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), 80);
		}

		// Token: 0x0600C6A7 RID: 50855 RVA: 0x0048C328 File Offset: 0x0048A528
		private bool IsIncapableOfWholeWorkType(Pawn p, WorkTypeDef work)
		{
			for (int i = 0; i < work.workGiversByPriority.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < work.workGiversByPriority[i].requiredCapacities.Count; j++)
				{
					PawnCapacityDef capacity = work.workGiversByPriority[i].requiredCapacities[j];
					if (!p.health.capacities.CapableOf(capacity))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C6A8 RID: 50856 RVA: 0x0048C3A3 File Offset: 0x0048A5A3
		protected override Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			return this.GetLabelRect(headerRect);
		}

		// Token: 0x0600C6A9 RID: 50857 RVA: 0x0048C3AC File Offset: 0x0048A5AC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C6AA RID: 50858 RVA: 0x0048C3D0 File Offset: 0x0048A5D0
		private float GetValueToCompare(Pawn pawn)
		{
			if (pawn.workSettings == null || !pawn.workSettings.EverWork)
			{
				return -2f;
			}
			if (pawn.WorkTypeIsDisabled(this.def.workType))
			{
				return -1f;
			}
			return pawn.skills.AverageOfRelevantSkillsFor(this.def.workType);
		}

		// Token: 0x0600C6AB RID: 50859 RVA: 0x0048C428 File Offset: 0x0048A628
		private Rect GetLabelRect(Rect headerRect)
		{
			float x = headerRect.center.x;
			Rect result = new Rect(x - this.cachedWorkLabelSize.x / 2f, headerRect.y, this.cachedWorkLabelSize.x, this.cachedWorkLabelSize.y);
			if (this.def.moveWorkTypeLabelDown)
			{
				result.y += 20f;
			}
			return result;
		}

		// Token: 0x0600C6AC RID: 50860 RVA: 0x0048C49C File Offset: 0x0048A69C
		protected override string GetHeaderTip(PawnTable table)
		{
			TaggedString t = string.Concat(new string[]
			{
				this.def.workType.gerundLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor),
				"\n\n",
				this.def.workType.description,
				"\n\n",
				PawnColumnWorker_WorkPriority.SpecificWorkListString(this.def.workType)
			});
			t += "\n";
			if (this.def.sortable)
			{
				t += "\n" + "ClickToSortByThisColumn".Translate().Colorize(ColoredText.SubtleGrayColor);
			}
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				if (Find.PlaySettings.useWorkPriorities)
				{
					t += "\n" + "WorkPriorityShiftClickTip".Translate().Colorize(ColoredText.SubtleGrayColor);
				}
				else
				{
					t += "\n" + "WorkPriorityShiftClickEnableDisableTip".Translate().Colorize(ColoredText.SubtleGrayColor);
				}
			}
			return t.Resolve();
		}

		// Token: 0x0600C6AD RID: 50861 RVA: 0x0048C5B8 File Offset: 0x0048A7B8
		private static string SpecificWorkListString(WorkTypeDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < def.workGiversByPriority.Count; i++)
			{
				stringBuilder.Append(" - " + def.workGiversByPriority[i].LabelCap);
				if (def.workGiversByPriority[i].emergency)
				{
					stringBuilder.Append(" (" + "EmergencyWorkMarker".Translate() + ")");
				}
				if (i < def.workGiversByPriority.Count - 1)
				{
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600C6AE RID: 50862 RVA: 0x0048C668 File Offset: 0x0048A868
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (pawn.workSettings != null && pawn.workSettings.EverWork && !pawn.WorkTypeIsDisabled(this.def.workType))
					{
						if (Find.PlaySettings.useWorkPriorities)
						{
							int priority = pawn.workSettings.GetPriority(this.def.workType);
							if (Event.current.button == 0 && priority != 1)
							{
								int num = priority - 1;
								if (num < 0)
								{
									num = 4;
								}
								pawn.workSettings.SetPriority(this.def.workType, num);
							}
							if (Event.current.button == 1 && priority != 0)
							{
								int num2 = priority + 1;
								if (num2 > 4)
								{
									num2 = 0;
								}
								pawn.workSettings.SetPriority(this.def.workType, num2);
							}
						}
						else if (pawn.workSettings.GetPriority(this.def.workType) > 0)
						{
							if (Event.current.button == 1)
							{
								pawn.workSettings.SetPriority(this.def.workType, 0);
							}
						}
						else if (Event.current.button == 0)
						{
							pawn.workSettings.SetPriority(this.def.workType, 3);
						}
					}
				}
				if (Find.PlaySettings.useWorkPriorities)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x04007102 RID: 28930
		private const int LabelRowHeight = 50;

		// Token: 0x04007103 RID: 28931
		private Vector2 cachedWorkLabelSize;
	}
}
