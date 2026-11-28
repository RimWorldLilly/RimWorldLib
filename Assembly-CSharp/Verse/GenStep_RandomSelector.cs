using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002CD RID: 717
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x0008248B File Offset: 0x0008068B
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00082494 File Offset: 0x00080694
		public override void Generate(Map map, GenStepParams parms)
		{
			RandomGenStepSelectorOption randomGenStepSelectorOption = this.options.RandomElementByWeight((RandomGenStepSelectorOption opt) => opt.weight);
			if (randomGenStepSelectorOption.genStep != null)
			{
				randomGenStepSelectorOption.genStep.Generate(map, parms);
			}
			if (randomGenStepSelectorOption.def != null)
			{
				randomGenStepSelectorOption.def.genStep.Generate(map, parms);
			}
		}

		// Token: 0x040012EC RID: 4844
		public List<RandomGenStepSelectorOption> options;
	}
}
