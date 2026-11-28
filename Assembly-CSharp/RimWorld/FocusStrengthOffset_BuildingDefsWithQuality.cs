using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001904 RID: 6404
	public class FocusStrengthOffset_BuildingDefsWithQuality : FocusStrengthOffset_BuildingDefs
	{
		// Token: 0x0600A08C RID: 41100 RVA: 0x00397000 File Offset: 0x00395200
		protected override float OffsetForBuilding(Thing b)
		{
			QualityCategory qualityCategory;
			if (b.TryGetQuality(out qualityCategory))
			{
				return this.focusPerQuality.Evaluate((float)qualityCategory);
			}
			return 0f;
		}

		// Token: 0x0600A08D RID: 41101 RVA: 0x0039702C File Offset: 0x0039522C
		public override string GetExplanation(Thing parent)
		{
			if (!parent.Spawned)
			{
				return this.GetExplanationAbstract(parent.def);
			}
			int value = this.BuildingCount(parent);
			string value2 = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKey.Translate(value, this.maxBuildings, value2) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x0600A08E RID: 41102 RVA: 0x003970D8 File Offset: 0x003952D8
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			string value = this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y.ToString("0%");
			return this.explanationKeyAbstract.Translate(this.maxBuildings, value) + ": +0-" + this.MaxOffset(null).ToString("0%");
		}

		// Token: 0x0600A08F RID: 41103 RVA: 0x00397164 File Offset: 0x00395364
		public override float MaxOffset(Thing parent = null)
		{
			return (float)this.maxBuildings * this.focusPerQuality.Points[this.focusPerQuality.Points.Count - 1].y;
		}

		// Token: 0x04005ED4 RID: 24276
		public SimpleCurve focusPerQuality;
	}
}
