using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5D RID: 3933
	public class RenderSkipFlagDef : Def
	{
		// Token: 0x0600652D RID: 25901 RVA: 0x00241351 File Offset: 0x0023F551
		public override void PostSetIndices()
		{
			FlagDefUtility.SetMaskFromIndex(this, ref this.mask);
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x0024135F File Offset: 0x0023F55F
		public static implicit operator ulong(RenderSkipFlagDef def)
		{
			if (def == null)
			{
				return 0UL;
			}
			return def.mask;
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x00241370 File Offset: 0x0023F570
		public override string ToString()
		{
			string str = base.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.mask);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return str + defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04003B85 RID: 15237
		private ulong mask;
	}
}
