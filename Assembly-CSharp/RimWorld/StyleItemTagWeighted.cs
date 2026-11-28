using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF1 RID: 4081
	public class StyleItemTagWeighted
	{
		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x0600671A RID: 26394 RVA: 0x00248A5B File Offset: 0x00246C5B
		public string Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x0600671B RID: 26395 RVA: 0x00248A63 File Offset: 0x00246C63
		public float TotalWeight
		{
			get
			{
				return this.baseWeight * this.weightFactor;
			}
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x00248A72 File Offset: 0x00246C72
		public StyleItemTagWeighted()
		{
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x00248A7A File Offset: 0x00246C7A
		public StyleItemTagWeighted(string tag, float baseWeight, float weightFactor = 1f)
		{
			this.tag = tag;
			this.baseWeight = baseWeight;
			this.weightFactor = weightFactor;
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x00248A97 File Offset: 0x00246C97
		public void Add(StyleItemTagWeighted other)
		{
			this.baseWeight += other.baseWeight;
			this.weightFactor *= other.weightFactor;
		}

		// Token: 0x040040D3 RID: 16595
		[NoTranslate]
		private string tag;

		// Token: 0x040040D4 RID: 16596
		private float baseWeight;

		// Token: 0x040040D5 RID: 16597
		private float weightFactor;
	}
}
