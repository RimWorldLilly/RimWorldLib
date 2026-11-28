using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E52 RID: 3666
	public class Thought_DecreeUnmet : Thought_Situational
	{
		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x060061A9 RID: 25001 RVA: 0x002339D0 File Offset: 0x00231BD0
		public override string LabelCap
		{
			get
			{
				string text = base.LabelCap;
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					int num = this.TicksSinceQuestUnmet(questPart_SituationalThought);
					if (num > 0)
					{
						text = text + " (" + num.ToStringTicksToDays("F0") + ")";
					}
				}
				return text;
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x060061AA RID: 25002 RVA: 0x00233A2C File Offset: 0x00231C2C
		public override string Description
		{
			get
			{
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					return base.CurStage.description.Formatted("(" + questPart_SituationalThought.quest.name + ")");
				}
				return "";
			}
		}

		// Token: 0x060061AB RID: 25003 RVA: 0x00233A94 File Offset: 0x00231C94
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
			if (questPart_SituationalThought == null)
			{
				return 0f;
			}
			float x = (float)this.TicksSinceQuestUnmet(questPart_SituationalThought) / 60000f;
			return (float)Mathf.RoundToInt(Thought_DecreeUnmet.MoodOffsetFromUnmetDaysCurve.Evaluate(x));
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x00233AFF File Offset: 0x00231CFF
		private int TicksSinceQuestUnmet(QuestPart_SituationalThought questPart)
		{
			return questPart.quest.TicksSinceAccepted - questPart.delayTicks;
		}

		// Token: 0x04003605 RID: 13829
		private static readonly SimpleCurve MoodOffsetFromUnmetDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, -5f),
				true
			},
			{
				new CurvePoint(15f, -15f),
				true
			}
		};
	}
}
