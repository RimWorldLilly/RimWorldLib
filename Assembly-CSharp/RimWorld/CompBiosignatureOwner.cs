using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183B RID: 6203
	public class CompBiosignatureOwner : ThingComp
	{
		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x06009B46 RID: 39750 RVA: 0x0037FD28 File Offset: 0x0037DF28
		public CompProperties_BiosignatureOwner Props
		{
			get
			{
				return (CompProperties_BiosignatureOwner)this.props;
			}
		}

		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x06009B47 RID: 39751 RVA: 0x0037FD38 File Offset: 0x0037DF38
		public string BiosignatureName
		{
			get
			{
				string result;
				if ((result = this.biosignatureName) == null)
				{
					result = (this.biosignatureName = AnomalyUtility.GetBiosignatureName(this.biosignature));
				}
				return result;
			}
		}

		// Token: 0x06009B48 RID: 39752 RVA: 0x0037FD64 File Offset: 0x0037DF64
		public override string CompInspectStringExtra()
		{
			string value;
			if (this.Props.requiresAnalysis)
			{
				AnalysisDetails analysisDetails;
				Find.AnalysisManager.TryGetAnalysisProgress(this.biosignature, out analysisDetails);
				string text;
				if (analysisDetails != null && analysisDetails.timesDone != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
					defaultInterpolatedStringHandler.AppendFormatted(this.BiosignatureName);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Analyzed".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(" x");
					defaultInterpolatedStringHandler.AppendFormatted<int>(analysisDetails.timesDone);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					text = "Unknown".Translate().Resolve().CapitalizeFirst();
				}
				value = text;
			}
			else
			{
				value = this.BiosignatureName;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("Biosignature".Translate());
			defaultInterpolatedStringHandler2.AppendLiteral(": ");
			defaultInterpolatedStringHandler2.AppendFormatted(value);
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x06009B49 RID: 39753 RVA: 0x0037FE5C File Offset: 0x0037E05C
		public override void PostPostMake()
		{
			this.biosignature = Rand.Int;
		}

		// Token: 0x06009B4A RID: 39754 RVA: 0x0037FE69 File Offset: 0x0037E069
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x04005C51 RID: 23633
		public int biosignature;

		// Token: 0x04005C52 RID: 23634
		private string biosignatureName;
	}
}
