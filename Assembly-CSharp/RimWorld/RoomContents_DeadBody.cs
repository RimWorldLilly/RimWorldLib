using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001245 RID: 4677
	public abstract class RoomContents_DeadBody : RoomContentsWorker
	{
		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06007400 RID: 29696 RVA: 0x0028A257 File Offset: 0x00288457
		protected virtual FloatRange CorpseAgeDaysRange
		{
			get
			{
				return new FloatRange(3f, 30f);
			}
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x06007401 RID: 29697 RVA: 0x0028A268 File Offset: 0x00288468
		protected virtual IntRange SurvivalPacksCountRange
		{
			get
			{
				return new IntRange(0, 2);
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06007402 RID: 29698 RVA: 0x0028A271 File Offset: 0x00288471
		protected virtual IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(1, 5);
			}
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06007403 RID: 29699 RVA: 0x0028A27A File Offset: 0x0028847A
		protected virtual IntRange CorpseRange
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x06007404 RID: 29700 RVA: 0x0028A283 File Offset: 0x00288483
		protected virtual bool AllHaveSameDeathAge
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x06007405 RID: 29701
		protected abstract ThingDef KillerThing { get; }

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06007406 RID: 29702
		protected abstract DamageDef DamageType { get; }

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06007407 RID: 29703
		protected abstract Tool ToolUsed { get; }

		// Token: 0x06007408 RID: 29704 RVA: 0x0028A286 File Offset: 0x00288486
		protected virtual IEnumerable<PawnKindDef> GetPossibleKinds()
		{
			return new RoomContents_DeadBody.<GetPossibleKinds>d__17(-2);
		}

		// Token: 0x06007409 RID: 29705 RVA: 0x0028A28F File Offset: 0x0028848F
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnCorpses(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600740A RID: 29706 RVA: 0x0028A2A4 File Offset: 0x002884A4
		protected virtual void SpawnCorpses(Map map, LayoutRoom room)
		{
			int randomInRange = this.CorpseRange.RandomInRange;
			if (this.AllHaveSameDeathAge)
			{
				this.setAgeTicks = new int?(Mathf.RoundToInt(this.CorpseAgeDaysRange.RandomInRange * 60000f));
			}
			else
			{
				this.setAgeTicks = null;
			}
			for (int i = 0; i < randomInRange; i++)
			{
				Corpse corpse = this.SpawnCorpse(map, room);
				int randomInRange2 = this.SurvivalPacksCountRange.RandomInRange;
				for (int j = 0; j < randomInRange2; j++)
				{
					Thing thing;
					GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack, null), corpse.Position, map, ThingPlaceMode.Near, out thing, null, null, true);
				}
			}
		}

		// Token: 0x0600740B RID: 29707 RVA: 0x0028A350 File Offset: 0x00288550
		protected Corpse SpawnCorpse(Map map, LayoutRoom room)
		{
			IntVec3 cell;
			if (!room.TryGetRandomCellInRoom(map, out cell, 1, 0, null, false))
			{
				return null;
			}
			return this.SpawnCorpse(cell, map);
		}

		// Token: 0x0600740C RID: 29708 RVA: 0x0028A378 File Offset: 0x00288578
		protected Corpse SpawnCorpse(IntVec3 cell, Map map)
		{
			PawnKindDef kind = this.GetPossibleKinds().RandomElement<PawnKindDef>();
			int deadTicks = this.setAgeTicks ?? Mathf.RoundToInt(this.CorpseAgeDaysRange.RandomInRange * 60000f);
			return this.SpawnCorpse(cell, kind, deadTicks, map, null, false);
		}

		// Token: 0x0600740D RID: 29709 RVA: 0x0028A3DC File Offset: 0x002885DC
		protected Corpse SpawnCorpse(IntVec3 cell, PawnKindDef kind, int deadTicks, Map map, float? fixedAge = null, bool forceNoGear = false)
		{
			return RoomGenUtility.SpawnCorpse(cell, kind, deadTicks, map, this.DamageType, fixedAge, forceNoGear, this.KillerThing, this.ToolUsed, new IntRange?(this.BloodFilthRange), null);
		}

		// Token: 0x04004913 RID: 18707
		private int? setAgeTicks;
	}
}
