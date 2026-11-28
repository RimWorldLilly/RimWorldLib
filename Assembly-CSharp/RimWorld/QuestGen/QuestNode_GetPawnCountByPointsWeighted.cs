using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021CE RID: 8654
	public class QuestNode_GetPawnCountByPointsWeighted : QuestNode
	{
		// Token: 0x0600D769 RID: 55145 RVA: 0x004FF668 File Offset: 0x004FD868
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D76A RID: 55146 RVA: 0x004FF672 File Offset: 0x004FD872
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D76B RID: 55147 RVA: 0x004FF680 File Offset: 0x004FD880
		private void SetVars(Slate slate)
		{
			float x = slate.Get<float>("points", 0f, false);
			float num = this.pointsCurve.GetValue(slate).Evaluate(x);
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			int num2;
			if (this.challengeRating.TryGetValue(slate, out num2))
			{
				if (num2 == 1)
				{
					num = Mathf.Min(num, (float)this.maxCountOneStar.GetValue(slate));
				}
				else if (num2 == 2)
				{
					num = Mathf.Min(num, (float)this.maxCountTwoStar.GetValue(slate));
				}
				else
				{
					num = Mathf.Min(num, (float)this.maxCountThreeStar.GetValue(slate));
				}
			}
			SimpleCurve value = this.chancesCurve.GetValue(slate);
			for (int i = value.Points.Count - 1; i >= 0; i--)
			{
				if (value.Points[i].x <= num)
				{
					value.Points.Insert(i + 1, new CurvePoint(num + 1f, 0f));
					break;
				}
				value.Points[i] = new CurvePoint(0f, 0f);
			}
			float num3 = Rand.ByCurve(value);
			if (this.roundRandom.GetValue(slate))
			{
				num3 = (float)GenMath.RoundRandom(num3);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), Mathf.Clamp(num3, 1f, num), false);
		}

		// Token: 0x040088CE RID: 35022
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088CF RID: 35023
		public SlateRef<int> challengeRating;

		// Token: 0x040088D0 RID: 35024
		public SlateRef<int> maxCountOneStar;

		// Token: 0x040088D1 RID: 35025
		public SlateRef<int> maxCountTwoStar;

		// Token: 0x040088D2 RID: 35026
		public SlateRef<int> maxCountThreeStar;

		// Token: 0x040088D3 RID: 35027
		public SlateRef<SimpleCurve> pointsCurve;

		// Token: 0x040088D4 RID: 35028
		public SlateRef<SimpleCurve> chancesCurve;

		// Token: 0x040088D5 RID: 35029
		public SlateRef<bool> roundRandom;
	}
}
