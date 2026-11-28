using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B88 RID: 2952
	public struct MeditationSpotAndFocus
	{
		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x060055A1 RID: 21921 RVA: 0x001FE05F File Offset: 0x001FC25F
		public bool IsValid
		{
			get
			{
				return this.spot.IsValid;
			}
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x001FE06C File Offset: 0x001FC26C
		public MeditationSpotAndFocus(LocalTargetInfo spot)
		{
			this.spot = spot;
			this.focus = LocalTargetInfo.Invalid;
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x001FE080 File Offset: 0x001FC280
		public MeditationSpotAndFocus(LocalTargetInfo spot, LocalTargetInfo focus)
		{
			this.spot = spot;
			this.focus = focus;
		}

		// Token: 0x040032EB RID: 13035
		public LocalTargetInfo spot;

		// Token: 0x040032EC RID: 13036
		public LocalTargetInfo focus;
	}
}
