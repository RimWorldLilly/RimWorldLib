using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200059C RID: 1436
	public static class ThingMaker
	{
		// Token: 0x06002AE1 RID: 10977 RVA: 0x00105BC0 File Offset: 0x00103DC0
		public static Thing MakeThing(ThingDef def, ThingDef stuff = null)
		{
			if (stuff != null && !stuff.IsStuff)
			{
				string[] array = new string[5];
				array[0] = "MakeThing error: Tried to make ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " from ";
				int num = 3;
				ThingDef thingDef = stuff;
				array[num] = ((thingDef != null) ? thingDef.ToString() : null);
				array[4] = " which is not a stuff. Assigning default.";
				Log.Error(string.Concat(array));
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (def.MadeFromStuff && stuff == null)
			{
				Log.Error("MakeThing error: " + ((def != null) ? def.ToString() : null) + " is madeFromStuff but stuff=null. Assigning default.");
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (!def.MadeFromStuff && stuff != null)
			{
				string[] array2 = new string[5];
				array2[0] = "MakeThing error: ";
				array2[1] = ((def != null) ? def.ToString() : null);
				array2[2] = " is not madeFromStuff but stuff=";
				int num2 = 3;
				ThingDef thingDef2 = stuff;
				array2[num2] = ((thingDef2 != null) ? thingDef2.ToString() : null);
				array2[4] = ". Setting to null.";
				Log.Error(string.Concat(array2));
				stuff = null;
			}
			Thing thing = (Thing)Activator.CreateInstance(def.thingClass);
			thing.def = def;
			thing.SetStuffDirect(stuff);
			thing.PostMake();
			thing.PostPostMake();
			return thing;
		}
	}
}
