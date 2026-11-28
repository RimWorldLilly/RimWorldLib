using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDF RID: 7647
	public class PawnTable_Mechs : PawnTable
	{
		// Token: 0x0600C6E7 RID: 50919 RVA: 0x0048DA38 File Offset: 0x0048BC38
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return input.OrderBy(new Func<Pawn, int>(PawnTable_Mechs.<LabelSortFunction>g__Overseer|0_0)).ThenBy(new Func<Pawn, int>(PawnTable_Mechs.<LabelSortFunction>g__ControlGroupIndex|0_1)).ThenBy((Pawn p) => p.KindLabel).ThenBy((Pawn p) => p.Label);
		}

		// Token: 0x0600C6E8 RID: 50920 RVA: 0x0048DAB0 File Offset: 0x0048BCB0
		public PawnTable_Mechs(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x0600C6E9 RID: 50921 RVA: 0x0048DAC0 File Offset: 0x0048BCC0
		[CompilerGenerated]
		internal static int <LabelSortFunction>g__Overseer|0_0(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null)
			{
				return int.MaxValue;
			}
			return overseer.thingIDNumber;
		}

		// Token: 0x0600C6EA RID: 50922 RVA: 0x0048DAE4 File Offset: 0x0048BCE4
		[CompilerGenerated]
		internal static int <LabelSortFunction>g__ControlGroupIndex|0_1(Pawn pawn)
		{
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (mechControlGroup == null)
			{
				return int.MaxValue;
			}
			return mechControlGroup.Index;
		}
	}
}
