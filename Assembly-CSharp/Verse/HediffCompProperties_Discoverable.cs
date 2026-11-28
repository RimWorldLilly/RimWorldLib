using System;

namespace Verse
{
	// Token: 0x02000426 RID: 1062
	public class HediffCompProperties_Discoverable : HediffCompProperties
	{
		// Token: 0x06001F08 RID: 7944 RVA: 0x000BD54A File Offset: 0x000BB74A
		public HediffCompProperties_Discoverable()
		{
			this.compClass = typeof(HediffComp_Discoverable);
		}

		// Token: 0x04001844 RID: 6212
		public bool sendLetterWhenDiscovered;

		// Token: 0x04001845 RID: 6213
		public string discoverLetterLabel;

		// Token: 0x04001846 RID: 6214
		public string discoverLetterText;

		// Token: 0x04001847 RID: 6215
		public MessageTypeDef messageType;

		// Token: 0x04001848 RID: 6216
		public LetterDef letterType;
	}
}
