using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020B3 RID: 8371
	public static class SketchGen
	{
		// Token: 0x0600D2F9 RID: 54009 RVA: 0x004D9924 File Offset: 0x004D7B24
		public static Sketch Generate(SketchResolverDef root, SketchResolveParams parms)
		{
			if (SketchGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.");
				return parms.sketch;
			}
			SketchGen.working = true;
			Sketch sketch;
			try
			{
				root.Resolve(parms);
				sketch = parms.sketch;
			}
			catch (Exception ex)
			{
				string str = "Error in SketchGen: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				sketch = parms.sketch;
			}
			finally
			{
				SketchGen.working = false;
			}
			return sketch;
		}

		// Token: 0x040085AE RID: 34222
		private static bool working;
	}
}
