using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002128 RID: 8488
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x0600D4BC RID: 54460 RVA: 0x004EC410 File Offset: 0x004EA610
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("doors", rp, null);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGen.symbolStack.Push(this.interior, resolveParams, null);
			}
			ResolveParams resolveParams2 = rp;
			if (this.useRandomCarpet)
			{
				resolveParams2.floorDef = (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			resolveParams2.noRoof = new bool?(!this.allowRoof);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams2, null);
		}

		// Token: 0x040086F4 RID: 34548
		public string interior;

		// Token: 0x040086F5 RID: 34549
		public bool useRandomCarpet;

		// Token: 0x040086F6 RID: 34550
		public bool allowRoof = true;
	}
}
