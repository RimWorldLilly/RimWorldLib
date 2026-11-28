using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001958 RID: 6488
	public class CompRitualSeat : ThingComp
	{
		// Token: 0x0600A2E7 RID: 41703 RVA: 0x003A3348 File Offset: 0x003A1548
		public override string CompInspectStringExtra()
		{
			string ideosString = this.GetIdeosString(null);
			if (string.IsNullOrEmpty(ideosString))
			{
				return null;
			}
			return "RitualSeatOf".Translate(ideosString.Named("IDEOS")).Resolve();
		}

		// Token: 0x0600A2E8 RID: 41704 RVA: 0x003A3384 File Offset: 0x003A1584
		private string GetIdeosString(List<Ideo> outIdeos = null)
		{
			CompRitualSeat.tmpStringBuilder.Clear();
			foreach (Ideo ideo in Find.IdeoManager.IdeosInViewOrder)
			{
				bool flag = false;
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_RitualSeat precept_RitualSeat = precept as Precept_RitualSeat;
					if (precept_RitualSeat != null && precept_RitualSeat.ThingDef == this.parent.def)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (outIdeos != null)
					{
						outIdeos.Add(ideo);
					}
					if (CompRitualSeat.tmpStringBuilder.Length > 0)
					{
						CompRitualSeat.tmpStringBuilder.Append(", ");
					}
					CompRitualSeat.tmpStringBuilder.Append(ideo.name.ApplyTag(ideo).Resolve());
				}
			}
			return CompRitualSeat.tmpStringBuilder.ToString();
		}

		// Token: 0x0600A2E9 RID: 41705 RVA: 0x003A3494 File Offset: 0x003A1694
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompRitualSeat.<SpecialDisplayStats>d__4 <SpecialDisplayStats>d__ = new CompRitualSeat.<SpecialDisplayStats>d__4(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04006049 RID: 24649
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x0400604A RID: 24650
		private static List<Ideo> tmpIdeos = new List<Ideo>();
	}
}
