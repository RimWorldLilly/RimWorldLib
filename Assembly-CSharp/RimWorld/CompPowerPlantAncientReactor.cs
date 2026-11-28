using System;

namespace RimWorld
{
	// Token: 0x0200135C RID: 4956
	public class CompPowerPlantAncientReactor : CompPowerPlant
	{
		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x06007AC5 RID: 31429 RVA: 0x002BB6B2 File Offset: 0x002B98B2
		protected override float DesiredPowerOutput
		{
			get
			{
				if (!this.HackableComp.IsHacked)
				{
					return base.DesiredPowerOutput;
				}
				return 0f;
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x06007AC6 RID: 31430 RVA: 0x002BB6D0 File Offset: 0x002B98D0
		public CompHackable HackableComp
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableComp) == null)
				{
					result = (this.hackableComp = this.parent.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x04004D81 RID: 19841
		private CompHackable hackableComp;
	}
}
