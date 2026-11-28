using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000192 RID: 402
	public class MentalBreakDef : Def
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0003D65D File Offset: 0x0003B85D
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0003D69D File Offset: 0x0003B89D
		public override IEnumerable<string> ConfigErrors()
		{
			MentalBreakDef.<ConfigErrors>d__14 <ConfigErrors>d__ = new MentalBreakDef.<ConfigErrors>d__14(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040009AC RID: 2476
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x040009AD RID: 2477
		public MentalStateDef mentalState;

		// Token: 0x040009AE RID: 2478
		public float baseCommonality;

		// Token: 0x040009AF RID: 2479
		public SimpleCurve commonalityFactorPerPopulationCurve;

		// Token: 0x040009B0 RID: 2480
		public MentalBreakIntensity intensity;

		// Token: 0x040009B1 RID: 2481
		public TraitDef requiredTrait;

		// Token: 0x040009B2 RID: 2482
		public GeneDef requiredGene;

		// Token: 0x040009B3 RID: 2483
		public bool anomalousBreak;

		// Token: 0x040009B4 RID: 2484
		public bool questLodgersCanDo = true;

		// Token: 0x040009B5 RID: 2485
		public PreceptDef requiredPrecept;

		// Token: 0x040009B6 RID: 2486
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x040009B7 RID: 2487
		private MentalBreakWorker workerInt;
	}
}
