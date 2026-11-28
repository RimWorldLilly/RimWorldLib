using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4A RID: 6730
	public class Building_Art : Building
	{
		// Token: 0x0600A897 RID: 43159 RVA: 0x003BE020 File Offset: 0x003BC220
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			string str = text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(StatDefOf.Beauty.LabelCap);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true, -1), ToStringNumberSense.Absolute, true));
			return str + defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
