using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020F3 RID: 8435
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x0600D40F RID: 54287 RVA: 0x004E5320 File Offset: 0x004E3520
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.25f))
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leafDecorated", rp, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", rp, null);
		}
	}
}
