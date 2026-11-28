using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001753 RID: 5971
	public class RoleEffect_PawnStatOffset : RoleEffect_PawnStatModifier
	{
		// Token: 0x060094B4 RID: 38068 RVA: 0x003634B8 File Offset: 0x003616B8
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.statDef.LabelCap + ": " + this.statDef.ValueToString(this.modifier, ToStringNumberSense.Offset, false);
		}
	}
}
