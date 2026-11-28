using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001754 RID: 5972
	public class RoleEffect_PawnStatFactor : RoleEffect_PawnStatModifier
	{
		// Token: 0x060094B6 RID: 38070 RVA: 0x003634F4 File Offset: 0x003616F4
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.statDef.LabelCap + ": " + this.statDef.ValueToString(this.modifier, ToStringNumberSense.Factor, false);
		}
	}
}
