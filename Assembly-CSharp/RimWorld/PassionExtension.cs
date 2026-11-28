using System;

namespace RimWorld
{
	// Token: 0x020015C0 RID: 5568
	public static class PassionExtension
	{
		// Token: 0x06008A03 RID: 35331 RVA: 0x0031F1BC File Offset: 0x0031D3BC
		public static Passion IncrementPassion(this Passion passion)
		{
			if (passion == Passion.None)
			{
				return Passion.Minor;
			}
			if (passion == Passion.Minor)
			{
				return Passion.Major;
			}
			return passion;
		}
	}
}
