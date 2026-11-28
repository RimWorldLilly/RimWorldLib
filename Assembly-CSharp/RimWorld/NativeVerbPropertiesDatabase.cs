using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F2F RID: 7983
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x0600CBA7 RID: 52135 RVA: 0x004B0AE8 File Offset: 0x004AECE8
		public static VerbProperties VerbWithCategory(VerbCategory id)
		{
			VerbProperties verbProperties = (from v in NativeVerbPropertiesDatabase.allVerbDefs
			where v.category == id
			select v).FirstOrDefault<VerbProperties>();
			if (verbProperties == null)
			{
				Log.Error("Failed to find Verb with id " + id.ToString());
			}
			return verbProperties;
		}

		// Token: 0x040081C5 RID: 33221
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();
	}
}
