using System;

namespace RimWorld
{
	// Token: 0x02001331 RID: 4913
	public class TileMutatorWorker_Pond : TileMutatorWorker_Lake
	{
		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x060079C4 RID: 31172 RVA: 0x002B5779 File Offset: 0x002B3979
		protected override float LakeRadius
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x060079C5 RID: 31173 RVA: 0x002B5780 File Offset: 0x002B3980
		protected override bool GenerateDeepWater
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x002B5783 File Offset: 0x002B3983
		public TileMutatorWorker_Pond(TileMutatorDef def) : base(def)
		{
		}
	}
}
