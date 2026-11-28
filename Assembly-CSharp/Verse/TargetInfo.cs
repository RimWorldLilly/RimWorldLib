using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006AC RID: 1708
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06003625 RID: 13861 RVA: 0x0015D794 File Offset: 0x0015B994
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06003626 RID: 13862 RVA: 0x0015D7AB File Offset: 0x0015B9AB
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06003627 RID: 13863 RVA: 0x0015D7B6 File Offset: 0x0015B9B6
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003628 RID: 13864 RVA: 0x0015D7BE File Offset: 0x0015B9BE
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06003629 RID: 13865 RVA: 0x0015D7D5 File Offset: 0x0015B9D5
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x0600362A RID: 13866 RVA: 0x0015D7E3 File Offset: 0x0015B9E3
		public bool Fogged
		{
			get
			{
				if (!this.HasThing)
				{
					return this.cellInt.Fogged(this.mapInt);
				}
				return this.thingInt.Fogged();
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x0600362B RID: 13867 RVA: 0x0015D80A File Offset: 0x0015BA0A
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

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x0015D82F File Offset: 0x0015BA2F
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

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x0015D84C File Offset: 0x0015BA4C
		public IntVec3 CenterCell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.OccupiedRect().CenterCell;
				}
				return this.cellInt;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x0015D87C File Offset: 0x0015BA7C
		public PlanetTile Tile
		{
			get
			{
				if (this.thingInt != null && this.thingInt.Tile.Valid)
				{
					return this.thingInt.Tile;
				}
				if (this.cellInt.IsValid && this.mapInt != null)
				{
					return this.mapInt.Tile;
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x0600362F RID: 13871 RVA: 0x0015D8D8 File Offset: 0x0015BAD8
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x0015D8F8 File Offset: 0x0015BAF8
		public Map Map
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.MapHeld;
				}
				return this.mapInt;
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x0015D914 File Offset: 0x0015BB14
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x0015D930 File Offset: 0x0015BB30
		public TargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				string str = "Constructed TargetInfo with cell=";
				IntVec3 intVec = cell;
				Log.Warning(str + intVec.ToString() + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x0015D984 File Offset: 0x0015BB84
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x0015D98C File Offset: 0x0015BB8C
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			if (t.HasThing)
			{
				return new LocalTargetInfo(t.Thing);
			}
			return new LocalTargetInfo(t.Cell);
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x0015D9B0 File Offset: 0x0015BBB0
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted TargetInfo to IntVec3 but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x0015D9E8 File Offset: 0x0015BBE8
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				string str = "Casted TargetInfo to Thing but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str + intVec.ToString(), 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x0015DA34 File Offset: 0x0015BC34
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x0015DAA5 File Offset: 0x0015BCA5
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x0015DAB1 File Offset: 0x0015BCB1
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x0015DAC9 File Offset: 0x0015BCC9
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x0015DAD7 File Offset: 0x0015BCD7
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x0015DB0C File Offset: 0x0015BD0C
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			return "null";
		}

		// Token: 0x0400232E RID: 9006
		private Thing thingInt;

		// Token: 0x0400232F RID: 9007
		private IntVec3 cellInt;

		// Token: 0x04002330 RID: 9008
		private Map mapInt;
	}
}
