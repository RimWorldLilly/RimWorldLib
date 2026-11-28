using System;

namespace Verse
{
	// Token: 0x0200031D RID: 797
	public static class ThingIDMaker
	{
		// Token: 0x0600187E RID: 6270 RVA: 0x000931D4 File Offset: 0x000913D4
		public static void GiveIDTo(Thing t)
		{
			if (!t.def.HasThingIDNumber)
			{
				return;
			}
			if (t.thingIDNumber != -1)
			{
				Log.Error("Giving ID to " + ((t != null) ? t.ToString() : null) + " which already has id " + t.thingIDNumber.ToString());
			}
			t.thingIDNumber = Find.UniqueIDsManager.GetNextThingID();
		}
	}
}
