using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A3 RID: 1699
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060035A0 RID: 13728 RVA: 0x0015B8BE File Offset: 0x00159ABE
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060035A1 RID: 13729 RVA: 0x0015B8D5 File Offset: 0x00159AD5
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060035A2 RID: 13730 RVA: 0x0015B8E0 File Offset: 0x00159AE0
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060035A3 RID: 13731 RVA: 0x0015B8E8 File Offset: 0x00159AE8
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060035A4 RID: 13732 RVA: 0x0015B8F5 File Offset: 0x00159AF5
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060035A5 RID: 13733 RVA: 0x0015B90C File Offset: 0x00159B0C
		public bool ThingDiscarded
		{
			get
			{
				return this.Thing != null && this.Thing.Discarded;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060035A6 RID: 13734 RVA: 0x0015B923 File Offset: 0x00159B23
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x0015B92F File Offset: 0x00159B2F
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060035A8 RID: 13736 RVA: 0x0015B954 File Offset: 0x00159B54
		public IntVec3 Cell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.PositionHeld;
				}
				return this.cellInt;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x0015B970 File Offset: 0x00159B70
		public Vector3 CenterVector3
		{
			get
			{
				if (this.thingInt != null)
				{
					if (this.thingInt.Spawned)
					{
						return this.thingInt.DrawPos;
					}
					if (this.thingInt.SpawnedOrAnyParentSpawned)
					{
						return this.thingInt.PositionHeld.ToVector3Shifted();
					}
					return this.thingInt.Position.ToVector3Shifted();
				}
				else
				{
					if (this.cellInt.IsValid)
					{
						return this.cellInt.ToVector3Shifted();
					}
					return default(Vector3);
				}
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x0015B9F5 File Offset: 0x00159BF5
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x0015BA09 File Offset: 0x00159C09
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x0015BA19 File Offset: 0x00159C19
		public bool TryGetPawn(out Pawn pawn)
		{
			pawn = this.Pawn;
			return pawn != null;
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x0015BA28 File Offset: 0x00159C28
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x0015BA30 File Offset: 0x00159C30
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x0015BA38 File Offset: 0x00159C38
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted LocalTargetInfo to IntVec3 but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x0015BA70 File Offset: 0x00159C70
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				string str = "Casted LocalTargetInfo to Thing but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str + intVec.ToString(), 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0015BAB9 File Offset: 0x00159CB9
		public TargetInfo ToTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new TargetInfo(this.Thing);
			}
			return new TargetInfo(this.Cell, map, false);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0015BAEA File Offset: 0x00159CEA
		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new GlobalTargetInfo(this.Thing);
			}
			return new GlobalTargetInfo(this.Cell, map, false);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0015BB1C File Offset: 0x00159D1C
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.thingInt != null || b.thingInt != null)
			{
				return a.thingInt == b.thingInt;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x0015BB77 File Offset: 0x00159D77
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0015BB83 File Offset: 0x00159D83
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x0015BB9B File Offset: 0x00159D9B
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x0015BBA9 File Offset: 0x00159DA9
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x0015BBD0 File Offset: 0x00159DD0
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString();
			}
			return "null";
		}

		// Token: 0x04002305 RID: 8965
		private Thing thingInt;

		// Token: 0x04002306 RID: 8966
		private IntVec3 cellInt;
	}
}
