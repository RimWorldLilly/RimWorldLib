using System;

namespace Verse.AI.Group
{
	// Token: 0x02000929 RID: 2345
	public class PsychicRitualToil_Wait : PsychicRitualToil
	{
		// Token: 0x060047CF RID: 18383 RVA: 0x001C2DF5 File Offset: 0x001C0FF5
		public PsychicRitualToil_Wait()
		{
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x001C2E05 File Offset: 0x001C1005
		public PsychicRitualToil_Wait(int durationTicks)
		{
			this.durationTicks = durationTicks;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x001C2E1C File Offset: 0x001C101C
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			this.durationTicks--;
			return this.durationTicks <= 0;
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x001C2E38 File Offset: 0x001C1038
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
		}

		// Token: 0x04002E7B RID: 11899
		private int durationTicks = 60;
	}
}
