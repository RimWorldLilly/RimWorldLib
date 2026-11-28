using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AE RID: 6574
	public class CompTechprint : ThingComp
	{
		// Token: 0x17001CB6 RID: 7350
		// (get) Token: 0x0600A502 RID: 42242 RVA: 0x003AEBC3 File Offset: 0x003ACDC3
		public CompProperties_Techprint Props
		{
			get
			{
				return (CompProperties_Techprint)this.props;
			}
		}

		// Token: 0x0600A503 RID: 42243 RVA: 0x003AEBD0 File Offset: 0x003ACDD0
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompTechprint.<CompFloatMenuOptions>d__2 <CompFloatMenuOptions>d__ = new CompTechprint.<CompFloatMenuOptions>d__2(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}
	}
}
