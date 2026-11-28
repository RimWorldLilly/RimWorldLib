using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020ED RID: 8429
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x0600D3FA RID: 54266 RVA: 0x004E4B30 File Offset: 0x004E2D30
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x0600D3FB RID: 54267 RVA: 0x004E4B88 File Offset: 0x004E2D88
		public override void Resolve(ResolveParams rp)
		{
			int num = Rand.RangeInclusive(5, rp.rect.Width - 1);
			int num2 = Rand.RangeInclusive(5, rp.rect.Height - 1);
			int num3 = Rand.RangeInclusive(0, rp.rect.Width - num);
			int num4 = Rand.RangeInclusive(0, rp.rect.Height - num2);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX + num3, rp.rect.minZ + num4, num, num2);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams, null);
		}

		// Token: 0x040086B9 RID: 34489
		private const int MinLength = 5;

		// Token: 0x040086BA RID: 34490
		private const int MaxRectSize = 15;
	}
}
