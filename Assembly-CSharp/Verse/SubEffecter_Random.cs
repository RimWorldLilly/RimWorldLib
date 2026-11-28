using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006CC RID: 1740
	public class SubEffecter_Random : SubEffecter
	{
		// Token: 0x0600370D RID: 14093 RVA: 0x00160DA8 File Offset: 0x0015EFA8
		public SubEffecter_Random(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
			if (this.def.children != null)
			{
				List<float> list = new List<float>();
				foreach (SubEffecterDef subEffecterDef in subDef.children)
				{
					list.Add(subEffecterDef.randomWeight);
				}
				this.child = subDef.children.RandomElementByWeight((SubEffecterDef p) => p.randomWeight).Spawn(parent);
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x00160E54 File Offset: 0x0015F054
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			SubEffecter subEffecter = this.child;
			if (subEffecter == null)
			{
				return;
			}
			subEffecter.SubEffectTick(A, B);
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x00160E68 File Offset: 0x0015F068
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			SubEffecter subEffecter = this.child;
			if (subEffecter == null)
			{
				return;
			}
			subEffecter.SubTrigger(A, B, overrideSpawnTick, force);
		}

		// Token: 0x04002435 RID: 9269
		public SubEffecter child;
	}
}
