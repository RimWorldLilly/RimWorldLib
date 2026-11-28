using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB6 RID: 8118
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x17002542 RID: 9538
		// (get) Token: 0x0600CE08 RID: 52744 RVA: 0x004BAADB File Offset: 0x004B8CDB
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17002543 RID: 9539
		// (get) Token: 0x0600CE09 RID: 52745 RVA: 0x004BAAE3 File Offset: 0x004B8CE3
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x17002544 RID: 9540
		// (get) Token: 0x0600CE0A RID: 52746 RVA: 0x004BAAEB File Offset: 0x004B8CEB
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		// Token: 0x17002545 RID: 9541
		// (get) Token: 0x0600CE0B RID: 52747 RVA: 0x004BAAF8 File Offset: 0x004B8CF8
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		// Token: 0x17002546 RID: 9542
		// (get) Token: 0x0600CE0C RID: 52748 RVA: 0x004BAB05 File Offset: 0x004B8D05
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17002547 RID: 9543
		// (get) Token: 0x0600CE0D RID: 52749 RVA: 0x004BAB0D File Offset: 0x004B8D0D
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17002548 RID: 9544
		// (get) Token: 0x0600CE0E RID: 52750 RVA: 0x004BAB15 File Offset: 0x004B8D15
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		// Token: 0x17002549 RID: 9545
		// (get) Token: 0x0600CE0F RID: 52751 RVA: 0x004BAB28 File Offset: 0x004B8D28
		public List<StatModifier> StatBases
		{
			get
			{
				BuildableDef buildableDef = this.defInt as BuildableDef;
				if (buildableDef != null)
				{
					return buildableDef.statBases;
				}
				AbilityDef abilityDef = this.defInt as AbilityDef;
				if (abilityDef != null)
				{
					return abilityDef.statBases;
				}
				return null;
			}
		}

		// Token: 0x1700254A RID: 9546
		// (get) Token: 0x0600CE10 RID: 52752 RVA: 0x004BAB62 File Offset: 0x004B8D62
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x1700254B RID: 9547
		// (get) Token: 0x0600CE11 RID: 52753 RVA: 0x004BAB6A File Offset: 0x004B8D6A
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x1700254C RID: 9548
		// (get) Token: 0x0600CE12 RID: 52754 RVA: 0x004BAB72 File Offset: 0x004B8D72
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700254D RID: 9549
		// (get) Token: 0x0600CE13 RID: 52755 RVA: 0x004BAB7D File Offset: 0x004B8D7D
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x0600CE14 RID: 52756 RVA: 0x004BAB88 File Offset: 0x004B8D88
		public static StatRequest For(Thing thing)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x0600CE15 RID: 52757 RVA: 0x004BABE4 File Offset: 0x004B8DE4
		public static StatRequest For(Thing thing, Pawn pawn)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			result.pawn = pawn;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x0600CE16 RID: 52758 RVA: 0x004BAC48 File Offset: 0x004B8E48
		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				defInt = def,
				stuffDefInt = stuffDef,
				qualityCategoryInt = quality
			};
		}

		// Token: 0x0600CE17 RID: 52759 RVA: 0x004BAC94 File Offset: 0x004B8E94
		public static StatRequest For(AbilityDef def, Pawn forPawn = null)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = def,
				qualityCategoryInt = QualityCategory.Normal,
				pawn = forPawn
			};
		}

		// Token: 0x0600CE18 RID: 52760 RVA: 0x004BACE8 File Offset: 0x004B8EE8
		public static StatRequest For(RoyalTitleDef def, Faction faction, Pawn pawn = null)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = null,
				faction = faction,
				qualityCategoryInt = QualityCategory.Normal,
				pawn = pawn
			};
		}

		// Token: 0x0600CE19 RID: 52761 RVA: 0x004BAD44 File Offset: 0x004B8F44
		public static StatRequest ForEmpty()
		{
			return new StatRequest
			{
				thingInt = null,
				defInt = null,
				stuffDefInt = null,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x0600CE1A RID: 52762 RVA: 0x004BAD7C File Offset: 0x004B8F7C
		public override string ToString()
		{
			if (this.Thing != null)
			{
				string str = "(";
				Thing thing = this.Thing;
				return str + ((thing != null) ? thing.ToString() : null) + ")";
			}
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			Thing thing2 = this.Thing;
			array[num] = ((thing2 != null) ? thing2.ToString() : null);
			array[2] = ", ";
			array[3] = ((this.StuffDef != null) ? this.StuffDef.defName : "null");
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x0600CE1B RID: 52763 RVA: 0x004BAE08 File Offset: 0x004B9008
		public override int GetHashCode()
		{
			int num = 0;
			num = Gen.HashCombineInt(num, (int)this.defInt.shortHash);
			if (this.thingInt != null)
			{
				num = Gen.HashCombineInt(num, this.thingInt.thingIDNumber);
			}
			if (this.stuffDefInt != null)
			{
				num = Gen.HashCombineInt(num, (int)this.stuffDefInt.shortHash);
			}
			num = Gen.HashCombineInt(num, this.qualityCategoryInt.GetHashCode());
			if (this.faction != null)
			{
				num = Gen.HashCombineInt(num, this.faction.GetHashCode());
			}
			if (this.pawn != null)
			{
				num = Gen.HashCombineInt(num, this.pawn.GetHashCode());
			}
			return num;
		}

		// Token: 0x0600CE1C RID: 52764 RVA: 0x004BAEAC File Offset: 0x004B90AC
		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x0600CE1D RID: 52765 RVA: 0x004BAEF8 File Offset: 0x004B90F8
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt && other.qualityCategoryInt == this.qualityCategoryInt && other.faction == this.faction && other.pawn == this.pawn;
		}

		// Token: 0x0600CE1E RID: 52766 RVA: 0x004BAF5B File Offset: 0x004B915B
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600CE1F RID: 52767 RVA: 0x004BAF65 File Offset: 0x004B9165
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04008347 RID: 33607
		private Thing thingInt;

		// Token: 0x04008348 RID: 33608
		private Def defInt;

		// Token: 0x04008349 RID: 33609
		private ThingDef stuffDefInt;

		// Token: 0x0400834A RID: 33610
		private QualityCategory qualityCategoryInt;

		// Token: 0x0400834B RID: 33611
		private Faction faction;

		// Token: 0x0400834C RID: 33612
		private Pawn pawn;
	}
}
