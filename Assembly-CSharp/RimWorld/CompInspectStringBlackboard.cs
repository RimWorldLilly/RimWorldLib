using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C8 RID: 6344
	public class CompInspectStringBlackboard : CompInspectString
	{
		// Token: 0x06009F07 RID: 40711 RVA: 0x00390F80 File Offset: 0x0038F180
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Props.inspectString);
			stringBuilder.Append(": +");
			stringBuilder.Append(0.2f.ToStringPercent());
			return stringBuilder.ToString();
		}
	}
}
