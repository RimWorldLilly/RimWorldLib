using System;

namespace RimWorld
{
	// Token: 0x02001807 RID: 6151
	public class CompAncientSmokeVent : CompAncientVent
	{
		// Token: 0x06009999 RID: 39321 RVA: 0x00378E44 File Offset: 0x00377044
		protected override void ToggleIndividualVent(bool on)
		{
			this.parent.GetComp<CompFleckEmitterLongTerm>().Enabled = on;
		}
	}
}
