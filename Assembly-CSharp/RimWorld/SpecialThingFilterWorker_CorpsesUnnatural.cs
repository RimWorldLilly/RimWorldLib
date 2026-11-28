using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4B RID: 8011
	public class SpecialThingFilterWorker_CorpsesUnnatural : SpecialThingFilterWorker
	{
		// Token: 0x0600CC68 RID: 52328 RVA: 0x004B59CC File Offset: 0x004B3BCC
		public override bool Matches(Thing t)
		{
			return t is UnnaturalCorpse;
		}
	}
}
