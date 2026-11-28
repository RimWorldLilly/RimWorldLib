using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001247 RID: 4679
	public class RoomContents_ReactorCorpseRoom : RoomContents_DeadBody
	{
		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06007413 RID: 29715 RVA: 0x0028A445 File Offset: 0x00288645
		protected override ThingDef KillerThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06007414 RID: 29716 RVA: 0x0028A448 File Offset: 0x00288648
		protected override DamageDef DamageType
		{
			get
			{
				return DamageDefOf.Bullet;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06007415 RID: 29717 RVA: 0x0028A44F File Offset: 0x0028864F
		protected override Tool ToolUsed
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06007416 RID: 29718 RVA: 0x0028A452 File Offset: 0x00288652
		protected override FloatRange CorpseAgeDaysRange
		{
			get
			{
				return new FloatRange(300f, 900f);
			}
		}

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06007417 RID: 29719 RVA: 0x0028A463 File Offset: 0x00288663
		protected override IntRange SurvivalPacksCountRange
		{
			get
			{
				return IntRange.Zero;
			}
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06007418 RID: 29720 RVA: 0x0028A46A File Offset: 0x0028866A
		protected override IntRange CorpseRange
		{
			get
			{
				return new IntRange(4, 6);
			}
		}

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06007419 RID: 29721 RVA: 0x0028A473 File Offset: 0x00288673
		protected override bool AllHaveSameDeathAge
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600741A RID: 29722 RVA: 0x0028A476 File Offset: 0x00288676
		protected override IEnumerable<PawnKindDef> GetPossibleKinds()
		{
			return new RoomContents_ReactorCorpseRoom.<GetPossibleKinds>d__14(-2);
		}
	}
}
