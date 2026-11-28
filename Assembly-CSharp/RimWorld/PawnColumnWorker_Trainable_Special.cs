using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD8 RID: 7640
	public class PawnColumnWorker_Trainable_Special : PawnColumnWorker
	{
		// Token: 0x0600C695 RID: 50837 RVA: 0x0048BDC1 File Offset: 0x00489FC1
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x0600C696 RID: 50838 RVA: 0x0048BDD4 File Offset: 0x00489FD4
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training == null || pawn.RaceProps.specialTrainables.NullOrEmpty<TrainableDef>())
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f);
			this.DoSpecialTrainableCheckbox(rect2, pawn, true);
		}

		// Token: 0x0600C697 RID: 50839 RVA: 0x0048BE48 File Offset: 0x0048A048
		private void DoSpecialTrainableCheckbox(Rect rect, Pawn pawn, bool doTooltip)
		{
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			PawnColumnWorker_Trainable_Special.GetStatus(pawn, out flag, out flag2, out flag3, out flag4);
			bool flag5 = flag2;
			Texture2D texChecked = flag ? TrainingCardUtility.LearnedTrainingTex : null;
			Texture2D texUnchecked = flag ? TrainingCardUtility.LearnedNotTrainingTex : null;
			Widgets.Checkbox(rect.position, ref flag2, rect.width, !flag3, true, texChecked, texUnchecked);
			if (flag2 != flag5)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTraining, KnowledgeAmount.Total);
				foreach (TrainableDef td in pawn.RaceProps.specialTrainables)
				{
					pawn.training.SetWantedRecursive(td, flag2);
				}
			}
			if (doTooltip)
			{
				this.DoSpecialTrainableTooltip(rect, pawn);
			}
		}

		// Token: 0x0600C698 RID: 50840 RVA: 0x0048BF0C File Offset: 0x0048A10C
		private void DoSpecialTrainableTooltip(Rect rect, Pawn pawn)
		{
			if (!Mouse.IsOver(rect))
			{
				return;
			}
			TooltipHandler.TipRegion(rect, delegate()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (TrainableDef trainableDef in pawn.RaceProps.specialTrainables)
				{
					bool flag;
					AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(trainableDef, out flag);
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.AppendLine(trainableDef.LabelCap + "\n\n" + trainableDef.description);
					if (!acceptanceReport.Accepted)
					{
						stringBuilder.AppendLine().AppendLine(acceptanceReport.Reason);
					}
					else if (!trainableDef.prerequisites.NullOrEmpty<TrainableDef>())
					{
						stringBuilder.AppendLine();
						foreach (TrainableDef trainableDef2 in trainableDef.prerequisites)
						{
							if (!pawn.training.HasLearned(trainableDef2))
							{
								stringBuilder.AppendLine("TrainingNeedsPrerequisite".Translate(trainableDef2.LabelCap));
							}
						}
					}
				}
				return stringBuilder.ToString();
			}, (int)(rect.y * 511f + rect.x));
		}

		// Token: 0x0600C699 RID: 50841 RVA: 0x0048BF57 File Offset: 0x0048A157
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x0600C69A RID: 50842 RVA: 0x0048BF67 File Offset: 0x0048A167
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600C69B RID: 50843 RVA: 0x0048BF7C File Offset: 0x0048A17C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x0600C69C RID: 50844 RVA: 0x0048BF8C File Offset: 0x0048A18C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C69D RID: 50845 RVA: 0x0048BFB0 File Offset: 0x0048A1B0
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.training == null || pawn.RaceProps.specialTrainables.NullOrEmpty<TrainableDef>())
			{
				return int.MinValue;
			}
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			PawnColumnWorker_Trainable_Special.GetStatus(pawn, out flag, out flag2, out flag3, out flag4);
			if (flag)
			{
				return 4;
			}
			if (!flag4)
			{
				return 0;
			}
			if (!flag3)
			{
				return 1;
			}
			if (!flag2)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x0600C69E RID: 50846 RVA: 0x0048C000 File Offset: 0x0048A200
		private static void GetStatus(Pawn pawn, out bool learned, out bool checkOn, out bool canTrain, out bool visible)
		{
			learned = true;
			checkOn = true;
			canTrain = true;
			visible = false;
			foreach (TrainableDef td in pawn.RaceProps.specialTrainables)
			{
				if (!pawn.training.HasLearned(td))
				{
					learned = false;
				}
				if (!pawn.training.GetWanted(td))
				{
					checkOn = false;
				}
				bool flag;
				if (!pawn.training.CanAssignToTrain(td, out flag))
				{
					canTrain = false;
				}
				if (flag)
				{
					visible = true;
				}
			}
		}
	}
}
