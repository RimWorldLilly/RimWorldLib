using System;

namespace Verse
{
	// Token: 0x02000545 RID: 1349
	public class CompWindSource : ThingComp
	{
		// Token: 0x0600279F RID: 10143 RVA: 0x000F6DEC File Offset: 0x000F4FEC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x04001C53 RID: 7251
		public float wind;
	}
}
