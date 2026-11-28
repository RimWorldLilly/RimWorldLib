using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200163A RID: 5690
	public abstract class PreceptComp_Thought : PreceptComp
	{
		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06008E93 RID: 36499 RVA: 0x00346804 File Offset: 0x00344A04
		public bool AffectsMood
		{
			get
			{
				for (int i = 0; i < this.thought.stages.Count; i++)
				{
					if (this.thought.stages[i].baseMoodEffect != 0f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06008E94 RID: 36500 RVA: 0x0034684C File Offset: 0x00344A4C
		private string ParseDescription(string description, int thoughtStage = -1)
		{
			bool flag = thoughtStage != -1;
			if (!flag)
			{
				thoughtStage = 0;
			}
			if (!description.NullOrEmpty())
			{
				IPreceptCompDescriptionArgs preceptCompDescriptionArgs = this.thought.Worker as IPreceptCompDescriptionArgs;
				if (preceptCompDescriptionArgs != null)
				{
					description = description.Formatted(preceptCompDescriptionArgs.GetDescriptionArgs());
				}
			}
			if (description.NullOrEmpty())
			{
				description = ((!this.thought.stages[thoughtStage].LabelAbstractCap.NullOrEmpty()) ? this.thought.stages[thoughtStage].LabelAbstractCap : this.thought.LabelCap.Resolve());
				description = description.Formatted(Gender.Male.Named("PAWN_gender"));
			}
			if (this.AffectsMood)
			{
				ThoughtStage thoughtStage2 = this.thought.stages[thoughtStage];
				if (this.thought.minExpectation != null)
				{
					description += " (" + "MinExpectation".Translate() + ": " + this.thought.minExpectation.LabelCap + ")";
				}
				else if (this.thought.minExpectationForNegativeThought != null && thoughtStage2.baseMoodEffect < 0f)
				{
					description += " (" + "MinExpectation".Translate() + ": " + this.thought.minExpectationForNegativeThought.LabelCap + ")";
				}
				string str;
				if (this.tooltipShowMoodRange && !flag)
				{
					float num = float.PositiveInfinity;
					float num2 = float.NegativeInfinity;
					for (int i = 0; i < this.thought.stages.Count; i++)
					{
						num = Mathf.Min(num, this.thought.stages[i].baseMoodEffect);
						num2 = Mathf.Max(num2, this.thought.stages[i].baseMoodEffect);
					}
					str = "PreceptThoughtMoodRange".Translate(num.ToStringWithSign("F0"), num2.ToStringWithSign("F0"));
				}
				else
				{
					str = thoughtStage2.baseMoodEffect.ToStringWithSign("F0");
				}
				return description + ": " + str;
			}
			return description + ": " + this.thought.stages[thoughtStage].baseOpinionOffset.ToStringWithSign("F0");
		}

		// Token: 0x06008E95 RID: 36501 RVA: 0x00346AE4 File Offset: 0x00344CE4
		public override IEnumerable<string> GetDescriptions()
		{
			PreceptComp_Thought.<GetDescriptions>d__6 <GetDescriptions>d__ = new PreceptComp_Thought.<GetDescriptions>d__6(-2);
			<GetDescriptions>d__.<>4__this = this;
			return <GetDescriptions>d__;
		}

		// Token: 0x04005762 RID: 22370
		public ThoughtDef thought;

		// Token: 0x04005763 RID: 22371
		[MustTranslate]
		public List<string> thoughtStageDescriptions;

		// Token: 0x04005764 RID: 22372
		public bool tooltipShowMoodRange;
	}
}
