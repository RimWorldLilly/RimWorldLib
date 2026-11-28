using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001566 RID: 5478
	public class Need_Authority : Need
	{
		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06008664 RID: 34404 RVA: 0x0030989A File Offset: 0x00307A9A
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (this.IsCurrentlyReigning || this.IsCurrentlyGivingSpeech)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06008665 RID: 34405 RVA: 0x003098BC File Offset: 0x00307ABC
		public AuthorityCategory CurCategory
		{
			get
			{
				float curLevel = this.CurLevel;
				if (curLevel < 0.01f)
				{
					return AuthorityCategory.Gone;
				}
				if (curLevel < 0.15f)
				{
					return AuthorityCategory.Weak;
				}
				if (curLevel < 0.3f)
				{
					return AuthorityCategory.Uncertain;
				}
				if (curLevel > 0.7f && curLevel < 0.85f)
				{
					return AuthorityCategory.Strong;
				}
				if (curLevel >= 0.85f)
				{
					return AuthorityCategory.Total;
				}
				return AuthorityCategory.Normal;
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06008666 RID: 34406 RVA: 0x00309910 File Offset: 0x00307B10
		public bool IsActive
		{
			get
			{
				return this.pawn.royalty != null && this.pawn.Spawned && this.pawn.Map != null && this.pawn.Map.IsPlayerHome && this.pawn.royalty.CanRequireThroneroom();
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06008667 RID: 34407 RVA: 0x0030996F File Offset: 0x00307B6F
		protected override bool IsFrozen
		{
			get
			{
				return this.pawn.Map == null || !this.pawn.Map.IsPlayerHome || this.FallPerDay <= 0f;
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x06008668 RID: 34408 RVA: 0x003099A4 File Offset: 0x00307BA4
		public float FallPerDay
		{
			get
			{
				if (this.pawn.royalty == null || !this.pawn.Spawned)
				{
					return 0f;
				}
				if (this.pawn.Map == null || !this.pawn.Map.IsPlayerHome)
				{
					return 0f;
				}
				float num = 0f;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
				}
				int num2 = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count<Pawn>();
				return num * this.FallFactorCurve.Evaluate((float)num2);
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06008669 RID: 34409 RVA: 0x00309A7C File Offset: 0x00307C7C
		public override bool ShowOnNeedList
		{
			get
			{
				return this.IsActive;
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x0600866A RID: 34410 RVA: 0x00309A84 File Offset: 0x00307C84
		public bool IsCurrentlyReigning
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.Reign;
			}
		}

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x0600866B RID: 34411 RVA: 0x00309A98 File Offset: 0x00307C98
		public bool IsCurrentlyGivingSpeech
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.GiveSpeech;
			}
		}

		// Token: 0x0600866C RID: 34412 RVA: 0x00309AAC File Offset: 0x00307CAC
		public Need_Authority(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600866D RID: 34413 RVA: 0x00309B10 File Offset: 0x00307D10
		public override void NeedInterval()
		{
			float num = 400f;
			float num2 = this.FallPerDay / num;
			if (this.IsFrozen)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count <= 1)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsCurrentlyReigning)
			{
				this.CurLevel += 2f / num;
				return;
			}
			if (this.IsCurrentlyGivingSpeech)
			{
				this.CurLevel += 3f / num;
				return;
			}
			this.CurLevel -= num2;
		}

		// Token: 0x040052BA RID: 21178
		public const float LevelGainPerDayOfReigning = 2f;

		// Token: 0x040052BB RID: 21179
		public const float LevelGainPerDayOfGivingSpeech = 3f;

		// Token: 0x040052BC RID: 21180
		private readonly SimpleCurve FallFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			},
			{
				new CurvePoint(5f, 1f),
				true
			}
		};
	}
}
