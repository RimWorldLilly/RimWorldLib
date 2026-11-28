using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000388 RID: 904
	public class DamageFlasher
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001B84 RID: 7044 RVA: 0x000A9858 File Offset: 0x000A7A58
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x000A986E File Offset: 0x000A7A6E
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x000A987C File Offset: 0x000A7A7C
		public Color CurColor
		{
			get
			{
				return Color.Lerp(Color.white, DamagedMatPool.DamagedMatStartingColor, (float)this.DamageFlashTicksLeft / 16f);
			}
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000A989A File Offset: 0x000A7A9A
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000A98AD File Offset: 0x000A7AAD
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000A98C2 File Offset: 0x000A7AC2
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04001690 RID: 5776
		private int lastDamageTick = -9999;

		// Token: 0x04001691 RID: 5777
		private const int DamagedMatTicksTotal = 16;
	}
}
