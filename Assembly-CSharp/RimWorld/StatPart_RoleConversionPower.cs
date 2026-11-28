using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA3 RID: 8099
	public class StatPart_RoleConversionPower : StatPart
	{
		// Token: 0x0600CDBC RID: 52668 RVA: 0x004B998C File Offset: 0x004B7B8C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Ideo != null)
				{
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null)
					{
						val *= role.def.convertPowerFactor;
					}
				}
			}
		}

		// Token: 0x0600CDBD RID: 52669 RVA: 0x004B99DC File Offset: 0x004B7BDC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Ideo != null)
				{
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null)
					{
						return "AbilityIdeoConvertBreakdownRole".Translate((req.Thing as Pawn).Named("PAWN"), role.Named("ROLE")) + ": " + role.def.convertPowerFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
					}
				}
			}
			return null;
		}
	}
}
