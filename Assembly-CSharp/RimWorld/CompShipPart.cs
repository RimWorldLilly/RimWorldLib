using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FC RID: 6652
	public class CompShipPart : ThingComp
	{
		// Token: 0x0600A782 RID: 42882 RVA: 0x003BA831 File Offset: 0x003B8A31
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompShipPart.<CompGetGizmosExtra>d__0 <CompGetGizmosExtra>d__ = new CompShipPart.<CompGetGizmosExtra>d__0(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A783 RID: 42883 RVA: 0x003BA844 File Offset: 0x003B8A44
		public void ShowReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!ShipUtility.LaunchFailReasons((Building)this.parent).Any<string>())
			{
				stringBuilder.AppendLine("ShipReportCanLaunch".Translate());
			}
			else
			{
				stringBuilder.AppendLine("ShipReportCannotLaunch".Translate());
				foreach (string value in ShipUtility.LaunchFailReasons((Building)this.parent))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(value);
				}
			}
			Dialog_MessageBox window = new Dialog_MessageBox(stringBuilder.ToString(), null, null, null, null, null, false, null, null, WindowLayer.Dialog);
			Find.WindowStack.Add(window);
		}
	}
}
