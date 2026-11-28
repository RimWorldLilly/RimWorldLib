using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA1 RID: 6817
	public class Building_BurningPowerCell : Building
	{
		// Token: 0x0600AC28 RID: 44072 RVA: 0x003CC622 File Offset: 0x003CA822
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("PuncturedWillExplode".Translate());
			stringBuilder.AppendInNewLine(base.GetInspectString());
			return stringBuilder.ToString();
		}
	}
}
