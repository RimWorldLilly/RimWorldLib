using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECD RID: 7885
	[DefOf]
	public static class LogEntryDefOf
	{
		// Token: 0x0600CA88 RID: 51848 RVA: 0x004A61CA File Offset: 0x004A43CA
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDefOf));
		}

		// Token: 0x04007F62 RID: 32610
		public static LogEntryDef MeleeAttack;
	}
}
