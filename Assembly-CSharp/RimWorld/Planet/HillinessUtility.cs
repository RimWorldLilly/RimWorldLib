using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002333 RID: 9011
	public static class HillinessUtility
	{
		// Token: 0x0600DE95 RID: 56981 RVA: 0x0052C77C File Offset: 0x0052A97C
		public static string GetLabel(this Hilliness h)
		{
			switch (h)
			{
			case Hilliness.Flat:
				return "Hilliness_Flat".Translate();
			case Hilliness.SmallHills:
				return "Hilliness_SmallHills".Translate();
			case Hilliness.LargeHills:
				return "Hilliness_LargeHills".Translate();
			case Hilliness.Mountainous:
				return "Hilliness_Mountainous".Translate();
			case Hilliness.Impassable:
				return "Hilliness_Impassable".Translate();
			default:
				Log.ErrorOnce("Hilliness label unknown: " + h.ToString(), 694362);
				return h.ToString();
			}
		}

		// Token: 0x0600DE96 RID: 56982 RVA: 0x0052C825 File Offset: 0x0052AA25
		public static string GetLabelCap(this Hilliness h)
		{
			return h.GetLabel().CapitalizeFirst();
		}
	}
}
