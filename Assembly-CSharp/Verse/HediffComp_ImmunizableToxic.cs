using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200048F RID: 1167
	public class HediffComp_ImmunizableToxic : HediffComp_Immunizable
	{
		// Token: 0x0600206B RID: 8299 RVA: 0x000C2A50 File Offset: 0x000C0C50
		public override float SeverityChangePerDay()
		{
			float num = base.SeverityChangePerDay();
			if (num < 0f && base.Pawn.Spawned && ModsConfig.BiotechActive)
			{
				if (base.Pawn.Position.IsPolluted(base.Pawn.Map) && base.Pawn.GetStatValue(StatDefOf.ToxicEnvironmentResistance, true, -1) < 1f)
				{
					num = 0f;
				}
				else if (!base.Pawn.Position.Roofed(base.Pawn.Map))
				{
					if (base.Pawn.Map.GameConditionManager.ActiveConditions.Any((GameCondition x) => x.def.conditionClass == typeof(GameCondition_ToxicFallout)) && base.Pawn.GetStatValue(StatDefOf.ToxicResistance, true, -1) < 1f)
					{
						num = 0f;
					}
				}
			}
			return num;
		}
	}
}
