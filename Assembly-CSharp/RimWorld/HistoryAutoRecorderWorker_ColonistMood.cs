using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001038 RID: 4152
	public class HistoryAutoRecorderWorker_ColonistMood : HistoryAutoRecorderWorker
	{
		// Token: 0x06006891 RID: 26769 RVA: 0x0024F598 File Offset: 0x0024D798
		public override float PullRecord()
		{
			List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			if (!allMaps_FreeColonists.Any<Pawn>())
			{
				return 0f;
			}
			return (from x in allMaps_FreeColonists
			where x.needs.mood != null
			select x).Average((Pawn x) => x.needs.mood.CurLevel * 100f);
		}
	}
}
