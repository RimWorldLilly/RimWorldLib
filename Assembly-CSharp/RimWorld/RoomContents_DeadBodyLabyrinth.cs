using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001246 RID: 4678
	public class RoomContents_DeadBodyLabyrinth : RoomContents_DeadBody
	{
		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x0600740F RID: 29711 RVA: 0x0028A41D File Offset: 0x0028861D
		protected override ThingDef KillerThing
		{
			get
			{
				return ThingDefOf.Fingerspike;
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06007410 RID: 29712 RVA: 0x0028A424 File Offset: 0x00288624
		protected override DamageDef DamageType
		{
			get
			{
				return DamageDefOf.Scratch;
			}
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06007411 RID: 29713 RVA: 0x0028A42B File Offset: 0x0028862B
		protected override Tool ToolUsed
		{
			get
			{
				return ThingDefOf.Fingerspike.tools[0];
			}
		}
	}
}
