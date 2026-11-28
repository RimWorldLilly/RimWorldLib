using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E58 RID: 3672
	public class Thought_Counselled : Thought_Memory
	{
		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x060061C1 RID: 25025 RVA: 0x00233E93 File Offset: 0x00232093
		public override int DurationTicks
		{
			get
			{
				return this.durationTicksOverride;
			}
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x00233E9B File Offset: 0x0023209B
		public override float MoodOffset()
		{
			return this.moodOffsetOverride;
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00233EA3 File Offset: 0x002320A3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.moodOffsetOverride, "moodOffsetOverride", 0f, false);
		}

		// Token: 0x04003608 RID: 13832
		public float moodOffsetOverride;
	}
}
