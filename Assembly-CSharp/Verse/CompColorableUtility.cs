using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053D RID: 1341
	public static class CompColorableUtility
	{
		// Token: 0x06002771 RID: 10097 RVA: 0x000F6668 File Offset: 0x000F4868
		public static void SetColor(this Thing t, Color newColor, bool reportFailure = true)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on non-ThingWithComps " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			CompColorable comp = thingWithComps.GetComp<CompColorable>();
			if (comp == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on Thing without CompColorable " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			if (!comp.Color.IndistinguishableFrom(newColor))
			{
				comp.SetColor(newColor);
			}
		}
	}
}
