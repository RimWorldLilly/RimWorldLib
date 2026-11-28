using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199F RID: 6559
	public class StudyNote
	{
		// Token: 0x04006162 RID: 24930
		public float threshold;

		// Token: 0x04006163 RID: 24931
		public FloatRange thresholdRange = FloatRange.Zero;

		// Token: 0x04006164 RID: 24932
		public float? studyKnowledgeAmount;

		// Token: 0x04006165 RID: 24933
		[MustTranslate]
		public string label;

		// Token: 0x04006166 RID: 24934
		[MustTranslate]
		public string text;
	}
}
