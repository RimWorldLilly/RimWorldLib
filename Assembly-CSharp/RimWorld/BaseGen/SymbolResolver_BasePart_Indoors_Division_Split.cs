using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020E1 RID: 8417
	public class SymbolResolver_BasePart_Indoors_Division_Split : SymbolResolver
	{
		// Token: 0x0600D3CB RID: 54219 RVA: 0x004E3514 File Offset: 0x004E1714
		private int ResolveMinLengthAfterSplit(ResolveParams rp)
		{
			return rp.minLengthAfterSplit.GetValueOrDefault(5);
		}

		// Token: 0x0600D3CC RID: 54220 RVA: 0x004E3523 File Offset: 0x004E1723
		private int ResolveMinWidthOrHeight(int minLengthAfterSplit)
		{
			return minLengthAfterSplit * 2 - 1;
		}

		// Token: 0x0600D3CD RID: 54221 RVA: 0x004E352C File Offset: 0x004E172C
		public override bool CanResolve(ResolveParams rp)
		{
			int num = this.ResolveMinWidthOrHeight(this.ResolveMinLengthAfterSplit(rp));
			return base.CanResolve(rp) && (rp.rect.Width >= num || rp.rect.Height >= num);
		}

		// Token: 0x0600D3CE RID: 54222 RVA: 0x004E3578 File Offset: 0x004E1778
		public override void Resolve(ResolveParams rp)
		{
			int num = this.ResolveMinLengthAfterSplit(rp);
			int num2 = this.ResolveMinWidthOrHeight(num);
			if (rp.rect.Width < num2 && rp.rect.Height < num2)
			{
				string str = "Too small rect. params=";
				ResolveParams resolveParams = rp;
				Log.Warning(str + resolveParams.ToString());
				return;
			}
			if ((Rand.Bool && rp.rect.Height >= num2) || rp.rect.Width < num2)
			{
				int num3 = Rand.RangeInclusive(num - 1, rp.rect.Height - num);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num3 + 1);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams2, null);
				ResolveParams resolveParams3 = rp;
				resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num3, rp.rect.Width, rp.rect.Height - num3);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams3, null);
				return;
			}
			int num4 = Rand.RangeInclusive(num - 1, rp.rect.Width - num);
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num4 + 1, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams4, null);
			ResolveParams resolveParams5 = rp;
			resolveParams5.rect = new CellRect(rp.rect.minX + num4, rp.rect.minZ, rp.rect.Width - num4, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams5, null);
		}

		// Token: 0x040086A9 RID: 34473
		private const int MinLengthAfterSplit = 5;
	}
}
