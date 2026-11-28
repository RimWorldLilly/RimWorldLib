using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020000C7 RID: 199
	public abstract class DeathActionWorker
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002C622 File Offset: 0x0002A822
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0002C629 File Offset: 0x0002A829
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060007AC RID: 1964
		public abstract void PawnDied(Corpse corpse, Lord prevLord);

		// Token: 0x04000366 RID: 870
		public DeathActionProperties props;
	}
}
