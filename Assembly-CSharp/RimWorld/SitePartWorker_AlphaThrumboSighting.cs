using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200176C RID: 5996
	public class SitePartWorker_AlphaThrumboSighting : SitePartWorker
	{
		// Token: 0x06009562 RID: 38242 RVA: 0x003658B6 File Offset: 0x00363AB6
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}
	}
}
