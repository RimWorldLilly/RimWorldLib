using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEE RID: 6894
	public class FleshbeastGroundSpawner : PawnGroundSpawner
	{
		// Token: 0x17001FBC RID: 8124
		// (get) Token: 0x0600B013 RID: 45075 RVA: 0x003E0B8C File Offset: 0x003DED8C
		protected override SoundDef SustainerSound
		{
			get
			{
				return SoundDefOf.FleshbeastDigging;
			}
		}
	}
}
