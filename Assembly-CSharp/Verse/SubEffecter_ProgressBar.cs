using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000DB RID: 219
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x0002FF11 File Offset: 0x0002E111
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002FF1C File Offset: 0x0002E11C
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.linearScale.x = 0.68f;
				this.mote.linearScale.z = 0.12f;
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002FF78 File Offset: 0x0002E178
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040003BA RID: 954
		public MoteProgressBar mote;

		// Token: 0x040003BB RID: 955
		private const float Width = 0.68f;

		// Token: 0x040003BC RID: 956
		private const float Height = 0.12f;
	}
}
