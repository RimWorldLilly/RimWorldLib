using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8E RID: 8078
	public class StatPart_Hyperlinks : StatPart
	{
		// Token: 0x0600CD62 RID: 52578 RVA: 0x004B85E7 File Offset: 0x004B67E7
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}

		// Token: 0x0600CD63 RID: 52579 RVA: 0x004B85EA File Offset: 0x004B67EA
		public override void TransformValue(StatRequest req, ref float val)
		{
		}

		// Token: 0x0600CD64 RID: 52580 RVA: 0x004B85EC File Offset: 0x004B67EC
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			StatPart_Hyperlinks.<GetInfoCardHyperlinks>d__3 <GetInfoCardHyperlinks>d__ = new StatPart_Hyperlinks.<GetInfoCardHyperlinks>d__3(-2);
			<GetInfoCardHyperlinks>d__.<>4__this = this;
			return <GetInfoCardHyperlinks>d__;
		}

		// Token: 0x0400830A RID: 33546
		public List<ThingDef> thingDefs;
	}
}
