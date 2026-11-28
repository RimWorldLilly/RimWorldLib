using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5C RID: 3932
	public class MapMeshFlagDef : Def
	{
		// Token: 0x06006529 RID: 25897 RVA: 0x002412DE File Offset: 0x0023F4DE
		public override void PostSetIndices()
		{
			FlagDefUtility.SetMaskFromIndex(this, ref this.mask);
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x002412EC File Offset: 0x0023F4EC
		public static implicit operator ulong(MapMeshFlagDef def)
		{
			if (def == null)
			{
				return 0UL;
			}
			return def.mask;
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x002412FC File Offset: 0x0023F4FC
		public override string ToString()
		{
			string str = base.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(this.mask);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return str + defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04003B84 RID: 15236
		private ulong mask;
	}
}
