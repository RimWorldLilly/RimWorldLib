using System;

namespace Verse
{
	// Token: 0x020006C9 RID: 1737
	public class SubEffecter_CameraShakeSingle : SubEffecter_CameraShake
	{
		// Token: 0x06003706 RID: 14086 RVA: 0x00160B37 File Offset: 0x0015ED37
		public SubEffecter_CameraShakeSingle(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x00160B41 File Offset: 0x0015ED41
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			base.DoShake(A);
		}
	}
}
