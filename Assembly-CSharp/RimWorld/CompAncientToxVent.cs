using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001808 RID: 6152
	public class CompAncientToxVent : CompAncientVent
	{
		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x0600999B RID: 39323 RVA: 0x00378E5F File Offset: 0x0037705F
		protected override bool AppliesEffectsToPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600999C RID: 39324 RVA: 0x00378E62 File Offset: 0x00377062
		protected override void ToggleIndividualVent(bool on)
		{
			this.parent.GetComp<CompFleckEmitterLongTerm>().Enabled = on;
		}

		// Token: 0x0600999D RID: 39325 RVA: 0x00378E75 File Offset: 0x00377075
		protected override void ApplyPawnEffect(Pawn pawn, int delta)
		{
			ToxicUtility.DoPawnToxicDamage(pawn, 0.0028977108f * (float)delta);
		}
	}
}
