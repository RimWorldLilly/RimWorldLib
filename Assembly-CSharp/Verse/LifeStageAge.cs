using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200014C RID: 332
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x06000A28 RID: 2600 RVA: 0x00038B40 File Offset: 0x00036D40
		public Texture2D GetIcon(Pawn forPawn)
		{
			if (this.def.iconTex != null)
			{
				return this.def.iconTex;
			}
			int count = forPawn.RaceProps.lifeStageAges.Count;
			int num = forPawn.RaceProps.lifeStageAges.IndexOf(this);
			if (num == count - 1)
			{
				return LifeStageAge.AdultIcon;
			}
			if (num == 0)
			{
				return LifeStageAge.VeryYoungIcon;
			}
			return LifeStageAge.YoungIcon;
		}

		// Token: 0x04000675 RID: 1653
		public LifeStageDef def;

		// Token: 0x04000676 RID: 1654
		public float minAge;

		// Token: 0x04000677 RID: 1655
		public SoundDef soundCall;

		// Token: 0x04000678 RID: 1656
		public SoundDef soundAngry;

		// Token: 0x04000679 RID: 1657
		public SoundDef soundWounded;

		// Token: 0x0400067A RID: 1658
		public SoundDef soundDeath;

		// Token: 0x0400067B RID: 1659
		public SoundDef soundAmbience;

		// Token: 0x0400067C RID: 1660
		public SoundDef soundAttack;

		// Token: 0x0400067D RID: 1661
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x0400067E RID: 1662
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x0400067F RID: 1663
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);
	}
}
