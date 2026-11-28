using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x020001F9 RID: 505
	public class TickList
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0004AFA4 File Offset: 0x000491A4
		private int TickInterval
		{
			get
			{
				switch (this.tickType)
				{
				case TickerType.Normal:
					return 1;
				case TickerType.Rare:
					return 250;
				case TickerType.Long:
					return 2000;
				default:
					return -1;
				}
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0004AFE0 File Offset: 0x000491E0
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0004B03C File Offset: 0x0004923C
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0004B088 File Offset: 0x00049288
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0004B0D8 File Offset: 0x000492D8
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0004B0E6 File Offset: 0x000492E6
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0004B0F4 File Offset: 0x000492F4
		public void Tick()
		{
			for (int i = 0; i < this.thingsToRegister.Count; i++)
			{
				this.BucketOf(this.thingsToRegister[i]).Add(this.thingsToRegister[i]);
			}
			this.thingsToRegister.Clear();
			for (int j = 0; j < this.thingsToDeregister.Count; j++)
			{
				this.BucketOf(this.thingsToDeregister[j]).Remove(this.thingsToDeregister[j]);
			}
			this.thingsToDeregister.Clear();
			if (DebugSettings.fastEcology)
			{
				Find.World.tileTemperatures.ClearCaches();
				for (int k = 0; k < this.thingLists.Count; k++)
				{
					List<Thing> list = this.thingLists[k];
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].def.category == ThingCategory.Plant)
						{
							list[l].TickLong();
						}
					}
				}
			}
			List<Thing> list2 = this.thingLists[Find.TickManager.TicksGame % this.TickInterval];
			for (int m = 0; m < list2.Count; m++)
			{
				Thing thing = list2[m];
				if (!thing.Destroyed)
				{
					try
					{
						thing.DoTick();
					}
					catch (Exception value)
					{
						string text;
						if (!thing.Spawned)
						{
							text = "";
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
							defaultInterpolatedStringHandler.AppendLiteral(" (at ");
							defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(thing.Position);
							defaultInterpolatedStringHandler.AppendLiteral(")");
							text = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						string value2 = text;
						if (Prefs.DevMode)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(20, 3);
							defaultInterpolatedStringHandler2.AppendLiteral("Exception ticking ");
							defaultInterpolatedStringHandler2.AppendFormatted(thing.ToStringSafe<Thing>());
							defaultInterpolatedStringHandler2.AppendFormatted(value2);
							defaultInterpolatedStringHandler2.AppendLiteral(": ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value);
							Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
						else
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(59, 3);
							defaultInterpolatedStringHandler3.AppendLiteral("Exception ticking ");
							defaultInterpolatedStringHandler3.AppendFormatted(thing.ToStringSafe<Thing>());
							defaultInterpolatedStringHandler3.AppendFormatted(value2);
							defaultInterpolatedStringHandler3.AppendLiteral(". Suppressing further errors. Exception: ");
							defaultInterpolatedStringHandler3.AppendFormatted<Exception>(value);
							Log.ErrorOnce(defaultInterpolatedStringHandler3.ToStringAndClear(), thing.thingIDNumber ^ 576876901);
						}
					}
				}
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0004B368 File Offset: 0x00049568
		private List<Thing> BucketOf(Thing t)
		{
			int num = t.GetHashCode();
			if (num < 0)
			{
				num *= -1;
			}
			int index = num % this.TickInterval;
			return this.thingLists[index];
		}

		// Token: 0x04000E45 RID: 3653
		private TickerType tickType;

		// Token: 0x04000E46 RID: 3654
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x04000E47 RID: 3655
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x04000E48 RID: 3656
		private List<Thing> thingsToDeregister = new List<Thing>();
	}
}
