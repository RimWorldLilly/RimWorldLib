using System;

namespace RimWorld
{
	// Token: 0x0200163F RID: 5695
	public static class PreceptMaker
	{
		// Token: 0x06008EA6 RID: 36518 RVA: 0x00346DA9 File Offset: 0x00344FA9
		public static Precept MakePrecept(PreceptDef def)
		{
			Precept precept = (Precept)Activator.CreateInstance(def.preceptClass);
			precept.def = def;
			precept.PostMake();
			return precept;
		}
	}
}
