using System;

namespace Verse
{
	// Token: 0x0200041F RID: 1055
	public class HediffCompProperties_DisappearsDisableable : HediffCompProperties_Disappears
	{
		// Token: 0x06001EF6 RID: 7926 RVA: 0x000BD373 File Offset: 0x000BB573
		public HediffCompProperties_DisappearsDisableable()
		{
			this.compClass = typeof(HediffComp_DisappearsDisableable);
		}

		// Token: 0x04001842 RID: 6210
		public bool initiallyDisabled;
	}
}
