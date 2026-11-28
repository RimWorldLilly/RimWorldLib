using System;
using System.Text;
using RimWorld.Utility;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9F RID: 8095
	public class StatPart_ReloadMarketValue : StatPart
	{
		// Token: 0x0600CDAB RID: 52651 RVA: 0x004B94FF File Offset: 0x004B76FF
		public override void TransformValue(StatRequest req, ref float val)
		{
			StatPart_ReloadMarketValue.TransformAndExplain(req, ref val, null);
		}

		// Token: 0x0600CDAC RID: 52652 RVA: 0x004B950C File Offset: 0x004B770C
		public override string ExplanationPart(StatRequest req)
		{
			float num = 1f;
			StringBuilder stringBuilder = new StringBuilder();
			StatPart_ReloadMarketValue.TransformAndExplain(req, ref num, stringBuilder);
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600CDAD RID: 52653 RVA: 0x004B953C File Offset: 0x004B773C
		private static void TransformAndExplain(StatRequest req, ref float val, StringBuilder explanation)
		{
			Thing thing = req.Thing;
			IReloadableComp reloadableComp = (thing != null) ? thing.TryGetComp<CompApparelReloadable>() : null;
			IReloadableComp reloadableComp2;
			if ((reloadableComp2 = reloadableComp) == null)
			{
				Thing thing2 = req.Thing;
				reloadableComp2 = ((thing2 != null) ? thing2.TryGetComp<CompEquippableAbilityReloadable>() : null);
			}
			IReloadableComp reloadableComp3 = reloadableComp2;
			if (reloadableComp3 == null)
			{
				return;
			}
			if (reloadableComp3.RemainingCharges == reloadableComp3.MaxCharges)
			{
				return;
			}
			if (reloadableComp3.AmmoDef != null)
			{
				int num = reloadableComp3.MaxAmmoNeeded(true);
				float num2 = -reloadableComp3.AmmoDef.BaseMarketValue * (float)num;
				val += num2;
				if (explanation != null)
				{
					explanation.AppendLine("StatsReport_ReloadMarketValue".Translate(reloadableComp3.AmmoDef.Named("AMMO"), num.Named("COUNT")) + ": " + num2.ToStringMoneyOffset(null));
				}
			}
			else
			{
				CompApparelVerbOwner_Charged compApparelVerbOwner_Charged = reloadableComp3 as CompApparelVerbOwner_Charged;
				if (compApparelVerbOwner_Charged != null && compApparelVerbOwner_Charged.Props.destroyOnEmpty)
				{
					float num3 = (float)reloadableComp3.RemainingCharges / (float)reloadableComp3.MaxCharges;
					if (explanation != null)
					{
						explanation.AppendLine("StatsReport_ReloadRemainingChargesMultipler".Translate(compApparelVerbOwner_Charged.Props.ChargeNounArgument, reloadableComp3.LabelRemaining) + ": x" + num3.ToStringPercent());
					}
					val *= num3;
				}
			}
			if (val < 0f)
			{
				val = 0f;
			}
		}
	}
}
