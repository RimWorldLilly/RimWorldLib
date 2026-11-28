using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001244 RID: 4676
	public class RoomContents_CorpseRoom : RoomContents_DeadBodyLabyrinth
	{
		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x060073FD RID: 29693 RVA: 0x0028A23B File Offset: 0x0028843B
		protected override IntRange CorpseRange
		{
			get
			{
				return new IntRange(5, 10);
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x060073FE RID: 29694 RVA: 0x0028A245 File Offset: 0x00288445
		protected override IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(4, 10);
			}
		}
	}
}
