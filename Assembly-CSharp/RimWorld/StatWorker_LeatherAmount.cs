using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD2 RID: 8146
	public class StatWorker_LeatherAmount : StatWorker
	{
		// Token: 0x0600CEB7 RID: 52919 RVA: 0x004C18A0 File Offset: 0x004BFAA0
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			StatWorker_LeatherAmount.<GetInfoCardHyperlinks>d__0 <GetInfoCardHyperlinks>d__ = new StatWorker_LeatherAmount.<GetInfoCardHyperlinks>d__0(-2);
			<GetInfoCardHyperlinks>d__.<>4__this = this;
			<GetInfoCardHyperlinks>d__.<>3__statRequest = statRequest;
			return <GetInfoCardHyperlinks>d__;
		}
	}
}
