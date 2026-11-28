using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020017BE RID: 6078
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x06009799 RID: 38809 RVA: 0x0036FC1E File Offset: 0x0036DE1E
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600979A RID: 38810 RVA: 0x0036FC21 File Offset: 0x0036DE21
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600979B RID: 38811 RVA: 0x0036FC33 File Offset: 0x0036DE33
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_ScatterThingsNearPlayerStart.<GetSummaryListEntries>d__3 <GetSummaryListEntries>d__ = new ScenPart_ScatterThingsNearPlayerStart.<GetSummaryListEntries>d__3(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}
	}
}
