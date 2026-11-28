using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D34 RID: 7476
	public class FloatMenuOptionProvider_LoadOntoPackAnimal : FloatMenuOptionProvider
	{
		// Token: 0x17002384 RID: 9092
		// (get) Token: 0x0600C174 RID: 49524 RVA: 0x00467B4C File Offset: 0x00465D4C
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002385 RID: 9093
		// (get) Token: 0x0600C175 RID: 49525 RVA: 0x00467B4F File Offset: 0x00465D4F
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002386 RID: 9094
		// (get) Token: 0x0600C176 RID: 49526 RVA: 0x00467B52 File Offset: 0x00465D52
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C177 RID: 49527 RVA: 0x00467B55 File Offset: 0x00465D55
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsFormingCaravan() && !context.map.IsPlayerHome;
		}

		// Token: 0x0600C178 RID: 49528 RVA: 0x00467B74 File Offset: 0x00465D74
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_LoadOntoPackAnimal.<GetOptionsFor>d__7 <GetOptionsFor>d__ = new FloatMenuOptionProvider_LoadOntoPackAnimal.<GetOptionsFor>d__7(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
