using System;

namespace RimWorld
{
	// Token: 0x0200198A RID: 6538
	public class CompSpawnLarva : CompSpawnPawnOnDestroyed
	{
		// Token: 0x17001C85 RID: 7301
		// (get) Token: 0x0600A433 RID: 42035 RVA: 0x003AAA86 File Offset: 0x003A8C86
		protected override bool JoinLord
		{
			get
			{
				return this.parent.Faction != Faction.OfPlayer;
			}
		}
	}
}
