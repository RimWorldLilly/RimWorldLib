using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D40 RID: 7488
	public class FloatMenuOptionProvider_Romance : FloatMenuOptionProvider
	{
		// Token: 0x170023AD RID: 9133
		// (get) Token: 0x0600C1C0 RID: 49600 RVA: 0x00468B9C File Offset: 0x00466D9C
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023AE RID: 9134
		// (get) Token: 0x0600C1C1 RID: 49601 RVA: 0x00468B9F File Offset: 0x00466D9F
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023AF RID: 9135
		// (get) Token: 0x0600C1C2 RID: 49602 RVA: 0x00468BA2 File Offset: 0x00466DA2
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C1C3 RID: 49603 RVA: 0x00468BA5 File Offset: 0x00466DA5
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x0600C1C4 RID: 49604 RVA: 0x00468BAC File Offset: 0x00466DAC
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsColonist || clickedPawn.HostFaction != null)
			{
				return null;
			}
			if (clickedPawn.Drafted)
			{
				return null;
			}
			FloatMenuOption floatMenuOption;
			float num;
			bool flag = RelationsUtility.RomanceOption(context.FirstSelectedPawn, clickedPawn, out floatMenuOption, out num);
			if (floatMenuOption == null)
			{
				return null;
			}
			floatMenuOption.Label = (flag ? "CanRomance" : "CannotRomance").Translate(floatMenuOption.Label);
			return floatMenuOption;
		}
	}
}
