using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037B RID: 891
	public static class PawnNameColorUtility
	{
		// Token: 0x06001B31 RID: 6961 RVA: 0x000A76BC File Offset: 0x000A58BC
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000A78F8 File Offset: 0x000A5AF8
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000A7964 File Offset: 0x000A5B64
		public static Color PawnNameColorOf(Pawn pawn)
		{
			if (pawn.MentalStateDef != null)
			{
				return pawn.MentalStateDef.nameColor;
			}
			int index;
			if (pawn.Faction == null)
			{
				index = 0;
			}
			else
			{
				index = pawn.Faction.randomKey % 10;
			}
			if (pawn.IsPrisoner)
			{
				return PawnNameColorUtility.ColorsPrisoner[index];
			}
			if (pawn.IsSlave && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return PawnNameColorUtility.ColorBaseHostile;
			}
			if (pawn.IsSlave)
			{
				return PawnNameColorUtility.ColorSlave;
			}
			if (pawn.IsWildMan())
			{
				return PawnNameColorUtility.ColorWildMan;
			}
			if (pawn.Faction == null)
			{
				return PawnNameColorUtility.ColorsNeutral[index];
			}
			if (pawn.IsColonyMechRequiringMechanitor())
			{
				return PawnNameColorUtility.ColorUncontrolledPlayerMech;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return PawnNameColorUtility.ColorColony;
			}
			if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return PawnNameColorUtility.ColorsHostile[index];
			}
			return PawnNameColorUtility.ColorsNeutral[index];
		}

		// Token: 0x0400164E RID: 5710
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x0400164F RID: 5711
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04001650 RID: 5712
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04001651 RID: 5713
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04001652 RID: 5714
		public static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04001653 RID: 5715
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04001654 RID: 5716
		private static readonly Color ColorSlave = new Color32(222, 192, 22, byte.MaxValue);

		// Token: 0x04001655 RID: 5717
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04001656 RID: 5718
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04001657 RID: 5719
		private static readonly Color ColorUncontrolledPlayerMech = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04001658 RID: 5720
		private const int ColorShiftCount = 10;

		// Token: 0x04001659 RID: 5721
		private static readonly List<Color> ColorShifts = new List<Color>
		{
			new Color(1f, 1f, 1f),
			new Color(0.8f, 1f, 1f),
			new Color(0.8f, 0.8f, 1f),
			new Color(0.8f, 0.8f, 0.8f),
			new Color(1.2f, 1f, 1f),
			new Color(0.8f, 1.2f, 1f),
			new Color(0.8f, 1.2f, 1.2f),
			new Color(1.2f, 1.2f, 1.2f),
			new Color(1f, 1.2f, 1f),
			new Color(1.2f, 1f, 0.8f)
		};
	}
}
