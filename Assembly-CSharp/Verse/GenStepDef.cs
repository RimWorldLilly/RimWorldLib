using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200016D RID: 365
	public class GenStepDef : Def
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0003BDC2 File Offset: 0x00039FC2
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x04000862 RID: 2146
		public SitePartDef linkWithSite;

		// Token: 0x04000863 RID: 2147
		public float order;

		// Token: 0x04000864 RID: 2148
		public GenStep genStep;

		// Token: 0x04000865 RID: 2149
		public List<GenStepDef> preventsGenSteps;
	}
}
