using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A5 RID: 6309
	public class CompProperties_GiveThoughtToAllMapPawnsOnDestroy : CompProperties
	{
		// Token: 0x06009DFE RID: 40446 RVA: 0x0038B984 File Offset: 0x00389B84
		public CompProperties_GiveThoughtToAllMapPawnsOnDestroy()
		{
			this.compClass = typeof(CompGiveThoughtToAllMapPawnsOnDestroy);
		}

		// Token: 0x04005D7C RID: 23932
		public ThoughtDef thought;

		// Token: 0x04005D7D RID: 23933
		[MustTranslate]
		public string message;

		// Token: 0x04005D7E RID: 23934
		public bool onlyWhenKilled;

		// Token: 0x04005D7F RID: 23935
		public bool ignoreOnVanish = true;
	}
}
