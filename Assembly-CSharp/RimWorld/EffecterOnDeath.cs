using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED2 RID: 3794
	public class EffecterOnDeath : ThingComp
	{
		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x0600638B RID: 25483 RVA: 0x0023BBA0 File Offset: 0x00239DA0
		public CompProperties_EffecterOnDeath Props
		{
			get
			{
				return (CompProperties_EffecterOnDeath)this.props;
			}
		}

		// Token: 0x0600638C RID: 25484 RVA: 0x0023BBAD File Offset: 0x00239DAD
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			this.Props.effecterDef.SpawnMaintained(this.parent.PositionHeld, prevMap, 1f);
			base.Notify_Killed(prevMap, dinfo);
		}

		// Token: 0x040037AC RID: 14252
		private Effecter effecter;
	}
}
