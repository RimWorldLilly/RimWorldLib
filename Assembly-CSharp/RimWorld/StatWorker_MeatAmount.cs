using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD1 RID: 8145
	public class StatWorker_MeatAmount : StatWorker
	{
		// Token: 0x0600CEB3 RID: 52915 RVA: 0x004C1824 File Offset: 0x004BFA24
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn = req.Pawn;
			bool? flag;
			if (pawn == null)
			{
				flag = null;
			}
			else
			{
				RaceProperties raceProps = pawn.RaceProps;
				flag = ((raceProps != null) ? new bool?(raceProps.hasMeat) : null);
			}
			bool? flag2 = flag;
			return !flag2.GetValueOrDefault() && base.ShouldShowFor(req);
		}

		// Token: 0x0600CEB4 RID: 52916 RVA: 0x004C1878 File Offset: 0x004BFA78
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			StatWorker_MeatAmount.<GetInfoCardHyperlinks>d__1 <GetInfoCardHyperlinks>d__ = new StatWorker_MeatAmount.<GetInfoCardHyperlinks>d__1(-2);
			<GetInfoCardHyperlinks>d__.<>4__this = this;
			<GetInfoCardHyperlinks>d__.<>3__statRequest = statRequest;
			return <GetInfoCardHyperlinks>d__;
		}
	}
}
