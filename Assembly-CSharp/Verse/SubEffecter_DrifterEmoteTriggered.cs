using System;

namespace Verse
{
	// Token: 0x020000DA RID: 218
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x0002FEFD File Offset: 0x0002E0FD
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002FF07 File Offset: 0x0002E107
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			base.MakeMote(A, overrideSpawnTick);
		}
	}
}
