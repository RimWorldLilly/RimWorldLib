using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006CF RID: 1743
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06003715 RID: 14101 RVA: 0x00160F3C File Offset: 0x0015F13C
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x00160F46 File Offset: 0x0015F146
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
