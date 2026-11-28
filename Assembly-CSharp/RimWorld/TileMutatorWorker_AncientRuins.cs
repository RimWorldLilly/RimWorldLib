using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130D RID: 4877
	public class TileMutatorWorker_AncientRuins : TileMutatorWorker
	{
		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x0600791F RID: 31007 RVA: 0x002B0759 File Offset: 0x002AE959
		protected static GenStep_AncientRuins GenStep
		{
			get
			{
				return (GenStep_AncientRuins)GenStepDefOf.AncientRuins_Special.genStep;
			}
		}

		// Token: 0x06007920 RID: 31008 RVA: 0x002B076A File Offset: 0x002AE96A
		public TileMutatorWorker_AncientRuins(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007921 RID: 31009 RVA: 0x002B0774 File Offset: 0x002AE974
		public override void GenerateNonCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			try
			{
				TileMutatorWorker_AncientRuins.GenStep.GenerateRuins(map, default(GenStepParams), TileMutatorWorker_AncientRuins.MapFillPercentRange);
			}
			catch (Exception ex)
			{
				string str = "Exception while generating ancient ruins: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x04004BFC RID: 19452
		private static readonly FloatRange MapFillPercentRange = new FloatRange(0.3f, 0.5f);
	}
}
