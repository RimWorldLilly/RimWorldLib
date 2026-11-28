using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F1 RID: 6129
	public class CompAnalyzableBiosignature : CompAnalyzable
	{
		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x060098F7 RID: 39159 RVA: 0x003764BE File Offset: 0x003746BE
		public new CompProperties_CompAnalyzableBiosignature Props
		{
			get
			{
				return (CompProperties_CompAnalyzableBiosignature)this.props;
			}
		}

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x060098F8 RID: 39160 RVA: 0x003764CB File Offset: 0x003746CB
		public override int AnalysisID
		{
			get
			{
				return this.biosignature;
			}
		}

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x060098F9 RID: 39161 RVA: 0x003764D3 File Offset: 0x003746D3
		public override NamedArgument? ExtraNamedArg
		{
			get
			{
				return new NamedArgument?(AnomalyUtility.GetBiosignatureName(this.biosignature).Colorize(ColoredText.NameColor).Named("BIOSIGNATURE"));
			}
		}

		// Token: 0x060098FA RID: 39162 RVA: 0x003764F9 File Offset: 0x003746F9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x04005B69 RID: 23401
		public int biosignature;
	}
}
