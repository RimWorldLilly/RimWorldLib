using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA9 RID: 8105
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x0600CDD3 RID: 52691 RVA: 0x004B9E74 File Offset: 0x004B8074
		public override string ExplanationPart(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.BuildableDef.MadeFromStuff)
			{
				string t = (req.StuffDef != null) ? req.StuffDef.label : "None".TranslateSimple();
				string t2 = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null).ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Absolute) : "0";
				stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + t + "): " + t2);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": " + this.GetMultiplier(req).ToStringPercent("F0"));
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600CDD4 RID: 52692 RVA: 0x004B9F74 File Offset: 0x004B8174
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null) : 0f;
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x0600CDD5 RID: 52693 RVA: 0x004B9FB3 File Offset: 0x004B81B3
		private float GetMultiplier(StatRequest req)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(this.multiplierStat, true, -1);
			}
			return req.BuildableDef.GetStatValueAbstract(this.multiplierStat, null);
		}

		// Token: 0x0600CDD6 RID: 52694 RVA: 0x004B9FE6 File Offset: 0x004B81E6
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			StatPart_Stuff.<GetInfoCardHyperlinks>d__5 <GetInfoCardHyperlinks>d__ = new StatPart_Stuff.<GetInfoCardHyperlinks>d__5(-2);
			<GetInfoCardHyperlinks>d__.<>3__req = req;
			return <GetInfoCardHyperlinks>d__;
		}

		// Token: 0x0400833C RID: 33596
		public StatDef stuffPowerStat;

		// Token: 0x0400833D RID: 33597
		public StatDef multiplierStat;
	}
}
