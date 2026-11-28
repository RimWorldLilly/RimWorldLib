using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187E RID: 6270
	public class CompFacilityQualityBased : CompFacility
	{
		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x06009D09 RID: 40201 RVA: 0x00387D9D File Offset: 0x00385F9D
		public new CompProperties_FacilityQualityBased Props
		{
			get
			{
				return (CompProperties_FacilityQualityBased)this.props;
			}
		}

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x06009D0A RID: 40202 RVA: 0x00387DAA File Offset: 0x00385FAA
		public override List<StatModifier> StatOffsets
		{
			get
			{
				return this.statOffsets;
			}
		}

		// Token: 0x06009D0B RID: 40203 RVA: 0x00387DB2 File Offset: 0x00385FB2
		public virtual void PostQualitySet()
		{
			this.SetStatOffsets();
		}

		// Token: 0x06009D0C RID: 40204 RVA: 0x00387DBC File Offset: 0x00385FBC
		private void SetStatOffsets()
		{
			this.statOffsets.Clear();
			QualityCategory key;
			if (!this.parent.TryGetQuality(out key))
			{
				Log.Error("Could not get parent quality for CompFacilityQualityBased of " + this.parent.def.defName);
				return;
			}
			foreach (KeyValuePair<StatDef, Dictionary<QualityCategory, float>> keyValuePair in this.Props.statOffsetsPerQuality)
			{
				this.statOffsets.Add(new StatModifier
				{
					stat = keyValuePair.Key,
					value = keyValuePair.Value[key]
				});
			}
		}

		// Token: 0x06009D0D RID: 40205 RVA: 0x00387E78 File Offset: 0x00386078
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetStatOffsets();
			}
		}

		// Token: 0x04005D16 RID: 23830
		private List<StatModifier> statOffsets = new List<StatModifier>();
	}
}
